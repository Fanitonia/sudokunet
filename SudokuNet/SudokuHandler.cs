﻿using System.Diagnostics;

namespace SudokuNet
{
    public class SudokuHandler
    {
        private const int EMPTY_CELL = 0;

        private static Stopwatch timer = new Stopwatch();
        private static int solveStep = 0;

        #region Sudoku Creation Methods

        /// <summary>
        /// Initializes the specified Sudoku board by creating a new empty <see cref="Cell"/> for each position
        /// in the 9x9 grid. All cells are set to empty (value = 0) and marked as editable.
        /// </summary>
        public static void InitializeBoard(SudokuBoard board)
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
        /// Generates a new Sudoku puzzle on the specified board with the given number of clues.
        /// </summary>
        public static void GeneratePuzzle(SudokuBoard board, int clues)
        {
            InitializeBoard(board);
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

        // copies the 'mainField' to 'solvedField' and sets the 'canChange' value of the cells
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

                    copy[cordY, cordX] = new Cell(original[cordY, cordX].value, original[cordY, cordX].potentialValues, canChange);
                }
            }
        }
        #endregion

        #region Sudoku Validation Methods
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
            int rowStart = cordY - cordY % 3;
            int columnStart = cordX - cordX % 3;

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
        /// Determines whether the Sudoku puzzle is completely and correctly solved.
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
                    solveStep++;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates the current state of the Sudoku puzzle.
        /// Returns true if all filled cells do not violate Sudoku rules (no duplicates in any row, column, or 3x3 box).
        /// Does not require the puzzle to be completely filled or solved.
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
        /// Attempts to solve the given Sudoku puzzle up to the specified number of times.
        /// </summary>
        /// <returns>Returns true if the puzzle is successfully solved in any attempt; otherwise, returns false..</returns>
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
        /// <returns>False if it encounters a problem while solving. Otherwise true.</returns>
        public static bool TrySolve(SudokuBoard board)
        {
            return TrySolve(board, 1);
        }

        // Copies the field to solvedField and tries to solve it. Returns false if it encounters a problem.
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
                    if (cell.potentialValues.Count == 0 && cell.value == EMPTY_CELL)
                        return false;

                    solveStep++;
                }

                foreach (Cell cell in board.solvedField)
                {
                    if (cell.potentialValues.Count == FindSmallestPotential(board.solvedField) && cell.value == EMPTY_CELL)
                    {
                        cell.value = cell.potentialValues[random.Next(cell.potentialValues.Count)];
                        UpdateAllPotentials(board.solvedField);
                        break;
                    }

                    solveStep++;
                }
            } while (!IsSudokuSolved(board.solvedField));

            timer.Stop();

            board.SolvedTime = (int)timer.ElapsedMilliseconds;
            board.SolvedStep = solveStep;
            return true;
        }
        #endregion

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
            field[cordY, cordX].potentialValues.Clear();
            field[cordY, cordX].potentialValues.AddRange(tmpNumbers);
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
                if (cell.potentialValues.Count < smallest && cell.value == EMPTY_CELL)
                    smallest = cell.potentialValues.Count;
                solveStep++;
            }
            return smallest;
        }

        // // checks if the coordinates are in between 0-8 (inclusive)
        internal static bool IsCordValid(int cordX, int cordY)
        {
            if (cordX > 8 || cordX < 0)
                return false;
            else if (cordY > 8 || cordY < 0)
                return false;

            return true;
        }
    }
}