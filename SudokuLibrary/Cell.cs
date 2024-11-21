using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuLibrary
{
    public class Cell
    {
        public int value = 0;
        public bool canChange = true;
        public List<int> pNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        public int Value { get; set; }

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
