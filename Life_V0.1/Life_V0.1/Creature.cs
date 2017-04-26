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

        public bool dispalyForces = false; // Toogles drawing forces
        public int forceMagnifier = 100; // Tels program how much it should extend forces vectors, so they are visible

        private Random colorR = new Random(); // Used to generate random colors

        /// <summary>
        /// Initialisez cresture
        /// </summary>
        /// <param name="_Position">Creature start position</param>
        public Creature(PointF _Position)
        {
            Positon = _Position;
            Color = Color.White;
            Size = 20;
            boundryForce = new Vector2(new PointF(0,0));
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
                        new PointF((float)(baseForce.End.X * forceMagnifier + Math.Abs(Positon.X)),
                        (float)(baseForce.End.Y * forceMagnifier + Math.Abs(Positon.Y))));

                    g.DrawEllipse(
                        new Pen(Color.Red), 
                        (float)(baseForce.End.X * forceMagnifier + Math.Abs(Positon.X) - 3), 
                        (float)(baseForce.End.Y * forceMagnifier + Math.Abs(Positon.Y) - 3), 6, 6);
                }

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
    }
}
