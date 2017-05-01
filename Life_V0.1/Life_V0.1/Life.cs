using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Life_V0._1
{
    public partial class Main : Form
    {
        public static Rectangle Window; // Stores working window size

        public static int WindowXShift = 19; // How much window border takse from working area
        public static int WindowYShift = 39; // How much window border takse from working area

        public static Rectangle Border; // Declares border of life (gray square near edges)
        public int BorderSize = 10; // Size of border

        float size, speed, health, energy;
        PointF pSize, pSpeed, pHealth, pEnergy;
        bool DrawStatistics = false;
        bool DrawHealth = false;

        #region Grid

        public int[] gridSize = { 30, 15 };
        public bool showGrid = false;

        #endregion

        TheEngine gBuf;
        List<PointOfInterest> GlobalPointOfInterest = new List<PointOfInterest>();

        #region FPS Counter

        DateTime last = DateTime.Now;
        byte frames;
        byte lastFrames;

        Stopwatch gStopwatch = new Stopwatch();

        int fps, cTime, rTime;

        #endregion

        #region Tests

        List<Creature> Creatures = new List<Creature>();

        Random r1 = new Random(50);
        Random f1 = new Random(100);

        Vector tempV1 = new Vector(0, 0);
        Vector tempV2 = new Vector(0, 0);

        #endregion

        public Main()
        {
            InitializeComponent();

            #region Important

            this.DoubleBuffered = true;
            gBuf = new TheEngine(this);
            UpdateWindowSize();
            #endregion

            UpdateBorder();

            for (int i = 0; i < 1000; i++)
            {
                float x = (float)(r1.Next(Border.X, Border.X + Border.Width));
                float y = (float)(r1.Next(Border.Y, Border.X + Border.Height));

                GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x, y), (float)(r1.NextDouble())));
            }

            for (int i = 0; i < 10000; i++)
            {
                Creature tempCreature;

                float x = (float)(r1.Next(Border.X, Border.X + Border.Width));
                float y = (float)(r1.Next(Border.Y, Border.X + Border.Height));

                tempCreature = new Creature(
                    new PointF(x, y),
                    Color.FromArgb(r1.Next(255), r1.Next(255), r1.Next(255)),
                    r1.Next(5, 20),
                    r1.Next(0, Creatures.Count),
                    (float)(r1.Next(1000) / 1000f),
                    (float)(r1.Next(1000) / 1000f)
                    );

                tempCreature.eSub = (float)(r1.Next(1000) / 1000f);

                tempCreature.MaxSpeed = (float)(r1.Next(1000, 5000) / 1000f);
                tempCreature.InterestingPoints = GlobalPointOfInterest;

                Creatures.Add(tempCreature);
            }
        }

        /// <summary>
        /// Calculates FPS
        /// </summary>
        /// <returns></returns>
        public double GetFPS()
        {
            TimeSpan ts = DateTime.Now.Subtract(last);

            if (lastFrames > frames)
            {
                last = DateTime.Now;
                frames = 0;
                lastFrames = 0;
            }

            lastFrames = frames;

            if (ts.TotalSeconds > 0)
                return frames / ts.TotalSeconds;
            else
                return -1;
        }

        /// <summary>
        /// Updates working area rectangle
        /// </summary>
        public void UpdateWindowSize()
        {
            Window = new Rectangle();
            Window.Location = new Point(0, 0);
            Window.Size = new Size(this.Width - WindowXShift, this.Height - WindowYShift);
        }

        /// <summary>
        /// Updates border size
        /// </summary>
        public void UpdateBorder()
        {
            Border = new Rectangle();
            Border.Location = new Point(BorderSize, BorderSize);
            Border.Size = new Size(Window.Width - BorderSize * 2, Window.Height - BorderSize * 2);
        }

        #region Rendering

        public void DisplayGrid(Graphics g)
        {
            for (int i = 0; i < gridSize[0]; i++)
            {
                for (int z = 0; z < gridSize[1]; z++)
                {
                    g.DrawRectangle(new Pen(Color.Gray), i * Window.Width / gridSize[0], z * Window.Height / gridSize[1], Window.Width / gridSize[0], Window.Height / gridSize[1]);
                }
            }
        }

        /// <summary>
        /// Calculates everything
        /// </summary>
        private void Claculations()
        {
            gStopwatch.Reset();
            gStopwatch.Start();

            fps = (int)GetFPS();

            #region Calculations

            size = 0;
            speed = 0;
            health = 0;
            energy = 0;

            pSize = labelSize.Location;
            pSpeed = labelSpeed.Location;
            pHealth = labelHealth.Location;
            pEnergy = labelEnergy.Location;

            for (int i = Creatures.Count - 1; i >= 0; i--)
            {
                if (!Creatures[i].Dead)
                {
                    Creatures[i].CheckOutOfBorder(Border);
                    Creatures[i].Move();
                    Creatures[i].CheckHelath();

                    if (DrawStatistics)
                    {
                        if (Creatures[i].Size > size)
                        {
                            size = Creatures[i].Size;
                            pSize = Creatures[i].Position;
                        }

                        if (Creatures[i].MinSpeed > speed)
                        {
                            speed = Creatures[i].MinSpeed;
                            pSpeed = Creatures[i].Position;
                        }

                        if (Creatures[i].CurHealth > health)
                        {
                            health = Creatures[i].CurHealth;
                            pHealth = Creatures[i].Position;
                        }

                        if (Creatures[i].CurEnergy > energy)
                        {
                            energy = Creatures[i].CurEnergy;
                            pEnergy = Creatures[i].Position;
                        }
                    }

                }
                else
                {
                    for (int z = 0; z < Creatures[i].Size; z++)
                    {
                        GlobalPointOfInterest.Add(new PointOfInterest(new PointF(Creatures[i].Position.X + r1.Next(-10, 10), Creatures[i].Position.Y + r1.Next(-10, 10)), (float)r1.NextDouble()));
                    }


                    Creatures.RemoveAt(i);
                }
            }

            #endregion

            gStopwatch.Stop();
            TimeSpan ts = gStopwatch.Elapsed;
            cTime = ts.Milliseconds;
        }

        /// <summary>
        /// Triggers next frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefTimer_Tick(object sender, EventArgs e)
        {
            RefTimer.Stop();

            labelPopulation.Text = "Population: " + Creatures.Count;

            //if(!workers[workers.Count-1].bg.IsBusy)
            //Optymised();

            if (!backgroundWorkerCalculations.IsBusy)
                backgroundWorkerCalculations.RunWorkerAsync();

            this.Refresh();
        }

        /// <summary>
        /// Renders everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Paint(object sender, PaintEventArgs e)
        {
            gStopwatch.Reset();
            gStopwatch.Start();
            gBuf.ClearBuffer(Color.Black);

            #region Buffer Insertions

            gBuf.buffer.Graphics.DrawRectangle(new Pen(Color.Gray, 1), Border); // Draws border rectangle

            if (showGrid)
                DisplayGrid(gBuf.buffer.Graphics);

            for (int z = 0; z < GlobalPointOfInterest.Count; z++)
            {
                if (GlobalPointOfInterest[z] != null)
                {
                    try
                    {
                        gBuf.buffer.Graphics.DrawEllipse(new Pen(Color.FromArgb(222, 0, 255), 2), GlobalPointOfInterest[z].Point.X - 3, GlobalPointOfInterest[z].Point.Y - 3, 6, 6);
                    }
                    catch
                    {
                    }
                }
            }

            for (int i = 0; i < Creatures.Count; i++)
            {
                if (!Creatures[i].Dead)
                {
                    if (DrawHealth)
                        Creatures[i].DrawHealth = true;
                    else
                        Creatures[i].DrawHealth = false;

                    Creatures[i].Draw(gBuf.buffer.Graphics);
                }
            }

            if (DrawStatistics)
            {
                if (pSize != null)
                {
                    labelSize.Text = "Largest: " + size;
                    gBuf.buffer.Graphics.DrawLine(new Pen(Color.White), labelSize.Location, pSize);
                }

                if (pSpeed != null)
                {
                    labelSpeed.Text = "Fastest: " + speed;
                    gBuf.buffer.Graphics.DrawLine(new Pen(Color.Yellow), labelSpeed.Location, pSpeed);
                }

                if (pHealth != null)
                {
                    labelHealth.Text = "Healthiest: " + health;
                    gBuf.buffer.Graphics.DrawLine(new Pen(Color.Red), labelHealth.Location, pHealth);
                }

                if (pEnergy != null)
                {
                    labelEnergy.Text = "Most fit: " + energy;
                    gBuf.buffer.Graphics.DrawLine(new Pen(Color.Green), labelEnergy.Location, pEnergy);
                }
            }

            #endregion

            gBuf.RenderBuffer(e.Graphics);

            frames++;

            gStopwatch.Stop();
            TimeSpan ts = gStopwatch.Elapsed;
            labelRender.Text = "R: " + ts.Milliseconds + " ms";
            labelFPS.Text = "FPS: " + fps;
            labelClaculations.Text = "C: " + cTime + " ms";

            RefTimer.Start();
        }

        #endregion

        /// <summary>
        /// Occurs when window is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            gBuf.UpdateGraphicsBuffer();
            UpdateWindowSize();

            UpdateBorder();
        }

        /// <summary>
        /// Calculates all thing on separate thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerCalculations_DoWork(object sender, DoWorkEventArgs e)
        {
            Claculations();
        }

        /// <summary>
        /// Updates frame when calculations are done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerCalculations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// This is for debbuging, it sets The God position, based on mouse positon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePos = this.PointToClient(Cursor.Position);


            if (e.Button == MouseButtons.Left)
            {
                GlobalPointOfInterest.Add(new PointOfInterest(mousePos, (float)(r1.NextDouble())));

                for (int i = 0; i < Creatures.Count; i++)
                {
                    Creatures[i].InterestingPoints = GlobalPointOfInterest;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Creatures.Count > 0)
                {
                    for (int i = 0; i < Creatures.Count; i++)
                    {
                        Creatures[i].Position = mousePos;
                    }
                }
                else
                {
                    Creature tempCreature;
                    tempCreature = new Creature(
                        mousePos,
                        Color.FromArgb(r1.Next(255), r1.Next(255), r1.Next(255)),
                        r1.Next(5, 20),
                        r1.Next(0, Creatures.Count),
                        1,
                        1
                        );

                    tempCreature.MaxSpeed = (float)(r1.Next(1000, 5000) / 1000f);
                    tempCreature.InterestingPoints = GlobalPointOfInterest;

                    Creatures.Add(tempCreature);
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {

                for (int i = 0; i < Creatures.Count; i++)
                {
                    Creatures[i].ClearPointsOfInterest();
                }

            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    if (Creatures[i].DrawFieldOfViev)
                        Creatures[i].DrawFieldOfViev = false;
                    else
                        Creatures[i].DrawFieldOfViev = true;
                }
            }
            else if (e.KeyCode == Keys.W)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    if (Creatures[i].DrawGlobalVector)
                        Creatures[i].DrawGlobalVector = false;
                    else
                        Creatures[i].DrawGlobalVector = true;
                }
            }
            else if (e.KeyCode == Keys.E)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    if (Creatures[i].DrawShufflePoint)
                        Creatures[i].DrawShufflePoint = false;
                    else
                        Creatures[i].DrawShufflePoint = true;
                }
            }
            else if (e.KeyCode == Keys.R)
            {
                if (DrawStatistics)
                {
                    labelSize.Visible = false;
                    labelSpeed.Visible = false;
                    labelHealth.Visible = false;
                    labelEnergy.Visible = false;
                    DrawStatistics = false;
                }
                else
                {
                    labelSize.Visible = true;
                    labelSpeed.Visible = true;
                    labelHealth.Visible = true;
                    labelEnergy.Visible = true;
                    DrawStatistics = true;
                }
            }
            else if (e.KeyCode == Keys.T)
            {
                if (DrawHealth)
                    DrawHealth = false;
                else
                    DrawHealth = true;
            }
        }

    }
}
