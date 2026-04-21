using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Rectangle : Forme
    {
        double x;
        double y;
        double largeur;
        double hauteur;
        public Rectangle(int idelement, int ordre, int R, int G, int B, double x, double y, double largeur, double hauteur)
            : base(idelement, ordre, R, G, B)
        {
            this.x = x;
            this.y = y;
            this.largeur = largeur;
            this.hauteur = hauteur;
        }
        public override string GenererBaliseSvg()
        {
            return $"  <rect x=\"{x}\" y=\"{y}\" width=\"{largeur}\" height=\"{hauteur}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
