using System;
using System.Drawing;

namespace Life_V0._1
{
    public partial class Vector
    {
        public PointF Start { get; set; } // Translated start postion
        public PointF End { get; set; } // End of vector relative to orygin
        public PointF EndT { get; set; } // Translated End
        private float Length { get; set; } // Length of vector

        #region Constructors

        public Vector(float x, float y)
        {
            End = new PointF(x, y);
            EndT = End;
            Start = new PointF(0, 0);
        }

        public Vector(PointF _End)
        {
            End = _End;
            EndT = End;
            Start = new PointF(0, 0);
        }

        public Vector(Vector vector)
        {
            End = vector.End;
            EndT = vector.EndT;
            Start = vector.Start;
        }

        #endregion

        #region Operations

        public float GetLength()
        {
            return (float)Math.Sqrt(End.X * End.X + End.Y * End.Y);
        }

        public void Inverse()
        {
            End = new PointF(-End.X, -End.Y);
            EndT = new PointF(End.X + Math.Abs(Start.X), End.Y + Math.Abs(Start.Y));
        }

        public void Normalise()
        {
            float l = GetLength();

            End = new PointF(End.X / l, End.Y / l);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        #region Translate

        public void Translate(float x, float y)
        {
            Start = new PointF(x, y);
            EndT = new PointF(End.X + x, End.Y + y);
        }

        public void Translate(PointF _Start)
        {
            Start = _Start;
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        #endregion

        #region Adding

        public void Add(float constant)
        {
            End = new PointF(End.X + constant, End.Y + constant);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Add(Vector vector)
        {
            End = new PointF(End.X + vector.End.X, End.Y + vector.End.Y);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Add(Vector[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                End = new PointF(End.X + vectors[i].End.X, End.Y + vectors[i].End.Y);
            }

            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        #endregion

        #region Subtracting

        public void Sub(float constant)
        {
            End = new PointF(End.X - constant, End.Y - constant);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Sub(Vector vector)
        {
            End = new PointF(End.X - vector.End.X, End.Y - vector.End.Y);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Sub(Vector[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                End = new PointF(End.X - vectors[i].End.X, End.Y - vectors[i].End.Y);
            }

            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        #endregion

        #region Multiplying

        public void Mul(float constant)
        {
            End = new PointF(End.X * constant, End.Y * constant);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Mul(Vector vector)
        {
            End = new PointF(End.X * vector.End.X, End.Y * vector.End.Y);
            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        public void Mul(Vector[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                End = new PointF(End.X * vectors[i].End.X, End.Y * vectors[i].End.Y);
            }

            EndT = new PointF(End.X + Start.X, End.Y + Start.Y);
        }

        #endregion

        #endregion

    }
}
