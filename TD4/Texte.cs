using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Texte : Forme
    {
        // Position et contenu du texte
        double x;
        double y;
        string contenu;
        public Texte(int idelement, int ordre, int R, int G, int B, double x, double y, string contenu)
            : base(idelement, ordre, R, G, B)
        {
            // Initialisation du texte
            this.x = x;
            this.y = y;
            this.contenu = contenu;
        }
        public override string GenererBaliseSvg()
        {            // Génération de la balise SVG text            return $"  <text x=\"{x}\" y=\"{y}\" fill=\"rgb({R},{G},{B})\"{GenererTransformSvg()}>{contenu}</text>";
        }
    }
}
