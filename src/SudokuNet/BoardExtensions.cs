namespace SudokuNet;

/// <summary>
/// Provides extension methods for the <see cref="Board"/> class.
/// </summary>
public static class BoardExtensions
{
    /// <summary>
    /// Displays the current state of the board in a formatted grid layout on the console.
    /// </summary>
    /// <remarks>The method renders the board as a 9x9 grid, with rows and columns separated by lines and
    /// cells  represented by their values. Locked cells are displayed in red, while empty cells are shown as blank
    /// spaces.</remarks>
    /// <param name="board"></param>
    public static void PrintToConsole(this Board board)
    {
        ConsoleColor userForeColor = Console.ForegroundColor;
        int cordX = 0, cordY = 0;

        Console.WriteLine("┌───────┬───────┬───────┐");
        for (int y = 2; y <= 12; y++)
        {
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
                    if (board.field[cordY, cordX].value != Constants.EMPTY_CELL)
                    {
                        if (board.field[cordY, cordX].isLocked)
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write(board.field[cordY, cordX].value);
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

    /// <summary>
    /// Creates a deep copy of the specified <see cref="Board"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Board"/> instance that is a deep copy of the specified board.</returns>
    public static Board Clone(this Board board)
    {
        Board newBoard = new Board();

        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                newBoard.field[cordY, cordX] = new Cell(
                    board.field[cordY, cordX].value,
                    board.field[cordY, cordX].isLocked);
            }
        }

        newBoard.UpdateCandidates();

        return newBoard;
    }
}
