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


        #region Grid

        #endregion

        TheEngine gBuf;

        #region FPS Counter

        DateTime last = DateTime.Now;
        byte frames;
        byte lastFrames;

        Stopwatch gStopwatch = new Stopwatch();

        int fps, cTime, rTime;

        #endregion

        #region Ceatures

        Creature god = null;

        #endregion

        public Main()
        {
            InitializeComponent();

            #region Important

            this.DoubleBuffered = true;

            if (gBuf == null)
                gBuf = new TheEngine(this);

            UpdateWindowSize();

            #endregion
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

            if (god != null)
            {
                god.Move();
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

            if (god != null)
                god.Draw(gBuf.buffer.Graphics);

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
            UpdateWindowSize();

            if (gBuf == null)
                gBuf = new TheEngine(this);

            gBuf.UpdateGraphicsBuffer();
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
        /// This is for debuging, it sets The God position, based on mouse positon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePos = this.PointToClient(Cursor.Position);

            if (e.Button == MouseButtons.Left)
            {
                if (god == null)
                {
                    god = new Creature(mousePos);
                }
                else
                    god.target.Add(mousePos);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                god.target.Clear();
            }
            else if (e.Button == MouseButtons.Right)
            {
                god.Position = mousePos;
            }

        }

        /// <summary>
        /// This is also for debuging 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
            }
        }

    }
}
