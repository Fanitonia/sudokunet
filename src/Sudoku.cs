using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class Sudoku
    {
        private Cell[,] field = new Cell[9, 9];
        private Cell[,] solvedField = new Cell[9, 9];
        private readonly ConsoleColor usersColor = Console.ForegroundColor;
        private Stopwatch timer = new Stopwatch();

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
        // copies field to new solvedField to solve sudoku without changing main field
        private void CopyBoard()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                    solvedField[y, x] = new Cell(field[y, x].value, field[y, x].pNumbers);
            }
        }

        public void StartGame()
        {
            GenerateField();
            PrintBoard(field);
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
                    } while (field[y, x].value != 0);

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

        private void PrintBoard(Cell[,] board)
        {
            int fieldX = 0, fieldY = 0;

            Console.WriteLine("+---+---+---+");
            // rows of board
            for (int y = 2; y <= 12; y++)
            {
                // columns of board
                for (int x = 1; x <= 13; x++)
                {
                    if (x % 4 == 1)
                    {
                        if (y % 4 != 1)
                            Console.Write("|");
                        else
                            Console.Write("+");
                        continue;
                    }

                    if (y % 4 == 1)
                    {
                        Console.Write("-");
                        continue;
                    }

                    if (board[fieldY, fieldX].value != 0)
                        Console.Write(board[fieldY, fieldX].value);
                    else
                        Console.Write(" ");

                    fieldX++;
                }

                if (y % 4 != 1)
                    fieldY++;

                fieldX = 0;
                Console.WriteLine();
            }

            Console.WriteLine("+---+---+---+");
        }

        private bool IsPositionSuitable(int y, int x, int number, Cell[,] board)
        {
            // checks the column and the row of the position
            for (int i = 0; i < 9; i++)
            {
                if (board[y, i].value == number)
                    return false;
                    
                else if (board[i, x].value == number)
                    return false;
            }
            
            // finding top left position of the 3x3 are position's part of
            int rowStart = y - (y % 3);
            int columnStart = x - (x % 3);

            // checks the 3x3 area position's part of
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[rowStart + i, columnStart + j].value == number)
                        return false;
                }
            }
            return true;
        }

        // updates specific cell's potentials list
        private void UpdateCellPotentials(int y, int x, Cell[,] board)
        {
            List<int> tmpNumbers = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                if (IsPositionSuitable(y, x, i, board))
                    tmpNumbers.Add(i);
            }
            board[y,x].pNumbers.Clear();
            board[y,x].pNumbers.AddRange(tmpNumbers);
        }

        // updates all cells in field
        private void UpdateAllPotentials(Cell[,] board)
        {
            for(int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                    UpdateCellPotentials(y, x, board);
            }
        }

        // checks if generated sudoku table can be solved
        private bool Solve()
        {
            Random random = new Random();
            CopyBoard();
            do
            {
                foreach (Cell cell in solvedField)
                {
                    if (cell.pNumbers.Count == 0 && cell.value == 0)
                        return false;

                    if (cell.pNumbers.Count == FindSmallestPotential(solvedField) && cell.value == 0)
                        cell.value = cell.pNumbers[random.Next(cell.pNumbers.Count)];

                    UpdateAllPotentials(solvedField);
                }
            } while (!CheckForWin(solvedField));

            return true;
        }

        
        private int FindSmallestPotential(Cell[,] board)
        {
            int smallest = 9;
            foreach (Cell cell in board)
            {
                if(cell.pNumbers.Count < smallest && cell.value == 0)
                    smallest = cell.pNumbers.Count;
            }

            return smallest;
        }

        private bool CheckForWin(Cell[,] board)
        {
            int tmpHolder;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    tmpHolder = board[y, x].value;
                    board[y, x].value = 0;
                    if (IsPositionSuitable(y, x, tmpHolder, board) && tmpHolder != 0)
                        board[y, x].value = tmpHolder;
                    else
                        return false;
                }
            }
            return true;
        }
    }
}