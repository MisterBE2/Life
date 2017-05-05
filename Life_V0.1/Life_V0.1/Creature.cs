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

        public float massMul = 0.5f; // 1 m^2 = massMul kg

        #region Global variables

        float dx, dy, x, y, a, b, d, f, v, s; // Needed for gravity calculations

        #endregion

        #region Constructors

        public Creature(float x, float y)
        {
            Position = new PointF(x, y);
            Mass = 10;
            Color = Color.White;
        }

        public Creature(PointF _Position)
        {
            Position = _Position;
            Mass = 10;
            Color = Color.White;
        }

        #endregion

        #region Drawing

        public void Draw(Graphics g)
        {
            float r = GetRadius();
            g.FillEllipse(new SolidBrush(Color), Position.X - r, Position.Y - r, r * 2, r * 2);
        }

        #endregion

        #region Steering

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

        public void Move()
        {
           
        }

        #endregion
    }
}
