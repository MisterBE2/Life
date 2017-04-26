using System;
using System.Drawing;

namespace Life_V0._1
{
    class Vector2
    {
        public PointF Start { get; set; } // Origin shift point
        public PointF End { get; set; } // End point (direction)
        public double Length { get; set; } // Length of vector
        public double Angle { get; set; } // Angle from +X axis

        public double[] normal = new double[2]; // Stores delat x an Y between start and end - [0] is always x, [1] is always y

        /// <summary>
        /// Defines basic vector values
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public Vector2(float x, float y)
        {
            Start = new PointF(0, 0);
            End = new PointF(x, y);
            Angle = GetAngle();
            Length = GetLength();
            GetNormal();
        }

        /// <summary>
        /// Defines basic vector values
        /// </summary>
        /// <param name="_End"></param>
        public Vector2(PointF _End)
        {
            Start = new PointF(0, 0);
            End = _End;
            Angle = GetAngle();
            Length = GetLength();
            GetNormal();
        }

        /// <summary>
        /// Defines basic vector values
        /// </summary>
        /// <param name="_Start">Origin point</param>
        /// <param name="_End">End point (direction)</param>
        public Vector2(PointF _Start, PointF _End)
        {
            Start = _Start;
            End = _End;
            Angle = GetAngle();
            Length = GetLength();
            GetNormal();
        }

        /// <summary>
        /// Defines basic vector values
        /// </summary>
        /// <param name="_Start">Origin point</param>
        /// <param name="_Angle">Angle from +X axis</param>
        /// <param name="_Length">Lenght of vector</param>
        public Vector2(PointF _Start, double _Angle, double _Length)
        {
            Start = _Start;
            Angle = _Angle;
            Length = _Length;
            End = GetEndPoint();
            GetNormal();
        }

        /// <summary>
        /// Returns angle of a vector
        /// </summary>
        /// <returns></returns>
        public double GetAngle()
        {
            return 90 - (Math.Atan2(normal[0], normal[1]) * 180 / Math.PI);
        } // May cause some errors, due to shifting phase by 90 degree

        /// <summary>
        /// Returns lenght of a vector
        /// </summary>
        /// <returns></returns>
        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(Math.Abs(normal[0]), 2) + Math.Pow(Math.Abs(normal[1]), 2));
        }

        /// <summary>
        /// Calculates End point based on Angle and Lenght
        /// </summary>
        /// <returns></returns>
        public PointF GetEndPoint()
        {
            double x = Math.Ceiling(Math.Cos((Angle) * 0.01745329252) * Length);
            double y = Math.Ceiling(Math.Sin((Angle) * 0.01745329252) * Length);

            return new PointF((float)x, (float)y);
        } // May cause some errors, due to not changing Lenght proportions on X anf Y axis

        /// <summary>
        /// Calculates distance petween Start X and End X, Start Y, End Y
        /// </summary>
        public void GetNormal()
        {
            normal[0] = Start.X - End.X;
            normal[1] = Start.Y - End.Y;
        }
    
        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="vector">Vector to add</param>
        public void Add(Vector2 vector)
        {
            End = new PointF((float)(End.X + vector.End.X), (float)(End.Y + vector.End.Y));
        }

        /// <summary>
        /// Adds constant to vector
        /// </summary>
        /// <param name="ammount">Constant</param>
        public void Add(float ammount)
        {
            End = new PointF((float)(End.X + ammount), (float)(End.Y + ammount));
        }

        /// <summary>
        /// Adds vector list
        /// </summary>
        /// <param name="vectors">Vectors to add</param>
        public void Add(Vector2[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
                End = new PointF((float)(End.X + vectors[i].End.X), (float)(End.Y + vectors[i].End.Y));
        }

        /// <summary>
        /// Divides vector by ammount
        /// </summary>
        /// <param name="ammount">Divide factor</param>
        public void Div(float ammount)
        {
            End = new PointF((float)(End.X / ammount), (float)(End.Y / ammount));
        }

        /// <summary>
        /// Divides vector by another vactor
        /// </summary>
        /// <param name="vector">Divide factor</param>
        public void Div(Vector2 vector)
        {
            End = new PointF((float)(End.X / vector.End.X), (float)(End.Y / vector.End.Y));
        }

        /// <summary>
        /// Draws vector to given buffer
        /// </summary>
        /// <param name="g">Targeted buffer</param>
        /// <param name="c">Color of lines</param>
        public void Draw(Graphics g, Color c)
        {
            PointF newEnd = new PointF(End.X + Math.Abs(Start.X), End.Y + Math.Abs(Start.Y));
            g.DrawLine(new Pen(c), Start, newEnd);
            g.DrawEllipse(new Pen(c), newEnd.X - 3, newEnd.Y - 3, 6, 6);
        }

        /// <summary>
        /// Inverts vestor
        /// </summary>
        public void Invert()
        {
            End = new PointF(-End.X, - End.Y);
        }
    }
}
