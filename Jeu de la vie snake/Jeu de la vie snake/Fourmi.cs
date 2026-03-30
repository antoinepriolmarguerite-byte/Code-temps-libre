using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jeu_de_la_vie_snake
{
    internal struct Fourmi
    {
        public int Ligne;
        public int Colonne;
        public char Direction;
        public int Age;
        public string couleur;
        public string symbole;

        public Fourmi(int ligne, int colonne, char direction)
        {
            Ligne = ligne;
            Colonne = colonne;
            Direction = direction;
            Age = 0;
            couleur = "";
            symbole = "@";//age initial
        }
    }
}
