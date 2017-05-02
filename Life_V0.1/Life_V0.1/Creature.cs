using System.Drawing;
using System.Collections.Generic;
using System;

namespace Life_V0._1
{
    public partial class Creature
    {
        public PointF Position { get; set; }
        public float Size { get; set; }
        public Color Color { get; set; }
        public float Health { get; set; }
        public float Energy { get; set; }
        public float MaxSpeed = 2; // Maximum speed of creature
        public float MinSpeed = 0.2f; // Minimum speed of creature
        public float MaxForce = 1.68f; // Maxiumum force

        public float gorwSpeed = 0.001f;
        public float MaxSize = 50;
        public long age = 0;
        public long matureAge = 1000;
        public long LifeDuration = 10000;
        public int MaxChildern = 25;
        public float GoodForce = 1;
        public float BadForce = -1;
        public float EatSpeed = 0.5f;
        public float CurEnergy;
        public float CurHealth;
        private float hSub;
        public float eSub = 0.01f;
        public bool Dead = false;
        private Color tempColor;
        public bool eat = false;
        public bool stopEat = false;

        public int Randomer { get; set; }

        public float ViewLength = 100; // How far can creature see
        public float tempView; // Hom far can creature see while eating
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

        Main MainForm;

        #region Position Variables

        float dx, dy, d, s, f;
        float gA, gB;
        float pass = 0;

        Random r1;

        #endregion

        #region Constructors

        public Creature(float x, float y, Main _MainForm)
        {
            Position = new PointF(x, y);
            Size = 10;
            Color = Color.White;
            Randomer = 1;
            MainForm = _MainForm;
            DoOnStart();
        }

        public Creature(PointF _Position, Main _MainForm)
        {
            Position = _Position;
            Size = 10;
            Color = Color.White;
            Randomer = 1;
            MainForm = _MainForm;
            DoOnStart();
        }

        public Creature(PointF _Position, Color _Color, float _Size, int _seed, float _Health, float _Energy, Main _MainForm)
        {
            Position = _Position;
            Size = _Size;
            Color = _Color;
            Randomer = _seed;
            Health = _Health;
            Energy = _Energy;
            MainForm = _MainForm;
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
            tempView = ViewLength;
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

                g.DrawString("E: " + CurEnergy,
                    new Font("Arial", 7),
                    new SolidBrush(Color.White),
                    new PointF(Position.X, Position.Y + 10),
                    new StringFormat());

                g.DrawString("Eat: " + eat,
                    new Font("Arial", 7),
                    new SolidBrush(Color.White),
                    new PointF(Position.X, Position.Y + 20),
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
            //PointOfInterest myPoint = new PointOfInterest(_Point, _Force);
            //InterestingPoints.Add(myPoint);
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
            if (age >= LifeDuration)
                Dead = true;

            if (age >= matureAge)
            {
                bool clone = r1.NextDouble() > 0.95;
                if (clone)
                {
                    if (CurEnergy > 1.5 * Energy)
                    {
                        int sizeJump = 21 + r1.Next(-10, 10);

                        if (Size >= sizeJump)
                        {
                            int children = r1.Next(5, MaxChildern);

                            for (int i = 0; i < children; i++)
                            {
                                Creature child = new Creature(Position, tempColor, 20f / children, r1.Next(1000), Health + 0.01f, Energy + 0.01f, MainForm);

                                child.BadForce += 0.01f;
                                child.GoodForce += 0.01f;
                                child.EatSpeed += 0.01f;
                                child.MinSpeed += 0.01f;
                                child.MaxSpeed += 0.01f;
                                child.MaxForce += 0.01f;
                                child.LifeDuration += 1;
                                child.MaxChildern += 1;
                                child.MaxSize += 1;
                                child.gorwSpeed += 0.1f;

                                if (r1.NextDouble() > 0.8)
                                    child.Health += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.Energy += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.GoodForce += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.BadForce += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.MinSpeed += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.MaxForce += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.MaxSpeed += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.MaxForce += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.EatSpeed += r1.Next(-1000, 1000) / 1000f;

                                if (r1.NextDouble() > 0.8)
                                    child.LifeDuration += r1.Next(-10, 10);

                                if (r1.NextDouble() > 0.8)
                                    child.MaxChildern += r1.Next(-2, 10);

                                MainForm.AddCreature(child);
                            }

                            Size -= sizeJump;

                            if (Size <= 0)
                                Dead = true;

                            CurEnergy -= 0.5f * Energy;
                        }
                    }
                }
            }

            if (CurEnergy < 0.25 * Energy)
                eat = true;


            if (stopEat)
            {
                if (CurEnergy > 0.75 * Energy + r1.NextDouble())
                {
                    eat = false;
                    stopEat = false;
                }
            }

            if (CurEnergy < 0.2 * Energy)
            {
                if (age > matureAge * 0.5)
                    hSub = 0.001f;
                else
                    hSub = 0;

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

                if (Size < MaxSize)
                    Size += gorwSpeed + CurEnergy * 0.001f;

                if (MinSpeed < MaxSpeed)
                    MinSpeed += 0.001f;

                ViewLength += 0.0001f;

            }

            CurEnergy -= eSub;
        }

        public void Move()
        {
            age++;

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
                        s = (float)(InterestingPoints[i].Value * f);

                        if (InterestingPoints[i].IsGood)
                            s = s * GoodForce;
                        else
                            s = s * BadForce;

                        if(d < 1 && InterestingPoints[i].Value < 0.5)
                            InterestingPoints.RemoveAt(i);

                        if (d < Size + InterestingPoints[i].Value)
                        {
                            if (DeletePointOfInterestOnEntry)
                            {
                                //float bite = EatSpeed + Size / 100.0f + Math.Abs(Energy - CurEnergy) * 0.3f;
                                //float bite = EatSpeed + Math.Abs(Energy - CurEnergy) * 0.01f;
                                float bite = EatSpeed;

                                if (InterestingPoints[i].Value < EatSpeed)
                                    bite = InterestingPoints[i].Value;

                                if (InterestingPoints[i].IsGood)
                                    CurEnergy += bite;
                                else
                                {
                                    CurEnergy -= bite;
                                    CurHealth -= bite / 10;
                                }

                                if (InterestingPoints[i].Value - EatSpeed < 0)
                                    InterestingPoints.RemoveAt(i);

                                if (bite < 0)
                                    bite = -bite;

                                InterestingPoints[i].Value = InterestingPoints[i].Value - bite;

                                if (r1.NextDouble() > 0.55)
                                    stopEat = true;
                            }
                            else
                                force.Mul(0);
                        }
                        else
                            force.Mul(s);

                        SteeringVectors.Add(force);
                    }
                }
            }

            GlobalForce = new Vector(0, 0);

            if (shuffle)
            {
                if (ShufflePoint == null)
                {
                    //Rectangle r = new Rectangle(Main.Border.X + 100, Main.Border.Y + 100, Main.Border.Width - 100, Main.Border.Height - 100);
                    PointF shuffleP = new PointF(0,0);

                    if (eat)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            shuffleP = new PointF(r1.Next((int)(Position.X - (ViewLength + Size + 100)), (int)(Position.X + ViewLength + Size + 100)), r1.Next((int)(Position.Y - (ViewLength + Size + 100)), (int)(Position.Y + ViewLength + Size + 100)));

                            if (Main.ShuffleBorder.Contains((int)shuffleP.X, (int)shuffleP.Y))
                                break;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            shuffleP = new PointF(r1.Next((int)(Position.X - (ViewLength + Size)), (int)(Position.X + ViewLength + Size)), r1.Next((int)(Position.Y - (ViewLength + Size)), (int)(Position.Y + ViewLength + Size)));

                            if (GetDistance(Position, shuffleP) < ViewLength + Size && Main.ShuffleBorder.Contains((int)shuffleP.X, (int)shuffleP.Y))
                                break;
                        }
                    }

                    if(shuffleP.X != 0 && shuffleP.Y != 0)
                        ShufflePoint = new PointOfInterest(shuffleP);
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
