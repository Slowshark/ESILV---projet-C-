using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Ellipse : Forme
    {
        // Centre et rayons de l'ellipse
        double cx;
        double cy;
        double rx; // Rayon en X
        double ry; // Rayon en Y
        public Ellipse(int idelement, int ordre, int R, int G, int B, double cx, double cy, double rx, double ry)
            : base(idelement, ordre, R, G, B)
        {
            // Initialisation des paramètres
            this.cx = cx;
            this.cy = cy;
            this.rx = rx;
            this.ry = ry;
        }
        public override string GenererBaliseSvg()
        {
            return $"  <ellipse cx=\"{cx}\" cy=\"{cy}\" rx=\"{rx}\" ry=\"{ry}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
