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
    /// <returns>A <see cref="Board"/> object representing the generated Sudoku puzzle. The board will contain the specified
    /// number of clues, with all other cells empty.</returns>
    public static Board GeneratePuzzle(int clues)
    {
        Board board = new Board();
        Board solvedBoard;

        while (!Solve(board, out solvedBoard)) { }

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
    /// Attempts to solve the given board and returns a value indicating whether the operation was successful. (Uses 10 attempts by default)
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
        solvedBoard = board;

        if (attempts < 1)
            throw new Exception("Attempts cannot be smaller than 1");

        if (!board.IsSudokuValid())
            throw new Exception("The provided board is not valid and cannot be solved.");

        for (int i = 0; i < attempts; i++)
        {
            if (Solve(board, out solvedBoard))
                return true;
        }

        return false;
    }

    private static bool Solve(Board board, out Board solvedBoard)
    {
        Random random = new Random();
        Board tmpBoard = board.Clone();
        solvedBoard = board;

        tmpBoard.UpdateCandidates();

        do
        {
            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.candidates.Count == 0)
                    return false;
            }

            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.candidates.Count == FindSmallestCandidateCount(tmpBoard))
                {
                    cell.value = cell.candidates[random.Next(cell.candidates.Count)];
                    tmpBoard.UpdateCandidates();
                    break;
                }
            }
        } while (!tmpBoard.IsSudokuSolved());

        solvedBoard = tmpBoard;
        return true;
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
