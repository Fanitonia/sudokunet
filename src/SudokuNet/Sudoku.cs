using System.Diagnostics;

namespace SudokuNet;

/// <summary>
/// Provides static methods for creating, solving, and manipulating Sudoku puzzles.
/// </summary>
public static class Sudoku
{
    /// <summary>
    /// Initializes the specified game board by populating all cells with empty cells.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> instance to initialize.</param>
    public static void InitializeBoard(Board board)
    {
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                board.field[cordY, cordX] = new Cell();
            }
        }
    }

    /// <summary>
    /// Generates a Sudoku puzzle with the specified number of clues.
    /// </summary>
    /// <param name="clues">The number of clues (filled cells) to include in the generated puzzle.</param>
    /// <returns>A <see cref="Board"/> object representing the generated Sudoku puzzle. The board will contain the specified
    /// number of clues, with all other cells empty.</returns>
    public static Board GeneratePuzzle(int clues)
    {
        Board board = new Board();
        Board solvedBoard;

        while (!TrySolve(board, out solvedBoard)) { }

        Random random = new Random();
        int cordX, cordY;

        for (int i = 0; i < clues; i++)
        {
            do
            {
                cordY = random.Next(9);
                cordX = random.Next(9);
            } while (solvedBoard.field[cordY, cordX].value == Constants.EMPTY_CELL);

            board.field[cordY, cordX].value = solvedBoard.field[cordY, cordX].value;
            board.field[cordY, cordX].isLocked = true;
            solvedBoard.field[cordY, cordX].value = Constants.EMPTY_CELL;
        }

        board.UpdateCandidates();

        return board;
    }

    /// <summary>
    /// Attempts to solve the specified Sudoku board.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> instance to solve.</param>
    /// <param name="solvedBoard">When this method returns, contains the solved <see cref="Board"/>, or a cloned copy of the input board if solving failed.</param>
    /// <param name="attempts">The maximum number of solving attempts. Default is 1000. Must be greater than 0.</param>
    /// <returns><c>true</c> if the board was successfully solved; otherwise, <c>false</c>.</returns>
    public static bool TrySolve(Board board, out Board solvedBoard, int attempts = 1000)
    {
        solvedBoard = board.Clone();

        if (attempts < 1)
            throw new ArgumentException("Attempts cannot be smaller than 1");

        if (!board.IsSudokuValid())
            throw new Exception("The provided board is not valid and cannot be solved.");

        Random random = new Random();
        Board tmpBoard;
        bool canContinue;
        bool isSolved;

        for (int attempt = 0; attempt < attempts; attempt++)
        {
            tmpBoard = board.Clone();
            canContinue = true;
            isSolved = false;

            do
            {
                foreach (Cell cell in tmpBoard.field)
                {
                    if (cell.value == Constants.EMPTY_CELL && cell.candidates.Count == 0)
                        canContinue = false;
                }

                if (!canContinue)
                    break;

                foreach (Cell cell in tmpBoard.field)
                {
                    if (cell.value == Constants.EMPTY_CELL && cell.candidates.Count == FindSmallestCandidateCount(tmpBoard))
                    {
                        cell.value = cell.candidates[random.Next(cell.candidates.Count)];
                        tmpBoard.UpdateCandidates();
                        break;
                    }
                }

                isSolved = tmpBoard.IsSudokuSolved();
            } while (!isSolved);

            if (isSolved)
            {
                solvedBoard = tmpBoard;
                return true;
            }
        }

        return false;
    }

    private static int FindSmallestCandidateCount(Board board)
    {
        int smallest = 9;
        foreach (Cell cell in board.field)
        {
            if (cell.candidates.Count < smallest && cell.value == Constants.EMPTY_CELL)
                smallest = cell.candidates.Count;
        }
        return smallest;
    }
}
