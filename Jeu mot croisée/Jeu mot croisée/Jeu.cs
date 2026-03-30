using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeu_mot_croisée
{
    public class Jeu
    {
        //initialisation de la classe
        private Joueur joueur1;
        private Joueur joueur2;
        private Plateau plateau;
        private Dictionnaire dictionnaire;

        public Jeu()
        {
            string choix = "";
            dictionnaire = new Dictionnaire("Mots_Français.txt");
            //Affichage du menu
            do
            {
                Console.WriteLine("Choisissez le type de plateau :");
                Console.WriteLine("1 - Plateau défini");
                Console.WriteLine("2 - Plateau aléatoire");
                Console.WriteLine("\n\n0 - Sortir");

                choix = Console.ReadLine();
                Console.Clear();
            } while (choix != "1" && choix != "2" && choix != "0");
            Thread.Sleep(1000);
            //répartition du choix de l'utilisateur
            if (choix == "1")
            {
                //choix pour matrice prédéfini
                // Dossier où se trouvent tes fichiers CSV
                string dossier = @"C:\Users\margu\source\repos\Projet Mot Glissés\Projet Mot Glissés\bin\Debug\net5.0";

                // Récupération de tous les fichiers .csv
                string[] fichiers = Directory.GetFiles(dossier, "*.csv");

                if (fichiers.Length == 0)
                {
                    Console.WriteLine("Aucun fichier CSV trouvé dans le dossier.");
                    return;
                }

                Console.WriteLine("Liste des plateaus disponibles :\n");

                // Affichage avec index
                for (int i = 0; i < fichiers.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {Path.GetFileName(fichiers[i])}");
                }

                int choixdef;
                while (true)
                {

                    if (int.TryParse(Console.ReadLine(), out choixdef)
                        && choixdef >= 1
                        && choixdef <= fichiers.Length)
                    {
                        break;
                    }

                    Console.WriteLine("Choix invalide, veuillez entrer un numéro correct.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("Liste des plateaus disponibles :\n");
                    for (int i = 0; i < fichiers.Length; i++)
                    {
                        Console.WriteLine($"{i + 1} - {Path.GetFileName(fichiers[i])}");
                    }
                }

                // Fichier choisi
                string fichierChoisi = fichiers[choixdef - 1];
                plateau = Plateau.GenererDepuisCSV(fichierChoisi);
            }
            else if (choix == "2")
            {//choix pour la matrice aléatoire
                plateau = Plateau.GenererAléatoirement("Lettre.txt", 8, 8); // méthode déjà existante ou à ajouter
            }
            else if (choix == "0")
            {
                //arret du jeu
                Console.WriteLine("Fin du jeu");
                return;
            }


        }
        public Plateau Plateau
        { get { return plateau; } }


        public void Demarrer()
        {
            //initialisation des variables
            string line = "";
            int occ = 0;
            int scorePoids;
            string poids = "";
            List<string> mots = new List<string>();
            Console.Clear();
            double partie = 0;
            double tour = 0;
            string tampon;
            Thread.Sleep(1000);
            //Demande du temps pour les tours
            do
            {
                Console.Write("Durée des tours (en secondes): ");
                tampon = Console.ReadLine();
                if ((double.TryParse(tampon, out tour)) == false)
                {
                    tour = -1;
                }
            } while (tour <= 0);
            //Demande du temps pour la partie
            do
            {
                Console.Write("Durée de la partie (en minutes): ");
                tampon = Console.ReadLine();
                if ((double.TryParse(tampon, out partie)) == false)
                {
                    partie = -1;
                }
            } while (partie <= 0);
            //création des joueur
            Console.Write("Nom du joueur 1 : ");
            joueur1 = new Joueur(Console.ReadLine());

            Console.Write("Nom du joueur 2 : ");
            joueur2 = new Joueur(Console.ReadLine());
            Thread.Sleep(1000);
            Joueur courant = joueur1;

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Tour de {courant.Nom}\n");
            plateau.Afficher();
            //initialisation des timers
            DateTime finPartie = DateTime.Now.AddMinutes(partie);
            DateTime finTour = DateTime.Now.AddSeconds(tour);


            while (true)
            {

                // Met à jour le nom du joueur et efface l'affichage de certaines lignes
                Console.SetCursorPosition(8, 0);
                Console.Write(new string(' ', courant.Nom.Length));
                Console.SetCursorPosition(8, 0);
                Console.Write(courant.Nom + "\n");
                plateau.Afficher();
                Console.SetCursorPosition(0, plateau.Lignes + 3);
                Console.Write("Entrez un mot: ");
                Console.SetCursorPosition(0, plateau.Lignes + 6);
                Console.Write(new string(' ', 80));
                Console.WriteLine(new string(' ', 300));

                //methode pour lire le mot de l'utilisateur
                string mot = LireMinuteur(plateau, finTour, finPartie);
                Console.SetCursorPosition(0, plateau.Lignes + 6);
                //changement de tour après la fin du chrono
                if ((finTour - DateTime.Now).TotalSeconds <= 0)
                {
                    Console.WriteLine("Temps du tour écoulé ! Changement de tour.");
                    Console.WriteLine("Appuyez sur Entrée...");
                    if (courant == joueur1) { courant = joueur2; }
                    else if (courant == joueur2) { courant = joueur1; }
                    Thread.Sleep(3000);
                    finTour = DateTime.Now.AddSeconds(tour);
                }
                //arret de la partie après la fin de la partie
                else if ((finPartie - DateTime.Now).TotalSeconds <= 0)
                {
                    Console.SetCursorPosition(0, plateau.Lignes + 6);
                    Console.Write(new string(' ', 80));
                    Console.WriteLine(new string(' ', 300));
                    Console.SetCursorPosition(0, plateau.Lignes + 6);
                    courant = joueur1;
                    Console.WriteLine(joueur1);
                    courant = joueur2;
                    Console.WriteLine(joueur2);
                    return;
                }
                //cas où le mot de l'utilisateur est vide
                else if (mot == "")
                {
                    Console.WriteLine("Mot vide !");
                    Thread.Sleep(1000);
                }
                else
                {
                    //recherche du mot dans la matrice
                    mot = mot.ToUpper();
                    int[,] coordonné = new int[mot.Length, 2];
                    int[,] result = plateau.RechercheMot(mot, 1, plateau.Lignes - 1, 0, coordonné);
                    //cas où le mot de l'utilisateur a déjà ete saisi
                    if (courant.Contient(mot))
                    {
                        Console.WriteLine("Mot déjà trouvé !");
                    }
                    //cas où le mot de l'utilisateur est introuvable
                    else if (result == null)
                    {
                        Console.WriteLine("Mot introuvable sur le plateau !");
                    }
                    //cas où le mot de l'utilisateur n'est pas dans le dictionnaire
                    else if (!dictionnaire.EstValide(mot))
                    {
                        Console.WriteLine("Mot non valide !");
                    }
                    else
                    {
                        //ajout du mot dans les mots trouvé et mise à jour du score
                        courant.AddMot(mot);
                        for (int i = 0; i < mot.Length; i++)
                        {
                            StreamReader sr = new StreamReader("Lettre.txt");
                            do
                            {
                                line = sr.ReadLine();
                                if (line == null) { break; }
                                mots = new List<string>(line.Split(','));
                            } while (mot[i].ToString() != mots[0]);
                            courant.AddScore((int.Parse(mots[2]) + 1) * 10);
                            sr.Close();
                        }
                        //mise à jour du plateau
                        plateau.MajPlateau(mot, result);

                        //affichage du score du joueur 
                        Console.WriteLine("Mot accepté !");
                        Console.WriteLine(courant);
                    }
                    Thread.Sleep(1000);
                }


            }
        }

        private string LireMinuteur(Plateau plateau, DateTime finTour, DateTime finPartie)
        {
            int inputX = 0;
            int inputY = plateau.Lignes + 1;

            Console.SetCursorPosition(inputX, inputY + 3);
            Console.Write(new string(' ', 30));
            string saisie = null;
            //temps restant pour le tour 
            TimeSpan restant = finTour - DateTime.Now;

            var lectureTask = Task.Run(() =>
            {
                Console.SetCursorPosition(inputX, inputY + 3);
                saisie = Console.ReadLine();
            });

            while (restant.TotalSeconds > 0)
            {
                restant = finTour - DateTime.Now;
                //temps restant pour la partie
                TimeSpan restantGlobal = finPartie - DateTime.Now;
                if (restantGlobal.TotalSeconds <= 0)
                {
                    //fin du chrono de la partie
                    Console.SetCursorPosition(inputX, inputY + 5);
                    Console.Write("Temps global écoulé ! Fin de la partie.   ");
                    return null;
                }
                //mise à jour de l'affichage du chrono
                Console.SetCursorPosition(inputX, inputY);
                Console.Write(new string(' ', 30));
                Console.SetCursorPosition(inputX, inputY);
                Console.Write("Global: " + restantGlobal.Minutes + ":" + restantGlobal.Seconds);
                Console.SetCursorPosition(inputX, inputY + 1);
                Console.Write(new string(' ', 30));
                Console.SetCursorPosition(inputX, inputY + 1);
                Console.Write("Joueur: " + restant.Seconds);
                Console.SetCursorPosition(inputX, inputY + 3);

                if (lectureTask.Wait(1000))
                    break;
            }

            if (!lectureTask.IsCompleted)
            {

                return null;
            }

            return saisie;
        }
    }
}