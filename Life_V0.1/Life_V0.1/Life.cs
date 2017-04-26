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
        bool outOfBoundry = false;

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

            TheGod = new Creature(new PointF(Window.Width/2, Window.Height/2));
            TheGod.AttachForce(new Vector2(new PointF(0.1f, 0.2f)));
            TheGod.AttachForce(new Vector2(new PointF(-0.4f, 0.1f)));
            TheGod.dispalyForces = true;
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

            Rectangle hyst = new Rectangle();
            hyst.Location = new Point((int)(Border.X + 0.2 * Border.X), (int)(Border.Y + 0.2 * Border.Y));
            hyst.Size = new Size((int)(Border.Width - 2* (0.2 * Border.X)), (int)(Border.Height -2* ( 0.2 * Border.Y)));

            if (!TheGod.IsColide(Border))
                outOfBoundry = true;

            if (TheGod.IsColide(hyst))
            {
                outOfBoundry = false;
                TheGod.boundryForce = new Vector2(0, 0);
            }

            if (!outOfBoundry)
                TheGod.Color = Color.LightGreen;

            else
            {
                TheGod.Color = Color.Red;

                if (TheGod.Positon.X >= Window.Width / 2)
                    TheGod.boundryForce.Add(new Vector2(-0.01f, 0));
                else if (TheGod.Positon.X < Window.Height / 2)
                    TheGod.boundryForce.Add(new Vector2(0.01f, 0));

                if (TheGod.Positon.Y >= Window.Height / 2)
                    TheGod.boundryForce.Add(new Vector2(0, -0.01f));
                else if (TheGod.Positon.Y < Window.Width / 2)
                    TheGod.boundryForce.Add(new Vector2(0, 0.01f));
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

            BorderSize = Window.Height / 2 - 100;

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
        }
    }
}
