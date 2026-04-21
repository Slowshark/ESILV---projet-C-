using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Polygone : Forme
    {
        string points;

        public Polygone(int idelement, int ordre, int R, int G, int B, string points)
            : base(idelement, ordre, R, G, B)
        {
            this.points = points;
        }
        public override string GenererBaliseSvg()
        {
            return $"  <polygon points=\"{points}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
