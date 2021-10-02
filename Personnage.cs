using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Graphics;
using System.Drawing;
using SdlDotNet.Input;

namespace MaPremiereApplication.Sources
{
    class Personnage
    {
        public Surface m_bloc;
        public Point m_position;

        // Variables pour l'animation
        private int m_SpriteSheetOffset = 0;
        private int m_codeNiveau;

        // Detection collision
        public bool collision_haut = false;
        public bool collision_bas = false;
        public bool collision_gauche = false;
        public bool collision_droite = false;
        public bool collision_bloquante = true;
        public bool collision_ennemi_cote = false;
        public bool collision_ennemi_bas = false;
        public bool collision_dechet = false;
        public bool collision_poubelle = false;
        public bool collision_oxygene = false;

        //bloc qu'on collisionne
        Bloc m_blocCollision_bas = null;
        Bloc m_blocCollision_haut = null;
        Bloc m_blocCollision_gauche = null;
        Bloc m_blocCollision_droite = null;

        // Variables pour le saut
        public double vitesse_x = +1;//vitesse horizontale
        public double vitesse_y = +10;//vitesse verticale
        public double vitesse_gravite = +20;//vitesse gravite
        public bool saut = false;
        public bool saut_droit = false;
        public bool saut_gauche = false;
        public int temps_saut = 0;
        public Key ancienneTouche = Key.N;

        //orientation du personnage pour rebondir
        public String orientation = "droite";

        //on stocke l'ennemi qu'on collisionne
        public MonstreDechet monstreCollision = null;
        public int temps_saut_ennemi = 0;
        public bool saut_ennemi = false;

        //est-ce qu'on descend
        public bool persoDescend = true;

        //sante
        public int sante = 100;
        public int temps_invulnerable_sante = 0;
        public int temps_invulnerable_oxygene_sante = 0;
        public int temps_invulnerable_ennemi_sante = 0;
        //oxygene
        public int oxygene = 100;
        public int temps_invulnerable_oxygene = 0;
        //dechet inventaire
        public int inventaire = 0;
        public Point positionDechet = new Point();
        public Surface surfaceDechet = null;

        //poubelle collisionne
        public Bloc poubelleCollision = null;

        //index du dechet a supprimer
        public int indexDechet = -1;

        //type de poubelle qu'on collisionne
        int typePoubelleCollision = 0;

        //score
        public int score = 0;
        //nombre de dechets mal tries
        public int nbMauvaisTri = 0;

        public bool a_mal_trie = false;

        Surface perso_droite1;
        Surface perso_droite2;
        Surface perso_droite3;
        Surface perso_gauche1;
        Surface perso_gauche2;
        Surface perso_gauche3;

        //Initialisation des variables
        public Personnage(Surface ecranVideo, int codeNiveau)
        {
            m_codeNiveau = codeNiveau;
            m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_face.png").Convert(ecranVideo, true, true);
            m_bloc.Transparent = true;
            m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);
            m_position = new Point(0, 0);
        }


        //Montre le point sur l'ecran
        public void show(Surface ecranVideo)
        {
            ecranVideo.Blit(m_bloc, m_position);
        }


        public bool gestionSante()
        {
            //si on a encore de la vie on verifie si on en perd
            if (sante > 0)
            {
                //perte de vie en fonction de l'oxygene
                if (temps_invulnerable_oxygene_sante == 0)
                {
                    if (oxygene == 0)
                    {
                        sante--;
                        temps_invulnerable_oxygene_sante = 10;
                    }
                }
                else
                {
                    temps_invulnerable_oxygene_sante--;
                }


                //perte de vie en fonction des ennemis
                if (collision_ennemi_cote)
                {
                    gestionCollisionEnnemiCote();
                }
                else if (temps_invulnerable_ennemi_sante > 0)
                {
                    temps_invulnerable_ennemi_sante--;
                }

                return true;
            }
            //si on a plus de vie on meurt
            else
            {
                return false;

            }
        }

        public bool clignoterPersonnage()
        {
            if ((temps_invulnerable_ennemi_sante > 0) && (temps_invulnerable_ennemi_sante % 2 == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void gestionOxygene()
        {
            if (temps_invulnerable_oxygene == 0)
            {
                if (collision_oxygene == true)
                {
                    if (oxygene < 100)
                    {
                        if (oxygene < 90)
                        {
                            oxygene += 10;
                        }
                        else
                        {
                            oxygene = 100;
                        }
                    }
                }
                else
                {
                    if (oxygene > 0)
                    {
                        oxygene--;
                    }
                }
                temps_invulnerable_oxygene = 10;
            }
            else
            {
                temps_invulnerable_oxygene--;
            }

        }

        public void gestionCollisionEnnemiCote()
        {
            if (collision_ennemi_cote)
            {
                //perte de vie en fonction des ennemis
                if (temps_invulnerable_ennemi_sante == 0)
                {
                    if (monstreCollision != null)
                    {
                        //if (monstreCollision.m_code == 34) Program.soundManager.playSE("HURT_RAD");
                        Program.soundManager.playSE("HURT");
                    }

                    if (sante >= 10)
                    {
                        sante -= 10;
                        temps_invulnerable_ennemi_sante = 30;
                    }
                    else
                    {

                        sante = 0;
                        temps_invulnerable_ennemi_sante = 30;

                    }
                }
                else
                {
                    temps_invulnerable_ennemi_sante--;
                }
            }
            else if (temps_invulnerable_ennemi_sante > 0)
            {
                temps_invulnerable_ennemi_sante--;
            }
        }

        public void testeSiPersoDescend()
        {
            if ((temps_saut < 6) && (saut == true))
            {
                persoDescend = false;
            }
            else if ((saut == true))
            {
                persoDescend = true;
            }
            else
            {
                persoDescend = true;
            }
        }

        public void gestionDestructionDechet(Scene niveau1)
        {
            if (indexDechet != -1)
            {
                if (((niveau1.m_blocs.ElementAt(indexDechet).m_position.Y > m_position.Y + m_bloc.Rectangle.Height)
                    || (niveau1.m_blocs.ElementAt(indexDechet).m_position.X + niveau1.m_blocs.ElementAt(indexDechet).m_bloc.Rectangle.Width < m_position.X)
                    || (m_position.X + m_bloc.Rectangle.Width < niveau1.m_blocs.ElementAt(indexDechet).m_position.X))
                    && (niveau1.m_blocs.ElementAt(indexDechet).m_bloc.Rectangle.Height < m_bloc.Rectangle.Height))
                {
                    indexDechet = -1;
                    collision_dechet = false;
                    inventaire = 0;
                }
                else
                {
                    Program.soundManager.playSE("PICK");
                    positionDechet = niveau1.m_blocs.ElementAt(indexDechet).m_position;
                    surfaceDechet = niveau1.m_blocs.ElementAt(indexDechet).getSDLSurface();
                    niveau1.m_blocs.RemoveAt(indexDechet);
                    indexDechet = -1;
                    collision_dechet = false;
                }
            }
        }

        public void gestionCollisionPoubelle(Scene niveau1)
        {
            if ((collision_poubelle) && (persoDescend) && (poubelleCollision != null) && (m_position.Y + m_bloc.Rectangle.Height <= poubelleCollision.m_position.Y))
            {
                if (inventaire > 0)
                {

                    if ((typePoubelleCollision == 11 && inventaire == 21) || (typePoubelleCollision == 12 && inventaire == 22) || (typePoubelleCollision == 13 && inventaire == 23))
                    {
                        if (inventaire == 21) Program.soundManager.playSE("SORT_GLASS");
                        else if (inventaire == 22) Program.soundManager.playSE("SORT_PLAST");
                        else if (inventaire == 23) Program.soundManager.playSE("SORT_PAPER");
                        //on jette le dechet, mise a jour de l'inventaire
                        //Console.WriteLine("\t Déchet déposé [type {0}] !", inventaire);
                        inventaire = 0;
                        positionDechet = new Point();
                        surfaceDechet = null;

                        //mise a jour du score
                        score += 100;
                        //Console.WriteLine("\t Nouveau score : {0} ; Mauvais tri = {1}", score, nbMauvaisTri);
                    }
                    else
                    {
                        a_mal_trie = true;


                        //on inflige des degats
                        
                        if (sante >= 10)
                        {
                            sante -= 10;
                            temps_invulnerable_ennemi_sante = 30;
                        }
                        else
                        {

                            sante = 0;
                            temps_invulnerable_ennemi_sante = 30;

                        }
                        
                        




                        //on fait apparaitre un autre monstre
                        Program.soundManager.playSE("SORT_BAD");
                        int typeMonstre = 0;
                        switch (inventaire)
                        {
                            case 21:
                                typeMonstre = 31;
                                break;
                            case 22:
                                typeMonstre = 32;
                                break;
                            case 23:
                                typeMonstre = 33;
                                break;
                        }

                        String img = @"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\monstre_dechet_droite.png";
                        Bloc temp = new Bloc(img, poubelleCollision.m_position.X, poubelleCollision.m_position.Y, typeMonstre);
                        MonstreDechet nouvelEnnemi = new MonstreDechet(temp, niveau1.m_codeNiveau);

                        //on recentre l'image
                        if (nouvelEnnemi.m_bloc.Rectangle.Height < poubelleCollision.m_bloc.Rectangle.Height)
                        {
                            nouvelEnnemi.m_position.Y += poubelleCollision.m_bloc.Rectangle.Height - nouvelEnnemi.m_bloc.Rectangle.Height;
                        }
                        else if (nouvelEnnemi.m_bloc.Rectangle.Height > poubelleCollision.m_bloc.Rectangle.Height)
                        {
                            nouvelEnnemi.m_position.Y -= nouvelEnnemi.m_bloc.Rectangle.Height - poubelleCollision.m_bloc.Rectangle.Height;
                        }
                        nouvelEnnemi.m_code = typeMonstre;
                        nouvelEnnemi.m_visible = true;
                        nouvelEnnemi.collision_avec_perso = false;
                        niveau1.m_blocs_ennemis.Insert(0, nouvelEnnemi);
                        niveau1.m_blocs.Insert(0, nouvelEnnemi);

                        //mise a jour du score et du nombre de dechets mal tries
                        score -= 150;
                        nbMauvaisTri++;
                        Console.WriteLine("\t Nouveau score : {0} ; Mauvais tri = {1}", score, nbMauvaisTri);

                        //mise a jour de l'inventaire
                        inventaire = 0;
                        positionDechet = new Point();
                        surfaceDechet = null;
                        //le score ne peut pas etre negatif
                        //score = (score < 0) ? 0 : score;
                    }
                }
            }
        }




        public void gestionCollisionEnnemiBas(Scene niveau1, Surface ecranVideo)
        {
            if (temps_invulnerable_ennemi_sante == 0)
            {
                if (persoDescend == true)
                {
                    if ((collision_ennemi_bas == true) && (collision_ennemi_cote == false) && (monstreCollision != null) && (a_mal_trie == false))
                    {
                        if ((niveau1.m_blocs.Count > 0) && (niveau1.m_blocs_ennemis.Count > 0))
                        {
                            foreach (Bloc bloc in niveau1.m_blocs)
                            {
                                if (bloc.id_bloc == monstreCollision.id_bloc)
                                {
                                    if ((bloc.m_code == 31) || (bloc.m_code == 32) || (bloc.m_code == 33))
                                    {
                                        //on touche l'ennemi pour un meilleur effet collision
                                        //mouvementEnBas((int)vitesse_y * (int)8, niveau1);
                                        mouvementEnBas((int)vitesse_y * (int)4, niveau1);

                                        //on rebondis sur l'ennemi
                                        saut_ennemi = true;

                                        //on créé le déchet a faire apparaitre
                                        if (bloc.m_code == 31)
                                        {
                                            //on change le code de l'ennemi il devient un dechet
                                            bloc.m_code = 21;
                                            // Nombre aléatoire pour le chargement des blocs à images aléatoires
                                            int rndNumber;
                                            Random rnd = new Random();
                                            //on sauvegarde la taille de l'ennemi
                                            int saveTailleEnnemi = bloc.m_bloc.Rectangle.Height;
                                            //on change son image en image de dechet
                                            rndNumber = rnd.Next(0, 4);
                                            if (rndNumber == 1)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-vert1.png");
                                            }
                                            else if (rndNumber == 2)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-vert2.png");
                                            }
                                            else
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-vert3.png");
                                            }

                                            //on centre l'image
                                            if (bloc.m_bloc.Rectangle.Height < saveTailleEnnemi)
                                            {

                                                bloc.m_position.Y += saveTailleEnnemi - bloc.m_bloc.Rectangle.Height;
                                            }
                                            else if (bloc.m_bloc.Rectangle.Height > saveTailleEnnemi)
                                            {
                                                bloc.m_position.Y -= bloc.m_bloc.Rectangle.Height - saveTailleEnnemi;
                                            }

                                            Program.soundManager.playSE("KILL");

                                        }
                                        else if (bloc.m_code == 32)
                                        {
                                            //on change le code de l'ennemi il devient un dechet
                                            bloc.m_code = 22;
                                            // Nombre aléatoire pour le chargement des blocs à images aléatoires
                                            int rndNumber;
                                            Random rnd = new Random();
                                            //on sauvegarde la taille de l'ennemi
                                            int saveTailleEnnemi = bloc.m_bloc.Rectangle.Height;
                                            //on change son image en image de dechet
                                            rndNumber = rnd.Next(0, 4);
                                            if (rndNumber == 1)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-jaune1.png");
                                            }
                                            else if (rndNumber == 2)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-jaune2.png");
                                            }
                                            else
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-jaune3.png");
                                            }

                                            //on centre l'image
                                            if (bloc.m_bloc.Rectangle.Height < saveTailleEnnemi)
                                            {

                                                bloc.m_position.Y += saveTailleEnnemi - bloc.m_bloc.Rectangle.Height;
                                            }
                                            else if (bloc.m_bloc.Rectangle.Height > saveTailleEnnemi)
                                            {
                                                bloc.m_position.Y -= bloc.m_bloc.Rectangle.Height - saveTailleEnnemi;
                                            }
                                            Program.soundManager.playSE("KILL");
                                        }
                                        else if (bloc.m_code == 33)
                                        {
                                            //on change le code de l'ennemi il devient un dechet
                                            bloc.m_code = 23;
                                            // Nombre aléatoire pour le chargement des blocs à images aléatoires
                                            int rndNumber;
                                            Random rnd = new Random();
                                            //on sauvegarde la taille de l'ennemi
                                            int saveTailleEnnemi = bloc.m_bloc.Rectangle.Height;
                                            //on change son image en image de dechet
                                            rndNumber = rnd.Next(0, 4);
                                            if (rndNumber == 1)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-gris1.png");
                                            }
                                            else if (rndNumber == 2)
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-gris2.png");
                                            }
                                            else
                                            {
                                                bloc.m_bloc = new Surface(@"..\..\images\niveau" + Convert.ToString(niveau1.m_codeNiveau) + "\\dechet-gris3.png");
                                            }

                                            //on centre l'image
                                            if (bloc.m_bloc.Rectangle.Height < saveTailleEnnemi)
                                            {

                                                bloc.m_position.Y += saveTailleEnnemi - bloc.m_bloc.Rectangle.Height;
                                            }
                                            else if (bloc.m_bloc.Rectangle.Height > saveTailleEnnemi)
                                            {
                                                bloc.m_position.Y -= bloc.m_bloc.Rectangle.Height - saveTailleEnnemi;
                                            }
                                            Program.soundManager.playSE("KILL");
                                        }

                                    }
                                    break;
                                }
                            }
                            foreach (Bloc bloc in niveau1.m_blocs_ennemis)
                            {
                                if (bloc.id_bloc == monstreCollision.id_bloc)
                                {
                                    if ((bloc.m_code == 31) || (bloc.m_code == 32) || (bloc.m_code == 33))
                                    {
                                        niveau1.m_blocs_ennemis.Remove(bloc);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        public void sautEnnemi(Scene niveau1, Surface ecranVideo)
        {


            if (saut_ennemi == true)
            {
                //mise a jour etat du clavier
                KeyboardState keyboard = new KeyboardState();

                //si on a pas fini le saut
                if (temps_saut_ennemi < 16)
                {
                    // si c'est un saut vers la gauche
                    if (((orientation.Equals("gauche")) || (keyboard.IsKeyPressed(Key.LeftArrow))) && (collision_gauche == false) && (collision_droite == false) && (collision_haut == false) && (collision_bas == false))
                    {
                        mouvementAGauche((int)vitesse_x * (int)15, niveau1, ecranVideo, true);
                        if (temps_saut_ennemi < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)4, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)4, niveau1);
                        }
                        temps_saut_ennemi++;

                        metAZeroCollision();
                        GetBoundingBoxCollisionStatus(niveau1);
                        
                        annuleCollisionGaucheEnnemi(niveau1, ecranVideo);
                       // annuleCollisionDroite(niveau1, ecranVideo);
                        annuleCollisionBasse(niveau1);
                        annuleCollisionHaute(niveau1);
                    }
                    // si c'est un saut vers la droite
                    else if (((orientation.Equals("droite")) || (keyboard.IsKeyPressed(Key.RightArrow))) && (collision_droite == false) && (collision_gauche == false) && (collision_haut == false) && (collision_bas == false))
                    {
                        mouvementADroite((int)vitesse_x * (int)15, niveau1, ecranVideo, true);
                        if (temps_saut_ennemi < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)4, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)4, niveau1);
                        }
                        temps_saut_ennemi++;

                        metAZeroCollision();
                        GetBoundingBoxCollisionStatus(niveau1);
                        
                        //annuleCollisionGauche(niveau1, ecranVideo);
                        annuleCollisionDroiteEnnemi(niveau1, ecranVideo);
                        annuleCollisionBasse(niveau1);
                        annuleCollisionHaute(niveau1);
                    }
                    //si c'est un saut vertical
                    else if ((collision_haut == false) && (collision_bas == false) && (collision_droite == false) && (collision_gauche == false))
                    {
                        if (temps_saut_ennemi < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)4, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)4, niveau1);
                        }
                        temps_saut_ennemi++;

                        metAZeroCollision();
                        GetBoundingBoxCollisionStatus(niveau1);
                        
                        annuleCollisionGaucheEnnemi(niveau1, ecranVideo);
                        annuleCollisionDroiteEnnemi(niveau1, ecranVideo);
                        annuleCollisionBasse(niveau1);
                        annuleCollisionHaute(niveau1);
                    }
                    //si on arrete le saut avant sa fin
                    else
                    {
                        temps_saut_ennemi = 0;
                        saut_ennemi = false;
                        //annuleCollisionBasse(niveau1);
                        //annuleCollisionHaute(niveau1);
                        //annuleCollisionGauche(niveau1, ecranVideo);
                        //annuleCollisionDroite(niveau1, ecranVideo);
                        metAZeroCollision();
                        GetBoundingBoxCollisionStatus(niveau1);
                        
                        annuleCollisionGaucheEnnemi(niveau1, ecranVideo);
                        annuleCollisionDroiteEnnemi(niveau1, ecranVideo);
                        annuleCollisionBasse(niveau1);
                        annuleCollisionHaute(niveau1);
                        
                    }

                }
                else
                {
                    temps_saut_ennemi = 0;
                    saut_ennemi = false;
                    //annuleCollisionBasse(niveau1);
                    //annuleCollisionHaute(niveau1);
                    //annuleCollisionGauche(niveau1, ecranVideo);
                    //annuleCollisionDroite(niveau1, ecranVideo);
                    metAZeroCollision();
                    GetBoundingBoxCollisionStatus(niveau1);
                    
                    annuleCollisionGaucheEnnemi(niveau1, ecranVideo);
                    annuleCollisionDroiteEnnemi(niveau1, ecranVideo);
                    annuleCollisionBasse(niveau1);
                    annuleCollisionHaute(niveau1);
                }
            }


        }




        public void GetBoundingBoxCollisionStatus(Scene niveau1)
        {
            bool old_collision_oxygene = collision_oxygene;
            //l'oxygene baissera sauf si on rencontreun bloc oxygene
            collision_oxygene = false;
            //par defaut on ne touche pas d'ennemis
            collision_ennemi_bas = false;
            collision_ennemi_cote = false;
            monstreCollision = null;
            collision_poubelle = false;
            collision_dechet = false;
            typePoubelleCollision = 0;

            m_blocCollision_bas = null;
            m_blocCollision_haut = null;
            m_blocCollision_gauche = null;
            m_blocCollision_droite = null;

            poubelleCollision = null;


            foreach (Bloc b in niveau1.m_blocs)
            {
                /***************************************************************
                 * collision avec une plateforme au-dessus ou sous le personnage
                 ***************************************************************/
                bool collision_ht_bas_condition_x1 = false;
                bool collision_ht_bas_condition_x2 = false;
                bool collision_ht_bas_condition_x3 = false;


                /*
                 * collision sous le personnage
                 */
                bool collision_bas_condition_y1 = false;
                bool collision_bas_condition_y2 = false;

                //il y a une collision si la différence de hauteur entre deux blocs
                //est plus petite que la hauteur du bloc du personnage
                //la collision est en bas si la position du bloc est plus grande
                //que la position du personnage
                if ((b.m_position.Y - m_position.Y <= m_bloc.Rectangle.Height) && (b.m_position.Y - m_position.Y > 0) && (m_bloc.Rectangle.Height - b.m_bloc.Rectangle.Height <= 0))
                {
                    collision_bas_condition_y1 = true;
                }
                //si le personnage est plus haut que le bloc
                if (((b.m_position.Y - (m_bloc.Rectangle.Height + m_position.Y)) <= m_bloc.Rectangle.Height) && (b.m_position.Y - m_position.Y >= 0) && (m_bloc.Rectangle.Height - b.m_bloc.Rectangle.Height > 0))
                //(m_bloc.Rectangle.Height + m_position.Y <= b.m_position.Y) 
                {
                    collision_bas_condition_y2 = true;
                }

                //il faut que la position du personnage soit comprise
                //entre la position du bloc et sa longueur
                if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                {
                    if ((persoDescend == true) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) < 30) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) > 10) && (b.m_position.Y - m_position.Y <= (m_bloc.Rectangle.Height * 2)) && (m_position.Y < b.m_position.Y))
                    {
                        if ((m_position.X >= b.m_position.X) && (m_position.X < b.m_position.X + b.m_bloc.Rectangle.Width + (m_bloc.Rectangle.Width / 8)))
                        {
                            collision_ht_bas_condition_x1 = true;
                        }
                    }
                }
                else
                {
                    if ((m_position.X >= b.m_position.X) && (m_position.X < b.m_position.X + b.m_bloc.Rectangle.Width))
                    {
                        collision_ht_bas_condition_x1 = true;
                    }
                }

                //ou alors, il faut que la longueur du personnage
                //ne dépasse pas la longueur du bloc
                if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                {
                    //if ((persoDescend == true) && (m_position.Y + m_bloc.Rectangle.Height < b.m_position.Y - (98 - b.m_bloc.Rectangle.Height)) && (b.m_position.Y - m_position.Y <= (m_bloc.Rectangle.Height * 2)))
                    if ((persoDescend == true) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) < 30) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) > 10) && (b.m_position.Y - m_position.Y <= (m_bloc.Rectangle.Height * 2)) && (m_position.Y < b.m_position.Y))
                    {
                        if ((m_position.X + m_bloc.Rectangle.Width > b.m_position.X - (m_bloc.Rectangle.Width / 8)) && (m_position.X + m_bloc.Rectangle.Width <= b.m_position.X + b.m_bloc.Rectangle.Width))
                        {                                                              // >=
                            collision_ht_bas_condition_x2 = true;
                        }
                    }
                }
                else
                {
                    if ((m_position.X + m_bloc.Rectangle.Width > b.m_position.X) && (m_position.X + m_bloc.Rectangle.Width < b.m_position.X + b.m_bloc.Rectangle.Width))
                    {                                                              // >=
                        collision_ht_bas_condition_x2 = true;
                    }
                }

                //si le personnage est plus large que le bloc
                if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                {
                    if ((persoDescend == true) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) < 30) && (b.m_position.Y - (m_position.Y + m_bloc.Rectangle.Height) > 10) && (b.m_position.Y - m_position.Y <= (m_bloc.Rectangle.Height * 2)) && (m_position.Y < b.m_position.Y))
                    {
                        if ((b.m_position.X - m_position.X > 0) && ((m_position.X + m_bloc.Rectangle.Width) - (b.m_position.X + b.m_bloc.Rectangle.Width) > 0) && (m_bloc.Rectangle.Width - b.m_bloc.Rectangle.Width > 0))
                        {                                                              // >=
                            collision_ht_bas_condition_x3 = true;
                        }
                    }
                }
                else
                {
                    if ((b.m_position.X - m_position.X > 0) && ((m_position.X + m_bloc.Rectangle.Width) - (b.m_position.X + b.m_bloc.Rectangle.Width) > 0) && (m_bloc.Rectangle.Width - b.m_bloc.Rectangle.Width > 0))
                    {                                                              // >=
                        collision_ht_bas_condition_x3 = true;
                    }
                }



                //Console.WriteLine("x : {0} _y1 : {1} _y2 : {2}", collision_bas_condition_y, collision_bas_condition_x1, collision_bas_condition_x2);
                if ((collision_bas_condition_y1 || collision_bas_condition_y2) && (collision_ht_bas_condition_x1 || collision_ht_bas_condition_x2 || collision_ht_bas_condition_x3))
                {
                    m_blocCollision_bas = b;

                    // S'il s'agit d'un bloc plateforme 1, 2, 3, 4, 6, 7, 8 ou 9 on est bloqué
                    if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9))
                    {
                        // Rendre la collision bloquante
                        collision_bloquante = true;

                        //Console.WriteLine("Collision bas");
                        collision_bas = true;
                    }
                    else
                    {
                        // Rendre la collision non bloquante
                        collision_bloquante = false;
                    }

                    // S'il s'agit d'un bloc ennemi
                    if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                    {
                        //Console.WriteLine("Collision bas");
                        collision_ennemi_bas = true;
                        monstreCollision = new MonstreDechet(b, m_codeNiveau);
                    }

                    // S'il s'agit d'un bloc dechet
                    if ((b.m_code == 21) || (b.m_code == 22) || (b.m_code == 23))
                    {
                        if (saut_ennemi == false)
                        {
                            if (inventaire == 0)
                            {
                                collision_dechet = true;
                                inventaire = b.m_code;

                                foreach (Bloc blocd in niveau1.m_blocs)
                                {
                                    if (blocd.m_position.Equals(b.m_position))
                                    {
                                        if ((blocd.m_code == 21) || (blocd.m_code == 22) || (blocd.m_code == 23))
                                        {
                                            indexDechet = niveau1.m_blocs.IndexOf(blocd);
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }


                    // S'il s'agit d'un bloc poubelle
                    if ((b.m_code == 11) || (b.m_code == 12) || (b.m_code == 13))
                    {
                        collision_poubelle = true;
                        typePoubelleCollision = b.m_code;
                        poubelleCollision = b;
                    }
                    else
                    {
                        collision_poubelle = false;
                    }

                    // S'il s'agit d'un bloc oxygene
                    if ((b.m_code == 99))
                    {
                        if (old_collision_oxygene == false) Program.soundManager.playSE("OXYG");
                        collision_oxygene = true;
                    }


                }



                /*
                 * collision au dessus le personnage
                 */
                bool collision_haut_condition_y = false;

                //la collision est en bas si la position du personnage 
                //est plus grande que la position du bloc
                if ((m_position.Y - b.m_position.Y <= b.m_bloc.Rectangle.Height) && (m_position.Y - b.m_position.Y > 0))
                {                                               //personnage.m_bloc.Rectangle.Height
                    collision_haut_condition_y = true;
                }


                if (collision_haut_condition_y && (collision_ht_bas_condition_x1 || collision_ht_bas_condition_x2 || collision_ht_bas_condition_x3))
                {
                    m_blocCollision_haut = b;

                    // S'il s'agit d'un bloc plateforme 1, 2, 3, 4, 6, 7, 8 ou 9 on est bloqué
                    if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9))
                    {
                        // Rendre la collision bloquante
                        collision_bloquante = true;

                        //Console.WriteLine("Collision haut");
                        collision_haut = true;
                    }
                    else
                    {
                        // Rendre la collision non bloquante
                        collision_bloquante = false;
                    }

                    // S'il s'agit d'un bloc ennemi
                    if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                    {
                        // Console.WriteLine("Collision haut");
                        collision_ennemi_cote = true;
                        monstreCollision = new MonstreDechet(b, m_codeNiveau);
                    }

                    // S'il s'agit d'un bloc dechet
                    if ((b.m_code == 21) || (b.m_code == 22) || (b.m_code == 23))
                    {
                        if (saut_ennemi == false)
                        {
                            if (inventaire == 0)
                            {
                                collision_dechet = true;
                                inventaire = b.m_code;

                                foreach (Bloc blocd in niveau1.m_blocs)
                                {
                                    if (blocd.m_position.Equals(b.m_position))
                                    {
                                        if ((blocd.m_code == 21) || (blocd.m_code == 22) || (blocd.m_code == 23))
                                        {
                                            indexDechet = niveau1.m_blocs.IndexOf(blocd);
                                            break;
                                        }
                                    }
                                }

                            }
                        }

                    }



                    // S'il s'agit d'un bloc oxygene
                    if ((b.m_code == 99))
                    {
                        if (old_collision_oxygene == false) Program.soundManager.playSE("OXYG");
                        collision_oxygene = true;
                    }



                }



                /******************************************************************
                 * collision avec une plateforme à gauche ou à droite du personnage
                 ******************************************************************/
                bool collision_gche_droite_condition_y1 = false;
                bool collision_gche_droite_condition_y2 = false;

                /*
                 * collision à gauche
                 */
                bool collision_gauche_condition_x1 = false;

                //il y a une collision si la différence de longueur entre les deux blocs
                //est plus petite que la longueur du bloc du personnage
                //la collision est à gauche si la position du personnage
                //est plus grande que la position du bloc
                if (m_position.X - b.m_position.X <= b.m_bloc.Rectangle.Width && m_position.X - b.m_position.X > 0)
                {                                                 //personnage.m_bloc.Rectangle.Width
                    collision_gauche_condition_x1 = true;
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

                if (collision_gauche_condition_x1 && (collision_gche_droite_condition_y1 || collision_gche_droite_condition_y2))
                {

                    m_blocCollision_gauche = b;

                    // S'il s'agit d'un bloc plateforme 1, 2, 3, 4, 6, 7, 8 ou 9 on est bloqué
                    if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9))
                    {
                        // Rendre la collision bloquante
                        collision_bloquante = true;

                        //Console.WriteLine("Collision gauche");
                        collision_gauche = true;
                    }
                    else
                    {
                        // Rendre la collision non bloquante
                        collision_bloquante = false;
                    }

                    // S'il s'agit d'un bloc ennemi
                    if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                    {
                        //Console.WriteLine("Collision gauche");
                        collision_ennemi_cote = true;
                        monstreCollision = new MonstreDechet(b, m_codeNiveau);
                    }


                    // S'il s'agit d'un bloc dechet
                    if ((b.m_code == 21) || (b.m_code == 22) || (b.m_code == 23))
                    {
                        if (saut_ennemi == false)
                        {
                            if (inventaire == 0)
                            {
                                collision_dechet = true;
                                inventaire = b.m_code;

                                foreach (Bloc blocd in niveau1.m_blocs)
                                {
                                    if (blocd.m_position.Equals(b.m_position))
                                    {
                                        if ((blocd.m_code == 21) || (blocd.m_code == 22) || (blocd.m_code == 23))
                                        {
                                            indexDechet = niveau1.m_blocs.IndexOf(blocd);
                                            break;
                                        }
                                    }
                                }

                            }
                        }

                    }



                    // S'il s'agit d'un bloc oxygene
                    if ((b.m_code == 99))
                    {
                        if (old_collision_oxygene == false) Program.soundManager.playSE("OXYG");
                        collision_oxygene = true;
                    }


                }



                /*
                 * collision à droite
                 */
                bool collision_droite_condition_x1 = false;

                //la collision est à droite si la position du bloc
                //est plus grande que la position du personnage
                if (b.m_position.X - m_position.X <= m_bloc.Rectangle.Width && b.m_position.X - m_position.X > 0)
                {
                    collision_droite_condition_x1 = true;
                }

                if (collision_droite_condition_x1 && (collision_gche_droite_condition_y1 || collision_gche_droite_condition_y2))
                {

                    m_blocCollision_droite = b;

                    // S'il s'agit d'un bloc plateforme 1, 2, 3, 4, 6, 7, 8 ou 9 on est bloqué
                    if ((b.m_code == 1) || (b.m_code == 2) || (b.m_code == 3) || (b.m_code == 4) || (b.m_code == 6) || (b.m_code == 7) || (b.m_code == 8) || (b.m_code == 9))
                    {
                        // Rendre la collision bloquante
                        collision_bloquante = true;

                        //Console.WriteLine("Collision droite");
                        collision_droite = true;
                    }
                    else
                    {
                        // Rendre la collision non bloquante
                        collision_bloquante = false;
                    }

                    // S'il s'agit d'un bloc ennemi
                    if ((b.m_code == 31) || (b.m_code == 32) || (b.m_code == 33) || (b.m_code == 34))
                    {
                        //Console.WriteLine("Collision droite");
                        collision_ennemi_cote = true;
                        monstreCollision = new MonstreDechet(b, m_codeNiveau);
                    }


                    // S'il s'agit d'un bloc dechet
                    if ((b.m_code == 21) || (b.m_code == 22) || (b.m_code == 23))
                    {
                        if (saut_ennemi == false)
                        {
                            if (inventaire == 0)
                            {
                                collision_dechet = true;
                                inventaire = b.m_code;

                                foreach (Bloc blocd in niveau1.m_blocs)
                                {
                                    if (blocd.m_position.Equals(b.m_position))
                                    {
                                        if ((blocd.m_code == 21) || (blocd.m_code == 22) || (blocd.m_code == 23))
                                        {
                                            indexDechet = niveau1.m_blocs.IndexOf(blocd);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }



                    // S'il s'agit d'un bloc oxygene
                    if ((b.m_code == 99))
                    {
                        if (old_collision_oxygene == false) Program.soundManager.playSE("OXYG");
                        collision_oxygene = true;
                    }

                }

            }
        }

        public void gestionToucheDroiteMaintenu(Scene niveau1, Surface ecranVideo)
        {
            int jump = 10;
            //mise a jour etat du clavier
            KeyboardState keyboard = new KeyboardState();
            //si on garde appuye la touche droite et pas de collision a droite
            if ((keyboard.IsKeyPressed(Key.RightArrow)) && (collision_droite == false))
            {
                //si on est en train de sauter verticalement on se deplace moins vite dans les airs
                if ((saut == true) && (saut_droit == false) && (collision_bas == false))
                {
                    mouvementADroite((int)vitesse_x * (int)10, niveau1, ecranVideo, true);
                }
                //sinon on deplace le personnage normalement
                else if ((saut == false) && (saut_droit == false) && (collision_bas == true))
                {
                    mouvementADroite(jump, niveau1, ecranVideo, true);
                }
            }
        }

        public void gestionToucheGaucheMaintenu(Scene niveau1, Surface ecranVideo)
        {
            int jump = 10;
            //mise a jour etat du clavier
            KeyboardState keyboard = new KeyboardState();

            if ((keyboard.IsKeyPressed(Key.LeftArrow)) && (collision_gauche == false))
            {

                //si on est en train de sauter verticalement on se deplace moins vite dans les airs
                if ((saut == true) && (saut_gauche == false) && (collision_bas == false))
                {
                    mouvementAGauche((int)vitesse_x * (int)10, niveau1, ecranVideo, true);
                }
                //sinon on deplace le personnage normalement
                else if ((saut == false) && (saut_gauche == false) && (collision_bas == true))
                {
                    mouvementAGauche(jump, niveau1, ecranVideo, true);
                }

            }
        }

        public void gestionGravite(Scene niveau1, Surface ecranVideo)
        {
            //gravite
            //si on est en train de sauter on desactive la gravite
            if (saut == false)
            {
                //si on est dans les airs on tombe
                if (collision_bas == false)
                {
                    // evolution de la vitesse verticale
                    mouvementEnBas((int)vitesse_gravite, niveau1);
                }
            }
        }

        public void gestionSaut(Scene niveau1, Surface ecranVideo)
        {
            //mise a jour etat du clavier
            KeyboardState keyboard = new KeyboardState();

            //saut
            if (saut == true)
            {
                //si on a pas fini le saut
                if (temps_saut < 16)
                {
                    // si c'est un saut vers la gauche
                    if ((saut_gauche == true) && (keyboard.IsKeyPressed(Key.LeftArrow)) && (collision_gauche == false) && (collision_haut == false))
                    {
                        mouvementAGauche((int)vitesse_x * (int)15, niveau1, ecranVideo, true);
                        if (temps_saut < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)2, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)2, niveau1);
                        }
                        temps_saut++;

                    }
                    // si c'est un saut vers la droite
                    else if ((saut_droit == true) && (keyboard.IsKeyPressed(Key.RightArrow)) && (collision_droite == false) && (collision_haut == false))
                    {
                        mouvementADroite((int)vitesse_x * (int)15, niveau1, ecranVideo, true);
                        if (temps_saut < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)2, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)2, niveau1);
                        }
                        temps_saut++;
                    }
                    //si c'est un saut vertical
                    else if ((keyboard.IsKeyPressed(Key.Space)) && (collision_haut == false))
                    {
                        if (temps_saut < 8)
                        {
                            mouvementEnHaut((int)vitesse_y * (int)2, niveau1);
                        }
                        else
                        {
                            mouvementEnBas((int)vitesse_y * (int)2, niveau1);
                        }
                        temps_saut++;
                    }
                    //si on arrete le saut avant sa fin
                    else
                    {
                        temps_saut = 16;
                        saut = false;
                        saut_gauche = false;
                        saut_droit = false;
                    }

                }
                //si le saut est termine
                else
                {
                    //si on a touche le sol
                    if ((collision_bas == true) || (collision_gauche == true) || (collision_droite == true))
                    {
                        temps_saut = 0;
                        saut = false;
                        saut_gauche = false;
                        saut_droit = false;
                    }
                    //si on n'a pas encore touche le sol
                    else
                    {
                        temps_saut = 0;
                        saut = false;
                        saut_gauche = false;
                        saut_droit = false;
                    }
                }
            }
        }

        public void mouvementAGauche(int valeurMouvement, Scene niveau1, Surface ecranVideo, bool influenceOrientationPersonnage)
        {
            if (influenceOrientationPersonnage)
            {
                if (m_SpriteSheetOffset == 4)
                    m_SpriteSheetOffset = 1;

                // Si le personnage regardait vers la droite
                if (orientation == "droite")
                {
                    // Réinitialisation du compteur du sprite
                    m_SpriteSheetOffset = 1;
                }

                // Rechargement de l'image
                switch (m_SpriteSheetOffset)
                {
                    case 1:
                        if (perso_gauche1 == null) perso_gauche1 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_gauche1.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_gauche1;
                        break;
                    case 2:
                        if (perso_gauche2 == null) perso_gauche2 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_gauche2.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_gauche2;
                        break;
                    case 3:
                        if (perso_gauche3 == null) perso_gauche3 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_gauche3.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_gauche3;
                        break;
                }
                m_bloc.Transparent = true;
                m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);

                m_SpriteSheetOffset++;
            }

            // S'il n'y a pas collision et si le personnage n'essaie pas de sortir du cadre de la fenêtre
            if ((collision_gauche == false) && (m_position.X > 0))
            {
                int minX = int.MaxValue; // De combien la scène s'est-elle déplacée sur l'axe des X en partant de l'origine 0 ?

                foreach (Bloc b in niveau1.m_blocs)
                {
                    if (b.getPosition().X < minX)
                    {
                        minX = b.getPosition().X;
                    }
                }
                ///Console.WriteLine("minX : " + minX + " - maxX : ");

                // Si le personnage n'est pas au centre de l'écran
                if (m_position.X > (VariablesGlobales.H_Fen_Largeur / 2) - (VariablesGlobales.H_Largeur_Bloc / 2))
                {
                    // On déplace le personnage
                    m_position.X -= valeurMouvement;
                }
                else if (minX < 0)
                {
                    // On déplace la caméra
                    foreach (Bloc b in niveau1.m_blocs)
                    {
                        b.setPosition(b.getPosition().X + valeurMouvement, b.getPosition().Y);
                    }
                }
                else
                {
                    // Sinon on déplace uniquement le personnage
                    m_position.X -= valeurMouvement;
                }
            }
        }

        public void mouvementADroite(int valeurMouvement, Scene niveau1, Surface ecranVideo, bool influenceOrientationPersonnage)
        {
            if (influenceOrientationPersonnage)
            {
                if (m_SpriteSheetOffset == 4)
                    m_SpriteSheetOffset = 1;

                // Si le personnage regardait vers la gauche
                if (orientation == "gauche")
                {
                    // Réinitialisation du compteur du sprite
                    m_SpriteSheetOffset = 1;
                }

                // Rechargement de l'image
                switch (m_SpriteSheetOffset)
                {
                    case 1:
                        if (perso_droite1 == null) perso_droite1 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_droite1.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_droite1;
                        break;
                    case 2:
                        if (perso_droite2 == null) perso_droite2 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_droite2.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_droite2;
                        break;
                    case 3:
                        if (perso_droite3 == null) perso_droite3 = new Surface(@"..\..\images\niveau" + Convert.ToString(m_codeNiveau) + "\\perso_droite3.png").Convert(ecranVideo, true, true);
                        m_bloc = perso_droite3;
                        break;
                }
                m_bloc.Transparent = true;
                m_bloc.TransparentColor = Color.FromArgb(255, 174, 201);

                m_SpriteSheetOffset++;
            }

            // S'il n'y a pas collision et si le personnage n'essaie pas de sortir du cadre de la fenêtre
            if ((collision_droite == false) && (m_position.X < VariablesGlobales.H_Fen_Largeur - m_bloc.Size.Width))
            {
                int maxX = 0; // Position du bloc le plus éloigné sur l'axe des X
                int minX = int.MaxValue; // De combien la scène s'est-elle déplacée sur l'axe des X en partant de l'origine 0 ?

                foreach (Bloc b in niveau1.m_blocs)
                {
                    if (b.getPosition().X > maxX)
                    {
                        maxX = b.getPosition().X;
                    }
                    if (b.getPosition().X < minX)
                    {
                        minX = b.getPosition().X;
                    }
                }

                // Calcul à partir de maxX la position de l'abscisse max à ne pas dépasser
                int maxXCollider = maxX + VariablesGlobales.H_Largeur_Bloc - minX; // La position maxX du bloc + la largeur d'un bloc - le déplacement global effectué

                ///Console.WriteLine("minX : " + minX + " - maxX : " + maxX + " - maxXCollider : " + maxXCollider);

                // Si le personnage n'est pas au centre de l'écran
                if (m_position.X < (VariablesGlobales.H_Fen_Largeur / 2) - (VariablesGlobales.H_Largeur_Bloc / 2))
                {
                    // On déplace le personnage
                    m_position.X += valeurMouvement;
                }
                // Si la limite de la scène n'a pas atteint la limite de la fenêtre
                else if (maxX > VariablesGlobales.H_Fen_Largeur - VariablesGlobales.H_Largeur_Bloc)
                {
                    // On déplace la caméra
                    foreach (Bloc b in niveau1.m_blocs)
                    {
                        b.setPosition(b.getPosition().X - valeurMouvement, b.getPosition().Y);
                    }
                }
                else
                {
                    // Sinon on déplace uniquement le personnage
                    m_position.X += valeurMouvement;
                }
            }
        }

        public void mouvementEnHaut(int valeurMouvement, Scene niveau1)
        {
            // S'il n'y a pas collision et si le personnage n'essaie pas de sortir du cadre de la fenêtre
            if ((collision_haut == false) && (m_position.Y > 0))
            {
                int minY = int.MaxValue; // De combien la scène s'est-elle déplacée sur l'axe des Y en partant de l'origine 0 ?

                foreach (Bloc b in niveau1.m_blocs)
                {
                    if (b.getPosition().Y < minY)
                    {
                        minY = b.getPosition().Y;
                    }
                }

                ///Console.WriteLine("minY : " + minY + " - maxY : ");

                // Si le personnage n'est pas au centre de l'écran
                if (m_position.Y > (VariablesGlobales.H_Fen_Hauteur / 2) - (VariablesGlobales.H_Hauteur_Bloc / 2))
                {
                    // On déplace le personnage
                    m_position.Y -= valeurMouvement;
                }
                else if (minY < 0)
                {
                    foreach (Bloc b in niveau1.m_blocs)
                    {
                        b.setPosition(b.getPosition().X, b.getPosition().Y + valeurMouvement);
                    }
                }
                else
                {
                    // Sinon on déplace uniquement le personnage
                    m_position.Y -= valeurMouvement;
                }
            }
        }

        public void mouvementEnBas(int valeurMouvement, Scene niveau1)
        {
            // S'il n'y a pas collision et si le personnage n'essaie pas de sortir du cadre de la fenêtre
            if ((collision_bas == false) && (m_position.Y < VariablesGlobales.H_Fen_Hauteur - m_bloc.Size.Height))
            {
                int maxY = 0; // Position du bloc le plus éloigné sur l'axe des Y
                int minY = int.MaxValue; // De combien la scène s'est-elle déplacée sur l'axe des Y en partant de l'origine 0 ?

                foreach (Bloc b in niveau1.m_blocs)
                {
                    if (b.getPosition().Y > maxY)
                    {
                        maxY = b.getPosition().Y;
                    }
                    if (b.getPosition().Y < minY)
                    {
                        minY = b.getPosition().Y;
                    }
                }

                // Calcul à partir de maxY la position de l'abscisse max à ne pas dépasser
                int maxYCollider = maxY + VariablesGlobales.H_Hauteur_Bloc - minY; // La position maxY du bloc + la largeur d'un bloc - le déplacement global effectué

                ///Console.WriteLine("minY : " + minY + " - maxY : " + maxY + " - maxYCollider : " + maxYCollider);

                // Si le personnage n'est pas au centre de l'écran
                if (m_position.Y < (VariablesGlobales.H_Fen_Hauteur / 2) - (VariablesGlobales.H_Hauteur_Bloc / 2))
                {
                    // On déplace le personnage
                    m_position.Y += valeurMouvement;
                }
                else if (maxY > VariablesGlobales.H_Fen_Hauteur - VariablesGlobales.H_Hauteur_Bloc)
                {
                    // On déplace la caméra
                    foreach (Bloc b in niveau1.m_blocs)
                    {
                        b.setPosition(b.getPosition().X, b.getPosition().Y - valeurMouvement);
                    }
                }
                else
                {
                    // Sinon on déplace uniquement le personnage
                    m_position.Y += valeurMouvement;
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

        public void annuleCollisionBasse(Scene niveau1)
        {
            /*
            //on empeche toute collision basse
            while (collision_bas == true)
            {
                mouvementEnHaut(1,niveau1);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementEnBas(1,niveau1);
             */
            if (collision_bas == true)
            {
                int tailleMouvement = (m_position.Y + m_bloc.Rectangle.Height) - m_blocCollision_bas.m_position.Y;
                mouvementEnHaut(tailleMouvement, niveau1);
            }
        }

        public void annuleCollisionHaute(Scene niveau1)
        {
            /*
            //on empeche toute collision haute
            while (collision_haut == true)
            {
                mouvementEnBas(1,niveau1);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementEnHaut(1,niveau1);
             */
            if (collision_haut == true)
            {
                int tailleMouvement = (m_blocCollision_haut.m_position.Y + m_blocCollision_haut.m_bloc.Rectangle.Height) - m_position.Y;
                mouvementEnBas(tailleMouvement, niveau1);
            }
        }

        public void annuleCollisionGauche(Scene niveau1, Surface ecranVideo)
        {
            /*
            //on empeche toute collision gauche
            while (collision_gauche == true)
            {
                mouvementADroite(1,niveau1, ecranVideo, false);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementAGauche(1,niveau1, ecranVideo, false);
             */
            if ((collision_gauche == true) && (m_position.Y - m_blocCollision_gauche.m_position.Y < m_blocCollision_gauche.m_bloc.Rectangle.Height))
            {
                int tailleMouvement = (m_blocCollision_gauche.m_position.X + m_blocCollision_gauche.m_bloc.Rectangle.Width) - m_position.X;
                mouvementADroite(tailleMouvement, niveau1, ecranVideo, false);
            }
        }

        public void annuleCollisionDroite(Scene niveau1, Surface ecranVideo)
        {
            /*
            //on empeche toute collision droite
            while ((collision_droite == true))
            {
                mouvementAGauche(1,niveau1, ecranVideo, false);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementADroite(1,niveau1, ecranVideo, false);
             */
            if ((collision_droite == true) && (m_position.Y - m_blocCollision_droite.m_position.Y < m_blocCollision_droite.m_bloc.Rectangle.Height))
            {
                int tailleMouvement = (m_position.X + m_bloc.Rectangle.Width) - m_blocCollision_droite.m_position.X;
                mouvementAGauche(tailleMouvement, niveau1, ecranVideo, false);
            }
        }

        public void annuleCollisionGaucheEnnemi(Scene niveau1, Surface ecranVideo)
        {
            /*
            //on empeche toute collision gauche
            while (collision_gauche == true)
            {
                mouvementADroite(1,niveau1, ecranVideo, false);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementAGauche(1,niveau1, ecranVideo, false);
             */
            if (collision_gauche == true)
            {
                int tailleMouvement = (m_blocCollision_gauche.m_position.X + m_blocCollision_gauche.m_bloc.Rectangle.Width) - m_position.X;
                mouvementADroite(tailleMouvement, niveau1, ecranVideo, false);
            }
        }

        public void annuleCollisionDroiteEnnemi(Scene niveau1, Surface ecranVideo)
        {
            /*
            //on empeche toute collision droite
            while ((collision_droite == true))
            {
                mouvementAGauche(1,niveau1, ecranVideo, false);
                metAZeroCollision();
                GetBoundingBoxCollisionStatus(niveau1);
            }
            mouvementADroite(1,niveau1, ecranVideo, false);
             */
            if (collision_droite == true)
            {
                int tailleMouvement = (m_position.X + m_bloc.Rectangle.Width) - m_blocCollision_droite.m_position.X;
                mouvementAGauche(tailleMouvement, niveau1, ecranVideo, false);
            }
        }





    };
}
