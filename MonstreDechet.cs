using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Graphics;
using System.Drawing;

namespace MaPremiereApplication.Sources
{
    class MonstreDechet : Bloc
    {
        //attribut de l'heritage
        //public Surface m_bloc; // La surface du bloc
        //public Point m_position; // Position du bloc
        //public int m_code; // Code pour identifier le type de bloc
        //public bool m_visible; // Code pour savoir si le bloc sera visible ou pas
        //public bool m_traversable;
        //public bool collision_avec_perso = false;

        // Indique à quel dossier on ira chercher les images des monstres
        private int m_codeNiveau;

        bool collision_bloquante;
        bool collision_haut;
        bool collision_bas;
        bool collision_gauche;
        bool collision_droite;

        //bloc qu'on collisionne
        Bloc m_blocCollision_bas = null;
        Bloc m_blocCollision_haut = null;
        Bloc m_blocCollision_gauche = null;
        Bloc m_blocCollision_droite = null;

        //distance au personnage
        public int distance_perso = 500;

        //position du monstre par rapport au personnage
        bool sousLePersonnage = false;

        //teste si le personnage est proche de l'ennemi radioactif
        bool perso_proche_gauche = false;
        bool perso_proche_droite = false;
        bool sur_le_perso = false;

        // Images orientation monstre
        private Surface m_surfaceGauche;
        private Surface m_surfaceDroite;

        //Initialisation des variables
        public MonstreDechet(Bloc bloc, int codeNiveau)
        {
            m_codeNiveau = codeNiveau;
            id_bloc = bloc.id_bloc;
            m_bloc = bloc.m_bloc;
            m_position = bloc.m_position;
            m_code = bloc.m_code;
            m_visible = bloc.m_visible;
            m_traversable = bloc.m_traversable;

            switch (bloc.m_code)
            {
                case 31: // Monstre déchet 1
                    m_surfaceGauche = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_gauche.png");
                    m_surfaceDroite = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_droite.png");
                    break;
                case 32: // Monstre déchet 2
                    m_surfaceGauche = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_gauche.png");
                    m_surfaceDroite = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_droite.png");
                    break;
                case 33: // Monstre déchet 3
                    m_surfaceGauche = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_gauche.png");
                    m_surfaceDroite = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_dechet_droite.png");
                    break;
                case 34: // Monstre radioactif
                    m_surfaceGauche = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_radioactif_gauche.png");
                    m_surfaceDroite = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\monstre_radioactif_droite.png");
                    break;
            }

            direction = "gauche";
            collision_bloquante = false;



            collision_avec_perso = bloc.collision_avec_perso;

        }

        public void testeDistancePerso(Personnage personnage)
        {

            if ((m_position.Y < personnage.m_position.Y) || (personnage.m_position.Y < (m_position.Y - 127)))
            {
                perso_proche_droite = false;
                perso_proche_gauche = false;
                sur_le_perso = false;
                distance_perso = 500;
            }
            else
            {
                if ((m_position.X - personnage.m_position.X < 360) && (m_position.X - personnage.m_position.X > (personnage.m_bloc.Width / 2)))
                {
                    perso_proche_gauche = true;
                    perso_proche_droite = false;
                    sur_le_perso = false;
                    distance_perso = m_position.X - (personnage.m_position.X + personnage.m_bloc.Width);
                }
                else if ((personnage.m_position.X - m_position.X < 360) && (m_position.X + m_bloc.Rectangle.Width < (personnage.m_position.X + personnage.m_bloc.Width / 2)))
                {
                    perso_proche_gauche = false;
                    perso_proche_droite = true;
                    sur_le_perso = false;
                    distance_perso = personnage.m_position.X - (m_position.X + m_bloc.Rectangle.Width);
                }
                else if ((personnage.m_position.X - m_position.X <= personnage.m_bloc.Width) && (m_position.X - personnage.m_position.X <= personnage.m_bloc.Width))
                {
                    perso_proche_droite = false;
                    perso_proche_gauche = false;
                    sur_le_perso = true;
                    distance_perso = 0;
                }
                else
                {
                    perso_proche_droite = false;
                    perso_proche_gauche = false;
                    sur_le_perso = false;
                    distance_perso = 500;
                }
            }

        }

        public void deplacementMonstre(Scene niveau1, Personnage personnage, Surface ecranVideo)
        {

            testeDistancePerso(personnage);

            if ((m_code == 31) || (m_code == 32) || (m_code == 33))
            {
                if (direction.Equals("gauche"))
                {
                    // Recharger image
                    m_bloc = m_surfaceGauche;

                    if ((collision_gauche == false) || (collision_bloquante == false))
                    {

                        m_position.X -= 5;

                    }
                    else
                    {
                        direction = "droite";
                        m_position.X += 5;



                        // Recharger image
                        m_bloc = m_surfaceDroite;
                    }

                }
                else if (direction.Equals("droite"))
                {
                    // Recharger image
                    m_bloc = m_surfaceDroite;

                    if ((collision_droite == false) || (collision_bloquante == false))
                    {
                        m_position.X += 5;

                    }
                    else
                    {
                        direction = "gauche";
                        m_position.X -= 5;


                        // Recharger image
                        m_bloc = m_surfaceGauche;
                    }

                }

                if (personnage.m_position.Y + personnage.m_bloc.Height < m_position.Y)
                {
                    sousLePersonnage = true;
                }
                else
                {
                    sousLePersonnage = false;
                }
            }
            else if (m_code == 34)
            {
                if (sur_le_perso)
                {
                    //Console.Write("sur le perso");

                }
                else if (perso_proche_gauche)
                {
                    if ((collision_gauche == false) || (collision_bloquante == false))
                    {
                        direction = "gauche";
                        m_position.X -= 5;


                        // Rechargement image
                        m_bloc = m_surfaceGauche;
                    }
                }
                else if (perso_proche_droite)
                {
                    if ((collision_droite == false) || (collision_bloquante == false))
                    {
                        direction = "droite";
                        m_position.X += 5;

                        // Rechargement image
                        m_bloc = m_surfaceDroite;
                    }

                }
                else if (direction.Equals("gauche"))
                {
                    // Rechargement image
                    m_bloc = m_surfaceGauche;

                    if ((collision_gauche == false) || (collision_bloquante == false))
                    {
                        m_position.X -= 5;

                    }
                    else
                    {
                        direction = "droite";
                        m_position.X += 5;


                        // Rechargement image
                        m_bloc = m_surfaceDroite;
                    }

                }
                else if (direction.Equals("droite"))
                {
                    // Rechargement image
                    m_bloc = m_surfaceDroite;

                    if ((collision_droite == false) || (collision_bloquante == false))
                    {
                        m_position.X += 5;

                    }
                    else
                    {
                        direction = "gauche";
                        m_position.X -= 5;

                        // Rechargement image
                        m_bloc = m_surfaceGauche;
                    }

                }

                if (personnage.m_position.Y + personnage.m_bloc.Height < m_position.Y)
                {
                    sousLePersonnage = true;
                }
                else
                {
                    sousLePersonnage = false;
                }
            }

        }



        /*
        public void corrigeCollision()
        {
            if ((m_blocCollision_gauche != null) && (m_position.X < m_blocCollision_gauche.m_position.X + m_blocCollision_gauche.m_bloc.Rectangle.Width))
            {

                m_position.X += m_blocCollision_gauche.m_position.X + m_blocCollision_gauche.m_bloc.Rectangle.Width - m_position.X;
            }


            if ((m_blocCollision_droite != null) && (m_position.X + m_bloc.Rectangle.Width > m_blocCollision_droite.m_position.X))
            {
                m_position.X -= m_position.X + m_bloc.Rectangle.Width - m_blocCollision_droite.m_position.X;
            }
        }
         */


        public void detecteCollision(Scene niveau1, Personnage personnage)
        {
            //bloc qu'on collisionne
            Bloc m_blocCollision_gauche = null;
            Bloc m_blocCollision_droite = null;

            //par defaut on n'a pas de collision avec le perso
            setCollisionAvecPerso(false);



            if ((personnage.m_position.Y + personnage.m_bloc.Height <= m_position.Y) && (sousLePersonnage == true))
            {
                setCollisionAvecPerso(true);
            }



            foreach (Bloc b in niveau1.m_blocs)
            {
                // Si le bloc 
                if (b.m_position == m_position)
                    continue;

                // S'il s'agit d'un bloc plateforme 1, 2, 3, 4, 6, 7, 8 ou 9 ou d'un autre monstre ou d'un collider invisible ou d'une zone oxygene on rebrousse chemin donc colision bloquante
                if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9) || (b.m_code == 11) || (b.m_code == 12) || (b.m_code == 13) || (b.m_code == 30) || (b.m_code == 99))
                {
                    // Rendre la collision bloquante
                    collision_bloquante = true;
                }
                else
                {
                    // Sinon on permet au monstre de passer à travers
                    collision_bloquante = false;
                }


                if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9) || (b.m_code == 11) || (b.m_code == 12) || (b.m_code == 13) || (b.m_code == 30) || (b.m_code == 99))
                {

                    /***************************************************************
                     * collision avec une plateforme au-dessus ou sous le personnage
                     ***************************************************************/
                    bool collision_ht_bas_condition_x1 = false;
                    bool collision_ht_bas_condition_x2 = false;


                    /*
                     * collision sous le personnage
                     */
                    bool collision_bas_condition_y = false;

                    //il y a une collision si la différence de hauteur entre deux blocs
                    //est plus petite que la hauteur du bloc du personnage
                    //la collision est en bas si la position du bloc est plus grande
                    //que la position du personnage
                    if (b.m_position.Y - m_position.Y <= m_bloc.Rectangle.Height && b.m_position.Y - m_position.Y > 0)
                    {
                        collision_bas_condition_y = true;
                    }

                    //il faut que la position du personnage soit comprise
                    //entre la position du bloc et sa longueur
                    if (m_position.X >= b.m_position.X && m_position.X < b.m_position.X + b.m_bloc.Rectangle.Width)
                    {
                        collision_ht_bas_condition_x1 = true;
                    }

                    //ou alors, il faut que la longueur du personnage
                    //ne dépasse pas la longueur du bloc
                    if (m_position.X + m_bloc.Rectangle.Width > b.m_position.X && m_position.X + m_bloc.Rectangle.Width < b.m_position.X + b.m_bloc.Rectangle.Width)
                    {                                                              // >=
                        collision_ht_bas_condition_x2 = true;
                    }


                    //Console.WriteLine("x : {0} _y1 : {1} _y2 : {2}", collision_bas_condition_y, collision_bas_condition_x1, collision_bas_condition_x2);
                    if (collision_bas_condition_y && (collision_ht_bas_condition_x1 || collision_ht_bas_condition_x2))
                    {
                        //Console.WriteLine("Collision bas monstre");
                        collision_bas = true;
                    }



                    /*
                     * collision au dessus le personnage
                     */
                    bool collision_haut_condition_y = false;

                    //la collision est en bas si la position du personnage 
                    //est plus grande que la position du bloc
                    if (m_position.Y - b.m_position.Y <= b.m_bloc.Rectangle.Height && m_position.Y - b.m_position.Y > 0)
                    {                                               //personnage.m_bloc.Rectangle.Height
                        collision_haut_condition_y = true;
                    }


                    if (collision_haut_condition_y && (collision_ht_bas_condition_x1 || collision_ht_bas_condition_x2))
                    {
                        //Console.WriteLine("Collision haut monstre");
                        collision_haut = true;
                    }



                    /******************************************************************
                     * collision avec une plateforme à gauche ou à droite du personnage
                     ******************************************************************/
                    bool collision_gche_droite_condition_y1 = false;
                    bool collision_gche_droite_condition_y2 = false;


                    /*
                     * collision à gauche
                     */
                    bool collision_gauche_condition_x = false;

                    //il y a une collision si la différence de longueur entre les deux blocs
                    //est plus petite que la longueur du bloc du personnage
                    //la collision est à gauche si la position du personnage
                    //est plus grande que la position du bloc


                    if (m_position.X - b.m_position.X - (personnage.m_bloc.Rectangle.Width / 8) <= b.m_bloc.Rectangle.Width && m_position.X - b.m_position.X > 0)
                    {                                                 //personnage.m_bloc.Rectangle.Width
                        collision_gauche_condition_x = true;
                    }


                    //il faut que la position du personnage soit comprise
                    //entre la position du bloc et sa hauteur
                    if (m_position.Y >= b.m_position.Y && m_position.Y <= b.m_position.Y + b.m_bloc.Rectangle.Height)
                    {
                        collision_gche_droite_condition_y1 = true;
                    }

                    //ou alors, il faut que la hauteur du personnage
                    //ne dépasse pas la hauteur du bloc
                    if (m_position.Y + m_bloc.Rectangle.Height > b.m_position.Y && m_position.Y + m_bloc.Rectangle.Height <= b.m_position.Y + b.m_bloc.Rectangle.Height)
                    {
                        collision_gche_droite_condition_y2 = true;
                    }


                    if (collision_gauche_condition_x && (collision_gche_droite_condition_y1 || collision_gche_droite_condition_y2))
                    {
                        //Console.WriteLine("Collision gauche monstre");
                        m_blocCollision_gauche = b;
                        collision_gauche = true;
                    }



                    /*
                     * collision à droite
                     */
                    bool collision_droite_condition_x = false;

                    //la collision est à droite si la position du bloc
                    //est plus grande que la position du personnage


                    if (b.m_position.X - m_position.X - (personnage.m_bloc.Rectangle.Width / 8) <= m_bloc.Rectangle.Width && b.m_position.X - m_position.X > 0)
                    {
                        collision_droite_condition_x = true;
                    }


                    if (collision_droite_condition_x && (collision_gche_droite_condition_y1 || collision_gche_droite_condition_y2))
                    {
                        //Console.WriteLine("Collision droite monstre");
                        m_blocCollision_droite = b;
                        collision_droite = true;
                    }

                }
            }
        }

        public void metAZeroCollision()
        {
            collision_haut = false;
            collision_gauche = false;
            collision_droite = false;
            collision_bas = false;
        }

        public void annuleCollisionBasse(Scene niveau1, Personnage personnage)
        {
            //on empeche toute les collision basse
            while ((collision_bas == true) && (collision_bloquante == true))
            {
                m_position.Y -= 1;
                metAZeroCollision();
                detecteCollision(niveau1, personnage);
            }
            m_position.Y += 1;
        }

        public void annuleCollisionHaute(Scene niveau1, Personnage personnage)
        {
            //on empeche toute les collision haute
            while ((collision_haut == true) && (collision_bloquante == true))
            {
                m_position.Y += 1;
                metAZeroCollision();
                detecteCollision(niveau1, personnage);
            }
            m_position.Y -= 1;
        }

        public void annuleCollisionGauche(Scene niveau1, Personnage personnage)
        {

            //on empeche toute les collision gauche
            while ((collision_gauche == true) && (collision_bloquante == true))
            {
                m_position.X += 1;
                metAZeroCollision();
                detecteCollision(niveau1, personnage);
            }
            m_position.X -= 1;

        }

        public void annuleCollisionDroite(Scene niveau1, Personnage personnage)
        {

            //on empeche toute les collision droite
            while ((collision_droite == true) && (collision_bloquante == true))
            {
                m_position.X -= 1;
                metAZeroCollision();
                detecteCollision(niveau1, personnage);
            }
            m_position.X += 1;

        }


    }
}
