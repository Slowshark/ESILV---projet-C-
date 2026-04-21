using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public abstract class Transformation
    {
        public abstract string ToSvg();
    }
    public class Translation : Transformation
    {
        double dx;
        double dy;
        public Translation(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
        public override string ToSvg()
        {
            return $"translate({dx},{dy})";
        }
    }
    public class Rotation : Transformation
    {
        double angle;
        double cx;
        double cy;

        public Rotation(double angle, double cx, double cy)
        {
            this.angle = angle;
            this.cx = cx;
            this.cy = cy;
        }
        public override string ToSvg()
        {
            return $"rotate({angle},{cx},{cy})";
        }
    }
}
