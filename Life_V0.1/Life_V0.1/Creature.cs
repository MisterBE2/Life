using System.Drawing;
using System.Collections.Generic;
using System;

namespace Life_V0._1
{
    class Creature
    {
        public PointF Position { get; set; }
        public float Size { get; set; }
        public Color Color { get; set; }
        public float Health { get; set; }
        public float Energy { get; set; }
        public float MaxSpeed = 5;
        public float MinSpeed = 1f;

        public List<PointOfInterest> InterestingPoints = new List<PointOfInterest>();
        private List<Vector> SteeringVectors = new List<Vector>();

        private PointOfInterest OutOfBorderForce;
        private float MaxOutOfBorderSpeed = 1000;
        private float MinOutOfBorderSpeed = 10;

        public Vector GlobalForce;
        public bool DisplayGlobalVector = false;
        public bool DeletePointOfInterestOnEntry = true;

        #region Constructors

        public Creature(float x, float y)
        {
            Position = new PointF(x, y);
            Size = 10;
            Color = Color.White;
        }

        public Creature(PointF _Position)
        {
            Position = _Position;
            Size = 10;
            Color = Color.White;
        }

        public Creature(PointF _Position, Color _Color, float _Size)
        {
            Position = _Position;
            Size = _Size;
            Color = _Color;
        }

        #endregion

        #region Drawing

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), Position.X - Size / 2, Position.Y - Size / 2, Size, Size);

            if (GlobalForce != null)
            {
                Vector front = new Vector(GlobalForce);

                if (front.End != new PointF(0, 0))
                {
                    front.Normalise();
                    front.Mul(Size / 2);
                    front.Translate(Position);

                    g.FillEllipse(new SolidBrush(Color), front.EndT.X - Size / 4, front.EndT.Y - Size / 4, Size / 2, Size / 2);
                }
            }

            /*
            if (InterestingPoints.Count > 0)
            {
                double dx = InterestingPoints[0].Point.X - Position.X;
                double dy = InterestingPoints[0].Point.Y - Position.Y;

                int d = (int)Math.Sqrt(dx * dx + dy * dy);

                g.DrawString("D = " + d,
                    new Font("Arial", 10),
                    new SolidBrush(Color.White),
                    Position,
                    new StringFormat());
            }
            */

            if (DisplayGlobalVector)
            {
                if (GlobalForce != null)
                {
                    Vector vievForce = new Vector(GlobalForce);
                    vievForce.Mul(10);
                    vievForce.Translate(Position);
                    g.DrawLine(new Pen(Color.LightPink), Position, vievForce.EndT);
                }
            }
        }

        #endregion

        #region Steering

        public void AddPointOfInterest(PointF _Point, float _Force)
        {
            PointOfInterest myPoint = new PointOfInterest(_Point, _Force);
            InterestingPoints.Add(myPoint);
        }

        public void ClearPointsOfInterest()
        {
            InterestingPoints.Clear();
        }

        public void Move()
        {
            SteeringVectors.Clear();
            float speed;

            for (int i = InterestingPoints.Count - 1; i >= 0; i--)
            {
                Vector force = new Vector(InterestingPoints[i].Point);
                force.Sub(new Vector(Position));
                force.Normalise();

                double dx = InterestingPoints[i].Point.X - Position.X;
                double dy = InterestingPoints[i].Point.Y - Position.Y;

                double d = Math.Sqrt(dx * dx + dy * dy);
                double f = 1000 * (Size / (d * d));

                speed = (float)(InterestingPoints[i].Force * f);

                if (d < Size)
                {
                    if (DeletePointOfInterestOnEntry)
                        InterestingPoints.RemoveAt(i);
                    else
                        force.Mul(0);
                }
                else if (d < Size * 10)
                {
                    speed = (float)(speed * Size / f);

                    force.Mul(speed);
                }
                else
                    force.Mul(speed);

                SteeringVectors.Add(force);
            }

            GlobalForce = new Vector(0, 0);

            if (OutOfBorderForce == null)
            {
                if (SteeringVectors.Count > 0)
                {
                    GlobalForce.Add(SteeringVectors.ToArray());

                    speed = GlobalForce.GetLength();

                    if (speed > MaxSpeed)
                    {
                        GlobalForce.Normalise();
                        GlobalForce.Mul(MaxSpeed);
                    }
                    else if (speed < MinSpeed)
                    {
                        GlobalForce.Normalise();
                        GlobalForce.Mul(MinSpeed);
                    }
                }
            }
            else
            {
                Vector force = new Vector(OutOfBorderForce.Point);
                force.Sub(new Vector(Position));
                force.Normalise();
                force.Mul(OutOfBorderForce.Force);

                GlobalForce = force;
            }

            Position = new PointF(Position.X + GlobalForce.End.X, Position.Y + GlobalForce.End.Y);

        }

        public void CheckOutOfBorder(Rectangle border)
        {
            if (!border.Contains((int)Position.X, (int)Position.Y))
            {
                double dx = Main.Window.Width / 2 - Position.X;
                double dy = Main.Window.Height / 2 - Position.Y;

                double d = Math.Sqrt(dx * dx + dy * dy);

                float force = (float)(d / 100);

                if (force > MaxOutOfBorderSpeed)
                    force = MaxOutOfBorderSpeed;
                else if (force < MinOutOfBorderSpeed)
                    force = MinOutOfBorderSpeed;

                if (OutOfBorderForce == null)
                {
                    OutOfBorderForce = new PointOfInterest(new PointF(Main.Window.Width / 2, Main.Window.Height / 2), force);
                }
            }
            else
                OutOfBorderForce = null;
        }

        #endregion
    }
}
