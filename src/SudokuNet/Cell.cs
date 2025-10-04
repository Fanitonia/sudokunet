namespace SudokuNet;

internal class Cell
{
    internal int value;
    internal List<int> candidates = [];
    internal bool isLocked;

    internal Cell()
    {
        this.value = 0;
        this.candidates = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        this.isLocked = false;
    }

    //TODO: potentialValues can be numbers other than 1-9
    internal Cell(int value, List<int> potentialValues, bool isLocked)
    {
        this.value = value;
        this.candidates.AddRange(potentialValues);
        this.isLocked = isLocked;
    }
}

