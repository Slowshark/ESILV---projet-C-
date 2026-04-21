using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Chemin : Forme
    {
        // Définition du chemin SVG
        string path;
        public Chemin(int idelement, int ordre, int R, int G, int B, string path)
            : base(idelement, ordre, R, G, B)
        {
            // Initialisation du chemin
            this.path = path;
        }
        public override string GenererBaliseSvg()
        {            // Génération de la balise SVG path            return $"  <path d=\"{path}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
