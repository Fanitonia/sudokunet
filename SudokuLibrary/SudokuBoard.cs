
namespace SudokuLibrary
{
    public class SudokuBoard
    {
        private const int EMPTY_CELL = 0;

        internal Cell[,] mainField = new Cell[9, 9];
        internal Cell[,] solvedField = new Cell[9, 9];

        private int solvedTime = 0;
        private int solvedStep = 0;

        public int SolvedTime { get { return solvedTime; } internal set { solvedTime = value; } }
        public int SolvedStep { get { return solvedStep; } internal set { solvedStep = value; } }

        /// <summary>
        /// Creating a new empty Sudoku board.
        /// </summary>
        public SudokuBoard() 
        {
            SudokuSolver.CreateEmptyBoard(this);
        }

        /// <summary>
        /// Creating a new Sudoku puzzle with specified number of clues
        /// </summary>
        /// <param name="clues"></param>
        public SudokuBoard(int clues)
        {
            SudokuSolver.GeneratePuzzle(this, clues);
        }

        /// <summary>
        /// Sets the value of a specified cell.
        /// </summary>
        /// <returns>False if the cell cannot be changed. Otherwise true</returns>
        public bool SetCellValue(int cordX, int cordY, int value)
        {
            if (value > 9 || value < 0)
                throw new Exception("Value is invalid (it must be between 1-9)");

            if (!SudokuSolver.IsCordValid(cordX, cordY))
                throw new Exception("Coordinates are invalid");

            if (mainField[cordY, cordX].canChange)
            {
                mainField[cordY, cordX].value = value;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Gets the value of a specified cell.
        /// </summary>
        public int GetCellValue(int cordX, int cordY, bool getFromSolvedVersion)
        {
            if (!SudokuSolver.IsCordValid(cordX, cordY))
                throw new Exception("Coordinates are invalid");

            if (getFromSolvedVersion)
            {
                if (solvedField[0, 0] == null)
                    throw new Exception("There is no solved version of the puzzle");
                return solvedField[cordY, cordX].value;
            }
            else
                return mainField[cordY, cordX].value;
        }

        /// <summary>
        /// Deletes the value of the specified cell.
        /// </summary>
        /// <returns>False if the cell cannot be changed. Otherwise true.</returns>
        public bool DeleteCellValue(int cordX, int cordY)
        {
            if (!SudokuSolver.IsCordValid(cordX, cordY))
                throw new Exception("Coordinates are invalid");

            if (mainField[cordY, cordX].canChange)
            {
                mainField[cordY, cordX].value = 0;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks if the cell is part of the puzzle and cannot be changed by the user.
        /// </summary>
        public bool CanCellChange(int cordX, int cordY)
        {
            if (!SudokuSolver.IsCordValid(cordX, cordY))
                throw new Exception("Coordinates are invalid");

            return mainField[cordY, cordX].canChange;
        }

        /// <summary>
        /// Returns the count of empty cells on the board.
        /// </summary>
        public int GetNumberOfEmptyCells()
        {
            return GetNumberOfEmptyCells(mainField);
        }

        // returns the count of empty cells on the board
        private int GetNumberOfEmptyCells(Cell[,] board)
        {
            int emptyCell = 0;

            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                {
                    if (board[cordY, cordX].value == 0)
                    {
                        emptyCell++;
                    }
                }
            }

            return emptyCell;
        }

        /// <summary>
        /// Prints the Sudoku puzzle in a formatted layout. 
        /// </summary>
        public void PrintToConsole(bool printSolvedBoard)
        {
            ConsoleColor userForeColor = Console.ForegroundColor;
            ConsoleColor userBackColor = Console.BackgroundColor;
            int cordX = 0, cordY = 0;

            Cell[,] field;
            if (printSolvedBoard)
                field = solvedField;
            else
                field = mainField;

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
                        if (field[cordY, cordX].value != EMPTY_CELL)
                        {
                            if (field[cordY, cordX].canChange)
                                Console.ForegroundColor = ConsoleColor.Red;

                            Console.Write(field[cordY, cordX].value);
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
        }
    }
}
