namespace SudokuNet
{
    internal class Cell
    {
        public int value;

        public List<int> potentialValues = new List<int>();

        // Indicates whether the cell's value can be changed.
        public bool canChange;


        public Cell() { 
            this.value = 0;
            this.potentialValues = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            this.canChange = true;
        }

        public Cell(int value, List<int> potentialValues, bool canChange)
        {
            this.value = value;
            this.potentialValues.AddRange(potentialValues);
            this.canChange = canChange;
        }
    }
}
