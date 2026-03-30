using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace jeu_de_la_vie_snake
{
    public class Program
    {
        static void Main(string[] args)
        {
            Choixmenu();
        }
        static void Choixmenu()
        {
            Console.WriteLine("Bienvenue dans la simulation de la fourmi de Langton !");//écriture du menu pour le choix de la simulation
            Console.WriteLine("1- Version classique");
            Console.WriteLine("2- Version avancée");
            Console.WriteLine("3- Quitter");

            int choix = int.Parse(Console.ReadLine());//Arret du programme si le choix est "Quitter"
            if (choix == 1) { Console.WriteLine("Une fourmi va apparaitre au milieu de la matrice avec une direction défini aléatoirement,\n la fourmi va ensuite avancer d'une case.\nSi la fourmi se trouve sur une case blanche il va faire un quart de tour vers la droite et changer la couleur de la case en noir\nSi la fourmi se trouve sur une case noir il va faire un quart de tour vers la gauche et changer la couleur de la case en noir\nSi le déplacement atteint le bord du quadrillage la simulation s'arrête ou la simulation fait le nombre de cycle donné"); }
            else if (choix == 2) { Console.WriteLine("Des fourmis vont apparaitre aléatoire sur la matrice avec une direction défini aléatoirement,\n la fourmi va ensuite avancer d'une case.\nSi la fourmi se trouve sur une case blanche il va faire un quart de tour vers la droite et changer la couleur de la case en noir\nSi la fourmi se trouve sur une case noir il va faire un quart de tour vers la gauche et changer la couleur de la case en noir\nSi le déplacement d'une fourmi atteint le bord du quadrillage la fourmi apparaitra du coté opposé\nDeux fourmis ne peuvent pas être sur la même case et c'est le mouvement de la fourmi la plus âgée qui passe en priorité\nLa simulation s'arrête si elle fait le nombre de cycle donné"); }
            else if (choix == 3) return;
            Console.WriteLine();
            Console.WriteLine("Entrez les dimensions de la matrice (Longueur Largeur) :");//choix de l'utilisateur des dimensions de la matrice
            int longueur;
            int largeur;
            do
            {
                longueur = int.Parse(Console.ReadLine());
            } while (longueur < 0);
            do
            {
                largeur = int.Parse(Console.ReadLine());
            } while (largeur < 0);
            Case[,] matrice = Creationdematrice(largeur, longueur);//création de la matrice
            int nombreFourmis = 1;
            int cycles;
            if (choix == 2)
            {
                Console.WriteLine("Entrez le nombre de fourmis :");//choix de l'utilisateur pour le nombre de fourmi 
                do
                {
                    nombreFourmis = int.Parse(Console.ReadLine());
                } while (nombreFourmis < 0);
                Console.WriteLine("Entrez le nombre de cycles de simulation :");//choix de l'utilisateur pour le nombre de cycle 
                do
                {
                    cycles = int.Parse(Console.ReadLine());
                } while (cycles < 0);
                LancerSimulation2(cycles, matrice, nombreFourmis);//lancement de la deuxième simulation
                return;
            }

            Console.WriteLine("Entrez le nombre de cycles de simulation :");//choix de l'utilisateur pour le nombre de cycle 
            cycles = int.Parse(Console.ReadLine());
            LancerSimulation1(cycles, matrice);//lancement de la première simulation
        }
        static Case[,] Creationdematrice(int largeur, int longueur)
        {
            Case[,] matrice = new Case[largeur, longueur];//initialisation de la matrice et mise par défaut des cases

            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < longueur; j++)
                {
                    matrice[i, j].Couleur = 'B';
                    matrice[i, j].ContientFourmi = false;

                }
            }
            return matrice;

        }
        static void LancerSimulation1(int cycles, Case[,] matrice)
        {
            Fourmi fourmi1 = new Fourmi();//création de la fourmi
            fourmi1.Colonne = matrice.GetLength(0) / 2;//emplacement de la fourmi au centre
            fourmi1.Ligne = matrice.GetLength(1) / 2;
            fourmi1.Direction = directionaléatoire();//choix aléatoire de la direction
            char direction = fourmi1.Direction;
            matrice[fourmi1.Colonne, fourmi1.Ligne].ContientFourmi = true;
            Console.WriteLine("Cycle : " + 1);
            AfficherMatrice1(matrice);
            for (int cycle = 0; cycle <= cycles; cycle++)//répétition qui gère le nombre de cycle à faire avant l'arrêt du programme
            {
                if (Console.KeyAvailable)//programme pour la détection de la touche échap
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\nSimulation interrompue par l'utilisateur.");
                        break;
                    }
                }
                if (matrice[fourmi1.Colonne, fourmi1.Ligne].Couleur == 'B')//partie du programme pour le changement de direction
                {
                    switch (fourmi1.Direction)
                    {
                        case 'N': fourmi1.Direction = 'E'; break;
                        case 'E': fourmi1.Direction = 'S'; break;
                        case 'S': fourmi1.Direction = 'O'; break;
                        case 'O': fourmi1.Direction = 'N'; break;
                    }
                    ;
                    matrice[fourmi1.Colonne, fourmi1.Ligne].Couleur = 'N';
                }
                else
                {
                    switch (fourmi1.Direction)
                    {
                        case 'N': fourmi1.Direction = 'O'; break;
                        case 'E': fourmi1.Direction = 'N'; break;
                        case 'S': fourmi1.Direction = 'E'; break;
                        case 'O': fourmi1.Direction = 'S'; break;
                    }
                    ;
                    matrice[fourmi1.Colonne, fourmi1.Ligne].Couleur = 'B';
                }
                matrice[fourmi1.Colonne, fourmi1.Ligne].ContientFourmi = false;
                switch (fourmi1.Direction)//vérification pour le déplacement de la fourmi
                {
                    case 'N': fourmi1.Colonne = fourmi1.Colonne - 1; break;
                    case 'S': fourmi1.Colonne = fourmi1.Colonne + 1; break;
                    case 'O': fourmi1.Ligne = fourmi1.Ligne - 1; break;
                    case 'E': fourmi1.Ligne = fourmi1.Ligne + 1; break;
                }
                if (fourmi1.Colonne < 0 || fourmi1.Colonne >= matrice.GetLength(0) || fourmi1.Ligne < 0 || fourmi1.Ligne >= matrice.GetLength(1)) { AfficherMatrice1(matrice); Console.WriteLine(); Console.WriteLine("Cycle : " + cycle); return; }//vérification pour l'arrêt du programme si le déplacement dépasse la matrice
                matrice[fourmi1.Colonne, fourmi1.Ligne].ContientFourmi = true;
                fourmi1.Age = fourmi1.Age + 1;
                AfficherMatrice1(matrice);//affichage de la matrice
                Console.WriteLine();
                Console.WriteLine("Cycle : " + cycle);

            }

            return;
        }
        static void AfficherMatrice1(Case[,] matrice)//méthode pour l'affichage de la matrice
        {
            Thread.Sleep(50);
            Console.Clear();
            for (int i = 0; i < matrice.GetLength(1); i++) { Console.Write("--"); }
            Console.WriteLine();
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    if (matrice[i, j].ContientFourmi == true)
                    {
                        //écriture de l'emplacement des fourmis
                        if (matrice[i, j].Couleur == 'B') { Console.BackgroundColor = ConsoleColor.Black; }
                        else if (matrice[i, j].Couleur == 'N') { Console.BackgroundColor = ConsoleColor.White; }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("@ ");
                    }
                    else if (matrice[i, j].Couleur == 'B') { Console.BackgroundColor = ConsoleColor.White; Console.Write("  "); Console.BackgroundColor = ConsoleColor.Black; }
                    else if (matrice[i, j].Couleur == 'N') { Console.BackgroundColor = ConsoleColor.Black; Console.Write("  "); }//écriture des cases de la matrice avec leur couleur 
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;//remise au stade initial du fond de la console et des caractères
            for (int i = 0; i < matrice.GetLength(1); i++) { Console.Write("--"); }
        }
        static char directionaléatoire()//méthode pour le choix aléatoire de la direction
        {
            char directional = 'N';
            Random random = new Random();
            int direc = random.Next(4);
            switch (direc)
            {
                case 0: directional = 'N'; break;
                case 1: directional = 'O'; break;
                case 2: directional = 'S'; break;
                case 3: directional = 'E'; break;
            }
            return directional;

        }
        static void AfficherMatrice2(Case[,] matrice, Fourmi[] fourmi1)//méthode pour l'affichage de la matrice
        {
            Thread.Sleep(50);
            Console.Clear();
            int r = 0;
            for (int i = 0; i < matrice.GetLength(1); i++) { Console.Write("--"); }
            Console.WriteLine();
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    if (matrice[i, j].ContientFourmi == true)
                    {
                        for (int h = 0; h < fourmi1.Length; h++)//vérification de quel fourmi se trouve sur la case
                        {
                            if (i == fourmi1[h].Colonne && j == fourmi1[h].Ligne) { r = h; }
                        }
                        //écriture de l'emplacement des fourmis
                        if (matrice[i, j].Couleur == 'B') { Console.BackgroundColor = ConsoleColor.Black; }
                        else if (matrice[i, j].Couleur == 'N') { Console.BackgroundColor = ConsoleColor.White; }
                        switch (fourmi1[r].couleur)//changement de couleur des caractères en fonction de leur couleur
                        {
                            case "Red": Console.ForegroundColor = ConsoleColor.Red; Console.Write("@ "); break;
                            case "Yellow": Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("@ "); break;
                            case "Magenta": Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("@ "); break;
                            case "Cyan": Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("@ "); break;
                            case "Green": Console.ForegroundColor = ConsoleColor.Green; Console.Write("@ "); break;
                            case "Blue": Console.ForegroundColor = ConsoleColor.Blue; Console.Write("@ "); break;
                            case "DarkGray": Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("@ "); break;
                            case "Gray": Console.ForegroundColor = ConsoleColor.Gray; Console.Write("@ "); break;
                            case "DarkYellow": Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write("@ "); break;
                            case "DarkMagenta": Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.Write("@ "); break;
                            case "DarkRed": Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write("@ "); break;
                            case "DarkCyan": Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write("@ "); break;
                            case "DarkGreen": Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write("@ "); break;
                            case "DarkBlue": Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write("@ "); break;
                            default: Console.ForegroundColor = ConsoleColor.Red; Console.Write("@ "); break;
                        }

                    }
                    else if (matrice[i, j].Couleur == 'B') { Console.BackgroundColor = ConsoleColor.White; Console.Write("  "); Console.BackgroundColor = ConsoleColor.Black; }
                    else if (matrice[i, j].Couleur == 'N') { Console.BackgroundColor = ConsoleColor.Black; Console.Write("  "); }//écriture des cases de la matrice avec leur couleur 
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;//remise au stade initial du fond de la console et des caractères
            for (int i = 0; i < matrice.GetLength(1); i++) { Console.Write("--"); }

        }
        static void LancerSimulation2(int cycles, Case[,] matrice, int nombreFourmis)
        {
            Fourmi[] fourmi1 = Créationfourmi(matrice, nombreFourmis);//création des fourmis en fonction du nombre donné
            int longueur = matrice.GetLength(1);
            int hauteur = matrice.GetLength(0);
            int nfourmi = 0;
            matrice[fourmi1[0].Colonne, fourmi1[0].Ligne].ContientFourmi = true;//création de la matrice de Case
            Console.WriteLine("Cycle : " + 1);
            AfficherMatrice2(matrice, fourmi1);
            for (int cycle = 0; cycle <= cycles; cycle++)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\nSimulation interrompue par l'utilisateur.");
                        break;
                    }
                }

                if (cycle % 4 == 0 && nfourmi < nombreFourmis) { nfourmi++; }//arrivé des fourmi décalé dans le temps
                for (int i = 0; i < nfourmi; i++)//la répétition pour gérer les fourmis de la plus ancienne à la plus récente
                {
                    if (matrice[fourmi1[i].Colonne, fourmi1[i].Ligne].Couleur == 'B')//Changement de direction en fonction de la couleur de la case
                    {
                        switch (fourmi1[i].Direction)
                        {
                            case 'N': fourmi1[i].Direction = 'E'; break;
                            case 'E': fourmi1[i].Direction = 'S'; break;
                            case 'S': fourmi1[i].Direction = 'O'; break;
                            case 'O': fourmi1[i].Direction = 'N'; break;
                        }
                        ;
                        matrice[fourmi1[i].Colonne, fourmi1[i].Ligne].Couleur = 'N';
                    }
                    else
                    {
                        switch (fourmi1[i].Direction)
                        {
                            case 'N': fourmi1[i].Direction = 'O'; break;
                            case 'E': fourmi1[i].Direction = 'N'; break;
                            case 'S': fourmi1[i].Direction = 'E'; break;
                            case 'O': fourmi1[i].Direction = 'S'; break;
                        }
                        ;
                        matrice[fourmi1[i].Colonne, fourmi1[i].Ligne].Couleur = 'B';
                    }
                    matrice[fourmi1[i].Colonne, fourmi1[i].Ligne].ContientFourmi = false;
                    int hg = 0; int hd = 0; int bg = 0; int bd = 0;//vérification de la présence de fourmi sur la case d'arrivé
                    if (fourmi1[i].Colonne - 1 < 0) { hg = matrice.GetLength(0) - 1; }
                    else { hg = fourmi1[i].Colonne - 1; }
                    if (fourmi1[i].Colonne + 1 > matrice.GetLength(0) - 1) { hd = 0; }
                    else { hd = fourmi1[i].Colonne + 1; }
                    if (fourmi1[i].Ligne - 1 < 0) { bg = matrice.GetLength(1) - 1; }
                    else { bg = fourmi1[i].Ligne - 1; }
                    if (fourmi1[i].Ligne + 1 > matrice.GetLength(1) - 1) { bd = 0; }
                    else { bd = fourmi1[i].Ligne + 1; }
                    switch (fourmi1[i].Direction)
                    {
                        case 'N': if (matrice[hg, fourmi1[i].Ligne].ContientFourmi == false) { fourmi1[i].Colonne = fourmi1[i].Colonne - 1; } break; //déplacement dela fourmi en fonction de la direction
                        case 'S': if (matrice[hd, fourmi1[i].Ligne].ContientFourmi == false) { fourmi1[i].Colonne = fourmi1[i].Colonne + 1; } break;
                        case 'O': if (matrice[fourmi1[i].Colonne, bg].ContientFourmi == false) { fourmi1[i].Ligne = fourmi1[i].Ligne - 1; } break;
                        case 'E': if (matrice[fourmi1[i].Colonne, bd].ContientFourmi == false) { fourmi1[i].Ligne = fourmi1[i].Ligne + 1; } break;
                    }
                    if (fourmi1[i].Colonne < 0 || fourmi1[i].Colonne >= matrice.GetLength(0) || fourmi1[i].Ligne < 0 || fourmi1[i].Ligne >= matrice.GetLength(1))//fonctionnement de la matrice circulaire 
                    {
                        if (fourmi1[i].Colonne < 0) { fourmi1[i].Colonne = matrice.GetLength(0) - 1; }
                        else if (fourmi1[i].Colonne >= matrice.GetLength(0)) { fourmi1[i].Colonne = 0; }
                        else if (fourmi1[i].Ligne < 0) { fourmi1[i].Ligne = matrice.GetLength(1) - 1; }
                        else if (fourmi1[i].Ligne >= matrice.GetLength(1)) { fourmi1[i].Ligne = 0; }
                    }
                    matrice[fourmi1[i].Colonne, fourmi1[i].Ligne].ContientFourmi = true;
                    fourmi1[i].Age = fourmi1[i].Age + 1;//incrémentation pour l'age
                }

                AfficherMatrice2(matrice, fourmi1);//affichage de la matrice
                Console.WriteLine();
                Console.WriteLine("Cycle : " + cycle);
            }
            return;
        }
        static Fourmi[] Créationfourmi(Case[,] matrice, int nombreFourmis)
        {
            Fourmi[] fourmi = new Fourmi[nombreFourmis];//création du tableau Fourmi
            Random random = new Random();
            for (int i = 0; i < nombreFourmis; i++)
            {
                fourmi[i].Colonne = random.Next(matrice.GetLength(0));//choix aléatoire de l'emplacement
                fourmi[i].Ligne = random.Next(matrice.GetLength(1));
                fourmi[i].Direction = directionaléatoire();//choix aléatoire de la direction
                switch (i)//Choix des couleurs pour chaque fourmis
                {
                    case 0: fourmi[i].couleur = "Red"; break;
                    case 1: fourmi[i].couleur = "Blue"; break;
                    case 2: fourmi[i].couleur = "Gray"; break;
                    case 3: fourmi[i].couleur = "Green"; break;
                    case 4: fourmi[i].couleur = "Cyan"; break;
                    case 5: fourmi[i].couleur = "Magenta"; break;
                    case 6: fourmi[i].couleur = "Yellow"; break;
                    case 7: fourmi[i].couleur = "DarkYellow"; break;
                    case 8: fourmi[i].couleur = "DarkMagenta"; break;
                    case 9: fourmi[i].couleur = "DarkRed"; break;
                    case 10: fourmi[i].couleur = "DarkCyan"; break;
                    case 11: fourmi[i].couleur = "DarkGreen"; break;
                    case 12: fourmi[i].couleur = "DarkBlue"; break;
                    case 13: fourmi[i].couleur = "DarkGray"; break;
                    default: fourmi[i].couleur = "Red"; break;
                }
            }
            return fourmi;
        }
    }
}