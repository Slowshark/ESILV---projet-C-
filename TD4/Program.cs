using System;
using System.IO;

namespace TD4
{
    /// <summary>
    /// Programme principal (Developpeur B) :
    /// Lit un fichier CSV, cree un objet Dessin, charge les donnees CSV,
    /// et genere le fichier SVG correspondant.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Convertisseur CSV vers SVG");
            Console.WriteLine();

            string cheminCSV;

            // Recuperer le chemin du fichier CSV
            if (args.Length > 0)
            {
                cheminCSV = args[0];
            }
            else
            {
                Console.Write("Entrez le chemin du fichier CSV : ");
                cheminCSV = Console.ReadLine();
            }

            // Verif que le chemin n'est pas vide
            if (string.IsNullOrEmpty(cheminCSV))
            {
                Console.WriteLine("Erreur : chemin pas bon ou vide");
                return;
            }

            // Construire le chemin du fichier SVG de sortie
            // On remplace l'extension .csv par .svg
            string cheminSVG = Path.ChangeExtension(cheminCSV, ".svg");

            //Instanciation de l'objet Dessin 
            // et allocation de l'espace memoire
            Dessin dessin = new Dessin();

            //  try/catch global : gestion d'erreurs par exceptions (Exo 3) 
            try
            {
                // BLOC 2 : Lecture du fichier CSV
                Console.WriteLine($"Lecture du  CSV : {cheminCSV}");
                dessin.LireFichierCSV(cheminCSV);
                Console.WriteLine(dessin.ToString()); // Appel de ToString() 

                // BLOC 3 : Ecriture du fichier SVG
                Console.WriteLine($"Ecriture du fichier SVG : {cheminSVG}");
                dessin.EcrireFichierSVG(cheminSVG);

                Console.WriteLine();
                Console.WriteLine("Conversion terminee avec succes !");
                Console.WriteLine("Vous pouvez ouvrir le fichier SVG avec un navigateur internet.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Le programme ne peut pas continuer : fichier CSV introuvable.");
            }
            catch (IOException)
            {
                Console.WriteLine("Le programme ne peut pas continuer : erreur de lecture/ecriture.");
            }
            catch (Exception ex)
            {
                // Exception generique pour tout probleme imprevu
                Console.WriteLine($"Erreur inattendue : {ex.Message}");
            }
        }
    }
}
