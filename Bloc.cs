using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Graphics;
using System.Drawing;
using GarbageSoulReaper.Sources;

namespace MaPremiereApplication.Sources
{
    public class Bloc
    {
        public int id_bloc;

        public bool collision_avec_perso;

        public Surface m_bloc; // La surface du bloc
        public Point m_position; // Position du bloc
        public int m_code; // Code pour identifier le type de bloc
        public bool m_visible; // Code pour savoir si le bloc sera visible ou pas
        public bool m_traversable; // Savoir si un bloc est traversable ou pas
        Random rnd = new Random();
        private int alternerImageOxygene;

        public String direction;
        private bool sens = true;
        private int count = 0;
        private Surface m_oxy1;
        private Surface m_oxy2;
        public Surface m_radblocD;
        public Surface m_radblocG;

        // Crée le bloc avec une image par défaut aux coordonnées (0,0)
        public Bloc()
        {
            m_bloc = new Surface(@"..\..\images\niveau1\default_bloc.png");
            m_position = (new Point(0, 0));
            m_code = 0;
            id_bloc = ID_generator.getId();
            collision_avec_perso = false;
        }

        // Crée un bloc avec une image par défaut aux coordonnées spécifiées
        public Bloc(int positionX, int positionY, int codeBloc)
        {
            m_bloc = new Surface(@"..\..\images\niveau1\default_bloc.png");
            m_position = new Point(positionX, positionY);
            m_code = codeBloc;
            id_bloc = ID_generator.getId();
            collision_avec_perso = false;
        }

        // Crée le bloc avec une image dont le chemin et les coordonnées sont spécifiés
        public Bloc(String cheminImage, int positionX, int positionY, int codeBloc)
        {
            m_bloc = new Surface(cheminImage);
            m_position = (new Point(positionX, positionY));
            m_code = codeBloc;
            id_bloc = ID_generator.getId();
            collision_avec_perso = false;
        }

        // Crée le bloc avec une image correspondant au code image donné, aux coordonnées (positionX, positionY)
        public Bloc(int codeBloc, int positionX, int positionY, Surface ecranVideo, int codeNiveau)
        {
            alternerImageOxygene = 0;
            collision_avec_perso = false;
            id_bloc = ID_generator.getId();
            // On affecte le code envoyé à l'attribut m_code du bloc
            m_code = codeBloc;

            // Par défaut, un objet n'est pas traversable par le héros (à l'exception de certains)
            m_traversable = false;

            // Nombre aléatoire pour le chargement des blocs à images aléatoires
            int rndNumber;

            // On ouvre l'image correspondante au code envoyé
            switch(codeBloc)
            {
                /***********************/
                /**
                /*** BLOCS POUBELLES ***/
                /**
                /***********************/

                case 11:  // code 11: Poubelle verte
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\poubelle_verte.png");
                    m_visible = true;
                break;
                case 12:  // code 12: Poubelle jaune
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\poubelle_jaune.png");
                    m_visible = true;
                break;
                case 13:  // code 13: Poubelle grise
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\poubelle_grise.png");
                    m_visible = true;
                break;

                /***********************/
                /**
                /*** BLOCS DECHETS ***/
                /**
                /***********************/

                case 21:  // code 21: Déchet pour poubelle verte
                    rndNumber = rnd.Next(0, 4);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-vert1.png");
                    }
                    else if (rndNumber == 2)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-vert2.png");
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-vert3.png");
                    }
                    m_visible = true;
                break;
                case 22:  // code 22: Déchet pour poubelle jaune
                    rndNumber = rnd.Next(0, 4);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-jaune1.png");
                    }
                    else if (rndNumber == 2)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-jaune2.png");
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-jaune3.png");
                    }
                    m_visible = true;
                break;
                case 23:  // code 23: Déchet pour poubelle grise
                    rndNumber = rnd.Next(0, 4);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-gris1.png");
                    }
                    else if (rndNumber == 2)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-gris2.png");
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\dechet-gris3.png");
                    }
                    
                    m_visible = true;
                break;

                /*********************/
                /**
                /*** BLOCS ENNEMIS ***/
                /**
                /*********************/
                case 30:   // code 30 : Collider pour les ennemis
                    m_bloc = new Surface(VariablesGlobales.H_Largeur_Bloc, VariablesGlobales.H_Hauteur_Bloc).Convert(ecranVideo, true, false);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(0, 0, 0);
                    m_visible = true;
                    m_traversable = true;
                break;
                case 31:  // code 31: Ennemi déchet 1 (pour poubelle verte)
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\monstre_dechet_droite.png");
                    m_visible = true;
                break;
                case 32:  // code 32: Ennemi déchet 2 (pour poubelle jaune)
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\monstre_dechet_droite.png");
                    m_visible = true;
                break;
                case 33:  // code 33: Ennemi déchet 3 (pour poubelle grise)
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\monstre_dechet_droite.png");
                    m_visible = true;
                break;
                case 34: // code 34: Ennemi radioactif
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\monstre_radioactif_droite.png");
                    m_visible = true;
                break;

                /**************/
                /**
                /*** DIVERS ***/
                /**
                /**************/

                case 1:   // code 1 : Bloc plateforme 1: intérieur
                    rndNumber = rnd.Next(0, 4);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\1_roche1.png").Convert(ecranVideo, true, false);
                    }
                    else if (rndNumber == 2)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\1_roche2.png").Convert(ecranVideo, true, false);
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\1_roche3.png").Convert(ecranVideo, true, false);
                    }
                    m_visible = true;
                break;
                case 2:   // code 2 : Bloc plateforme: sol
                    rndNumber = rnd.Next(0, 5);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\2_roche_dessus1.png").Convert(ecranVideo, true, false);
                    }
                    else if (rndNumber == 2)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\2_roche_dessus2.png").Convert(ecranVideo, true, false);
                    }
                    else if (rndNumber == 3)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\2_roche_dessus3.png").Convert(ecranVideo, true, false);
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\2_roche_dessus4.png").Convert(ecranVideo, true, false);
                    }
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 3:   // code 3 : Bloc plateforme: plafond
                    rndNumber = rnd.Next(0, 3);
                    if (rndNumber == 1)
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\3_roche_dessous1.png").Convert(ecranVideo, true, false);
                    }
                    else
                    {
                        m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\3_roche_dessous2.png").Convert(ecranVideo, true, false);
                    }
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 4:   // code 4 : Bloc plateforme: exrémité en haut et en bas
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\4_roche_haut_bas.png").Convert(ecranVideo, true, true);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 6:   // code 6 : Bloc plateforme: extrémité droite haut
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\6_roche_droite_haut.png").Convert(ecranVideo, true, true);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 7:   // code 7 : Bloc plateforme: extrémité droite
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\7_roche_droite.png").Convert(ecranVideo, true, true);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 8:   // code 8 : Bloc plateforme: extrémité gauche haut
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\8_roche_gache_haut.png").Convert(ecranVideo, true, true);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;
                case 9:   // code 9 : Bloc plateforme: extrémité gauche
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\9_roche_gauche.png").Convert(ecranVideo, true, true);
                    m_bloc.Transparent = true;
                    m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
                    m_visible = true;
                break;

                case 99:   // code 99 : Zone récup. oxygène
                    //m_bloc = new Surface(VariablesGlobales.H_Largeur_Bloc, VariablesGlobales.H_Hauteur_Bloc).Convert(ecranVideo, true, false);
                    m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\oxygene1.png");
                    m_visible = false;
                    m_traversable = true;
                break;

                case 0:
                    m_traversable = true;
                    m_visible = false;
                    break;
                default:
                    // Sinon c'est qu'il s'agit d'un bloc invisible, un des codes suivants :
                    // code 0 : pas de bloc (donc pas d'image)
                    // code 3 : bloc invisible (donc pas d'image)
                    // code 4 : bloc tueur (donc pas d'image)
                    // code 5 : bloc position départ du héros (donc pas d'image)
                    m_visible = false;
                break;
            }

            // Et si le bloc créé a une hauteur inférieure à celle d'un bloc (RAPPEL : il ne faut jamais mettre des images de taille supérieure à celle des blocs plateforme)
            if (m_bloc.Height < VariablesGlobales.H_Hauteur_Bloc)
            {
                // On crée le bloc en le décalant sur l'axe des y de sorte à le placer exactement sur le sol
                m_position = new Point(positionX, positionY + (VariablesGlobales.H_Hauteur_Bloc - m_bloc.Height));
            }
            else
            {
                // Et si le bloc créé a une largeur inférieure à celle d'un bloc (RAPPEL : il ne faut jamais mettre des images de taille supérieure à celle des blocs plateforme)
                if (m_bloc.Width < VariablesGlobales.H_Largeur_Bloc)
                {
                    // On crée le bloc en le décalant sur l'axe des X de sorte à le placer exactement sur le sol
                    m_position = new Point(positionX + ((VariablesGlobales.H_Largeur_Bloc - (m_bloc.Width)) / 2), positionY);
                }
                else
                {
                    // Sinon on crée le bloc normalement
                    m_position = new Point(positionX, positionY);
                }
            }

            // Et si le bloc créé a une largeur inférieure à celle d'un bloc (RAPPEL : il ne faut jamais mettre des images de taille supérieure à celle des blocs plateforme)
            if (m_bloc.Width < VariablesGlobales.H_Largeur_Bloc)
            {
                // On crée le bloc en le décalant sur l'axe des X de sorte à le placer exactement sur le sol
                m_position.X = positionX + ((VariablesGlobales.H_Largeur_Bloc - (m_bloc.Width)) / 2);
            }
            else
            {
                // Sinon on crée le bloc normalement
                m_position = new Point(positionX, positionY);
            }
        }

        // Dessine le bloc sur la surface donnée
        public void dessiner(Surface ecranVideo, int codeNiveau)
        {
            // On recharge le l'image de l'oxygène
            if (m_code == 99)
            {
                // On change d'image tous les 5 FPS
                if (alternerImageOxygene > 5)
                {
                    if (m_oxy1 == null) m_oxy1 = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\oxygene2.png").Convert(ecranVideo, true, false);
                    m_bloc = m_oxy1;
                    if (alternerImageOxygene >= 10)
                    {
                        alternerImageOxygene = 0;
                    }
                }
                else
                {
                    if (m_oxy2 == null) m_oxy2 = new Surface(@"..\..\images\niveau" + Convert.ToString(codeNiveau) + "\\oxygene1.png").Convert(ecranVideo, true, false);
                    m_bloc = m_oxy2;
                }
                alternerImageOxygene++;
                ecranVideo.Blit(m_bloc, m_position);
            }
            else if (m_code == 34)
            {
                if (m_radblocD == null) m_radblocD = new Surface(@"..\..\images\niveau1\monstre_radioactifD.gif");
                if (m_radblocG == null) m_radblocG = new Surface(@"..\..\images\niveau1\monstre_radioactifG.gif");
                Surface s = m_radblocD;
                if (direction == "gauche") s = m_radblocG;
                s.AlphaBlending = true;
                if (sens)
                {
                    s.Alpha = (byte)(15 * (count));
                    ecranVideo.Blit(m_bloc, m_position);
                    ecranVideo.Blit(s, m_position);
                    count++;
                }
                else
                {
                    s.Alpha = (byte)(15 * (count));
                    ecranVideo.Blit(m_bloc, m_position);
                    ecranVideo.Blit(s, m_position);
                    count--;
                }
                if (count == 0 || count == 17) sens = !sens;
            }
            else ecranVideo.Blit(m_bloc, m_position);
        }

        // Dessine le bloc sur la surface donnée
        public void dessiner(Surface ecranVideo, Point camera)
        {
            m_position.X -= camera.X;
            m_position.Y -= camera.Y;
            if (m_code == 34)
            {
                if (m_radblocD == null) m_radblocD = new Surface(@"..\..\images\niveau1\monstre_radioactifD.gif");
                if (m_radblocG == null) m_radblocG = new Surface(@"..\..\images\niveau1\monstre_radioactifG.gif");
                Surface s = m_radblocD;
                s.AlphaBlending = true;
                if (sens)
                {
                    s.Alpha = (byte)(15 * (count));
                    ecranVideo.Blit(m_bloc, m_position);
                    ecranVideo.Blit(s, m_position);
                    count++;
                }
                else
                {
                    s.Alpha = (byte)(15 * (count));
                    ecranVideo.Blit(m_bloc, m_position);
                    ecranVideo.Blit(s, m_position);
                    count--;
                }
                if (count == 0 || count == 17) sens = !sens;
            }
            else ecranVideo.Blit(m_bloc, m_position);
        }

        /*** Accesseurs et Mutateurs ***/

        public Point getPosition()
        {
            return m_position;
        }

        public void setPosition(int x, int y)
        {
            m_position = new Point(x, y);
        }

        public Surface getSDLSurface()
        {
            return m_bloc;
        }

        public void setSDLSurface(Surface surfaceBloc)
        {
            m_bloc = surfaceBloc;
        }

        public int getCode()
        {
            return m_code;
        }

        public void setCode(int codeBloc)
        {
            m_code = codeBloc;
        }

        public int getLargeur()
        {
            return m_bloc.Width;
        }

        public int getHauteur()
        {
            return m_bloc.Height;
        }

        public void setTraversable(bool traversable)
        {
            m_traversable = traversable;
        }

        public bool estVisible()
        {
            return m_visible;
        }
        
        public bool estTraversable()
        {
            return m_traversable;
        }

        public bool getCollisionAvecPerso()
        {
            return collision_avec_perso;
        }

        public void setCollisionAvecPerso(bool collisionAvec)
        {
            collision_avec_perso = collisionAvec;
        }

        public bool estDechet()
        {
            return (m_code - 20 > 0) && (m_code - 20 < 10);
        }

        public bool estEnnemi()
        {
            return (m_code - 30 > 0) && (m_code - 30 < 10);
        }

        public bool estRadioactif()
        {
            return (m_code == 34);
        }
    }
}