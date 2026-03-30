using Jeu_mot_croisée;
using System;


namespace Jeu_mot_croisée
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //départ du code et création de la classe jeu
            Console.Title = "🔤 Jeu des Mots Glissés";

            Console.WriteLine("Bienvenue dans le Jeu des Mots Glissés !");
            Console.WriteLine("========================================\n");

            Jeu jeu = new Jeu();
            //démarage du jeu
            if (jeu.Plateau != null)
            {
                jeu.Demarrer();
            }
        }
    }
}