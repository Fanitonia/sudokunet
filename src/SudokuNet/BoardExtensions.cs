namespace SudokuNet;

/// <summary>
/// Provides extension methods for the <see cref="Board"/> class.
/// </summary>
public static class BoardExtensions
{
    /// <summary>
    /// Displays the current state of the board in a formatted grid layout on the console.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> instance to display.</param>
    public static void PrintToConsole(this Board board)
    {
        Console.WriteLine("┌───────┬───────┬───────┐");

        for (int cordY = 0; cordY < 9; cordY++)
        {
            Console.Write("│ ");

            for (int cordX = 0; cordX < 9; cordX++)
            {
                int value = board.GetCell(cordX, cordY);

                Console.Write(value == 0 ? "." : value.ToString());
                Console.Write(" ");

                if ((cordX + 1) % 3 == 0 && cordX < 8)
                    Console.Write("│ ");
            }

            Console.WriteLine("│");

            if ((cordY + 1) % 3 == 0 && cordY < 8)
                Console.WriteLine("├───────┼───────┼───────┤");
        }

        Console.WriteLine("└───────┴───────┴───────┘");
    }

    /// <summary>
    /// Creates a deep copy of the specified <see cref="Board"/> instance.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> instance to clone.</param>
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
