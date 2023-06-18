using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_1_OOP_Not_refactored_
{
    public partial class Form1 : Form
    {
        private long _currentGeneration = 0L;
        private Graphics _graphics;
        private int _resolution;
        private bool[,] field;
        private int rows, columns;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled) return;
            _currentGeneration = 0;
            Text = $"Generation:{_currentGeneration}";
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            _resolution = Convert.ToInt32(nudResolution.Value);

            rows = pictureBox1.Height / _resolution;
            columns = pictureBox1.Width / _resolution;
            field=new bool[columns,rows];

            Random rnd = new Random();

            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next((int)nudDensity.Value)==0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();            
        }

        private void NextGeneration()
        {
            _graphics.Clear(Color.Black);

            bool[,] newField = new bool[columns, rows];


            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var haslife = field[x,y];

                    if(!haslife&& neighboursCount == 3)
                    {
                        newField[x,y] = true;
                    }
                    else if(haslife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newField[x,y] = false;
                    }
                    else
                    {
                        newField[x,y] = field[x, y];
                    }

                    if (haslife)
                    {
                        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution-1, _resolution-1);

                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation:{++_currentGeneration}";
        }
        private int CountNeighbours(int x,int y)
        {
            int count = 0;
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {

                    int col = (x + i + columns) % columns; 
                    int row = (y + j+rows)%rows;
                    bool isSelfChecking = col == x && row == y;
                    var haslife = field[col,row];
                    if (haslife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
            
           
        }
        private void StopGame()
        {
            if (!timer1.Enabled) return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled) return;
            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / _resolution;
                int y = e.Location.Y / _resolution;
                if(ValidateMousePosition(x,y))
                field[x, y] = true;
            }
            
            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / _resolution;
                int y = e.Location.Y / _resolution;
                if(ValidateMousePosition(x,y))
                field[x, y] = false;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }
        private bool ValidateMousePosition(int x,int y)
        {
            return x>=0&&y>=0&&x < columns && y < rows;
        }
    }
}
