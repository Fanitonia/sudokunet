using SudokuNet;

namespace SudokuDemo
{
    struct Cursor(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
    }

    class GameState(bool solved, bool solve, bool exit, bool newGame)
    {
        public bool Solved { get; set; } = solved;
        public bool Solve { get; set; } = solve;
        public bool Exit { get; set; } = exit;
        public bool NewGame { get; set; } = newGame;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            char selection = PromptMainMenu();

            if (selection == '1')
            {
                SudokuGame();
            }
            else if (selection == '2')
            {
                SudokuSolver();
            }
        }   

        static char PromptMainMenu()
        {
            char choice;
            bool isValidChoice = true;

            do
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║             SudokuNet Demo             ║");
                Console.WriteLine("╚════════════════════════════════════════╝\n");

                Console.WriteLine("Select what you want:");
                Console.WriteLine(" 1 - Sudoku Game");
                Console.WriteLine(" 2 - Sudoku Solver\n");

                Console.Write(isValidChoice ? "Your Choice: " : "Invalid Input! Please Try Again: ");
                choice = Console.ReadKey().KeyChar;
                isValidChoice = choice == '1' || choice == '2';
            }
            while (!isValidChoice);

            return choice;
        }
        static void SudokuGame()
        {
            GameState state;
            do
            {
                int selectedDifficultyClue = PromptDifficultySelection();
                Board board = Sudoku.GeneratePuzzle(selectedDifficultyClue);
                Board initialBoard = board.Clone();
                Cursor cursor = new Cursor(0, 0);

                state = new GameState(false, false, false, false);

                do
                {
                    Console.Clear();
                    DisplayBoard(board, cursor);
                    DisplayGameControls();
                    HandleInput(board, ref cursor, state);
                    state.Solved = board.IsSudokuSolved();

                } while (!state.Solved && !state.Solve && !state.NewGame && !state.Exit);

                if (state.Solved)
                {
                    Console.Clear();
                    DisplayBoard(board, cursor);
                    Console.WriteLine("You successfully solved the sudoku!");
                    Console.Write("Press Y for new game.");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                        state.NewGame = true;
                }
                else if (state.Solve)
                {
                    Board solvedBoard;
                    Sudoku.TrySolve(initialBoard, out solvedBoard);
                    Console.Clear();
                    DisplayBoard(solvedBoard, cursor, false);
                    Console.WriteLine("Auto solved.");
                    Console.Write("Press Y for new game.");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                        state.NewGame = true;
                }
                else if (state.Exit)
                {
                    Environment.Exit(0);
                }

            } while (state.NewGame);
        }

        static void SudokuSolver()
        {
            Board board = new Board();
            Cursor cursor = new Cursor(0, 0);
            GameState state = new GameState(false, false, false, false);

            do {
                Console.Clear();
                DisplayBoard(board, cursor);
                DisplayGameControls(true);
                HandleInput(board, ref cursor, state);
            }while (!state.Solve && !state.Exit);

            if (state.Solve)
            {
                if(!board.IsSudokuValid())
                {
                    Console.Clear();
                    DisplayBoard(board, cursor, false);
                    Console.WriteLine("Sudoku is not valid and can't be solved");
                    return;
                }

                Board solvedBoard;
                Sudoku.TrySolve(board, out solvedBoard);

                Console.Clear();
                DisplayBoard(solvedBoard, cursor, false);
                Console.WriteLine("Auto solved.");
            }
            else if (state.Exit)
            {
                Environment.Exit(0);
            }
        }

        static int PromptDifficultySelection()
        {
            char choice;
            bool isValidChoice = true;

            do
            {
                Console.Clear();
                Console.WriteLine("Select difficulty: ");
                Console.WriteLine(" 1 - Easy (45 clue)");
                Console.WriteLine(" 2 - Normal (35 clue)");
                Console.WriteLine(" 3 - Hard (25 clue)");
                Console.WriteLine(" 4 - Very Hard (18 clue)\n");

                Console.Write(isValidChoice ? "Your Choice: " : "Invalid Input! Please Try Again: ");
                choice = Console.ReadKey().KeyChar;
                isValidChoice = choice == '1' || choice == '2' || choice == '3' || choice == '4';
            } while (!isValidChoice);

            switch (choice)
            {
                case '1': return 45;
                case '2': return 35;
                case '3': return 25;
                case '4': return 18;
                default: return 35;
            }
        }

        static void DisplayBoard(Board board, Cursor cursor, bool showCursor = true)
        {
            Console.WriteLine("┌───────┬───────┬───────┐");

            for (int cordY = 0; cordY < 9; cordY++)
            {
                Console.Write("│ ");

                for (int cordX = 0; cordX < 9; cordX++)
                {
                    int value = board.GetCell(cordX, cordY);
                    bool isLocked = board.IsCellLocked(cordX, cordY);
                    bool isCursor = cordX == cursor.X && cordY == cursor.Y;

                    if (isCursor && showCursor)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (!isLocked)
                        Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.Write(value == 0 ? "." : value.ToString());
                    Console.ResetColor();
                    Console.Write(" ");

                    if ((cordX + 1) % 3 == 0 && cordX < 8)
                        Console.Write("│ ");
                }

                Console.WriteLine("│");

                if ((cordY + 1) % 3 == 0 && cordY < 8)
                    Console.WriteLine("├───────┼───────┼───────┤");
            }

            Console.WriteLine("└───────┴───────┴───────┘");
        }

        static void DisplayGameControls(bool showSudokuSolverControls = false)
        {
            Console.WriteLine();
            Console.WriteLine("┌─ Controls ──────────────────────────┐");
            Console.WriteLine("│ W A S D : Move the Cursor           │");
            Console.WriteLine("│ 1-9     : Write Number              │");
            Console.WriteLine("│ 0       : Clean Cell                │");
            Console.WriteLine("│ R       : Auto Solve                │");

            if(!showSudokuSolverControls)
                    Console.WriteLine("│ Y       : New Game                  │");

            Console.WriteLine("│ Q       : Exit                      │");
            Console.WriteLine("└─────────────────────────────────────┘");
        }

        static void HandleInput(Board board, ref Cursor cursor, GameState state)
        {
            var input = Console.ReadKey().Key;

            switch (input)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (cursor.Y > 0) cursor.Y--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (cursor.Y < 8) cursor.Y++;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (cursor.X < 8) cursor.X++;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (cursor.X > 0) cursor.X--;
                    break;
                case ConsoleKey.R:
                    state.Solve = true;
                    break;
                case ConsoleKey.Q:
                    state.Exit = true;
                    break;
                case ConsoleKey.Y:
                    state.NewGame = true;
                    break;
                default:
                    if (TryGetDigit(input, out var digit))
                        board.SetCell(cursor.X, cursor.Y, digit);
                    break;
            }
        }

        static bool TryGetDigit(ConsoleKey key, out int digit)
        {
            if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
            {
                digit = key - ConsoleKey.D0;
                return true;
            }
            if (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9)
            {
                digit = key - ConsoleKey.NumPad0;
                return true;
            }
            digit = -1;
            return false;
        }
    }
}
