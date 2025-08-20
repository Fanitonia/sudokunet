namespace SudokuNet
{
    internal class Cell
    {
        // Value of the cell (0 means empty)
        public int value = 0;

        // List of possible numbers that this cell can hold (1-9)
        public List<int> potentialValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Indicates whether the cell's value can be changed (true for editable, false for fixed cells)
        public bool canChange = true;


        // Default constructor
        public Cell() { }

        // Constructor with value
        public Cell(int value, List<int> potentialValues, bool canChange)
        {
            this.value = value;
            this.potentialValues.Clear();
            this.potentialValues.AddRange(potentialValues);
            this.canChange = canChange;
        }
    }
}
