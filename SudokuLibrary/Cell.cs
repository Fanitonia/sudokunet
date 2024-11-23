namespace SudokuLibrary
{
    internal class Cell
    {
        public int value = 0;                                       // value of the cell (0 means empty)
        public bool canChange = true;                               // 
        public List<int> pNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];    // potential numbers that cell can have (this is for solving and generation algorithms) 

        public Cell() { }

        public Cell(int value, List<int> pNumbers, bool canChange)
        {
            this.value = value;
            this.pNumbers.Clear();
            this.pNumbers.AddRange(pNumbers);
            this.canChange = canChange;
        }
    }
}
