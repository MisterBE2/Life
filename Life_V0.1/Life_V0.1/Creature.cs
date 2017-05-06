using System.Drawing;
using System.Collections.Generic;
using System;

namespace Life_V0._1
{
    public partial class Creature
    {
        public PointF Position { get; set; } // Position of creature
        public float Mass { get; set; }
        public Color Color { get; set; } // Color of creature

        public PointF lastPosition;
        public float massMul = 5f; // 1 m^2 = massMul kg
        public int seed = 2;
        private Main MainForm;
        private SolidBrush MainBrush;

        public bool DrawDiagnostics = true;

        #region Delete!

        public PointF target;
        public float speed = 4f;

        #endregion

        #region Global variables

        float dx, dy, x, y, a, b, d, f, v, s; // Needed for gravity calculations

        Vector Global;
        Random r1;

        #endregion

        #region Constructors

        public Creature(float x, float y, Main _Main)
        {
            Position = new PointF(x, y);
            MainForm = _Main;
            DoOnStart();
        }

        public Creature(PointF _Position, Main _Main)
        {
            Position = _Position;
            MainForm = _Main;
            DoOnStart();
        }

        public void DoOnStart()
        {
            r1 = new Random(seed);
            Mass = 10;
            Color = Color.White;
            MainBrush = new SolidBrush(Color);
        }

        public void UpdateSeed(int _seed)
        {
            seed = _seed;
            r1 = new Random(seed);
        }

        #endregion

        #region Chunks

        public Chunk curChunks = null; // On which chunks creature exists
        public Chunk lastLeaved = null; // Lastly leaved chunk
        public List<Chunk> searchedChunks = new List<Chunk>(); // Which chunks wehere searched for creaure
        public bool litChunkOnEntry = true; // Decides if chunk should lit while it contain creature
        public int MaxSearchDimension = 2; // How many shells from lastLeaved, program needs to check for creature

        /// <summary>
        /// Checks if chunk is assigned to creature
        /// </summary>
        /// <param name="chunk">Chunk to check</param>
        /// <returns></returns>
        public bool IsChunkAssigned(Chunk chunk)
        {
            return chunk == curChunks;
        }

        /// <summary>
        /// Attach chunk to list if it desn't exist
        /// </summary>
        /// <param name="chunk">Chunk to add</param>
        public void AttachChunk(Chunk chunk)
        {
            curChunks = chunk;
        }

        /// <summary>
        /// Removes given chunk from assigned to creature
        /// </summary>
        /// <param name="chunk"></param>
        public void RemoveChunk(Chunk chunk)
        {
            if (chunk == curChunks)
                curChunks = null;
        }

        /// <summary>
        /// Check if creature is assigned to any chunk
        /// </summary>
        /// <returns></returns>
        public bool AssignedToChunk()
        {
            return curChunks != null;
        }

        /// <summary>
        /// Flags given chunk as lastly visited
        /// </summary>
        /// <param name="chunk">Chunk to flag</param>
        public void FlagChunkLast(Chunk chunk)
        {
            lastLeaved = chunk;
        }

        /// <summary>
        /// Looks for creature in last chunk neighbours
        /// </summary>
        /// <param name="last">Last knonw chunk</param>
        /// <param name="dimesnion">Chow many layers check</param>
        public bool FindInNeighbours(Chunk last, int dimension)
        {
            bool found = false;
            Point lastPosition = last.index;

            // index = x + y * ilość kolumn
            //int loops = 8 * dimension;

            int x, y, index, columns, count, loops;
            columns = MainForm.ChunksColumns();
            count = MainForm.GetChunksCount();
            loops = 2 * dimension + 1;

            for (int i = 0; i < loops; i++)
            {
                if (found)
                    break;

                y = lastPosition.Y - (dimension - i);

                for (int j = 0; j < loops; j++)
                {
                    x = lastPosition.X - (dimension - j);

                    if (x == lastPosition.X && y == lastPosition.Y)
                        continue;

                    index = x + y * columns;

                    if (index >= 0 && index <= count - 1)
                    {
                        Chunk searched = MainForm.GetChunk(index);

                        if (!searchedChunks.Contains(searched))
                        {
                            searched.Searched(true);
                            searchedChunks.Add(searched);

                            if (MainForm.ChunkContainCreature(this, index))
                            {
                                searched.AddCreature(this);
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// Updates ocupied chunks
        /// </summary>
        public void UpdateChunks()
        {
            if (lastPosition != Position)
            {
                if (!AssignedToChunk())
                {
                    bool sucess = false;

                    for (int i = 0; i < searchedChunks.Count; i++)
                    {
                        searchedChunks[i].Searched(false);
                    }

                    searchedChunks.Clear();

                    if (lastLeaved != null)
                    {
                        for (int i = 1; i <= MaxSearchDimension; i++)
                        {
                            if (FindInNeighbours(lastLeaved, i))
                            {
                                sucess = true;
                                break;
                            }
                        }
                    }

                    if (!sucess)
                    {
                        if (!MainForm.CheckAllChunks(this))
                            MainForm.AssignCreatureToChunk(this);
                    }
                }
                else
                    curChunks.UpdateCreature(this);
            }

            if (curChunks != null && litChunkOnEntry)
                curChunks.Lit(litChunkOnEntry);
        }

        #endregion

        #region Drawing

        public void Draw(Graphics g)
        {
            if (target != Position && DrawDiagnostics)
                g.FillEllipse(new SolidBrush(Color.Magenta), target.X - 2, target.Y - 2, 4, 4);

            float r = GetRadius();
            g.FillEllipse(MainBrush, Position.X - r * 2, Position.Y - r * 2, r * 4, r * 4);
        }

        #endregion

        #region Steering

        public PointF GetRandomTarget()
        {
            return new PointF(r1.Next(Main.Window.Width), r1.Next(Main.Window.Height));
        }

        public float GetDistance(PointF p1, PointF p2)
        {
            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float GetArea()
        {
            return massMul * Mass;
        }

        public float GetRadius()
        {
            return (float)Math.Sqrt(GetArea() - Math.PI);
        }

        /*
        public Vector CalculateVectors(PointF[] points, double[] mass)
        {
            Vector result = new Vector(0, 0);
            Vector TempVel;

            for (int i = 0; i < points.Length; i++)
            {
                TempVel = new Vector(points[i]);
                TempVel.Sub(new Vector(Position));
                TempVel.Normalise();

                d = GetDistance(Position, points[i]);
                a = (float)(G * mass[i] / (d * d));

                s = a / 2;
                s /= 1;

                TempVel.Mul(s);
                result.Add(TempVel);
            }

            return result;
        }
        */

        public void UpdatePosition()
        {
            lastPosition = Position;
        }

        public void Move()
        {
            if (target != Position)
            {
                Global = new Vector(target);
                Global.Sub(new Vector(Position));
                Global.Normalise();
                Global.Mul(speed);

                Position = new PointF(Position.X + Global.EndT.X, Position.Y + Global.EndT.Y);

                if (GetDistance(Position, target) <= GetRadius() * 2)
                    target = GetRandomTarget();
            }
        }

        #endregion
    }
}
