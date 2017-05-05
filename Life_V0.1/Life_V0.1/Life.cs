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

        #region CPS Counter

        DateTime last = DateTime.Now;

        Stopwatch gStopwatch = new Stopwatch();

        float cps = 0, cTime, rTime;
        byte displayDelay = 100; // How many loop before update
        byte curLoop = 0;

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
        /// Calculates ammount of cycles done in one secon
        /// </summary>
        /// <returns></returns>
        public double GetCPS()
        {
            TimeSpan ts = DateTime.Now.Subtract(last);
            last = DateTime.Now;

            if (ts.Milliseconds > 0)
                return 1000 / ts.Milliseconds;
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

            cps = (float)GetCPS();

            #region Calculations

            if (god != null)
            {

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

            gStopwatch.Stop();

            TimeSpan ts = gStopwatch.Elapsed;

            curLoop++;

            if (curLoop >= displayDelay)
            {
                curLoop = 0;

                labelRender.Text = "R: " + ts.Milliseconds + " ms";
                labelFPS.Text = "CPS: " + cps; // Cycles per second
                labelClaculations.Text = "C: " + cTime + " ms";
            }

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
            }
            else if (e.Button == MouseButtons.Middle)
            {

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
