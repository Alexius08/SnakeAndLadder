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
            for (int i = 0; i<4; i++)
            {
                p[i] = new Player();
                p[i].playorder = i;
                p[i].isactive = i < 2;
            }
        }

        public class Player
        {
            public int location = 1;
            public int playorder = 0;
            public bool isactive = false;

            public Point calctokenlocation()
            {
                return new Point(
                    ((location-1)%10)*60+5+30*(playorder%2),
                    575-((location-1)/10)*60-30*(playorder/2));
            }
        }

        Player[] p = new Player[4];
        Point[] tokenloc = new Point[4];
        bool GameOngoing = false;
        Image gameboard = Image.FromFile(Environment.CurrentDirectory + "\\gameboard.jpg");

        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            if (GameOngoing)
            {
                Brush[] PlayerColor = new Brush[] { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Gold };
                for (int i = 0; i < 4; i++)
                {
                    tokenloc[i] = p[i].calctokenlocation();
                }

                Pen gridline = new Pen(Brushes.Black, 1);
                int bottom = Height - 23;

                e.Graphics.DrawImage(gameboard, 0, 0, 600, 600);
                int tokensize = 20;
                for (byte i = 1; i <= 10; i++)
                {
                    e.Graphics.DrawLine(gridline, 60 * i, 0, 60 * i, bottom);
                    e.Graphics.DrawLine(gridline, 0, 60 * i, Width, 60 * i);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (p[i].isactive) e.Graphics.FillEllipse(PlayerColor[i], tokenloc[i].X, tokenloc[i].Y, tokensize, tokensize);
                }
            }

            Pen gridline = new Pen(Brushes.Black, 1);
            int bottom = Height - 23;
            Image gameboard = Image.FromFile(Environment.CurrentDirectory + "\\gameboard.jpg");
            e.Graphics.DrawImage(gameboard, 0, 0, 600, 600);
            int tokensize = 20;
            for (byte i = 1; i <= 10; i++)
            {
                e.Graphics.DrawLine(gridline, 60*i, 0, 60*i, bottom);
                e.Graphics.DrawLine(gridline, 0, 60 * i, Width, 60 * i);
            }
            e.Graphics.FillEllipse(Brushes.Blue,tokenloc[0].X, tokenloc[0].Y,tokensize,tokensize);
            e.Graphics.FillEllipse(Brushes.Red, tokenloc[1].X, tokenloc[1].Y, tokensize, tokensize);
            e.Graphics.FillEllipse(Brushes.Green, tokenloc[2].X, tokenloc[2].Y, tokensize, tokensize);
            e.Graphics.FillEllipse(Brushes.Gold, tokenloc[3].X, tokenloc[3].Y, tokensize, tokensize);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
