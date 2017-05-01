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
        public float MaxSpeed = 2; // Maximum speed of creature
        public float MinSpeed = 0.2f; // Minimum speed of creature
        public float MaxForce = 1.68f; // Maxiumum force

        public float CurEnergy;
        public float CurHealth;
        private float hSub;
        public float eSub = 0.01f;
        public bool Dead = false;
        private Color tempColor;
        public bool eat = false;

        public int Randomer { get; set; }

        public float ViewLength = 100; // How far can creature see
        public bool shuffle = false;

        public List<PointOfInterest> InterestingPoints = new List<PointOfInterest>();
        private PointOfInterest ShufflePoint;
        private List<Vector> SteeringVectors = new List<Vector>();

        private PointOfInterest OutOfBorderForce;
        private float MaxOutOfBorderSpeed = 1000;
        private float MinOutOfBorderSpeed = 10;

        public Vector GlobalForce;
        public bool DeletePointOfInterestOnEntry = true;

        public bool DrawGlobalVector = false;
        public bool DrawFieldOfViev = false;
        public bool DrawShufflePoint = false;
        public bool DrawHealth = false;

        #region Position Variables

        float dx, dy, d, s, f;
        float gA, gB;
        float pass = 0;

        Random r1;

        #endregion

        #region Constructors

        public Creature(float x, float y)
        {
            Position = new PointF(x, y);
            Size = 10;
            Color = Color.White;
            Randomer = 1;
            DoOnStart();
        }

        public Creature(PointF _Position)
        {
            Position = _Position;
            Size = 10;
            Color = Color.White;
            Randomer = 1;
            DoOnStart();
        }

        public Creature(PointF _Position, Color _Color, float _Size, int _seed, float _Health, float _Energy)
        {
            Position = _Position;
            Size = _Size;
            Color = _Color;
            Randomer = _seed;
            Health = _Health;
            Energy = _Energy;
            DoOnStart();
        }

        private void DoOnStart()
        {
            gA = (MaxSpeed - MinSpeed) / MaxForce;
            gB = MinSpeed;

            r1 = new Random(Randomer);
            tempColor = Color;
            CurEnergy = Energy;
            CurHealth = Health;
        }

        #endregion

        #region Drawing

        public void Draw(Graphics g)
        {
            if (DrawFieldOfViev)
                DrawFiledOfView(g);

            g.FillEllipse(new SolidBrush(Color), Position.X - Size / 2, Position.Y - Size / 2, Size, Size);

            if (DrawShufflePoint)
                if (ShufflePoint != null)
                    g.FillEllipse(new SolidBrush(Color), ShufflePoint.Point.X - 2, ShufflePoint.Point.Y - 2, 4, 4);

            /*
            if (GlobalForce != null)
                g.DrawString(pass.ToString(),
                        new Font("Arial", 10),
                        new SolidBrush(Color.White),
                        Position,
                        new StringFormat());
            */

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

            if (DrawGlobalVector)
            {
                if (GlobalForce != null)
                {
                    Vector vievForce = new Vector(GlobalForce);
                    vievForce.Mul(10);
                    vievForce.Translate(Position);
                    g.DrawLine(new Pen(Color.LightPink), Position, vievForce.EndT);
                }
            }

            if (DrawHealth)
            {
                g.DrawString("H: " + CurHealth,
                    new Font("Arial", 7),
                    new SolidBrush(Color.White),
                    Position,
                    new StringFormat());
            }
        }

        public void DrawFiledOfView(Graphics g)
        {
            if (GlobalForce != null)
            {
                g.DrawEllipse(new Pen(Color.LightGreen), Position.X - (ViewLength + Size / 2), Position.Y - (ViewLength + Size / 2), ViewLength * 2 + Size, ViewLength * 2 + Size);
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

        public float GetDistance(PointF p1, PointF p2)
        {
            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void CheckHelath()
        {
            if (CurEnergy < 0.25 * Energy)
                eat = true;

            if (CurEnergy > 0.75 * Energy)
                eat = false;

            if (CurEnergy < 0.2 * Energy)
            {
                hSub = 0.001f;

                if (CurHealth <= Health)
                {
                    byte r = (byte)(CurHealth * tempColor.R);
                    byte g = (byte)(CurHealth * tempColor.G);
                    byte b = (byte)(CurHealth * tempColor.B);

                    if (r < 0.2 * tempColor.R)
                        r = (byte)(0.2 * tempColor.R);

                    if (g < 0.2 * tempColor.G)
                        g = (byte)(0.2 * tempColor.G);

                    if (b < 0.2 * tempColor.B)
                        b = (byte)(0.2 * tempColor.B);

                    Color = Color.FromArgb(r, g, b);

                }
                else
                    Color = tempColor;

                if (CurHealth <= 0)
                {
                    Dead = true;
                }
                else
                    CurHealth -= hSub;
            }
            else

            if (CurEnergy >= 0.6 * Energy)
            {
                CurHealth += 0.01f;

                if (Size < 50)
                    Size += 0.001f;

                if (MinSpeed < MaxSpeed)
                    MinSpeed += 0.0001f;

                ViewLength += 0.00001f;

            }

            CurEnergy -= eSub;
        }

        public void Move()
        {
            SteeringVectors.Clear();

            shuffle = true;

            if (eat)
            {
                for (int i = InterestingPoints.Count - 1; i >= 0; i--)
                {
                    d = GetDistance(Position, InterestingPoints[i].Point);

                    if (d <= ViewLength + Size)
                    {
                        shuffle = false;

                        if (ShufflePoint != null)
                            ShufflePoint = null;

                        Vector force = new Vector(InterestingPoints[i].Point);
                        force.Sub(new Vector(Position));
                        force.Normalise();

                        f = 10 * (Size / (d * d));
                        s = (float)(InterestingPoints[i].Force * f);

                        if (d < Size - Size * 0.5f)
                        {
                            if (DeletePointOfInterestOnEntry)
                            {
                                InterestingPoints.RemoveAt(i);
                                CurEnergy += 0.5f;
                            }
                            else
                                force.Mul(0);
                        }
                        else
                            force.Mul(InterestingPoints[i].Force * f);

                        SteeringVectors.Add(force);
                    }
                }
            }

            GlobalForce = new Vector(0, 0);

            if (shuffle)
            {
                if (ShufflePoint == null)
                {
                    PointF shuffleP;

                    if (eat)
                    {
                        while (true)
                        {
                            shuffleP = new PointF(r1.Next((int)(Position.X - (ViewLength + Size + 100)), (int)(Position.X + ViewLength + Size + 100)), r1.Next((int)(Position.Y - (ViewLength + Size + 100)), (int)(Position.Y + ViewLength + Size + 100)));

                            if (Main.Border.Contains((int)shuffleP.X, (int)shuffleP.Y))
                                break;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            shuffleP = new PointF(r1.Next((int)(Position.X - (ViewLength + Size)), (int)(Position.X + ViewLength + Size)), r1.Next((int)(Position.Y - (ViewLength + Size)), (int)(Position.Y + ViewLength + Size)));

                            if (GetDistance(Position, shuffleP) < ViewLength + Size && Main.Border.Contains((int)shuffleP.X, (int)shuffleP.Y))
                                break;
                        }
                    }

                    ShufflePoint = new PointOfInterest(shuffleP, (float)r1.NextDouble());
                }
                else
                {
                    d = GetDistance(Position, ShufflePoint.Point);

                    Vector force = new Vector(ShufflePoint.Point);
                    force.Sub(new Vector(Position));
                    force.Normalise();

                    f = 10 * (Size / (d * d));
                    s = (float)(ShufflePoint.Force * f);

                    if (d < Size - Size * 0.5f)
                    {
                        ShufflePoint = null;
                    }
                    else
                        force.Mul(ShufflePoint.Force * f);

                    SteeringVectors.Add(force);
                }
            }

            if (OutOfBorderForce == null)
            {
                if (SteeringVectors.Count > 0)
                {
                    GlobalForce.Add(SteeringVectors.ToArray());

                    s = GlobalForce.GetLength();
                    s = gA * s + gB;

                    GlobalForce.Normalise();
                    GlobalForce.Mul(s);

                    if (shuffle)
                        s = MinSpeed;
                    else
                        s = MaxSpeed;

                    if (GlobalForce.GetLength() > s)
                    {
                        GlobalForce.Normalise();
                        GlobalForce.Mul(s);
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
                dx = Main.Window.Width / 2 - Position.X;
                dy = Main.Window.Height / 2 - Position.Y;

                d = (float)Math.Sqrt(dx * dx + dy * dy);

                f = (float)(d / 100);

                if (f > MaxOutOfBorderSpeed)
                    f = MaxOutOfBorderSpeed;
                else if (f < MinOutOfBorderSpeed)
                    f = MinOutOfBorderSpeed;

                if (OutOfBorderForce == null)
                {
                    OutOfBorderForce = new PointOfInterest(new PointF(Main.Window.Width / 2, Main.Window.Height / 2), f);
                }
            }
            else
                OutOfBorderForce = null;
        }

        #endregion
    }
}
