using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public partial class Sudoku
    {
        private Cell[,] field = new Cell[9, 9];
        private Cell[,] solvedField = new Cell[9, 9];
        private Stopwatch timer = new Stopwatch();
        private readonly ConsoleColor usersColor = Console.ForegroundColor;
        private const int EMPTY_CELL = 0;

        private class Cell
        {
            public int value = 0;
            public List<int> pNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

            public Cell() { }
            public Cell(int value, List<int> pNumbers)
            {
                this.value = value;
                this.pNumbers.Clear();
                this.pNumbers.AddRange(pNumbers);
            }
        }

        public void StartGame()
        {
            GenerateField();
            PrintBoard(field);
            PrintBoard(solvedField);
            Console.WriteLine(timer.ElapsedMilliseconds + " ms");
        }

        private void GenerateField()
        {
            Random random = new Random();
            int x, y;
            do
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                        field[i, j] = new Cell();
                }

                for (int i = 0; i < 25; i++)
                {
                    do
                    {
                        y = random.Next(9);
                        x = random.Next(9);
                    } while (field[y, x].value != EMPTY_CELL);

                    try
                    {
                        field[y, x].value = field[y, x].pNumbers[random.Next(field[y, x].pNumbers.Count)];
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    UpdateAllPotentials(field);
                }
            } while (!Solve());
        }
        // i can't explain this
        private void PrintBoard(Cell[,] board)
        {
            int fieldX = 0, fieldY = 0;

            Console.WriteLine("┌───────┬───────┬───────┐");
            // rows of board
            for (int y = 2; y <= 12; y++)
            {
                // columns of board
                for (int x = 1; x <= 25; x++)
                {
                    if (x % 8 == 1)
                    {
                        if (y == 5 || y == 9 || y == 14)
                            Console.Write("┼");
                        else
                            Console.Write("│");
                        continue;
                    }

                    if (y % 4 == 1)
                    {
                        Console.Write("─");
                        continue;
                    }

                    if(x % 2 == 0)
                    {
                        Console.Write(" ");
                        continue;
                    }

                    if (x % 2 == 1)
                    {
                        if (board[fieldY, fieldX].value != EMPTY_CELL)
                            Console.Write(board[fieldY, fieldX].value);
                        else
                            Console.Write(" ");

                        fieldX++;
                    }
                }

                if (y % 4 != 1)
                    fieldY++;

                fieldX = 0;
                Console.WriteLine();
            }
            Console.WriteLine("└───────┴───────┴───────┘");
        }
    }
}