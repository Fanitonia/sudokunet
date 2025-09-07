namespace SudokuNet;

public class Board
{
    internal Cell[,] field = new Cell[9, 9];

    public int EmptyCellCount
    {
        get { return GetNumberOfEmptyCells(field); }
    }

    public Board()
    {
        Sudoku.InitializeBoard(this);
    }

    /// <summary>
    /// Attempts to set the value of a cell at the specified coordinates.
    /// </summary>
    /// <param name="value">The value to set in the cell. Must be between 1 and 9, inclusive.</param>
    /// <returns><see langword="true"/> if the value was successfully set; otherwise, <see langword="false"/> if the cell is
    /// locked.</returns>
    public bool SetCell(int cordX, int cordY, int value)
    {
        if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 1-9)");

        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (!field[cordY, cordX].isLocked)
        {
            field[cordY, cordX].value = value;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Retrieves the value of the cell at the specified coordinates.
    /// </summary>
    /// <returns>The value of the cell at the specified coordinates.</returns>
    public int GetCell(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return field[cordY, cordX].value;
    }

    /// <summary>
    /// Deletes the value of a cell at the specified coordinates if the cell is not locked.
    /// </summary>
    /// <returns><see langword="true"/> if the cell value was successfully deleted; otherwise, <see langword="false"/> if the
    /// cell is locked.</returns>
    public bool DeleteCellValue(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (!field[cordY, cordX].isLocked)
        {
            field[cordY, cordX].value = 0;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Locks all non-empty cells in the board, preventing further modifications to their values.
    /// </summary>
    public void Lock()
    {
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                if (field[cordY, cordX].value != Constants.EMPTY_CELL)
                {
                    field[cordY, cordX].isLocked = true;
                }
            }
        }
    }

    /// <summary>
    /// Locks the specified cell at the given coordinates.
    /// </summary>
    public void Lock(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = true;
    }

    /// <summary>
    /// Unlocks all cells in the board by setting their locked state to <see langword="false"/>.
    /// </summary>
    public void Unlock()
    {
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                field[cordY, cordX].isLocked = false;
            }
        }
    }

    /// <summary>
    /// Unlocks the specified cell at the given coordinates.
    /// </summary>
    public void Unlock(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = false;
    }

    /// <summary>
    /// Loads a Sudoku board from a string representation.
    /// </summary>
    /// <remarks>This method initializes the Sudoku board based on the provided string. Each character in the
    /// string corresponds to a cell in the board, read row by row from top to bottom and left to right. A value of 0
    /// indicates an empty cell.</remarks>
    public void LoadBoardFrom(string boardString)
    {
        if (boardString.Length != 81)
            throw new Exception("Board string is invalid. It's lenght must be 81");

        Sudoku.InitializeBoard(this);
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                char c = boardString[cordY * 9 + cordX];
                int value = (int)char.GetNumericValue(c);

                if (value < 0 || value > 9)
                    throw new Exception("Board string is invalid. It must contain only numbers between 0-9");
                field[cordY, cordX].value = value;
            }
        }
    }

    /// <summary>
    /// Determines whether a specified value can be placed at the given position on the board without violating Sudoku
    /// rules.
    /// </summary>
    /// <param name="value">The value to validate, which must be between 1 and 9.</param>
    /// <returns><see langword="true"/> if the specified value can be placed at the given position without conflicting with the
    /// rules of Sudoku; otherwise, <see langword="false"/>.</returns>
    public bool IsPositionSuitable(int cordX, int cordY, int value)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");
        else if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 1-9)");

        // checks the column and the row of the position
        for (int i = 0; i < 9; i++)
        {
            if (this.field[cordY, i].value == value)
                return false;

            else if (this.field[i, cordX].value == value)
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
                if (this.field[rowStart + i, columnStart + j].value == value)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the current state of the Sudoku board satisfies the rules of a solved puzzle.
    /// </summary>
    /// <returns><see langword="true"/> if the Sudoku board is solved and adheres to all Sudoku rules; otherwise, <see
    /// langword="false"/>.</returns>
    public bool IsSudokuSolved()
    {
        int tmpHolder;
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                tmpHolder = this.field[cordY, cordX].value;
                this.field[cordY, cordX].value = Constants.EMPTY_CELL;
                if (this.IsPositionSuitable(cordX, cordY, tmpHolder) && tmpHolder != Constants.EMPTY_CELL)
                    this.field[cordY, cordX].value = tmpHolder;
                else
                {
                    this.field[cordY, cordX].value = tmpHolder;
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Determines whether the current state of the Sudoku board is valid.
    /// </summary>
    /// <remarks>A Sudoku board is considered valid if all cells adhere to the rules of Sudoku, meaning no
    /// duplicate values exist in any row, column, or 3x3 subgrid. Empty cells are allowed and do not affect
    /// validity.</remarks>
    /// <returns><see langword="true"/> if the Sudoku board is in a valid state; otherwise, <see langword="false"/>.</returns>
    public bool IsSudokuValid()
    {
        int tmpHolder;
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                tmpHolder = this.field[cordY, cordX].value;
                this.field[cordY, cordX].value = Constants.EMPTY_CELL;
                if (tmpHolder == Constants.EMPTY_CELL || IsPositionSuitable(cordX, cordY, tmpHolder))
                    this.field[cordY, cordX].value = tmpHolder;
                else
                {
                    this.field[cordY, cordX].value = tmpHolder;
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified cell on the board is locked.
    /// </summary>
    /// <returns><see langword="true"/> if the cell at the specified coordinates is locked; otherwise, <see langword="false"/>.</returns>
    public bool IsCellLocked(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return this.field[cordY, cordX].isLocked;
    }

    private static int GetNumberOfEmptyCells(Cell[,] field)
    {
        int emptyCell = 0;

        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                if (field[cordY, cordX].value == 0)
                {
                    emptyCell++;
                }
            }
        }

        return emptyCell;
    }
}