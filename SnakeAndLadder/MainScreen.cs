using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAndLadder
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            Pen gridline = new Pen(Brushes.Black, 1);
            int bottom = Height - 23;
            Image gameboard = Image.FromFile(Environment.CurrentDirectory + "\\gameboard.jpg");
            e.Graphics.DrawImage(gameboard, 0, 0, 600, 600);
            for (byte i = 1; i <= 10; i++)
            {
                e.Graphics.DrawLine(gridline, 60*i, 0, 60*i, bottom);
                e.Graphics.DrawLine(gridline, 0, 60 * i, Width, 60 * i);
            }
        }
    }
}
