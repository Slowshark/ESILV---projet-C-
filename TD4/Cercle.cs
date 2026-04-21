using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public class Cercle : Forme
    {
        double cx;
        double cy;
        double r;
        public Cercle(int idelement, int ordre, int R, int G, int B, double cx, double cy, double r)
            : base(idelement, ordre, R, G, B)
        {
            this.cx = cx;
            this.cy = cy;
            this.r = r;
        }
        public override string GenererBaliseSvg()
        {
            return $"  <circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"fill:rgb({R},{G},{B})\"{GenererTransformSvg()} />";
        }
    }
}
