namespace SudokuNet;

internal class Cell
{
    public int value;
    public List<int> potentialValues = new List<int>();
    public bool isLocked;

    public Cell()
    {
        this.value = 0;
        this.potentialValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        this.isLocked = true;
    }

    //TODO: potentialValues can be numbers other than 1-9
    public Cell(int value, List<int> potentialValues, bool isLocked)
    {
        this.value = value;
        this.potentialValues.AddRange(potentialValues);
        this.isLocked = isLocked;
    }
}

