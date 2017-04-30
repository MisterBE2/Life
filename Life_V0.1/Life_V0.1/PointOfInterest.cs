using System.Drawing;

namespace Life_V0._1
{
    public partial class PointOfInterest
    {
        public PointF Point { get; set; } // Interesting point
        public float Force { get; set; } // How attractive is this point
        public bool Visible { get; set; } // Is this point visible

        public PointOfInterest(PointF _Point, float _Force)
        {
            Point = _Point;
            Force = _Force;
            Visible = true;
        }
    }
}
