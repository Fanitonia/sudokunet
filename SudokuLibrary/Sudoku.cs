using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace SudokuLibrary
{
    public class Sudoku
    {
        private static Stopwatch timer = new Stopwatch();
        private static int solveStep = 0;

        private const int EMPTY_CELL = 0;

        public static long SolvedTime { get { return timer.ElapsedMilliseconds; } }
        public static int SolvedStep { get { return solveStep; } }

        // updates potential list of a specific cell
        private static void UpdateCellPotentials(int cordX, int cordY, Cell[,] field)
        {
            List<int> tmpNumbers = new List<int>();
            for (int testValue = 1; testValue <= 9; testValue++)
            {
                if (IsPositionSuitable(cordX, cordY, testValue, field))
                    tmpNumbers.Add(testValue);
                solveStep++;
            }
            field[cordY, cordX].pNumbers.Clear();
            field[cordY, cordX].pNumbers.AddRange(tmpNumbers);
        }

        // updates the potential list of alll cells in the field
        private static void UpdateAllPotentials(Cell[,] field)
        {
            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                    UpdateCellPotentials(cordX, cordY, field);
            }
        }

        // finds the cell with the fewest potential numbers (returns the number count)
        private static int FindSmallestPotential(Cell[,] field)
        {
            int smallest = 9;
            foreach (Cell cell in field)
            {
                if (cell.pNumbers.Count < smallest && cell.value == EMPTY_CELL)
                    smallest = cell.pNumbers.Count;
                solveStep++;
            }
            return smallest;
        }

        #region Sudoku Creation Methods
        /// <summary>
        /// Creates an empty Sudoku board (cells are considered empty if their value is 0).
        /// </summary>
        public static void CreateEmptyBoard(SudokuBoard board)
        {
            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                {
                    board.mainField[cordY, cordX] = new Cell();
                }
            }
        }

        /// <summary>
        /// Generates a Sudoku puzzle with the specified number of clues.
        /// </summary>
        public static void GeneratePuzzle(SudokuBoard board, int clues)
        {
            CreateEmptyBoard(board);
            while (!Solve(board)) {}

            int cordX, cordY;
            Random random = new Random();

            for (int i = 0; i < clues; i++)
            {
                do
                {
                    cordY = random.Next(9);
                    cordX = random.Next(9);
                } while (board.mainField[cordY, cordX].value != EMPTY_CELL);

                board.mainField[cordY, cordX].value = board.solvedField[cordY, cordX].value;
                board.mainField[cordY, cordX].canChange = false;
                board.solvedField[cordY, cordX].canChange = false;
            }
        }

        private static void CopyAndSetBoard(Cell[,] original, Cell[,] copy)
        {
            bool canChange;
            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                {
                    if (original[cordY, cordX].value != EMPTY_CELL)
                    {
                        canChange = false;
                        original[cordY, cordX].canChange = false;
                    }
                    else
                        canChange = true;

                    copy[cordY, cordX] = new Cell(original[cordY, cordX].value, original[cordY, cordX].pNumbers, canChange);
                }
            }
        }
        #endregion

        #region Sudoku Validation Methods
        /// <summary>
        /// Checks if a value can be placed in the specified position of the Sudoku puzzle. 
        /// </summary>
        /// <returns>Returns false if the value cannot be placed at the specified position, otherwise true.</returns>
        public static bool IsPositionSuitable(SudokuBoard board, int cordX, int cordY, int value)
        {
            return IsPositionSuitable(cordX, cordY, value, board.mainField);
        }

        private static bool IsPositionSuitable(int cordX, int cordY, int value, Cell[,] field)
        {
            if (!IsCordValid(cordX, cordY))
                throw new Exception("Coordinates are invalid");
            else if (value > 9 || value < 0)
                throw new Exception("Value is invalid (it must be between 1-9)");

            // checks the column and the row of the position
            for (int i = 0; i < 9; i++)
            {
                if (field[cordY, i].value == value)
                    return false;

                else if (field[i, cordX].value == value)
                    return false;
                solveStep++;
            }

            // finding top left position of the 3x3 are position's part of
            int rowStart = cordY - (cordY % 3);
            int columnStart = cordX - (cordX % 3);

            // checks the 3x3 area position's part of
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (field[rowStart + i, columnStart + j].value == value)
                        return false;
                    solveStep++;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if Sudoku solved correctly.
        /// </summary>
        public static bool IsSudokuSolved(SudokuBoard board)
        {
            return IsSudokuSolved(board.mainField);
        }

        private static bool IsSudokuSolved(Cell[,] field)
        {
            int tmpHolder;
            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                {
                    tmpHolder = field[cordY, cordX].value;
                    field[cordY, cordX].value = EMPTY_CELL;
                    if (IsPositionSuitable(cordX, cordY, tmpHolder, field) && tmpHolder != EMPTY_CELL)
                        field[cordY, cordX].value = tmpHolder;
                    else
                    {
                        field[cordY, cordX].value = tmpHolder;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the current state of the puzzle is valid.
        /// </summary>
        public static bool IsSudokuValid(SudokuBoard board)
        {
            int tmpHolder;
            for (int cordY = 0; cordY < 9; cordY++)
            {
                for (int cordX = 0; cordX < 9; cordX++)
                {
                    tmpHolder = board.mainField[cordY, cordX].value;
                    board.mainField[cordY, cordX].value = EMPTY_CELL;
                    if (tmpHolder == EMPTY_CELL || IsPositionSuitable(board, cordX, cordY, tmpHolder))
                        board.mainField[cordY, cordX].value = tmpHolder;
                    else
                    {
                        board.mainField[cordY, cordX].value = tmpHolder;
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Solving Methods
        /// <summary>
        /// Attempts to solve the puzzle for specified number of attempts.
        /// </summary>
        public static bool TrySolve(SudokuBoard board, int attempts)
        {
            if (attempts < 1)
                throw new Exception("Attempts cannot be smaller than 1");

            if (!IsSudokuValid(board))
                return false;

            for (int i = 0; i < attempts; i++)
            {
                if (Solve(board))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to solve the puzzle once.
        /// </summary>
        public static bool TrySolve(SudokuBoard board)
        {
            return TrySolve(board, 1);
        }

        // Copies the field to solvedField and tries to solve it.
        private static bool Solve(SudokuBoard board)
        {
            Random random = new Random();
            CopyAndSetBoard(board.mainField, board.solvedField);

            solveStep = 0;
            timer.Restart();
            timer.Start();
            UpdateAllPotentials(board.solvedField);

            do
            {
                foreach (Cell cell in board.solvedField)
                {
                    if (cell.pNumbers.Count == 0 && cell.value == EMPTY_CELL)
                        return false;

                    solveStep++;
                }

                foreach (Cell cell in board.solvedField)
                {
                    if (cell.pNumbers.Count == FindSmallestPotential(board.solvedField) && cell.value == EMPTY_CELL)
                    {
                        cell.value = cell.pNumbers[random.Next(cell.pNumbers.Count)];
                        UpdateAllPotentials(board.solvedField);
                        break;
                    }

                    solveStep++;
                }
            } while (!IsSudokuSolved(board.solvedField));

            timer.Stop();
            return true;
        }
        #endregion

        private static bool IsCordValid(int cordX, int cordY)
        {
            if (cordX > 8 || cordX < 0)
                return false;
            else if (cordY > 8 || cordY < 0)
                return false;

            return true;
        }
    }
}