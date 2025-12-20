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

    internal Cell(int value, List<int> potentialValues, bool isLocked)
    {
        bool isPotentialValuesValid = potentialValues.All(value => value >= 1 && value <= 9);
        bool isValueValid = value >= 0 && value <= 9;

        if (!isPotentialValuesValid)
            throw new ArgumentException("Potential values must be between 1 and 9.");
        if (!isValueValid)
            throw new ArgumentException("Cell value must be between 0 and 9.");


        this.value = value;
        this.candidates.AddRange(potentialValues);
        this.isLocked = isLocked;
    }
}

