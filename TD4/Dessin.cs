using System;
using System.Collections.Generic; 
using System.IO;                  

namespace TD4
{

    public class Dessin
    {
        
        private List<Forme> elements;

        public List<Forme> Elements
    
            get { return elements; }
        }

        public Dessin()
        {
            elements = new List<Forme>();
        }

        public void AjouterElement(Forme element)
        {
            elements.Add(element);
        }

        public void LireFichierCSV(string cheminFichier)
        {
            elements.Clear(); 

            List<string[]> lignesTransformations = new List<string[]>();

            try
            {
                using (StreamReader lecteur = new StreamReader(cheminFichier))
                {
                    string ligne;

                    while ((ligne = lecteur.ReadLine()) != null)
                    {
                        // Ignore les lignes vides
                        ligne = ligne.Trim();
                        if (string.IsNullOrEmpty(ligne))
                            continue;

                        // decoupe la ligne en tableau de chaines a partir de ;
                        string[] champs = ligne.Split(';');

                        // champ forme
                        string type = champs[0].Trim();

                        try
                        {
                            if (type == "Translation" || type == "Rotation")
                            {
                                // Stocke transfo pour apres
                                lignesTransformations.Add(champs);
                            }
                            else
                            {
                                // Cree la forme correspondante 
                                Forme forme = CreerForme(champs, type);
                                if (forme != null)
                                {
                                    // Ajout a la collection 
                                    elements.Add(forme);
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            // Exception si intparse() ou doubleparse() echoue
                            Console.WriteLine($"Erreur de format sur la ligne : {ligne}");
                            Console.WriteLine($"  Detail : {ex.Message}");
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Exception si nombre champs pas insuffisant
                            Console.WriteLine($"Erreur : nombre de champs insuffisant sur la ligne : {ligne}");
                            Console.WriteLine($"  Detail : {ex.Message}");
                        }
                    }
                }

                // Applique transformations d'avant
                AppliquerTransformations(lignesTransformations);
            }
            catch (FileNotFoundException)
            {
                // Exception fichier n'existe pas
                Console.WriteLine($"Erreur : le fichier '{cheminFichier}' est introuvable.");
                throw; // Relance l'exception pour la gerer
            }
            catch (IOException ex)
            {
                // Exception : erreur d'entree/sortie lors de la lecture
                Console.WriteLine($"Erreur d'entree/sortie lors de la lecture du fichier : {ex.Message}");
                throw;
            }
        }

        private Forme CreerForme(string[] champs, string type)
        {
            // Conversion de l'identifiant avec int.Parse()
            int idElement = int.Parse(champs[1].Trim());

            switch (type)
            {
                case "Cercle":
                    return new Cercle(
                        idElement,
                        int.Parse(champs[8].Trim()),     // ordre
                        int.Parse(champs[5].Trim()),     // R
                        int.Parse(champs[6].Trim()),     // G
                        int.Parse(champs[7].Trim()),     // B
                        double.Parse(champs[2].Trim()),  // cx
                        double.Parse(champs[3].Trim()),  // cy
                        double.Parse(champs[4].Trim())   // r
                    );

                case "Rectangle":
                    return new Rectangle(
                        idElement,
                        int.Parse(champs[9].Trim()),     // ordre
                        int.Parse(champs[6].Trim()),     // R
                        int.Parse(champs[7].Trim()),     // G
                        int.Parse(champs[8].Trim()),     // B
                        double.Parse(champs[2].Trim()),  // x
                        double.Parse(champs[3].Trim()),  // y
                        double.Parse(champs[4].Trim()),  // largeur
                        double.Parse(champs[5].Trim())   // hauteur
                    );

                case "Ellipse":
                    return new Ellipse(
                        idElement,
                        int.Parse(champs[9].Trim()),     // ordre
                        int.Parse(champs[6].Trim()),     // R
                        int.Parse(champs[7].Trim()),     // G
                        int.Parse(champs[8].Trim()),     // B
                        double.Parse(champs[2].Trim()),  // cx
                        double.Parse(champs[3].Trim()),  // cy
                        double.Parse(champs[4].Trim()),  // rx
                        double.Parse(champs[5].Trim())   // ry
                    );

                case "Polygone":
                    return new Polygone(
                        idElement,
                        int.Parse(champs[6].Trim()),     // ordre
                        int.Parse(champs[3].Trim()),     // R
                        int.Parse(champs[4].Trim()),     // G
                        int.Parse(champs[5].Trim()),     // B
                        champs[2].Trim()                 // points
                    );

                case "Chemin":
                    return new Chemin(
                        idElement,
                        int.Parse(champs[6].Trim()),     // ordre
                        int.Parse(champs[3].Trim()),     // R
                        int.Parse(champs[4].Trim()),     // G
                        int.Parse(champs[5].Trim()),     // B
                        champs[2].Trim()                 // path
                    );

                case "Texte":
                    return new Texte(
                        idElement,
                        int.Parse(champs[8].Trim()),     // ordre
                        int.Parse(champs[5].Trim()),     // R
                        int.Parse(champs[6].Trim()),     // G
                        int.Parse(champs[7].Trim()),     // B
                        double.Parse(champs[2].Trim()),  // x
                        double.Parse(champs[3].Trim()),  // y
                        champs[4].Trim()                 // contenu
                    );

                default:
                    Console.WriteLine($"Attention : type de forme inconnu '{type}', ligne ignoree.");
                    return null;
            }
        }

        private void AppliquerTransformations(List<string[]> lignesTransformations)
        {
            foreach (string[] champs in lignesTransformations)
            {
                string type = champs[0].Trim();
                int idElement = int.Parse(champs[1].Trim());

                // Rechercher forme par idElement dans liste
                Forme forme = TrouverFormeParId(idElement);

                if (forme == null)
                {
                    Console.WriteLine($"Attention : transformation pour l'element {idElement} introuvable.");
                    continue;
                }

                try
                {
                    if (type == "Translation")
                    {
                        double dx = double.Parse(champs[2].Trim());
                        double dy = double.Parse(champs[3].Trim());
                        forme.AjouterTransformation(new Translation(dx, dy));
                    }
                    else if (type == "Rotation")
                    {
                        double alpha = double.Parse(champs[2].Trim());
                        double x = double.Parse(champs[3].Trim());
                        double y = double.Parse(champs[4].Trim());
                        forme.AjouterTransformation(new Rotation(alpha, x, y));
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Erreur de format dans la transformation : {ex.Message}");
                }
            }
        }

        private Forme TrouverFormeParId(int idElement)
        {
            // Parcours de la List<Forme> avec foreach
            foreach (Forme f in elements)
            {
                if (f.IdElement == idElement)
                    return f;
            }
            return null;
        }

        
        
        public void EcrireFichierSVG(string cheminFichier)
        {
            try
            {
                // Tri de la collection par ordre d'affichage
                elements.Sort();

                // Ouverture du fichier en ecriture avec StreamWriter
                using (StreamWriter ecrivain = new StreamWriter(cheminFichier))
                {
                    // Ecriture de la balise d'ouverture SVG 
                    ecrivain.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");

                    // Parcours de la collection triee avec foreach
                    foreach (Forme forme in elements)
                    {
                        ecrivain.WriteLine(forme.GenererBaliseSvg());
                    }

                    // Ecriture de la balise de fermeture SVG 
                    ecrivain.WriteLine("</svg>");
                }

                Console.WriteLine($"Fichier SVG ecrit avec succes : {cheminFichier}");
                Console.WriteLine($"  Nombre d'elements : {elements.Count}"); // Count : propriete de List<T>
            }
            catch (UnauthorizedAccessException ex)
            {
                // Exception : pas les droits d'ecriture
                Console.WriteLine($"Erreur : acces refuse pour l'ecriture du fichier '{cheminFichier}'.");
                Console.WriteLine($"  Detail : {ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                // Exception : erreur d'entree/sortie lors de l'ecriture
                Console.WriteLine($"Erreur d'entree/sortie lors de l'ecriture du fichier : {ex.Message}");
                throw;
            }
        }

        
        /// Methode ToString() : description du dessin 
        public override string ToString()
        {
            return $"Dessin contenant {elements.Count} elements graphiques.";
        }
    }
}
