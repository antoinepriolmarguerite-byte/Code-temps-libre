using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jeu_incendie
{
    internal struct Cellule
    {
        public string Type;
        public char Symbole;
        public int Degré;
        public bool EnFeu;

        public Cellule(string type, char symbole, int degré, bool enFeu)
        {
            Type = type;
            Symbole = symbole;
            Degré = degré;
            EnFeu = enFeu;
        }

        public void Afficher()
        {
            if (EnFeu)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed; // Fond rouge pour le feu
            }
            else
            {
                // Assignation des couleurs selon le type
                switch (Type)
                {
                    case "feuille":
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        break;
                    case "herbe":
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    case "arbre":
                        Console.BackgroundColor = ConsoleColor.DarkYellow; // Marron approximatif
                        break;
                    case "rocher":
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        break;
                    case "eau":
                        Console.BackgroundColor = ConsoleColor.Blue;
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Black; // Fond par défaut
                        break;
                }
            }
        }
    }
}