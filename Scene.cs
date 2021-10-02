using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;
using MaPremiereApplication.Sources;


namespace MaPremiereApplication.Sources
{
    class Scene
    {
        public List<Bloc> m_blocs;
        public List<Bloc> m_blocs_ennemis;
        public int m_codeNiveau;
        public Point positionPersonnage;
        public int scoreMax;

        // Crée la scène à partir d'une liste de blocs passée en paramètre
        public Scene(List<Bloc> blocsDeLaScene)
        {
            m_blocs = new List<Bloc>();
            m_blocs_ennemis = new List<Bloc>();
            foreach (Bloc bloc in blocsDeLaScene)
            {
                if ((bloc.m_code == 31) || (bloc.m_code == 32) || (bloc.m_code == 33) || (bloc.m_code == 34))
                {
                    MonstreDechet monstre = new MonstreDechet(bloc, m_codeNiveau);
                    m_blocs_ennemis.Add(monstre);
                    m_blocs.Add(monstre);
                }
                else
                {
                    m_blocs.Add(bloc);
                }
            }

            //calcul du score maximal pour le niveau
            scoreMax = setScoreMax();
            Console.WriteLine("Score maximal = {0}", scoreMax);
        }

        // Crée la scène à partir d'un fichier texte dont le chemin est spécifié en paramètre
        public Scene(String cheminFichierDeScene, Surface ecranVideo, int codeNiveau)
        {
            m_blocs = new List<Bloc>();
            m_blocs_ennemis = new List<Bloc>();
            m_codeNiveau = codeNiveau;

            // Création d'une instance de StreamReader pour permettre la lecture de notre fichier 
            TextReader lecteurFichier = new StreamReader(cheminFichierDeScene);

            // Lecture de la première ligne
            String ligneLue = lecteurFichier.ReadLine();

            // Récupération du nombre de lignes et de colonnes
            string[] premiereLigneScindee = ligneLue.Split('x');
            int nb_lignes = Convert.ToInt32(premiereLigneScindee[0]);
            int nb_colonnes = Convert.ToInt32(premiereLigneScindee[1]);

            // Le matrice qui contiendra le code image de chaque bloc
            int[,] valeursScene = new int[nb_lignes, nb_colonnes];

            // On parcourt le reste du fichier
            for (int i = 0; i < nb_lignes; i++)
            {
                // Pour chaque ligne lue
                ligneLue = lecteurFichier.ReadLine();
                string[] ligneScindee = ligneLue.Split(' ');

                // Pour chaque valeur présente sur la ligne
                for (int j = 0; j < ligneScindee.Length; j++)
                {
                    // On met à jour notre matrice
                    valeursScene[i, j] = Convert.ToInt32(ligneScindee[j]);
                }
            }

            // Pour savoir si la position du héros a été définie dans le fichier texte
            bool positionPersonnageDefinie = false;

            // Enfin, on construit notre scène à partir de la matrice ainsi obtenue
            for (int l = 0; l < nb_lignes; l++)
            {
                for (int c = 0; c < nb_colonnes; c++)
                {
                    // Créer le bloc de code et de position relatives aux coordonnées des images de la scène
                    if (valeursScene[l, c] != 0)
                    {
                        // S'il s'agit du code 5 : position de départ du héros (aucun bloc à créer)
                        if (valeursScene[l, c] == 5)
                        {
                            positionPersonnage = new Point(c * VariablesGlobales.H_Largeur_Bloc, l * VariablesGlobales.H_Hauteur_Bloc);
                            positionPersonnageDefinie = true;
                        }
                        else
                        {
                            // On l'ajoute enfin à la liste des blocs de la scène
                            Bloc newbloc = new Bloc(valeursScene[l, c], c * VariablesGlobales.H_Largeur_Bloc, l * VariablesGlobales.H_Hauteur_Bloc, ecranVideo, codeNiveau);

                            if ((newbloc.m_code == 31) || (newbloc.m_code == 32) || (newbloc.m_code == 33) || (newbloc.m_code == 34))
                            {
                                MonstreDechet monstre = new MonstreDechet(newbloc, m_codeNiveau);
                                m_blocs_ennemis.Add(monstre);
                                m_blocs.Add(monstre);
                            }
                            else
                            {
                                m_blocs.Add(newbloc);
                            }

                            
                        }
                    }
                }
            }

            // Si la position du héros n'a pas encore été définie
            if (!positionPersonnageDefinie)
            {
                positionPersonnage.X = 0;
                positionPersonnage.Y = 0;
            }


            //calcul du score maximal pour le niveau
            scoreMax = setScoreMax();
            Console.WriteLine("Score maximal = {0}", scoreMax);
        }

        // Dessine la scène à partir de la liste des blocs, des sprites (pas encore fait) et des personnages (pas encore fait) et autres...
        public void dessinerScene(Surface ecranVideo)
        {
            // Dessine chaque bloc sur la surface donnée
            foreach (Bloc bloc in m_blocs)
            {
                bloc.dessiner(ecranVideo, m_codeNiveau);
            }
        }

        // Dessine la scène à partir de la liste des blocs, des sprites (pas encore fait) et des personnages (pas encore fait) et autres...
        public void dessinerScene(Surface ecranVideo, Point camera)
        {
            // Dessine chaque bloc sur la surface donnée
            foreach (Bloc bloc in m_blocs)
            {
                bloc.dessiner(ecranVideo, camera);
            }
        }

        // Positionne le personnage à l'endroit indiqué dans le fichier texte
        public void positionnerPersonnage(Personnage personnage)
        {
            personnage.m_position.X = positionPersonnage.X;
            personnage.m_position.Y = positionPersonnage.Y;
        }

        // Centre la caméra sur le personnage
        public void positionnerCamera(Personnage personnage)
        {
            if ((positionPersonnage.X != 0) && (positionPersonnage.Y != 0))
            {
                // On compte le déplacement à faire faire à tous les blocs de façon à centrer la caméra sur l'axe des X et celui des Y
                int deplacementBlocSurX = personnage.m_position.X - (VariablesGlobales.H_Fen_Largeur / 2) - (VariablesGlobales.H_Largeur_Bloc / 2);
                int deplacementBlocSurY = personnage.m_position.Y - (VariablesGlobales.H_Fen_Hauteur / 2) - (VariablesGlobales.H_Hauteur_Bloc / 2);

                // Déplace tous les blocs
                foreach (Bloc bloc in m_blocs)
                {
                    bloc.setPosition(bloc.getPosition().X - deplacementBlocSurX, bloc.getPosition().Y - deplacementBlocSurY);
                }

                // Re-centrer le personnage
                personnage.m_position.X -= deplacementBlocSurX;
                personnage.m_position.Y -= deplacementBlocSurY;
            }
        }

        //renvoie le score maximal en fonction du contenu de la scène
        public int setScoreMax()
        {
            int nb = 0;
            foreach (Bloc bloc in m_blocs)
            {
                if (bloc.estDechet() || (bloc.estEnnemi() && !bloc.estRadioactif()))
                {
                    nb++;
                }
            }

            return (nb * 100);
        }

        //condition de réussite du niveau
        public bool estReussie(Personnage heros)
        {
            if (heros.inventaire > 0 || heros.sante <= 0) {
                return (false);
            }
            foreach (Bloc bloc in m_blocs)
            {
                if (bloc.estDechet() || (bloc.estEnnemi() && !bloc.estRadioactif()))
                {
                    return (false);
                }
            }
            //Console.WriteLine("Inventaire : {0} & Santé : {1}", heros.inventaire, heros.sante);
            return (true);
        }

        public int getCodeNiveau () {
            return this.m_codeNiveau;
        }

    }
}
