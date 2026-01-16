using SudokuNet;

namespace SudokuDemo
{
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
                Console.Clear();
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

        static void SudokuGame()
        {
            int selectedDifficultyClue = PromptDifficultySelection();
            Board board = Sudoku.GeneratePuzzle(selectedDifficultyClue);
            Console.Clear();
        }
    }
}
