using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution=1;
        private GameEngine gameEngine;
        private SoundPlayer player;
        private string MusicPath = "D:\\ProjectC#\\GameOfLife\\obj\\Debug\\PhoneMusic.wav";

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeSound()
        {
            player = new SoundPlayer();
            player.SoundLocation = MusicPath;
            player.Load();
            player.PlayLooping();
        }
        private void TheBeginning()
        {
            if (timer1.Enabled)
                return;

            if (player == null)
                InitializeSound();

            NUDResolution.Enabled = false;
            NUDDensity.Enabled = false;
            resolution = (int)NUDResolution.Value;

            gameEngine = new GameEngine
                (
                       ROWS : pictureBox1.Height / resolution,
                       COLS : pictureBox1.Width / resolution,
                       Density: (int)NUDDensity.Minimum + (int)NUDDensity.Maximum - (int)NUDDensity.Value
                );

            lblNumberOfGen.Text = Convert.ToString(gameEngine.CurrentGeneration);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(System.Drawing.Color.Black);

            var field = gameEngine.GetCurrentGeneration();
            for(int x=0;x<field.GetLength(0);x++)
            {
                for(int y=0;y<field.GetLength(1);y++)
                {
                    if(field[x,y])
                        graphics.FillRectangle(System.Drawing.Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }            

            pictureBox1.Refresh();
            lblNumberOfGen.Text = Convert.ToString(gameEngine.CurrentGeneration);
            lblPopulation.Text = Convert.ToString(gameEngine.Population);
            gameEngine.NextGeneration();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            NUDResolution.Enabled = true;
            NUDDensity.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            TheBeginning();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopGame();
            btnResume.Enabled = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)//Если таймер выключен
                return;

            var x = e.Location.X / resolution;
            var y = e.Location.Y / resolution;

            if (e.Button == MouseButtons.Left)
                gameEngine.AddCell(x, y);

            if (e.Button == MouseButtons.Right)
                gameEngine.RemoveCell(x, y);
        }

        private void btnResume_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
            btnResume.Enabled = false;
        }

        private void NUDResolution_ValueChanged(object sender, EventArgs e)
        {
            btnResume.Enabled = false;
        }

        private void NUDDensity_ValueChanged(object sender, EventArgs e)
        {
            btnResume.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)NUDTimer.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                player.Stop();
            if (checkBox1.Checked == false)
                player.PlayLooping();
        }
    }
}
