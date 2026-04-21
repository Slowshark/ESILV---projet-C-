using System;
using System.Collections.Generic; // Collection generique List<T> (cf. Cours 2)
using System.IO;                  // StreamReader, StreamWriter pour la lecture/ecriture de fichiers

namespace TD4
{
    /// <summary>
    /// Classe Dessin : le Chef d'Orchestre des Fichiers (Developpeur B - Blocs 2 et 3).
    /// Contient une List generique de Forme (collection generique, cf. Cours 2).
    /// Gere la lecture du fichier CSV et l'ecriture du fichier SVG.
    /// Implemente la gestion d'erreurs par exceptions (try/catch, cf. Exercice 3).
    /// </summary>
    public class Dessin
    {
        // =====================================================================
        // ATTRIBUT PRIVE : collection generique List<T> (cf. Cours 2 - List<T>)
        // List<Forme> : tableau dynamique d'objets Forme
        // Polymorphisme : la liste peut contenir des Cercle, Rectangle, etc.
        // car ils heritent tous de la classe abstraite Forme (cf. Heritage)
        // =====================================================================
        private List<Forme> elements;

        /// <summary>
        /// Propriete en lecture seule : acces controle a la liste (principe d'encapsulation, cf. Cours 1).
        /// </summary>
        public List<Forme> Elements
        {
            get { return elements; }
        }

        /// <summary>
        /// Constructeur : initialise la collection generique List<Forme>.
        /// Le constructeur reserve l'espace memoire pour la liste (cf. Cours 1 - Constructeurs).
        /// </summary>
        public Dessin()
        {
            // Instanciation de la collection generique (cf. Cours 2 - List<T>)
            elements = new List<Forme>();
        }

        /// <summary>
        /// Methode d'instance AjouterElement : ajoute une forme a la liste avec Add().
        /// (cf. Cours 2 - List<T>.Add())
        /// </summary>
        public void AjouterElement(Forme element)
        {
            elements.Add(element);
        }

        // =====================================================================
        // BLOC 2 : LECTURE DU FICHIER CSV (Developpeur B)
        // Ouvrir le fichier, lire les lignes, faire des Split(";"),
        // convertir les textes en nombres avec int.Parse() ou double.Parse().
        // Gestion d'erreurs par Exceptions (try/catch) - Exercice 3.
        // =====================================================================

        /// <summary>
        /// Lit un fichier CSV et remplit la List de formes.
        /// Utilise StreamReader pour ouvrir et lire le fichier ligne par ligne.
        /// Chaque ligne est decoupee avec Split(";") pour extraire les champs.
        /// Les conversions de texte en nombre se font avec int.Parse() et double.Parse().
        /// </summary>
        public void LireFichierCSV(string cheminFichier)
        {
            // Vider la liste avant de charger un nouveau fichier
            elements.Clear(); // cf. Cours 2 - List<T>.Clear()

            // Liste temporaire pour stocker les lignes de transformation
            // (on les applique apres avoir cree toutes les formes)
            List<string[]> lignesTransformations = new List<string[]>();

            // ---- try/catch : gestion d'erreurs par exceptions (Exercice 3) ----
            try
            {
                // Ouverture du fichier avec StreamReader (flux de lecture)
                using (StreamReader lecteur = new StreamReader(cheminFichier))
                {
                    string ligne;

                    // Lecture ligne par ligne jusqu'a la fin du fichier
                    while ((ligne = lecteur.ReadLine()) != null)
                    {
                        // Ignorer les lignes vides
                        ligne = ligne.Trim();
                        if (string.IsNullOrEmpty(ligne))
                            continue;

                        // Split(";") : decoupe la ligne en tableau de chaines
                        string[] champs = ligne.Split(';');

                        // Le premier champ indique le type de forme
                        string type = champs[0].Trim();

                        // ---- Traitement selon le type ----
                        try
                        {
                            if (type == "Translation" || type == "Rotation")
                            {
                                // Stocker les transformations pour les appliquer apres
                                lignesTransformations.Add(champs);
                            }
                            else
                            {
                                // Creer la forme correspondante (polymorphisme)
                                Forme forme = CreerForme(champs, type);
                                if (forme != null)
                                {
                                    // Ajout a la collection generique avec Add() (cf. Cours 2)
                                    elements.Add(forme);
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            // Exception si int.Parse() ou double.Parse() echoue
                            Console.WriteLine($"Erreur de format sur la ligne : {ligne}");
                            Console.WriteLine($"  Detail : {ex.Message}");
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Exception si le nombre de champs est insuffisant apres Split
                            Console.WriteLine($"Erreur : nombre de champs insuffisant sur la ligne : {ligne}");
                            Console.WriteLine($"  Detail : {ex.Message}");
                        }
                    }
                }

                // Appliquer les transformations aux formes correspondantes
                AppliquerTransformations(lignesTransformations);
            }
            catch (FileNotFoundException)
            {
                // Exception : le fichier n'existe pas
                Console.WriteLine($"Erreur : le fichier '{cheminFichier}' est introuvable.");
                throw; // Relancer l'exception pour que l'appelant puisse la gerer
            }
            catch (IOException ex)
            {
                // Exception : erreur d'entree/sortie lors de la lecture
                Console.WriteLine($"Erreur d'entree/sortie lors de la lecture du fichier : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cree une Forme a partir des champs CSV.
        /// Utilise int.Parse() et double.Parse() pour convertir les textes en nombres.
        /// Polymorphisme : retourne un objet de type Forme qui est en fait
        /// une instance d'une classe fille (Cercle, Rectangle, etc.)
        /// Le constructeur de chaque classe fille appelle base() pour initialiser
        /// les attributs herites de la classe mere Forme (cf. Heritage - mot cle base).
        /// </summary>
        private Forme CreerForme(string[] champs, string type)
        {
            // Conversion de l'identifiant avec int.Parse()
            int idElement = int.Parse(champs[1].Trim());

            switch (type)
            {
                case "Cercle":
                    // csv : Cercle;idElement;cx;cy;r;R;G;B;ordre
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
                    // csv : Rectangle;idElement;x;y;l;h;R;G;B;ordre
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
                    // csv : Ellipse;idElement;cx;cy;rx;ry;R;G;B;ordre
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
                    // csv : Polygone;idElement;points;R;G;B;ordre
                    return new Polygone(
                        idElement,
                        int.Parse(champs[6].Trim()),     // ordre
                        int.Parse(champs[3].Trim()),     // R
                        int.Parse(champs[4].Trim()),     // G
                        int.Parse(champs[5].Trim()),     // B
                        champs[2].Trim()                 // points
                    );

                case "Chemin":
                    // csv : Chemin;idElement;path;R;G;B;ordre
                    return new Chemin(
                        idElement,
                        int.Parse(champs[6].Trim()),     // ordre
                        int.Parse(champs[3].Trim()),     // R
                        int.Parse(champs[4].Trim()),     // G
                        int.Parse(champs[5].Trim()),     // B
                        champs[2].Trim()                 // path
                    );

                case "Texte":
                    // csv : Texte;idElement;x;y;contenu;R;G;B;ordre
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

        /// <summary>
        /// Applique les transformations (Translation, Rotation) aux formes correspondantes.
        /// Cree des objets Translation ou Rotation (classes filles de Transformation)
        /// et les ajoute a la forme via AjouterTransformation() (cf. Heritage et Polymorphisme).
        /// Recherche la forme par son idElement dans la liste avec foreach (cf. Cours 2).
        /// </summary>
        private void AppliquerTransformations(List<string[]> lignesTransformations)
        {
            // Parcours de la collection avec foreach (cf. Cours 2 - parcourir une collection)
            foreach (string[] champs in lignesTransformations)
            {
                string type = champs[0].Trim();
                int idElement = int.Parse(champs[1].Trim());

                // Rechercher la forme par son idElement dans la liste
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
                        // csv : Translation;idElement;dx;dy
                        double dx = double.Parse(champs[2].Trim());
                        double dy = double.Parse(champs[3].Trim());
                        // Polymorphisme : on cree un objet Translation (classe fille de Transformation)
                        forme.AjouterTransformation(new Translation(dx, dy));
                    }
                    else if (type == "Rotation")
                    {
                        // csv : Rotation;idElement;alpha;x;y
                        double alpha = double.Parse(champs[2].Trim());
                        double x = double.Parse(champs[3].Trim());
                        double y = double.Parse(champs[4].Trim());
                        // Polymorphisme : on cree un objet Rotation (classe fille de Transformation)
                        forme.AjouterTransformation(new Rotation(alpha, x, y));
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Erreur de format dans la transformation : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Recherche une forme dans la collection par son idElement.
        /// Parcours avec foreach (cf. Cours 2 - parcourir une collection avec foreach).
        /// </summary>
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

        // =====================================================================
        // BLOC 3 : ECRITURE DU FICHIER SVG (Developpeur B)
        // Ajouter la balise <svg> au debut, la balise </svg> a la fin,
        // et ecrire le tout dans un nouveau fichier.
        // Les elements sont ecrits dans l'ordre d'affichage (tri par IComparable).
        // =====================================================================

        /// <summary>
        /// Ecrit le fichier SVG a partir des formes de la liste.
        /// 1. Trie les formes par leur ordre d'affichage grace a IComparable (cf. Classe Abstraite et Interface).
        /// 2. Ecrit la balise d'ouverture svg.
        /// 3. Pour chaque forme, appelle GenererBaliseSvg() (polymorphisme : virtual/override).
        /// 4. Ecrit la balise de fermeture svg.
        /// </summary>
        public void EcrireFichierSVG(string cheminFichier)
        {
            // ---- try/catch : gestion d'erreurs par exceptions (Exercice 3) ----
            try
            {
                // Tri de la collection par ordre d'affichage
                // Sort() utilise IComparable<Forme>.CompareTo() implemente dans Forme
                // (cf. Classe Abstraite et Interface - implementation de l'interface IComparable)
                elements.Sort();

                // Ouverture du fichier en ecriture avec StreamWriter (flux d'ecriture)
                using (StreamWriter ecrivain = new StreamWriter(cheminFichier))
                {
                    // Ecriture de la balise d'ouverture SVG (point d'entree du fichier SVG)
                    ecrivain.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");

                    // Parcours de la collection triee avec foreach (cf. Cours 2)
                    // Polymorphisme : chaque forme appelle SA propre version de GenererBaliseSvg()
                    // (methode abstraite overridee dans chaque classe fille, cf. Heritage et Polymorphisme)
                    foreach (Forme forme in elements)
                    {
                        ecrivain.WriteLine(forme.GenererBaliseSvg());
                    }

                    // Ecriture de la balise de fermeture SVG (point de sortie du fichier SVG)
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

        /// <summary>
        /// Methode ToString() : description du dessin (cf. Cours 1 - methode ToString()).
        /// </summary>
        public override string ToString()
        {
            return $"Dessin contenant {elements.Count} elements graphiques.";
        }
    }
}
