using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Jeu_mot_croisée
{
    public class Joueur
    {
        //initialisation de la classe
        public string Nom { get; private set; }
        public List<string> MotsTrouves { get; private set; }
        public int Score { get; private set; }

        public Joueur(string nom)
        {
            while (nom == null || nom == "")
            {
                nom = Console.ReadLine();
            }

            Nom = nom;
            MotsTrouves = new List<string>();
            Score = 0;
        }
        //ajoute le mot dans les mots trouvés
        public void AddMot(string mot)
        {
            MotsTrouves.Add(mot);
        }
        //regarde si le mot est contenu dans les mots trouvés
        public bool Contient(string mot)
        {
            return MotsTrouves.Contains(mot);
        }
        //Ajoute le score du mot au score global du joueur
        public void AddScore(int val)
        {
            Score += val;
        }
        //Affichage du joueur avec son score et les mots qu'il a trouvé
        public override string ToString()
        {
            string phrase = $"Joueur: {Nom}, Score: {Score}, Mots: ";
            if (MotsTrouves.Count > 0)
            {
                phrase = phrase + MotsTrouves[0];
                for (int i = 1; i < MotsTrouves.Count; i++)
                {
                    phrase = phrase + ", " + MotsTrouves[i];
                }
            }
            return phrase;
        }
    }
}