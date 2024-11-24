using SudokuLibrary;

namespace ConsoleUI
{
    internal class SudokuConsole
    {
        public const int EMPTY_CELL = 0;
        public static bool endGame = false;

        public static SudokuBoard board = new SudokuBoard();

        private struct Cursor
        {
            public static int x = 0;
            public static int y = 0;
        }

        static void Main(string[] args)
        {
            do
            {
                
                int input;
                do
                {
                    Console.Clear();
                    Console.WriteLine("<---<Sudoku>--->");
                    Console.WriteLine("1. New Game");
                    Console.WriteLine("2. Sudoku Solver");
                } while (!(int.TryParse(Console.ReadLine(), out input) && (input > 0 && input < 3)));

                if (input == 1)
                {
                    SudokuHandler.GeneratePuzzle(board, 34);
                    do
                    {
                        PrintBoard(false);
                        HandleInput(false);
                    } while (!endGame);
                }
                else if (input == 2)
                {
                    SudokuHandler.InitializeBoard(board);
                    do
                    {
                        PrintBoard(true);
                        HandleInput(true);
                    } while (!endGame);
                }

                RestartGame();
            } while(!endGame);

        }

        public static void PrintBoard(bool isSolverMode)
        {
            ConsoleColor userForeColor = Console.ForegroundColor;
            ConsoleColor userBackColor = Console.BackgroundColor;

            Console.Clear();
            int cordX = 0, cordY = 0;

            if (isSolverMode)
                Console.WriteLine("<-Enter a sudoku puzzle->");

            Console.WriteLine("┌───────┬───────┬───────┐");
            // rows of board
            for (int y = 2; y <= 12; y++)
            {
                // columns of board
                for (int x = 1; x <= 25; x++)
                {
                    if (x % 8 == 1)
                    {
                        if (y == 5 || y == 9 || y == 14)
                            Console.Write("┼");
                        else
                            Console.Write("│");
                        continue;
                    }

                    if (y % 4 == 1)
                    {
                        Console.Write("─");
                        continue;
                    }

                    if (x % 2 == 0)
                    {
                        Console.Write(" ");
                        continue;
                    }

                    if (x % 2 == 1)
                    {
                        if (cordY == Cursor.y && cordX == Cursor.x && !endGame)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                            Console.BackgroundColor = userBackColor;
                        }
                        else if (board.GetCellValue(cordX, cordY, endGame) != EMPTY_CELL)
                        {
                            if (board.CanCellChange(cordX,cordY) && !isSolverMode)
                                Console.ForegroundColor = ConsoleColor.Red;
                            else if (isSolverMode && board.CanCellChange(cordX,cordY))
                                Console.ForegroundColor = ConsoleColor.Red;

                            Console.Write(board.GetCellValue(cordX, cordY, endGame));
                            Console.ForegroundColor = userForeColor;
                        }
                        else
                            Console.Write(" ");

                        cordX++;
                    }
                }
                if (y % 4 != 1)
                    cordY++;

                cordX = 0;
                Console.WriteLine();
            }
            Console.WriteLine("└───────┴───────┴───────┘");

            Console.WriteLine();
            if (!endGame)
                Console.WriteLine("Move -> UP, DOWN, RIGHT, LEFT arrow buttons\nPick Number -> 1-9\nDelete Number -> Del\nSolve -> S\nClose -> Q");
        }

        public static void HandleInput(bool isSolverMode)
        {
            ConsoleKeyInfo input;
            input = Console.ReadKey(true);

            if (char.IsNumber(input.KeyChar))
            {
                board.SetCellValue(Cursor.x, Cursor.y, (int)char.GetNumericValue(input.KeyChar));
                return;
            }

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    if (Cursor.y > 0)
                        Cursor.y--;
                    break;
                case ConsoleKey.DownArrow:
                    if (Cursor.y < 8)
                        Cursor.y++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (Cursor.x > 0)
                        Cursor.x--;
                    break;
                case ConsoleKey.RightArrow:
                    if (Cursor.x < 8)
                        Cursor.x++;
                    break;
                case ConsoleKey.Delete:
                    board.DeleteCellValue(Cursor.x, Cursor.y);
                    break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.S:
                    if (!isSolverMode)
                    {
                        EndGame(false);
                        return;
                    }
                        
                    else
                    {
                        if (SudokuHandler.TrySolve(board, 10))
                        {
                            EndGame(true);
                            return;
                        }
                        else
                        {
                            PrintBoard(true);
                            endGame = true;
                            Console.WriteLine("There is no solution.");
                            return;
                        }
                    }
                default:
                    break;
            }

            if (SudokuHandler.IsSudokuSolved(board))
                WinGame();
        }

        public static void EndGame(bool isSolverMode)
        {
            endGame = true;
            PrintBoard(isSolverMode);
            Console.WriteLine($"Sudoku solved in {board.SolvedTime} ms\nTotal Steps: {board.SolvedStep}");
        }

        public static void WinGame()
        {
            endGame = true;
            PrintBoard(false);
            Console.WriteLine("\n     YOU DID WIN!!!");
        }

        public static void RestartGame()
        {
            Console.WriteLine("\nPress Enter to restart or Q to quit.");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        endGame = false;
                        return;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }
    }
}
