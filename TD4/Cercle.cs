using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Cercle : Forme
    {
        // Coordonnées du centre du cercle
        double cx;
        double cy;
        double r; // Rayon du cercle
        public Cercle(int idelement, int ordre, int R, int G, int B, double cx, double cy, double r)
            : base(idelement, ordre, R, G, B)
        {
            // Initialisation des paramètres
            this.cx = cx;
            this.cy = cy;
            this.r = r;
        }
        public override string GenererBaliseSvg()
        {
            // Génération de la balise SVG du cercle avec ses propriétés
            return $"  <circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
