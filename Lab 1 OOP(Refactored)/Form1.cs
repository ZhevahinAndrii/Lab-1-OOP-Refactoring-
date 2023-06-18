using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_1_OOP_Refactored_
{
    public partial class Form1 : Form
    {
        
        private Graphics _graphics;
        private int _resolution;
        private GameEngine engine;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled) return;
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            _resolution = Convert.ToInt32(nudResolution.Value);

            engine = new GameEngine(pictureBox1.Height / _resolution, pictureBox1.Width / _resolution,
                (int)nudDensity.Minimum+(int)nudDensity.Maximum-(int)nudDensity.Value);
            
            
            Text = $"Generation:{engine.CurrentGeneration}";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void DrawNextGeneration()
        {
            _graphics.Clear(Color.Black);
            var field = engine.GetCurrentGeneration();
            for(int x = 0; x < field.GetLength(0); x++)
            {
                for(int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                    {
                        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution - 1, _resolution - 1);
                    }
                }
            }
            pictureBox1.Refresh();
            Text =$"Generation {engine.CurrentGeneration}";
            engine.NextGeneration();
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
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
                engine.AddCell(x, y);
            }

            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / _resolution;
                int y = e.Location.Y / _resolution;
                engine.RemoveCell(x, y);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }


       
        
    }

}
