using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD4
{
    public abstract class Forme : IComparable<Forme>
    {
        // Identifiant et ordre de la forme
        protected int idelement;
        protected int ordre;
        // Composantes RGB de la couleur
        protected int R;
        protected int G;
        protected int B;
        // Liste des transformations appliquées
        protected List<Transformation> Transformations;

        public Forme(int idelement, int ordre, int R, int G, int B)
        {
            // Initialisation des propriétés de base
            this.idelement = idelement;
            this.ordre = ordre;
            this.R = R;
            this.G = G;
            this.B = B;
            this.Transformations = new List<Transformation>();
        }

        public int IdElement
        {
            get { return idelement; }
        }

        public int Ordre
        {
            get { return ordre; }
        }

        public int CompareTo(Forme autre)
        {
            return this.ordre.CompareTo(autre.ordre);
        }
        public void AjouterTransformation(Transformation t)
        {
            this.Transformations.Add(t);
        }

        
        protected string GenererTransformSvg()
        {
            if (Transformations.Count == 0)
                return "";
            string trans = "";
            // Parcours inverse : la derniere transformation ajoutee doit apparaitre en premier dans le SVG
            for (int i = Transformations.Count - 1; i >= 0; i--)
            {
                trans += Transformations[i].ToSvg() + " ";
            }
            return $" transform=\"{trans.Trim()}\"";
        }

        public abstract string GenererBaliseSvg();
    }
}
