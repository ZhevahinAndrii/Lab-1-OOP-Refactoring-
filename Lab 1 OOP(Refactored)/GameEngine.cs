﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace Lab_1_OOP_Refactored_
{
    public class GameEngine
    {
        public uint CurrentGeneration { get; private set; }
        private bool[,] field;
        private readonly int rows, columns;
        private Random random = new Random();
        public GameEngine(int rows,int columns,int density)
        {
            this.rows = rows;
            this.columns = columns;
            field = new bool[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }
        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {

                    int col = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;
                    bool isSelfChecking = col == x && row == y;
                    var haslife = field[col, row];
                    if (haslife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public void NextGeneration()
        {
            

            bool[,] newField = new bool[columns, rows];


            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var haslife = field[x, y];

                    if (!haslife && neighboursCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (haslife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = field[x, y];
                    }
                }
            }
            field = newField;
            CurrentGeneration++;
            
        }
        public bool[,] GetCurrentGeneration()
        {
            var result = new bool[columns, rows];
            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }
        public void AddCell(int x,int y)
        {
            UpdatedCell(x, y, true);
        }
        public void RemoveCell(int x,int y)
        {
            UpdatedCell(x, y, false);
        }
        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < columns && y < rows;
        }
       private void UpdatedCell(int x,int y,bool state)
        {
            if (ValidateCellPosition(x, y))
            {
                field[x, y] = state;
            }
        }  
    }
}
