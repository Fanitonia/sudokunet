using System.Text.Json;

namespace SudokuNet;

public class Board
{
    internal Cell[,] field = new Cell[9, 9];

    public int EmptyCellCount { 
        get { return GetNumberOfEmptyCells(field); } 
    }

    public Board()
    {
        Sudoku.InitializeBoard(this);
    }

    public Board Clone()
    {
        string json = JsonSerializer.Serialize(this);
        return JsonSerializer.Deserialize<Board>(json)!;
    }

    public bool SetCell(int cordX, int cordY, int value)
    {
        if (value > 9 || value < 0)
            throw new Exception("Value is invalid (it must be between 1-9)");

        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (field[cordY, cordX].isLocked)
        {
            field[cordY, cordX].value = value;
            return true;
        }
        else
            return false;
    }

    public int GetCell(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        return field[cordY, cordX].value;
    }

    public bool DeleteCellValue(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        if (field[cordY, cordX].isLocked)
        {
            field[cordY, cordX].value = 0;
            return true;
        }
        else
            return false;
    }

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

    public void Lock(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = true;
    }

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

    public void Unlock(int cordX, int cordY)
    {
        if (!Helper.IsCordValid(cordX, cordY))
            throw new Exception("Coordinates are invalid");

        field[cordY, cordX].isLocked = false;
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




