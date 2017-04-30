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

        TheEngine gBuf;
        List<PointOfInterest> GlobalPointOfInterest = new List<PointOfInterest>();

        #region FPS Counter

        DateTime last = DateTime.Now;
        long frames;
        long lastFrames;

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

                float x = (float)(r1.Next(0, Window.Width));
                float y = (float)(r1.Next(0, Window.Height));

                GlobalPointOfInterest.Add(new PointOfInterest(new PointF(x, y), (float)(r1.NextDouble())));
            }

            for (int i = 0; i < 1; i++)
            {
                Creature tempCreature;

                float x = Window.Width/2;
                float y = Window.Height/2;
                tempCreature = new Creature(
                    new PointF(x, y),
                    Color.FromArgb(r1.Next(255), r1.Next(255), r1.Next(255)),
                    r1.Next(7, 20)
                    );

                tempCreature.MaxSpeed = (float)(r1.NextDouble() * 2);
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
                last = DateTime.Now;

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

        /// <summary>
        /// Calculates everything
        /// </summary>
        private void Claculations()
        {
            gStopwatch.Reset();
            gStopwatch.Start();

            fps = (int)GetFPS();

            #region Calculations

            for (int i = 0; i < Creatures.Count; i++)
            {
                Creatures[i].CheckOutOfBorder(Border);
                Creatures[i].Move();
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

            if (!backgroundWorkerCalculations.IsBusy)
                backgroundWorkerCalculations.RunWorkerAsync();

            //this.Refresh();
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
            //gBuf.buffer.Graphics.DrawEllipse(new Pen(Color.Gray), GlobalPointOfInterest.X - 3, GlobalPointOfInterest.Y - 3, 6, 6);

            for (int z = 0; z < Creatures[0].InterestingPoints.Count; z++)
            {
                gBuf.buffer.Graphics.DrawEllipse(new Pen(Color.FromArgb(222, 0, 255), 2), Creatures[0].InterestingPoints[z].Point.X - 3, Creatures[0].InterestingPoints[z].Point.Y - 3, 6, 6);

                string force = string.Format("{0:0.00}", Creatures[0].InterestingPoints[z].Force);

                /*
                gBuf.buffer.Graphics.DrawString("F = " + force,
                new Font("Arial", 7),
                new SolidBrush(Color.White),
                Creatures[0].InterestingPoints[z].Point.X, Creatures[0].InterestingPoints[z].Point.Y, 
                new StringFormat());
                */
            }

            for (int i = 0; i < Creatures.Count; i++)
            {
                Creatures[i].Draw(gBuf.buffer.Graphics);
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
            GlobalPointOfInterest.Add(new PointOfInterest(mousePos, (float)(r1.NextDouble())));

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    Creatures[i].InterestingPoints = GlobalPointOfInterest;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    Creatures[i].Position = mousePos;
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
    }
}
