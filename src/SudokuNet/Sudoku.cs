using System.Diagnostics;

namespace SudokuNet;

public static class Sudoku
{
    private static int step = 0;

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

    public static Board GeneratePuzzle(int clues)
    {
        Board tmpboard = new Board();
        Board board = new Board();

        while (!Solve(tmpboard, out board)) { }

        int cordX, cordY;
        Random random = new Random();

        for (int i = 0; i < clues; i++)
        {
            do
            {
                cordY = random.Next(9);
                cordX = random.Next(9);
            } while (board.field[cordY, cordX].value != Constants.EMPTY_CELL);

            board.field[cordY, cordX].value = board.field[cordY, cordX].value;
            board.field[cordY, cordX].isLocked = false;
            board.field[cordY, cordX].value = Constants.EMPTY_CELL;
        }

        return board;
    }
    public static bool TrySolve(Board board, out Board solvedBoard, int? attempts)
    {
        solvedBoard = default;

        if (attempts == null)
            attempts = 10;

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

        Stopwatch timer = new Stopwatch();
        step = 0;
        timer.Start();

        UpdateAllPotentials(tmpBoard);

        do
        {
            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.potentialValues.Count == 0)
                    return false;
                step++;
            }

            foreach (Cell cell in tmpBoard.field)
            {
                if (cell.value == Constants.EMPTY_CELL && cell.potentialValues.Count == FindSmallestPotential(tmpBoard))
                {
                    cell.value = cell.potentialValues[random.Next(cell.potentialValues.Count)];
                    UpdateAllPotentials(tmpBoard);
                    break;
                }
                step++;
            }
        } while (!tmpBoard.IsSudokuSolved());
        
        timer.Stop();
        solvedBoard = tmpBoard;
        return true;
    }

    private static void UpdateAllPotentials(Board board)
    {
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
                UpdateCellPotentials(cordX, cordY, board);
        }
    }

    private static void UpdateCellPotentials(int cordX, int cordY, Board board)
    {
        List<int> tmpNumbers = new List<int>();
        for (int testValue = 1; testValue <= 9; testValue++)
        {
            if (board.IsPositionSuitable(cordX, cordY, testValue))
                tmpNumbers.Add(testValue);
            step++;
        }
        board.field[cordY, cordX].potentialValues.Clear();
        board.field[cordY, cordX].potentialValues.AddRange(tmpNumbers);
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
