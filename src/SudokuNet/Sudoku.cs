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
    /// <param name="clues">The number of clues to include in the generated puzzle. Must be a non-negative integer.</param>
    /// <returns>A <see cref="Board"/> object representing the generated Sudoku puzzle. The board will contain the specified
    /// number of clues, with all other cells empty.</returns>
    public static Board GeneratePuzzle(int clues)
    {
        Board board = new Board();
        Board solvedBoard;

        while (!Solve(board, out solvedBoard)) { }

        int cordX, cordY;
        Random random = new Random();

        for (int i = 0; i < clues; i++)
        {
            do
            {
                cordY = random.Next(9);
                cordX = random.Next(9);
            } while (solvedBoard.field[cordY, cordX].value == Constants.EMPTY_CELL);

            board.field[cordY, cordX].value = solvedBoard.field[cordY, cordX].value;
            board.field[cordY, cordX].potentialValues = solvedBoard.field[cordY, cordX].potentialValues;
            board.field[cordY, cordX].isLocked = true;
            solvedBoard.field[cordY, cordX].value = Constants.EMPTY_CELL;
        }

        return board;
    }

    /// <summary>
    /// Attempts to solve the given board and returns a value indicating whether the operation was successful.
    /// </summary>
    /// <param name="solvedBoard">When this method returns, contains the solved board if the operation was successful; otherwise, contains the
    /// original board.</param>
    /// <returns><see langword="true"/> if the board was successfully solved; otherwise, <see langword="false"/>.</returns>
    public static bool TrySolve(Board board, out Board solvedBoard)
    {
        return TrySolve(board, out solvedBoard, 10);
    }

    /// <summary>
    /// Attempts to solve the given Sudoku board within the specified number of attempts.
    /// </summary>
    /// <param name="solvedBoard">When this method returns, contains the solved <see cref="Board"/> if the puzzle was successfully solved;
    /// otherwise, contains the default value of <see cref="Board"/>.</param>
    /// <param name="attempts">The maximum number of attempts to try solving the puzzle.</param>
    /// <returns><see langword="true"/> if the Sudoku puzzle was successfully solved within the specified number of attempts;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TrySolve(Board board, out Board solvedBoard, int attempts)
    {
        solvedBoard = default;

        if (attempts < 1)
            throw new Exception("Attempts cannot be smaller than 1");

        if (!board.IsSudokuValid())
            return false;

        for (int i = 0; i < attempts; i++)
        {
            if (Solve(board, out solvedBoard))
            {
                return true;
            }

        }

        return false;
    }

    private static bool Solve(Board board, out Board solvedBoard)
    {
        Random random = new Random();
        Board tmpBoard = board.Clone();
        solvedBoard = default;

        tmpBoard.UpdatePotentials();

        do
        {
            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.potentialValues.Count == 0)
                    return false;
            }

            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.potentialValues.Count == FindSmallestPotential(tmpBoard))
                {
                    cell.value = cell.potentialValues[random.Next(cell.potentialValues.Count)];
                    tmpBoard.UpdatePotentials();
                    break;
                }
            }
        } while (!tmpBoard.IsSudokuSolved());

        solvedBoard = tmpBoard;
        return true;
    }

    private static int FindSmallestPotential(Board board)
    {
        int smallest = 9;
        foreach (Cell cell in board.field)
        {
            if (cell.potentialValues.Count < smallest && cell.value == Constants.EMPTY_CELL)
                smallest = cell.potentialValues.Count;
        }
        return smallest;
    }
}
