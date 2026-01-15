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
    /// <param name="board">A string representation of the Sudoku board (must be 81 characters long, containing digits 0-9).</param>
    public Board(string board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with provided one-dimensional integer array.
    /// </summary>
    /// <param name="board">A one-dimensional integer array representing the Sudoku board (must contain 81 elements with values 0-9).</param>
    public Board(int[] board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with provided two-dimensional integer array.
    /// </summary>
    /// <param name="board">A two-dimensional integer array representing the Sudoku board (must be 9x9 with values 0-9).</param>
    public Board(int[][] board)
    {
        LoadBoardFrom(board);
    }

    /// <summary>
    /// Attempts to set the value of a cell at the specified coordinates.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
    /// <param name="value">The value to set in the cell (0-9).</param>
    /// <param name="updateCandidates">If <see langword="true"/>, updates candidate values for all cells after setting; otherwise, candidates remain unchanged. Default is <see langword="true"/>.</param>
    /// <returns><see langword="true"/> if the value was successfully set; otherwise, <see langword="false"/> if the cell is locked.</returns>
    public bool SetCell(int cordX, int cordY, int value, bool updateCandidates = true)
    {
        if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 0-9)");

        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (field[cordY, cordX].isLocked)
            return false;

        field[cordY, cordX].value = value;

        if (updateCandidates)
            UpdateCandidates();

        return true;
    }

    /// <summary>
    /// Retrieves the value of the cell at the specified coordinates.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
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
        foreach (var cell in field)
        {
            if (cell.value != Constants.EMPTY_CELL)
                cell.isLocked = true;
        }
    }

    /// <summary>
    /// Locks the specified cell at the given coordinates.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
    public void Lock(int cordX, int cordY)
    {
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = true;
    }

    /// <summary>
    /// Unlocks all cells in the board, allowing modifications to their values.
    /// </summary>
    public void Unlock()
    {
        foreach (var cell in field)
        {
            cell.isLocked = false;
        }
    }

    /// <summary>
    /// Unlocks the specified cell at the given coordinates. Allows modifications to its value.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
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
    /// <param name="boardString">The string representation of the Sudoku board (must be 81 characters long, containing digits 0-9).</param>
    /// <param name="updateCandidates">If <see langword="true"/>, updates candidate values after loading; otherwise, candidates are not updated. Default is <see langword="true"/>.</param>
    public void LoadBoardFrom(string boardString, bool updateCandidates = true)
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

        if (updateCandidates)
            UpdateCandidates();
    }

    /// <summary>
    /// Loads a Sudoku board from a one-dimensional array of integers.
    /// </summary>
    /// <remarks>This method initializes the Sudoku board based on the provided array. Each value in the
    /// array corresponds to a cell in the board, read row by row from top to bottom and left to right. A value of 0
    /// indicates an empty cell.</remarks>
    /// <param name="boardInt">A one-dimensional integer array representing the Sudoku board (must contain 81 elements with values 0-9).</param>
    /// <param name="updateCandidates">If <see langword="true"/>, updates candidate values after loading; otherwise, candidates are not updated. Default is <see langword="true"/>.</param>
    public void LoadBoardFrom(int[] boardInt, bool updateCandidates = true)
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

        if (updateCandidates)
            UpdateCandidates();
    }

    /// <summary>
    /// Loads a Sudoku board from a two-dimensional integer array.
    /// </summary>
    /// <remarks>This method initializes the Sudoku board based on the provided two-dimensional array. Each value in the
    /// array corresponds to a cell in the board. A value of 0 indicates an empty cell.</remarks>
    /// <param name="boardInt">A two-dimensional integer array representing the Sudoku board (must be 9x9 with values 0-9).</param>
    /// <param name="updateCandidates">If <see langword="true"/>, updates candidate values after loading; otherwise, candidates are not updated. Default is <see langword="true"/>.</param>
    public void LoadBoardFrom(int[][] boardInt, bool updateCandidates = true)
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

        if (updateCandidates)
            UpdateCandidates();
    }

    /// <summary>
    /// Determines whether a specified value can be placed at the given position on the board without violating Sudoku
    /// rules.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
    /// <param name="value">The value to check (0-9).</param>
    /// <returns><see langword="true"/> if the specified value can be placed at the given position without conflicting with the
    /// rules of Sudoku; otherwise, <see langword="false"/>.</returns>
    public bool IsPositionSuitable(int cordX, int cordY, int value)
    {
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 1-9)");

        // checks the column and the row of the position
        for (int i = 0; i < 9; i++)
        {
            if (field[cordY, i].value == value)
                return false;

            else if (field[i, cordX].value == value)
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
                if (field[rowStart + i, columnStart + j].value == value)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the current state of the Sudoku board solved.
    /// </summary>
    /// <returns><see langword="true"/> if the Sudoku board is solved; otherwise, <see langword="false"/>.</returns>
    public bool IsSudokuSolved()
    {
        int tmpHolder;
        for (int cordY = 0; cordY < 9; cordY++)
        {
            for (int cordX = 0; cordX < 9; cordX++)
            {
                tmpHolder = field[cordY, cordX].value;
                field[cordY, cordX].value = Constants.EMPTY_CELL;

                if (tmpHolder != Constants.EMPTY_CELL && IsPositionSuitable(cordX, cordY, tmpHolder))
                    field[cordY, cordX].value = tmpHolder;
                else
                {
                    field[cordY, cordX].value = tmpHolder;
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
                tmpHolder = field[cordY, cordX].value;
                field[cordY, cordX].value = Constants.EMPTY_CELL;

                if (tmpHolder == Constants.EMPTY_CELL || IsPositionSuitable(cordX, cordY, tmpHolder))
                    field[cordY, cordX].value = tmpHolder;
                else
                {
                    field[cordY, cordX].value = tmpHolder;
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Determines whether the specified cell on the board is locked.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
    /// <returns><see langword="true"/> if the cell at the specified coordinates is locked; otherwise, <see langword="false"/>.</returns>
    public bool IsCellLocked(int cordX, int cordY)
    {
        if (!Utils.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return field[cordY, cordX].isLocked;
    }

    /// <summary>
    /// Retrieves the list of candidate values for the specified coordinates.
    /// </summary>
    /// <param name="cordX">The X coordinate of the cell (0-8).</param>
    /// <param name="cordY">The Y coordinate of the cell (0-8).</param>
    /// <returns>An array of integers representing the candidate values for the cell at the specified coordinates.</returns>
    public int[] GetCandidates(int cordX, int cordY)
    {
        return field[cordY, cordX].candidates.ToArray();
    }

    /// <summary>
    /// Updates the candidate values for all cells.
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
            if (IsPositionSuitable(cordX, cordY, testValue))
                tmpNumbers.Add(testValue);
        }

        field[cordY, cordX].candidates.Clear();
        field[cordY, cordX].candidates.AddRange(tmpNumbers);
    }

    private int GetNumberOfEmptyCells()
    {
        int emptyCellCount = 0;

        foreach (var cell in field)
        {
            if (cell.value == Constants.EMPTY_CELL)
                emptyCellCount++;
        }

        return emptyCellCount;
    }
}