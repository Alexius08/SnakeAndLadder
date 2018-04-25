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
                p[i].PlayOrder = i;
                p[i].IsActive = i < 2;
            }
        }

        public class Player
        {
            public int Location = 1;
            public int PlayOrder = 0;
            public bool IsActive = false;

            public Point CalcTokenLocation()
            {
                return new Point(
                    ((Location-1)%10)*60+5+30*(PlayOrder%2),
                    575-((Location-1)/10)*60-30*(PlayOrder/2));
            }
        }

        Player[] p = new Player[4];
        Point[] TokenLoc = new Point[4];
        bool GameOngoing = false;
        Image GameBoard = Image.FromFile(Environment.CurrentDirectory + "\\gameboard.jpg");
        List<int> PlayerTurn = new List<int>();

        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            if (GameOngoing)
            {
                Brush[] PlayerColor = new Brush[] { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Gold };
                for (int i = 0; i < 4; i++)
                {
                    TokenLoc[i] = p[i].CalcTokenLocation();
                }

                Pen GridLine = new Pen(Brushes.Black, 1);
                Pen Indicator = new Pen(Brushes.Black, 3);
                int bottom = Height - 23;

                e.Graphics.DrawImage(GameBoard, 0, 0, 600, 600);
                int TokenSize = 20;
                for (byte i = 1; i <= 10; i++)
                {
                    e.Graphics.DrawLine(GridLine, 60 * i, 0, 60 * i, bottom);
                    e.Graphics.DrawLine(GridLine, 0, 60 * i, Width, 60 * i);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (p[i].IsActive) e.Graphics.FillEllipse(PlayerColor[i], TokenLoc[i].X, TokenLoc[i].Y, TokenSize, TokenSize);
                    if (PlayerTurn.Count>0 && i == PlayerTurn[0]) e.Graphics.DrawEllipse(Indicator, TokenLoc[i].X, TokenLoc[i].Y, TokenSize, TokenSize);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            GameOngoing = true;
            Height = 710; //642 without buttons at bottom
            Width = 618;
            for (int i = 0; i < 4; i++)
            {
                if(i < nudPlayerCount.Value)
                {
                    p[i].IsActive = true;
                    PlayerTurn.Add(i);
                }
                else
                {
                    p[i].IsActive = false;
                }
            }
            Random random = new Random();
            PlayerTurn = PlayerTurn.OrderBy(x => random.Next()).ToList();
            lblTurnIndicator.Text = "It's Player " + (PlayerTurn[0] + 1) + "'s turn";
            lblPlayerCount.Hide();
            nudPlayerCount.Hide();
            btnPlay.Hide();
            Refresh();            
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            GC.Collect();
            GameOngoing = false;
            Height = 71;
            Width = 234;
            btnRollDice.Enabled = true;
            btnRollDice.Text = "Roll Dice\n0";
            lblPlayerCount.Show();
            nudPlayerCount.Show();
            btnPlay.Show();
            Refresh();
            foreach (Player P in p)
            {
                if (P.IsActive) P.Location = 1;
            }
            PlayerTurn = new List<int> { };
        }

        int MovesLeft = 0;

        Dictionary<int, int> SnakePoint = new Dictionary<int, int> {
            { 25, 5 },
            { 34, 1 },
            { 47, 19 },
            { 65, 52 },
            { 87, 57 },
            { 91, 61 },
            { 99, 69 } };

        Dictionary<int, int> LadderPoint = new Dictionary<int, int> {
            { 3, 51 },
            { 6, 27 },
            { 20, 70 },
            { 36, 55 },
            { 63, 95 },
            { 68, 98 } };

        bool hasOverflow;
        private void tmrTurnCounter_Tick(object sender, EventArgs e)
        {
            if (MovesLeft == 0)
            {
                if (p[PlayerTurn[0]].Location == 100)
                {
                    lblTurnIndicator.Text = "Player " + (PlayerTurn[0] + 1) + " wins";
                    tmrTurnCounter.Stop();
                }
                else if (SnakePoint.ContainsKey(p[PlayerTurn[0]].Location))
                {
                    p[PlayerTurn[0]].Location = SnakePoint[p[PlayerTurn[0]].Location];
                }
                else if (LadderPoint.ContainsKey(p[PlayerTurn[0]].Location))
                {
                    p[PlayerTurn[0]].Location = LadderPoint[p[PlayerTurn[0]].Location];
                }
                else
                {
                    tmrTurnCounter.Stop();
                    PlayerTurn.Add(PlayerTurn[0]);
                    PlayerTurn.RemoveAt(0);
                    btnRollDice.Enabled = true;
                    lblTurnIndicator.Text = "It's Player " + (PlayerTurn[0] + 1) + "'s turn";
                }                
                hasOverflow = false;
            }
            else
            {
                if (p[PlayerTurn[0]].Location==100 && MovesLeft > 0)
                {
                    hasOverflow = true;
                }
                p[PlayerTurn[0]].Location += hasOverflow ? -1 : 1;
                MovesLeft--;
            }
            Invalidate();
        }
        private void btnRollDice_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            MovesLeft = random.Next(1, 7);
            btnRollDice.Text = "Roll Dice\n" + MovesLeft;
            btnRollDice.Enabled = false;
            lblTurnIndicator.Text = "Player " + (PlayerTurn[0] + 1) + " is moving";
            tmrTurnCounter.Start();
        }
    }
}
