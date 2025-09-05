namespace SudokuNet;

public static class BoardExtensions
{
    public static bool IsPositionSuitable(this Board board, int cordX, int cordY, int value)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");
        else if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 1-9)");

        // checks the column and the row of the position
        for (int i = 0; i < 9; i++)
        {
            if (board.field[cordY, i].value == value)
                return false;

            else if (board.field[i, cordX].value == value)
                return false;
        }

        // finding top left position of the 3x3 are position's part of
        int rowStart = cordY - cordY % 3;
        int columnStart = cordX - cordX % 3;

        // checks the 3x3 area position's part of
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board.field[rowStart + i, columnStart + j].value == value)
                    return false;
            }
        }

        return true;
    }

    public static bool IsSudokuSolved(this Board board)
    {
        int tmpHolder;
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                tmpHolder = board.field[cordY, cordX].value;
                board.field[cordY, cordX].value = Constants.EMPTY_CELL;
                if (board.IsPositionSuitable(cordX, cordY, tmpHolder) && tmpHolder != Constants.EMPTY_CELL)
                    board.field[cordY, cordX].value = tmpHolder;
                else
                {
                    board.field[cordY, cordX].value = tmpHolder;
                    return false;
                }
            }
        }
        return true;
    }

    public static bool IsSudokuValid(this Board board)
    {
        int tmpHolder;
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                tmpHolder = board.field[cordY, cordX].value;
                board.field[cordY, cordX].value = Constants.EMPTY_CELL;
                if (tmpHolder == Constants.EMPTY_CELL || IsPositionSuitable(board, cordX, cordY, tmpHolder))
                    board.field[cordY, cordX].value = tmpHolder;
                else
                {
                    board.field[cordY, cordX].value = tmpHolder;
                    return false;
                }
            }
        }

        return true;
    }

    public static bool IsCellLocked(this Board board, int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return board.field[cordY, cordX].isLocked;
    }

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
}
