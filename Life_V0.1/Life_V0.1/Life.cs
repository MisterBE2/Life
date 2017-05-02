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
        public int BorderSize = 20; // Size of border

        public static Rectangle ShuffleBorder;
        public int ShuffleBorderSize = 50;

        float size, speed, health, energy;
        long age;

        PointF pSize, pSpeed, pHealth, pEnergy;

        List<PointF> pAge = new List<PointF>();

        bool DrawStatistics = false;
        bool DrawHealth = false;

        #region Grid

        public int[] gridSize = { 30, 15 };
        public bool showGrid = false;

        #endregion

        TheEngine gBuf;
        public List<PointOfInterest> GlobalPointOfInterest = new List<PointOfInterest>();

        #region FPS Counter

        DateTime last = DateTime.Now;
        byte frames;
        byte lastFrames;

        Stopwatch gStopwatch = new Stopwatch();

        int fps, cTime, rTime;

        #endregion

        #region Tests

        List<Creature> Creatures = new List<Creature>();
        List<Creature> CreaturesToAdd = new List<Creature>();

        Random r1 = new Random(47);
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

            for (int i = 0; i < 500; i++)
            {
                AddRandomFood();
            }

            for (int i = GlobalPointOfInterest.Count-1; i >= 0; i--)
            {
                if (!Border.Contains(new Point((int)(GlobalPointOfInterest[i].Point.X), (int)(GlobalPointOfInterest[i].Point.Y))))
                    GlobalPointOfInterest.RemoveAt(i);
            }

            for (int i = 0; i < 500; i++)
            {
                AddRandomCreature();
            }
        }

        public void AddRandomCreature()
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
                (float)(r1.Next(1000) / 1000f),
                this
                );

            tempCreature.eSub = r1.Next(100) / 10000f;
            tempCreature.GoodForce = r1.Next(-1000, 1000) / 1000f;
            tempCreature.BadForce = r1.Next(-1000, 1000) / 1000f;
            tempCreature.MaxSize = r1.Next(70);
            tempCreature.gorwSpeed = r1.Next(100) / 10000f;
            //tempCreature.BadForce = -100;
            tempCreature.ViewLength = r1.Next((int)tempCreature.Size, 100);
            tempCreature.MaxSpeed = (float)(r1.Next(1000, 5000) / 1000f);
            tempCreature.matureAge = r1.Next(600, 2000);
            tempCreature.LifeDuration = r1.Next(1000, 10000);
            tempCreature.MaxChildern = r1.Next(1, 20);
            tempCreature.InterestingPoints = GlobalPointOfInterest;

            CreaturesToAdd.Add(tempCreature);
        }

        public void AddRandomCreature(PointF Position)
        {
            Creature tempCreature;

            float x = (float)(r1.Next(Border.X, Border.X + Border.Width));
            float y = (float)(r1.Next(Border.Y, Border.X + Border.Height));

            tempCreature = new Creature(
                Position,
                Color.FromArgb(r1.Next(255), r1.Next(255), r1.Next(255)),
                r1.Next(5, 20),
                r1.Next(0, Creatures.Count),
                (float)(r1.Next(1000) / 1000f),
                (float)(r1.Next(1000) / 1000f),
                this
                );

            tempCreature.eSub = r1.Next(100) / 10000f;
            tempCreature.GoodForce = r1.Next(-1000, 1000) / 1000f;
            tempCreature.BadForce = r1.Next(-1000, 1000) / 1000f;
            tempCreature.MaxSize = r1.Next(70);
            tempCreature.gorwSpeed = r1.Next(100) / 10000f;
            //tempCreature.BadForce = -100;
            tempCreature.ViewLength = r1.Next((int)tempCreature.Size, 100);
            tempCreature.MaxSpeed = (float)(r1.Next(1000, 5000) / 1000f);
            tempCreature.matureAge = r1.Next(600, 2000);
            tempCreature.LifeDuration = r1.Next(1000, 10000);
            tempCreature.MaxChildern = r1.Next(1, 20);
            tempCreature.InterestingPoints = GlobalPointOfInterest;

            CreaturesToAdd.Add(tempCreature);
        }

        public void AddRandomFood()
        {
            float x = (float)(r1.Next(Border.X, Border.X + Border.Width));
            float y = (float)(r1.Next(Border.Y, Border.X + Border.Height));

            bool type = r1.NextDouble() > 0.25;

            if (r1.NextDouble() > 0.95)
            {
                float x2 = 0;
                float y2 = 0;

                for (int i = 0; i < r1.Next(3, 50); i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        x2 = r1.Next((int)(x - r1.Next(10, 100)), (int)(x + r1.Next(10, 100)));
                        y2 = r1.Next((int)(y - r1.Next(10, 100)), (int)(y + r1.Next(10, 100)));

                        if (Border.Contains(new Point((int)x2, (int)y2)))
                            break;  
                    }

                    if (x2 != 0 && y2 != 0)
                        GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x2, y2), (float)(r1.NextDouble()), type, (float)(r1.NextDouble() * r1.Next(25, 80))));
                }
            }
            else
                GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x, y), (float)(r1.NextDouble()), r1.NextDouble() > 0.25, (float)(r1.NextDouble() * r1.Next(2, 25))));
        }

        public void AddRandomFood(PointF Position)
        {
            bool type = r1.NextDouble() > 0.25;

            if (r1.NextDouble() > 0.95)
            {
                float x2 = 0;
                float y2 = 0;

                for (int i = 0; i < r1.Next(3, 50); i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        x2 = r1.Next((int)(Position.X - r1.Next(10, 100)), (int)(Position.X + r1.Next(10, 100)));
                        y2 = r1.Next((int)(Position.Y - r1.Next(10, 100)), (int)(Position.Y + r1.Next(10, 100)));

                        if (Border.Contains(new Point((int)x2, (int)y2)))
                            break;
 
                    }

                    if(x2 !=0 && y2 !=0)
                        GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x2, y2), (float)(r1.NextDouble()), type, (float)(r1.NextDouble() * r1.Next(25, 80))));
                }
            }
            else
                GlobalPointOfInterest.Add(new PointOfInterest(Position, (float)(r1.NextDouble()), r1.NextDouble() > 0.25, (float)(r1.NextDouble() * r1.Next(2, 25))));
        }

        public void AddCreature(PointF _Position, Color _Color, float _Size, int _Seed, float _Health, float _Energy)
        {
            CreaturesToAdd.Add(new Creature(_Position, _Color, _Size, _Seed, _Health, _Energy, this));
        }

        public void AddCreature(Creature creature)
        {
            CreaturesToAdd.Add(creature);
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

            ShuffleBorder = new Rectangle();
            ShuffleBorder.Location = new Point(ShuffleBorderSize, ShuffleBorderSize);
            ShuffleBorder.Size = new Size(Window.Width - ShuffleBorderSize * 2, Window.Height - ShuffleBorderSize * 2);
        }

        public float GetDistance(PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
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

            if(CreaturesToAdd.Count > 0)
            {
                for (int i = 0; i < CreaturesToAdd.Count; i++)
                {
                    Creatures.Add(CreaturesToAdd[i]);
                }
                CreaturesToAdd.Clear();
            }

            size = 0;
            speed = 0;
            health = 0;
            energy = 0;
            age = 0;

            pSize = labelSize.Location;
            pSpeed = labelSpeed.Location;
            pHealth = labelHealth.Location;
            pEnergy = labelEnergy.Location;
            //pAge.Clear();

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

                        if (Creatures[i].age > age)
                        {
                            age = Creatures[i].age;
                            //pAge.Add(Creatures[i].Position);
                        }

                    }

                }
                else
                {

                    //GlobalPointOfInterest.Add(new PointOfInterest(new PointF(Creatures[i].Position.X + r1.Next(-10, 10), Creatures[i].Position.Y + r1.Next(-10, 10)), (float)r1.NextDouble(), true, Creatures[i].Size));

                    bool newPoint = true;

                    /*
                    for (int j = 0; j < GlobalPointOfInterest.Count; j++)
                    {
                        float d = GetDistance(Creatures[i].Position, GlobalPointOfInterest[j].Point);

                        if (d <= GlobalPointOfInterest[j].Value + Creatures[i].Size)
                        {
                            newPoint = false;

                            if (r1.NextDouble() > 0.5)
                                GlobalPointOfInterest[j].Value += Creatures[i].Size;
                            else
                                GlobalPointOfInterest[j].Value -= Creatures[i].Size;

                            break;
                        }
                    }
                    */

                    if (newPoint)
                        GlobalPointOfInterest.Add(new PointOfInterest(Creatures[i].Position, Creatures[i].age / 10000f, r1.NextDouble() > 0.35, Creatures[i].Size * 0.85f));

                    Creatures.RemoveAt(i);
                }
            }

            if (GlobalPointOfInterest.Count < Creatures.Count)
            {
                if (r1.NextDouble() > 0.8)
                {
                    float x = (float)(r1.Next(Border.X, Border.X + Border.Width));
                    float y = (float)(r1.Next(Border.Y, Border.X + Border.Height));

                    GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x, y), (float)(r1.NextDouble()), r1.NextDouble() > 0.2, (float)(r1.NextDouble()) * r1.Next(1, 10)));
                }
            }


            for (int i = 0; i < GlobalPointOfInterest.Count; i++)
            {
                if (GlobalPointOfInterest[i].Value > 150)
                {
                    GlobalPointOfInterest[i].Value = 150;
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
                        Color c = Color.DarkRed;

                        if (GlobalPointOfInterest[z].IsGood)
                            c = Color.DarkGreen;

                        gBuf.buffer.Graphics.FillEllipse(new SolidBrush(c), GlobalPointOfInterest[z].Point.X - GlobalPointOfInterest[z].Value / 2, GlobalPointOfInterest[z].Point.Y - GlobalPointOfInterest[z].Value / 2, GlobalPointOfInterest[z].Value, GlobalPointOfInterest[z].Value);
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

                if (pAge.Count > 0)
                {
                    labelAge.Text = "Oldest: " + age;

                    for (int i = 0; i < pAge.Count; i++)
                    {
                        gBuf.buffer.Graphics.DrawLine(new Pen(Color.Green), labelAge.Location, pAge[i]);
                    }
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
                AddRandomFood(mousePos);
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < 10; i++)
                {
                    AddRandomCreature(mousePos);
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
                    labelAge.Visible = false;
                    DrawStatistics = false;
                }
                else
                {
                    labelSize.Visible = true;
                    labelSpeed.Visible = true;
                    labelHealth.Visible = true;
                    labelEnergy.Visible = true;
                    labelAge.Visible = true;
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
            else if (e.KeyCode == Keys.Escape)
            {
                backgroundWorkerCalculations.CancelAsync();
            }
        }

    }
}
