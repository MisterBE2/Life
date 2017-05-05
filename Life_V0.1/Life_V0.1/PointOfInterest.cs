using System.Drawing;

namespace Life_V0._1
{
    public partial class PointOfInterest
    {
        public PointF Point { get; set; } // Interesting point
        public float Force { get; set; } // How attractive is this point
        public float Value { get; set; } // What gives this point
        public bool IsGood { get; set; } // Is this point good
        public bool Visible { get; set; } // Is this point visible

        public bool ToRemove = false;

        public PointOfInterest(PointF _Point, float _Force, bool _Good, float _Value)
        {
            Point = _Point;
            Force = _Force;
            Visible = true;
            Value = _Value;
            IsGood = _Good;
        }

        public PointOfInterest(PointF _Point)
        {
            Point = _Point;
            Force = 0.1f;
            Visible = true;
            Value = 0.5f;
            IsGood = true;
        }

        public PointOfInterest(PointF _Point, float _Force)
        {
            Point = _Point;
            Force = _Force;
            Visible = true;
            Value = 0.5f;
            IsGood = true;
        }

        public void Consume(float val)
        {

            Value = Value - val;

            if (val < 0)
                ToRemove = true;
        }
    }
}
