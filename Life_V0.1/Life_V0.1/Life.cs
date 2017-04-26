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
        public Rectangle Window; // Stores working window size

        static int WindowXShift = 19; // How much window border takse from working area
        static int WindowYShift = 39; // How much window border takse from working area

        public Rectangle Border; // Declares border of life (gray square near edges)
        public int BorderSize = 100; // Size of border

        private bool bypass = false; // Bypasses internal timer

        TheEngine gBuf;

        #region FPS Counter

        DateTime last = DateTime.Now;
        long frames;
        long lastFrames;

        Stopwatch gStopwatch = new Stopwatch();

        int fps, cTime, rTime;

        #endregion

        #region Tests

        Creature TheGod;

        List<Creature> Creatures = new List<Creature>();

        Random r1 = new Random(1);
        Random f1 = new Random(100);

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

            for (int i = 0; i < 100; i++)
            {

                Creature tempCreature;

                float x = (float)(r1.Next(-1000, 1000));
                float y = (float)(r1.Next(-1000, 1000));
                tempCreature = new Creature(
                    new PointF(x, y),
                    Color.FromArgb(r1.Next(255), r1.Next(255), r1.Next(255)),
                    r1.Next(2, 20)
                    );

                Vector2 v1 = new Vector2(r1.Next(-100, 100) / 100f, r1.Next(-100, 100) / 100f);
                Vector2 v2 = new Vector2(r1.Next(-100, 100) / 100f, r1.Next(-100, 100) / 100f);
                Vector2 v3 = new Vector2(r1.Next(-100, 100) / 100f, r1.Next(-100, 100) / 100f);

                tempCreature.AttachForce(v1);
                tempCreature.AttachForce(v2);
                tempCreature.AttachForce(v3);
                //tempCreature.dispalyForces = true;
                //tempCreature.forceMagnifier = 10;

                Creatures.Add(tempCreature);
            }

            TheGod = new Creature(new PointF(Window.Width / 2, Window.Height / 2));
            TheGod.AttachForce(new Vector2(new PointF(0.1f, 0.2f)));
            TheGod.AttachForce(new Vector2(new PointF(-0.4f, 0.1f)));
            //TheGod.dispalyForces = true;
            //TheGod.forceMagnifier = 10;
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

            TheGod.Move();
            TheGod.CheckBoundry(Border, Window);

            for (int i = 0; i < Creatures.Count; i++)
            {
                Creatures[i].Move();
                Creatures[i].CheckBoundry(Border, Window);
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

            TheGod.Draw(gBuf.buffer.Graphics); // Draws GOD Creature

            for (int i = 0; i < Creatures.Count; i++)
                Creatures[i].Draw(gBuf.buffer.Graphics);

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
            TheGod.Positon = mousePos;

            for (int i = 0; i < Creatures.Count; i++)
                Creatures[i].Positon = mousePos;
        }
    }
}
