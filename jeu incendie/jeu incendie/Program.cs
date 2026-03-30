using System;
using System.Threading;

namespace jeu_incendie
{
    internal class Program
    {
        public static Cellule[,] RemplissageAleatoire(int lignes, int colonnes)
        {
            Random rand = new Random();
            Cellule[,] foret = new Cellule[lignes, colonnes];

            string[] types = { "herbe", "arbre", "terrain", "feuille", "eau", "rocher" };
            char[] symboles = { 'x', '*', '+', ' ', '/', '#' };
            int[] degres = { 8, 10, 0, 4, 0, 50 };

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    int index = rand.Next(types.Length);
                    foret[i, j] = new Cellule(types[index], symboles[index], degres[index], false);
                }
            }

            return foret;
        }

        public static void Affichage(Cellule[,] foret)
        {
            int lignes = foret.GetLength(0);
            int colonnes = foret.GetLength(1);

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    foret[i, j].Afficher();
                    Console.Write(foret[i, j].Symbole);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public static void InitialiseFeu(Cellule[,] foret)
        {
            Random rand = new Random();
            int lignes = foret.GetLength(0);
            int colonnes = foret.GetLength(1);

            int i, j;
            do
            {
                i = rand.Next(lignes);
                j = rand.Next(colonnes);
            } while (foret[i, j].Type == "eau" || foret[i, j].Type == "terrain");

            foret[i, j].EnFeu = true;
        }

        public static void PropagationFeu(Cellule[,] foret)
        {
            int lignes = foret.GetLength(0);
            int colonnes = foret.GetLength(1);
            Cellule[,] prochaineForet = (Cellule[,])foret.Clone(); // Copie temporaire pour éviter les changements instantanés

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (foret[i, j].Type == "cendres")
                    {
                        prochaineForet[i, j].Type = "cendres éteintes";
                        prochaineForet[i, j].Symbole = '.';
                        prochaineForet[i, j].Degré = 0;
                        prochaineForet[i, j].EnFeu = false;
                    }
                    if (foret[i, j].EnFeu)
                    {
                        if (foret[i, j].Degré > 2)
                        {
                            prochaineForet[i, j].Degré--; // Réduction du degré du feu
                        }
                        else
                        {
                            if (foret[i, j].Degré == 2 || foret[i, j].Degré == 1 && foret[i, j].Type != "eau" && foret[i, j].Type != "terrain")
                            {
                                prochaineForet[i, j].Type = "cendres";
                                prochaineForet[i, j].Symbole = '-';
                                prochaineForet[i, j].Degré = 1;
                                prochaineForet[i, j].EnFeu = false;
                            }
                        }

                        // Vérification des voisins
                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                int ni = i + di, nj = j + dj;

                                if (ni >= 0 && ni < lignes && nj >= 0 && nj < colonnes) // Vérification des limites
                                {
                                    if (foret[ni, nj].EnFeu == false && (foret[ni, nj].Type == "herbe" || foret[ni, nj].Type == "arbre" || foret[ni, nj].Type == "feuille" || foret[ni, nj].Type == "rocher"))
                                    {
                                        prochaineForet[ni, nj].EnFeu = true;
                                        prochaineForet[ni, nj].Degré--;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Mise à jour de la forêt
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    foret[i, j] = prochaineForet[i, j];
                }
            }
        }

        static void Main(string[] args)
        {
            int lignes, colonnes;
            while (true)
            {
                Console.WriteLine("Entrez la taille de la forêt (lignes colonnes) : ");
                if (int.TryParse(Console.ReadLine(), out lignes) && int.TryParse(Console.ReadLine(), out colonnes) && lignes > 0 && colonnes > 0)
                    break;
                Console.WriteLine("Veuillez entrer des valeurs numériques valides !");
            }

            Cellule[,] foret = RemplissageAleatoire(lignes, colonnes);
            Affichage(foret);

            InitialiseFeu(foret);
            Console.WriteLine("\nForêt après déclenchement du feu :");
            Affichage(foret);

            Console.WriteLine("\nEntrez le nombre de tours de simulation : ");
            int tours;
            while (!int.TryParse(Console.ReadLine(), out tours) || tours < 0)
            {
                Console.WriteLine("Veuillez entrer un nombre valide de tours !");
            }

            for (int t = 0; t < tours; t++)
            {
                Console.WriteLine($"\nTour {t + 1}:");
                PropagationFeu(foret);
                Affichage(foret);
                Thread.Sleep(200);
            }
        }
    }
}