﻿using System;
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

        public Rectangle Border { get; set; } // Declares border of life (gray square near edges)
        public int BorderSize { get; set; }

        public Rectangle Window { get; set; } // Stores working window size

        static int WindowXShift = 19; // How much window border takse from working area
        static int WindowYShift = 39; // How much window border takse from working area

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

        List<Point> points = new List<Point>();
        Random F = new Random();
        long z = 0;

        #endregion

        public Main()
        {
            InitializeComponent();

            #region Important

            this.DoubleBuffered = true;
            gBuf = new TheEngine(this);
            UpdateWindowSize();

            #endregion

            BorderSize = 50;
            UpdateBorder();
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
            Window = new Rectangle(0, 0, this.Width - WindowXShift, this.Height - WindowYShift);
        }

        /// <summary>
        /// Updates border size
        /// </summary>
        public void UpdateBorder()
        {
            Border = new Rectangle(BorderSize, BorderSize, Window.Width - BorderSize * 2, Window.Height - BorderSize * 2);
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

            points.Clear();

            for (int i = 0; i < z; i++)
                points.Add(new Point(F.Next(Window.X, Window.Width), F.Next(Window.Y, Window.Height)));

            #endregion

            gStopwatch.Stop();
            TimeSpan ts = gStopwatch.Elapsed;
            cTime = ts.Milliseconds;

            z += 100;
        }

        /// <summary>
        /// Triggers next frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefTimer_Tick(object sender, EventArgs e)
        {
            RefTimer.Stop();

            if(!backgroundWorkerCalculations.IsBusy)
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
            //gBuf.buffer.Graphics.DrawEllipse(new Pen(Color.White, 1), Window.Width/2 - 25, Window.Height/2 - 25, 50, 50);

            if (points.Count > 100)
            {
                Point[] p = new Point[50];
                int step = 0;
                int y = 0;

                if (points.Count > 1000)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (step >= points.Count / p.Length)
                        {
                            if (y < p.Length - 1)
                                p[y] = points[i];

                            step = 0;
                            y++;
                        }

                        step++;
                    }
                }

                gBuf.buffer.Graphics.DrawClosedCurve(new Pen(Color.White), p);
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
    }
}
