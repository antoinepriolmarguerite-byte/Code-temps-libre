using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Jeu_mot_croisée
{
    //initialisation de la classe
    public class Plateau
    {
        private char[,] matrice;
        private int lignes;
        private int colonnes;

        public Plateau(char[,] matrice)
        {
            this.matrice = matrice;
            lignes = matrice.GetLength(0);
            colonnes = matrice.GetLength(1);
        }
        public int Lignes
        {
            get { return lignes; }
            set { lignes = value; }
        }
        //création de la matrice à partir du fichier csv
        public static Plateau GenererDepuisCSV(string cheminFichier)
        {
            //lecture du fichier csv
            List<string[]> lignesCSV = new List<string[]>();

            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // Séparateur ; ou , selon ton fichier
                    string[] parts = line.Split(';');
                    lignesCSV.Add(parts);
                }
            }

            int lignes = lignesCSV.Count;
            int colonnes = lignesCSV[0].Length;
            char[,] matrice = new char[lignes, colonnes];
            //création de la matrice
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    matrice[i, j] = char.ToUpper(lignesCSV[i][j][0]);
                }
            }

            return new Plateau(matrice);
        }
        //génération de la matrice aléatoirement
        public static Plateau GenererAléatoirement(string cheminFichier, int lignes, int colonnes)
        {
            Dictionary<char, int> lettresFreq = new Dictionary<char, int>();
            //lecture du fichier lettre
            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2 && char.TryParse(parts[0], out char lettre) && int.TryParse(parts[1], out int freq))
                    {
                        lettresFreq[char.ToUpper(lettre)] = freq;
                    }
                }
            }
            //création de la liste à partir du fichier lettre
            List<char> lettresDisponibles = new List<char>();
            foreach (var kvp in lettresFreq)
            {
                for (int i = 0; i < kvp.Value; i++)
                    lettresDisponibles.Add(kvp.Key);
            }
            //mélange aléatoire dans la liste
            Random r = new Random();
            for (int i = lettresDisponibles.Count - 1; i > 0; i--)
            {
                int j = r.Next(i + 1);
                (lettresDisponibles[i], lettresDisponibles[j]) = (lettresDisponibles[j], lettresDisponibles[i]);
            }
            //création de la matrice
            char[,] matrice = new char[lignes, colonnes];
            int index = 0;
            int totalCases = lignes * colonnes;
            int totalLettres = lettresDisponibles.Count;

            //Remplir les cases disponibles avec des lettres, sans jamais dépasser
            for (int i = lignes - 1; i >= 0 && index < totalLettres; i--)
            {
                for (int j = 0; j < colonnes && index < totalLettres; j++)
                {
                    matrice[i, j] = lettresDisponibles[index];
                    index++; // avance uniquement quand on écrit une lettre
                }
            }

            //Compléter le reste avec des espaces
            for (int i = lignes - 1; i >= 0; i--)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (matrice[i, j] == '\0') // case non remplie
                        matrice[i, j] = ' ';
                }
            }



            return new Plateau(matrice);
        }
        //Affichage de la matrice
        public void Afficher()
        {
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public int[,] RechercheMot(string mot, int i, int ligne, int colonne, int[,] coordonné)
        {
            List<int[]> listeretour = new List<int[]>();
            bool verifcoord = true;
            bool verif = false;
            int memoirei = 0;
            //recherche de la première lettre dans la première ligne
            for (int col = 0; col < colonnes; col++)
            {
                if (matrice[ligne, col] == mot[0])
                {
                    if (i == 1)
                    {
                        coordonné[0, 0] = lignes - 1;
                        coordonné[0, 1] = col;
                        colonne = col;
                    }

                    //recherche lettre par lettre
                    while (i < mot.Length)
                    {
                        //recherche dans les positions autour de la lettre actuel pour trouver la suivante
                        int l = ligne;
                        int c = colonne;
                        if (l - 1 >= 0)
                        {
                            if (matrice[l - 1, c] == mot[i]) { verif = true; ligne--; }
                        }
                        if (c + 1 < colonnes)
                        {
                            if (matrice[l, c + 1] == mot[i] && verif == false) { verif = true; colonne++; }
                            else if (matrice[l, c + 1] == mot[i] && verif == true) { int[] tab = { i + 1, l, c + 1 }; ; listeretour.Add(tab); }
                        }

                        if (c - 1 >= 0)
                        {
                            if (matrice[l, c - 1] == mot[i] && verif == false) { verif = true; colonne--; }
                            else if (matrice[l, c - 1] == mot[i] && verif == true) { int[] tab = { i + 1, l, c - 1 }; listeretour.Add(tab); }
                        }
                        if (c - 1 >= 0 && l - 1 >= 0)
                        {
                            if (matrice[l - 1, c - 1] == mot[i] && verif == false) { verif = true; colonne--; ligne--; }
                            else if (matrice[l - 1, c - 1] == mot[i] && verif == true) { int[] tab = { i + 1, l - 1, c - 1 }; listeretour.Add(tab); }
                        }
                        if (c + 1 < colonnes && l - 1 >= 0)
                        {
                            if (matrice[l - 1, c + 1] == mot[i] && verif == false) { verif = true; colonne++; ligne--; }
                            else if (matrice[l - 1, c + 1] == mot[i] && verif == true) { int[] tab = { i + 1, l - 1, c + 1 }; ; listeretour.Add(tab); }
                        }
                        //arret de la recherche si le mot n'est plus possible
                        if (verif == false && listeretour == null) { i = mot.Length; verifcoord = false; }
                        //arret de la recherche pour ce cas là et essaiye les autres cas possibles
                        else if (verif == false && listeretour != null)
                        {

                            memoirei = i;
                            i = mot.Length;
                            verifcoord = false;

                        }
                        else
                        {
                            verif = false;

                            i++;
                            coordonné[i - 1, 0] = ligne;
                            coordonné[i - 1, 1] = colonne;

                        }


                    }
                    //retour des coordonnées des lettres
                    if (verifcoord == true)
                    {

                        return coordonné;
                    }
                    //vérification si tous les cas pour trouver le mot dans la matrice ont été explorer
                    else if (verifcoord == false && listeretour != null)
                    {
                        for (int j = 1; j <= listeretour.Count; j++)
                        {

                            coordonné[memoirei - 1, 0] = listeretour[listeretour.Count - j][1];
                            coordonné[memoirei - 1, 1] = listeretour[listeretour.Count - j][2];
                            if ((coordonné = RechercheMot(mot, listeretour[listeretour.Count - j][0], listeretour[listeretour.Count - j][1], listeretour[listeretour.Count - j][2], coordonné)) != null)
                            {
                                return coordonné;
                            }

                        }
                        verifcoord = true;
                    }
                    verifcoord = true;
                    i = 1;
                }
            }

            return null;
        }
        //Mise à jour de la matrice
        public void MajPlateau(string mot, int[,] mat)
        {
            for (int i = mot.Length - 1; i >= 0; i--)
            {

                for (int j = mat[i, 0]; j > 0; j--)
                {
                    matrice[j, mat[i, 1]] = matrice[j - 1, mat[i, 1]];
                    matrice[j - 1, mat[i, 1]] = ' ';
                }
            }
        }
    }
}

