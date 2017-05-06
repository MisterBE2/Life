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

        TheEngine gBuf;

        #region Delete!

        bool pointMouse = false;

        #endregion

        #region Grid

        List<Chunk> Chunks = new List<Chunk>();
        int chunkSize = 100; // How many blocks are in chunk
        int[] chunksAmmount = { 15, 7 };
        bool drawChunks = false;
        bool drawLitChunks = false; // Tells protram if it need to bother filling active chunks
        bool drawChunkIndex = false;

        /// <summary>
        /// Adds new chunk
        /// </summary>
        /// <param name="chunk">Chunk to add</param>
        public void AddChunk(Chunk chunk)
        {
            Chunks.Add(chunk);
        }

        /// <summary>
        /// Initialises all chinks
        /// </summary>
        public void InitialiseGrid()
        {
            for (int i = 0; i < chunksAmmount[1]; i++)
            {
                int y = chunkSize * i;

                for (int j = 0; j < chunksAmmount[0]; j++)
                {
                    Rectangle chunkBox = new Rectangle();
                    chunkBox.Location = new Point(chunkSize * j, y);
                    chunkBox.Size = new Size(chunkSize, chunkSize);

                    AddChunk(new Chunk(chunkBox, new Point(j, i)));
                }
            }
        }

        /// <summary>
        /// Returns all chunk rectangles / areas
        /// </summary>
        /// <returns></returns>
        public Rectangle[] GetChunkRectangles()
        {
            Rectangle[] r = new Rectangle[Chunks.Count];

            for (int i = 0; i < Chunks.Count; i++)
                r[i] = Chunks[i].chunk;

            return r;
        }

        /// <summary>
        /// Draws chunks
        /// </summary>
        /// <param name="g">Graphic buffer to store rendered information</param>
        /// <param name="drawLit">Decides if it needs to draw active chunks</param>
        public void DrawChunks(Graphics g, bool drawLit)
        {
            if (drawLit)
            {
                foreach (Chunk chunk in Chunks)
                {
                    if (chunk.isLit)
                        g.FillRectangle(new SolidBrush(Color.LightGray), chunk.chunk);
                    else if (chunk.isSearched)
                        g.FillRectangle(new SolidBrush(Color.DarkRed), chunk.chunk);
                    else
                        g.DrawRectangle(new Pen(Color.Gray), chunk.chunk);
                }
            }
            else
                g.DrawRectangles(new Pen(Color.Gray), GetChunkRectangles());

            if (drawChunkIndex)
            {
                for (int i = 0; i < GetChunksCount(); i++)
                {
                    g.DrawString(
                       "X:" + Chunks[i].index.X + " Y:" + Chunks[i].index.Y + " I:" + i,
                       new Font("Arial", 10),
                       new SolidBrush(Color.LightGreen),
                       Chunks[i].chunk.Location
                    );
                }
            }
        }

        /// <summary>
        /// Returns total size of chunk grid
        /// </summary>
        /// <returns></returns>
        public Size GetTotalChunkSize()
        {
            Size s = new Size(0, 0);

            if (Chunks.Count > 0)
            {
                Chunk lastC = Chunks[Chunks.Count - 1];
                s = new Size(lastC.chunk.X + lastC.chunk.Width, lastC.chunk.Y + lastC.chunk.Height);
            }

            return s;
        }

        /// <summary>
        /// Checks if creature is assigned to any chunks
        /// </summary>
        /// <param name="creature">Creature to check</param>
        /// <returns></returns>
        public bool CheckAllChunks(Creature creature)
        {
            bool result = false;

            foreach (Chunk chunk in Chunks)
            {
                if (chunk.ContainAnyCreature())
                {
                    if (chunk.ContainCreature(creature))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check if creature is on almost all cheunks
        /// </summary>
        /// <param name="creature">Creature to check</param>
        /// <param name="skip">Which chunks should be skipped</param>
        /// <returns></returns>
        public bool CheckChunks(Creature creature, List<Chunk> skip)
        {
            bool result = false;

            foreach (Chunk chunk in Chunks)
            {
                if (skip != null)
                {
                    int temIndex = skip.BinarySearch(chunk);

                    if (temIndex < 0)
                        if (!chunk.ContainAnyCreature())
                        {
                            if (chunk.ContainCreature(creature))
                            {
                                result = true;
                                break;
                            }
                        }
                }
                else
                {
                    if (!chunk.ContainAnyCreature())
                    {
                        if (chunk.ContainCreature(creature))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if creature is inside given chunk area
        /// </summary>
        /// <param name="creature">Creature to check</param>
        /// <param name="index">Chunk index</param>
        /// <returns></returns>
        public bool ChunkContainCreature(Creature creature, int index)
        {
            return Chunks[index].ContainCreature(creature);
        }

        /// <summary>
        /// Returns chunk ammount
        /// </summary>
        /// <returns>Chunk count</returns>
        public int GetChunksCount()
        {
            return Chunks.Count;
        }

        /// <summary>
        /// Returns ammount of coolomns in curren chunk grid
        /// </summary>
        /// <returns>Colomns count</returns>
        public int ChunksColumns()
        {
            return chunksAmmount[0];
        }

        /// <summary>
        /// Assigns creature to all occupied chunks
        /// </summary>
        /// <param name="creature">Creature to be added</param>
        public void AssignCreatureToChunk(Creature creature)
        {
            for (int i = 0; i < GetChunksCount(); i++)
            {
                Chunks[i].AddCreature(creature);
            }
        }

        /// <summary>
        /// Assigns creature to given chunk
        /// </summary>
        /// <param name="creature">Creature to be added</param>
        /// <param name="chunk">Choosen chunk</param>
        public void AssignCreatureToChunk(Creature creature, Chunk chunk)
        {
            int index = Chunks.BinarySearch(chunk);
            Chunks[index].AddCreature(creature);
        }

        /// <summary>
        /// Assigns creature to given chunk
        /// </summary>
        /// <param name="creature">Creature to be added</param>
        /// <param name="cindex">Index of chunk</param>
        public void AssignCreatureToChunk(Creature creature, int index)
        {
            Chunks[index].AddCreature(creature);
        }

        /// <summary>
        /// Sets chunk search status to true
        /// </summary>
        /// <param name="state">Stato of status</param>
        /// <param name="index">Chunk index</param>
        public void SetChunkToSearch(bool state, int index)
        {
            Chunks[index].Searched(state);
        }

        /// <summary>
        /// Tells if chunk is in the list
        /// </summary>
        /// <param name="chunk">Chunk to check</param>
        /// <returns></returns>
        public bool ContainChunk(Chunk chunk)
        {
            return Chunks.Contains(chunk);
        }

        /// <summary>
        /// Returns chunk with given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Chunk on given index, else null</returns>
        public Chunk GetChunk(int index)
        {
            if (index >= 0 && index <= GetChunksCount() - 1)
                return Chunks[index];
            else
                return null;
        }

        /// <summary>
        /// Clears all searh and lit flags
        /// </summary>
        public void ClearFlags()
        {
            for (int i = 0; i < GetChunksCount(); i++)
            {
                Chunks[i].Lit(false);
                Chunks[i].Searched(false);
            }
        }

        #endregion

        #region CPS Counter

        DateTime last = DateTime.Now;

        Stopwatch gStopwatch = new Stopwatch();

        float cps = 0, cTime;
        byte displayDelay = 10; // How many loops before update
        byte curLoop = 0;

        #endregion

        #region Ceatures

        List<Creature> god = new List<Creature>();
        Random r1 = new Random();

        #endregion

        public Main()
        {
            InitializeComponent();

            #region Important

            this.DoubleBuffered = true;

            if (gBuf == null)
                gBuf = new TheEngine(this);

            UpdateWindowSize();
            InitialiseGrid();

            Size canvasNevSize = GetTotalChunkSize();

            if (canvasNevSize != new Size(0, 0))
                this.Size = new Size(canvasNevSize.Width + WindowXShift, canvasNevSize.Height + WindowYShift);

            this.CenterToScreen();

            #endregion

            for (int i = 0; i < 1; i++)
            {
                Creature g = new Creature(Window.Width / 2, Window.Height / 2, this);
                g.UpdateSeed(r1.Next(1000));
                g.target = g.GetRandomTarget();


                god.Add(g);
            }
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

            for (int i = 0; i < god.Count; i++)
            {
                god[i].Move();
                god[i].UpdateChunks();
                god[i].UpdatePosition();
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

            // Claculations();

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

            if (drawChunks)
                DrawChunks(gBuf.buffer.Graphics, drawLitChunks);

            //TODO: Use chunks to make faster rendering!
            for (int i = 0; i < god.Count; i++)
            {
                if (pointMouse)
                {
                    god[i].target = this.PointToClient(Cursor.Position);
                }

                god[i].Draw(gBuf.buffer.Graphics);
            }

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
                //ClearFlags();
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
            PointF mousePos = this.PointToClient(Cursor.Position);

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < god.Count; i++)
                    god[i].target = mousePos;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                pointMouse = !pointMouse;
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < god.Count; i++)
                    god[i].Position = mousePos;
            }

        }

        /// <summary>
        /// This is also for debuging 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                drawChunks = !drawChunks;
            }
            else if (e.KeyCode == Keys.F2)
            {
                drawLitChunks = !drawLitChunks;
            }
            else if (e.KeyCode == Keys.F3)
            {
                drawChunkIndex = !drawChunkIndex;
            }
        }
    }
}
