namespace SudokuNet;

/// <summary>
/// Represents a 9x9 Sudoku board, providing methods to manipulate and validate its state.
/// </summary>
public class Board
{
    internal Cell[,] field = new Cell[9, 9];

    /// <summary>
    /// Gets the number of empty cells in the current field.
    /// </summary>
    public int EmptyCellCount
    {
        get { return GetNumberOfEmptyCells(); }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class.
    /// </summary>
    public Board()
    {
        Sudoku.InitializeBoard(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with provided string.
    /// </summary>
    public Board(string board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with provided one-dimensional integer array.
    /// </summary>
    public Board(int[] board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with provided two-dimensional integer array.
    /// </summary>
    public Board(int[][] board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Attempts to set the value of a cell at the specified coordinates.
    /// </summary>
    /// <returns><see langword="true"/> if the value was successfully set; otherwise, <see langword="false"/> if the cell is
    /// locked.</returns>
    public bool SetCell(int cordX, int cordY, int value)
    {
        if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 0-9)");

        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (field[cordY, cordX].isLocked)
            return false;

        field[cordY, cordX].value = value;
        return true;
    }

    /// <summary>
    /// Retrieves the value of the cell at the specified coordinates.
    /// </summary>
    /// <returns>The value of the cell at the specified coordinates.</returns>
    public int GetCell(int cordX, int cordY)
    {
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return field[cordY, cordX].value;
    }

    /// <summary>
    /// Locks all non-empty cells in the board, preventing further modifications to their values.
    /// </summary>
    public void Lock()
    {
        foreach(var cell in field)
        {
            if (cell.value != Constants.EMPTY_CELL)
                cell.isLocked = true;
        }
    }

    /// <summary>
    /// Locks the specified cell at the given coordinates.
    /// </summary>
    public void Lock(int cordX, int cordY)
    {
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = true;
    }

    /// <summary>
    /// Unlocks all cells in the board by setting their locked state to <see langword="false"/>.
    /// </summary>
    public void Unlock()
    {
        foreach (var cell in field)
        {
            cell.isLocked = false;
        }
    }

    /// <summary>
    /// Unlocks the specified cell at the given coordinates.
    /// </summary>
    public void Unlock(int cordX, int cordY)
    {
        if (!Utils.IsCordValid(cordX, cordY))
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
    /// Loads a Sudoku board from a one-dimensional array of integers.
    /// </summary>
    /// <remarks>This method initializes the Sudoku board based on the provided array. Each value in the
    /// array corresponds to a cell in the board, read row by row from top to bottom and left to right. A value of 0
    /// indicates an empty cell.</remarks>
    public void LoadBoardFrom(int[] boardInt)
    {
        if (boardInt.Length != 81)
            throw new Exception("Board array is invalid. It's lenght must be 81");

        Sudoku.InitializeBoard(this);

        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                int value = boardInt[cordY * 9 + cordX];

                if (value < 0 || value > 9)
                    throw new Exception("Board array is invalid. It must contain only numbers between 0-9");

                field[cordY, cordX].value = value;
            }
        }
    }

    /// <summary>
    /// Loads a Sudoku board from a two-dimensional integer array.
    /// </summary>
    /// <remarks>This method initializes the Sudoku board based on the provided two-dimensional array. Each value in the
    /// array corresponds to a cell in the board. A value of 0 indicates an empty cell.</remarks>
    public void LoadBoardFrom(int[][] boardInt)
    {
        if (boardInt.Length != 9)
            throw new Exception("Board array is invalid. It's should have 9 rows");

        foreach (var boardIntArray in boardInt)
        {
            if (boardIntArray.Length != 9)
                throw new Exception("Board array is invalid. It's should have 9 columns");
        }

        Sudoku.InitializeBoard(this);

        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                int value = boardInt[cordY][cordX];

                if (value < 0 || value > 9)
                    throw new Exception("Board array is invalid. It must contain only numbers between 0-9");

                field[cordY, cordX].value = value;
            }
        }
    }

    /// <summary>
    /// Determines whether a specified value can be placed at the given position on the board without violating Sudoku
    /// rules.
    /// </summary>
    /// <returns><see langword="true"/> if the specified value can be placed at the given position without conflicting with the
    /// rules of Sudoku; otherwise, <see langword="false"/>.</returns>
    public bool IsPositionSuitable(int cordX, int cordY, int value)
    {
        if (!Utils.IsCordValid(cordX, cordY))
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
    /// Determines whether the current state of the Sudoku board solved.
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
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return this.field[cordY, cordX].isLocked;
    }

    /// <summary>
    /// Retrieves the list of candidate values for the specified coordinates.
    /// </summary>
    /// <param name="cordX">The X-coordinate of the target cell.</param>
    /// <param name="cordY">The Y-coordinate of the target cell.</param>
    /// <returns>An array of integers representing the candidate values for the cell at the specified coordinates.</returns>
    public int[] GetCandidates(int cordX, int cordY)
    {
        return field[cordY, cordX].candidates.ToArray();
    }

    /// <summary>
    /// Updates the potential values for all cells.
    /// </summary>
    /// <remarks>This method iterates through each cell on the board and recalculates its potential values 
    /// based on the current state of the board.</remarks>
    public void UpdateCandidates()
    {
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
                UpdateCellCandidates(cordX, cordY);
        }
    }

    private void UpdateCellCandidates(int cordX, int cordY)
    {
        List<int> tmpNumbers = [];
        for (int testValue = 1; testValue <= 9; testValue++)
        {
            if (this.IsPositionSuitable(cordX, cordY, testValue))
                tmpNumbers.Add(testValue);
        }
        field[cordY, cordX].candidates.Clear();
        field[cordY, cordX].candidates.AddRange(tmpNumbers);
    }

    private int GetNumberOfEmptyCells()
    {
        int emptyCell = 0;

        foreach(var cell in field)
        {
            if (cell.value == Constants.EMPTY_CELL)
                emptyCell++;
        }

        return emptyCell;
    }
}