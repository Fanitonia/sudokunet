using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public partial class Sudoku
    {
        private void CreateEmptyField()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    field[i, j] = new Cell();
            }
        }
        private void GenerateField()
        {
            Console.Write("Generating...");
            Random random = new Random();
            int x, y;
            do
            {
                CreateEmptyField();
                for (int i = 0; i < 30; i++)
                {
                    do
                    {
                        y = random.Next(9);
                        x = random.Next(9);
                    } while (field[y, x].value != EMPTY_CELL);

                    try
                    {
                        field[y, x].value = field[y, x].pNumbers[random.Next(field[y, x].pNumbers.Count)];
                        field[y, x].canChange = false;
                    }
                    catch (Exception)
                    {
                        break;
                    }

                    UpdateAllPotentials(field);
                }
            } while (!Solve());
        }
    }
}
