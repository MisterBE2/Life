using System.Drawing;
using System.Collections.Generic;
using System;

namespace Life_V0._1
{
    class Creature
    {
        public PointF Positon { get; set; } // Stores position of the Creature
        public Color Color { get; set; } // Stores creature color
        public float Size { get; set; } // Creture size

        public List<Vector2> forces = new List<Vector2>(); // Stores all movment vestors
        private List<Color> fColors = new List<Color>(); // Stores colors of forces
        public Vector2 baseForce;
        public Vector2 boundryForce; // Force which prevents creature to go outside of border box
        public bool outOfBoundry = false; // Tells if creaute is out of boundry (Border)

        private static float hystAmmout = 0.6f; // Ammpont of hysteresis when creature comes out of border
        private static float maxBorderSpeed = 0.1f; // Maximum speed when out of border
        private static float minBorderSpeed = 0.001f; // Minimum speed when out of border
        private static float maxDistanceOutOfBorder = 1000; // Distance after which maxBorderSpeed is triggered

        public bool dispalyForces = false; // Toogles drawing forces
        public bool fastForce = true; // Toggles drawing more foce informations
        public int forceMagnifier = 4; // Tels program how much it should extend forces vectors, so they are visible

        private Color tempColor; // Stores color of creature when outo f boundry

        private Random colorR = new Random(); // Used to generate random colors

        /// <summary>
        /// Initialisez cresture
        /// </summary>
        /// <param name="x">X positon</param>
        /// <param name="y">Y position</param>
        public Creature(float x, float y)
        {
            Positon = new PointF(x, y);
            Color = Color.White;
            tempColor = Color;
            Size = 20;
            boundryForce = new Vector2(new PointF(0, 0));
        }

        /// <summary>
        /// Initialisez cresture
        /// </summary>
        /// <param name="_Position">Creature start position</param>
        public Creature(PointF _Position)
        {
            Positon = _Position;
            Color = Color.White;
            tempColor = Color;
            Size = 20;
            boundryForce = new Vector2(new PointF(0, 0));
        }

        /// <summary>
        /// Initialisez cresture
        /// </summary>
        /// <param name="_Position">Creature start position</param>
        /// <param name="_Color">Creature color</param>
        /// <param name="_Size">Creature size</param>
        public Creature(PointF _Position, Color _Color, float _Size)
        {
            Positon = _Position;
            Color = _Color;
            Size = _Size;
            boundryForce = new Vector2(new PointF(0, 0));
            tempColor = Color;
        }

        /// <summary>
        /// Moves creature by all forces
        /// </summary>
        public void Move()
        {
            baseForce = new Vector2(Positon, new PointF(0, 0));
            baseForce.Add(forces.ToArray());
            baseForce.Add(boundryForce);

            Positon = new PointF(Positon.X + baseForce.End.X, Positon.Y + baseForce.End.Y);
        }

        /// <summary>
        /// Draws the Creatue to given buffer
        /// </summary>
        /// <param name="g">Targeted buffer</param>
        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), (float)(Positon.X - Size / 2), (float)(Positon.Y - Size / 2), Size, Size);

            if (dispalyForces)
            {
                if (baseForce != null)
                {
                    g.DrawLine(
                        new Pen(Color.Red),
                        Positon,
                        new PointF((float)(baseForce.End.X * forceMagnifier + Math.Abs(Positon.X - baseForce.End.X)),
                        (float)(baseForce.End.Y * forceMagnifier + Math.Abs(Positon.Y - baseForce.End.Y))));

                    if (!fastForce)
                        g.DrawEllipse(
                            new Pen(Color.Red),
                            (float)(baseForce.End.X * forceMagnifier + Math.Abs(Positon.X) - 3),
                            (float)(baseForce.End.Y * forceMagnifier + Math.Abs(Positon.Y) - 3), 6, 6);
                }

                if (!fastForce)
                {
                    for (int i = 0; i < forces.Count; i++)
                    {
                        g.DrawLine(
                            new Pen(fColors[i]),
                            Positon,
                            new PointF((float)(forces[i].End.X * forceMagnifier + Math.Abs(Positon.X)),
                            (float)(forces[i].End.Y * forceMagnifier + Math.Abs(Positon.Y))));
                        g.DrawEllipse(
                            new Pen(fColors[i]),
                            (float)(forces[i].End.X * forceMagnifier + Math.Abs(Positon.X) - 3),
                            (float)(forces[i].End.Y * forceMagnifier + Math.Abs(Positon.Y) - 3), 6, 6);
                    }
                }
            }
        }

        /// <summary>
        /// Insert new force, to drive creature
        /// </summary>
        /// <param name="f">Force</param>
        public void AttachForce(Vector2 f)
        {
            forces.Add(f);
            fColors.Add(Color.FromArgb(colorR.Next(255), colorR.Next(255), colorR.Next(255)));
        }

        /// <summary>
        /// Tels if creature is insde of a box
        /// </summary>
        /// <param name="colBox">Box to check</param>
        /// <returns></returns>
        public bool IsColide(Rectangle colBox)
        {
            return colBox.Contains((int)Positon.X, (int)Positon.Y);
        }

        /// <summary>
        /// Moves creature back inside border
        /// </summary>
        /// <param name="Border">End of creatures world</param>
        /// <param name="Window">Visible window working area</param>
        public void CheckBoundry(Rectangle Border, Rectangle Window)
        {
            Rectangle hyst = new Rectangle();
            hyst.Location = new Point(
                (int)(Border.X + hystAmmout * Border.X),
                (int)(Border.Y + hystAmmout * Border.Y));
            hyst.Size = new Size(
                (int)(Border.Width - 2 * (hystAmmout * Border.X)),
                (int)(Border.Height - 2 * (hystAmmout * Border.Y)));

            if (!IsColide(Border))
                outOfBoundry = true;

            if (IsColide(hyst))
            {
                outOfBoundry = false;

                boundryForce = new Vector2(boundryForce.End.X / 1.05f, boundryForce.End.Y / 1.05f);

                //if ((boundryForce.End.X > -0.01 && boundryForce.End.X < 0.01) || (boundryForce.End.Y > -0.01 && boundryForce.End.Y < 0.01))
                    //boundryForce = new Vector2(0, 0);
            }

            if (!outOfBoundry)
                Color = tempColor;

            else
            {
                double dx = Math.Abs(Positon.X - Window.Width / 2);
                double dy = Math.Abs(Positon.Y - Window.Height / 2);

                double distance = Math.Sqrt(dx * dx + dy * dy);

                double a = (minBorderSpeed - maxBorderSpeed) / (Window.Height / 2 - maxDistanceOutOfBorder);
                double b = (Window.Height / 2) / (a * minBorderSpeed);

                float speed = (float)(a * distance + b);

                if (speed > maxBorderSpeed)
                    speed = maxBorderSpeed;
                else if (speed < minBorderSpeed)
                    speed = minBorderSpeed;

                Color = Color.Red;

                if (Positon.X >= Window.Width / 2)
                    boundryForce.Add(new Vector2(-speed, 0));
                else if (Positon.X < Window.Height / 2)
                    boundryForce.Add(new Vector2(speed, 0));

                if (Positon.Y >= Window.Height / 2)
                    boundryForce.Add(new Vector2(0, -speed));
                else if (Positon.Y < Window.Width / 2)
                    boundryForce.Add(new Vector2(0, speed));
            }
        }
    }
}
