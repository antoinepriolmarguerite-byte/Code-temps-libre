using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Jeu_mot_croisée
{
    public class Dictionnaire
    {
        //initialisation de la classe
        private Dictionary<char, List<string>> motsParLettre;

        public Dictionnaire(string cheminFichier)
        {
            // Dictionnaire qui associe chaque lettre (A-Z) à une liste de mots
            motsParLettre = new Dictionary<char, List<string>>();

            // Lecture du fichier texte contenant les mots
            /*using (StreamReader sr = new StreamReader(cheminFichier))
            {
                char lettre = 'A'; // on commence par la lettre A
                string line;

                // création des listes contenant les lignes du fichier txt
                while ((line = sr.ReadLine()) != null && lettre <= 'Z')
                {
                    // Découpe la ligne en mots, séparés par espace, virgule ou point-virgule
                    List<string> mots = new List<string>(
                        line.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    );

                    // Tri avec MergeSort au lieu du tri par insertion
                    if (mots.Count > 1)
                        MergeSort(mots, 0, mots.Count - 1);

                    // Associe la liste triée à la lettre correspondante
                    motsParLettre[lettre] = mots;
                    lettre++; // passe à la lettre suivante
                }
            }*/
        }

        // Tri fusion (MergeSort)
        private void MergeSort(List<string> mots, int gauche, int droite)
        {
            // Si la sous-liste contient plus d'un élément
            if (gauche < droite)
            {
                // Trouve le milieu de la liste
                int milieu = (gauche + droite) / 2;

                // Trie récursivement la moitié gauche
                MergeSort(mots, gauche, milieu);

                // Trie récursivement la moitié droite
                MergeSort(mots, milieu + 1, droite);

                // Fusionne les deux moitiés triées
                Fusion(mots, gauche, milieu, droite);
            }
        }

        private void Fusion(List<string> mots, int gauche, int milieu, int droite)
        {
            // Tailles des deux sous-listes
            int n1 = milieu - gauche + 1;
            int n2 = droite - milieu;

            // Tableaux temporaires pour stocker les sous-listes
            string[] gaucheTab = new string[n1];
            string[] droiteTab = new string[n2];

            // Copie des éléments dans les sous-listes
            for (int i = 0; i < n1; i++) gaucheTab[i] = mots[gauche + i];
            for (int j = 0; j < n2; j++) droiteTab[j] = mots[milieu + 1 + j];

            // Index pour parcourir les sous-listes
            int iIndex = 0, jIndex = 0, k = gauche;

            // Fusionne les deux sous-listes en comparant les éléments
            while (iIndex < n1 && jIndex < n2)
            {
                // Si gaucheTab[i] > droiteTab[j], on place droiteTab[j]
                if (EstPlusGrand(gaucheTab[iIndex], droiteTab[jIndex]))
                {
                    mots[k] = droiteTab[jIndex];
                    jIndex++;
                }
                else
                {
                    // Sinon on place gaucheTab[i]
                    mots[k] = gaucheTab[iIndex];
                    iIndex++;
                }
                k++;
            }

            // Copie les éléments restants de gaucheTab
            while (iIndex < n1)
            {
                mots[k] = gaucheTab[iIndex];
                iIndex++;
                k++;
            }

            // Copie les éléments restants de droiteTab
            while (jIndex < n2)
            {
                mots[k] = droiteTab[jIndex];
                jIndex++;
                k++;
            }
        }

        // Fonction de comparaison manuelle : retourne true si s1 > s2
        private bool EstPlusGrand(string s1, string s2)
        {
            int len1 = s1.Length;
            int len2 = s2.Length;
            int minLen = Math.Min(len1, len2);

            // Compare caractère par caractère
            for (int i = 0; i < minLen; i++)
            {
                char c1 = char.ToUpper(s1[i]); // ignore la casse
                char c2 = char.ToUpper(s2[i]);

                if (c1 > c2) return true;   // s1 vient après s2
                if (c1 < c2) return false;  // s1 vient avant s2
            }

            // Si les préfixes sont identiques, le mot le plus long est considéré "après"
            return len1 > len2;
        }

        //vérifie que le mot transmis est présent dans dictionnaire
        public bool EstValide(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot)) return false;

            mot = mot.ToUpper();
            char initiale = mot[0];

            if (!motsParLettre.ContainsKey(initiale)) return false;

            List<string> liste = motsParLettre[initiale];
            return RechercheDichotomique(liste, mot);
        }
        //parcoure la liste de la première lettre du mot correspondant et vérifie si le mot est bien présent
        private bool RechercheDichotomique(List<string> liste, string mot)
        {
            int gauche = 0;
            int droite = liste.Count - 1;

            while (gauche <= droite)
            {
                int milieu = (gauche + droite) / 2;
                int comparaison = string.Compare(liste[milieu], mot, StringComparison.OrdinalIgnoreCase);

                if (comparaison == 0) return true;
                else if (comparaison < 0) gauche = milieu + 1;
                else droite = milieu - 1;
            }

            return false;
        }
    }
}