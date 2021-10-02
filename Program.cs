using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;
using System.Threading;
using Tao.Sdl;
using MaPremiereApplication.Sources;
using MaPremiereApplication.Sources.Scenes;
using GarbageSoulReaper.Sources;
using GarbageSoulReaper.Sources.Scenes;
using GarbageSoulReaper.Sources.Sound;
using SdlDotNet.Audio;

namespace MaPremiereApplication
{
    class Program
    {
        private static Surface m_ecranVideo;
        private static Surface m_backgroundImage;
        private static Scene niveau;
        private static Personnage personnage;

        private static SceneManager sceneManager;
        public  static SoundManager soundManager;
        private static FogManager fogManager;
        private static GUI GUI;
        private static int m_codeNiveau;

        private static GSRTimer timer;

        private static String gameState = "MENU";

        private static int canalDebute;
        private static int niveauDebute;

        private static bool musiqueEnPause;
        private static bool musiqueDeVictoire;
        private static bool musiqueDeDefaite;

        private static bool manqueOxy;
        private static bool musiqueOxyDebute;
        private static bool manqueVie;

        private static int ennemiRadioactifProche;

        static void Main( string[] args )
        {
            m_ecranVideo = Video.SetVideoMode(VariablesGlobales.H_Fen_Largeur, VariablesGlobales.H_Fen_Hauteur, 32, false, false, false, true);
            m_ecranVideo.Fill(Color.Black);
            Video.WindowCaption = "Garbage Soul Reaper";
            
            sceneManager = new SceneManager();
            soundManager = new SoundManager();
            fogManager = new FogManager();
            GUI = new GUI();
            timer = new GSRTimer();

            // Fixe le nombre de FPS
            Events.Fps = 25;

            // Boucle principale de l'application
            Events.Tick += new EventHandler<TickEventArgs>(TickEventHandler);
            
            // Gestion des événements clavier
            //Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);

            // Gestion des événements souris
            //Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonEventHandler);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(MouseButtonEventHandler);

            // Lorsque l'on clique sur la croix en haut de la fenêtre
            Events.Quit += new EventHandler<QuitEventArgs>(delegate(object sender, QuitEventArgs q_args)
            {
                // Quitter l'application
                Events.QuitApplication();
            });

            // Chargement de la scène en fonction du code niveau
            chargerNiveau(0);
            m_codeNiveau = 1;

            canalDebute = 0;
            niveauDebute = 0;

            musiqueEnPause = false;
            musiqueDeVictoire = false;
            musiqueDeDefaite = false;

            manqueOxy = false;
            musiqueOxyDebute = false;

            manqueVie = false;

            ennemiRadioactifProche = 0;

            soundManager.playBGM("CHAN7",0.0f);
            soundManager.fadeIN("CHAN7", 2000, gameState == "PAUSE");
            soundManager.playBGM("PROCHE_RAD", 0.0f);

            // Lancer le gestionnaire d'événnements
            Events.Run();
           
        }

        // Chargement des scènes
        public static void chargerNiveau(int codeNiveau)
        {
            // On réinitialise le timer à chaque chargement de niveau
            timer.reset();

            m_codeNiveau = codeNiveau;

            manqueVie = false;

            // Initialisation de la scène en fonction du codeNiveau reçu
            switch (codeNiveau)
            {
                case 0:
                    // Chargement de l'image du fond d'écran
                    m_backgroundImage = (new Surface(@"..\..\images\niveau1\background.jpg").Convert(m_ecranVideo, true, false));
                    fogManager.setFog(false);

                    // Chargement de la scène
                    niveau = new Scene(@"..\..\scenes\niveau1.txt", m_ecranVideo, 1);
                break;
                case 1:
                    // Changement de musique
                    soundManager.playBGM("CHAN1", 0.0f);
                    soundManager.fadeOUT("CHAN7", 2000);
                    soundManager.fadeIN("CHAN1", 2000, gameState == "PAUSE");
                    soundManager.playBGM("CHAN2", 0.0f);
                    soundManager.fadeIN("CHAN2", 2000, gameState == "PAUSE");
                    canalDebute = 0;
                    niveauDebute = 0;

                    // Chargement de l'image du fond d'écran
                    m_backgroundImage = (new Surface(@"..\..\images\niveau1\background.jpg").Convert(m_ecranVideo, true, false));
                    fogManager.setFog(false);

                    // Chargement de la scène
                    niveau = new Scene(@"..\..\scenes\niveau1.txt", m_ecranVideo, codeNiveau);
                break;
                case 2:
                    if (niveauDebute != 2)
                    {
                        // Changement de musique
                        soundManager.fadeOUT("CHAN1", 8000);
                        niveauDebute = 2;
                    }

                    // Chargement de l'image du fond d'écran
                    m_backgroundImage = (new Surface(@"..\..\images\niveau2\background.jpg").Convert(m_ecranVideo, true, false));
                    fogManager.setFog(true);

                    // Chargement de la scène
                    niveau = new Scene(@"..\..\scenes\niveau2.txt", m_ecranVideo, codeNiveau);
                break;
                case 3:
                    if (niveauDebute != 3)
                    {
                        // Changement de musique
                        soundManager.fadeOUT("CHAN2", 16000);
                        soundManager.fadeOUT("CHAN3", 8000);
                        soundManager.fadeOUT("CHAN4", 8000);
                        soundManager.fadeOUT("CHAN_NIV_2_1", 8000);
                        soundManager.fadeOUT("CHAN_NIV_2_2", 8000);

                        niveauDebute = 3;
                    }

                    // Chargement de l'image du fond d'écran
                    m_backgroundImage = (new Surface(@"..\..\images\niveau3\background.jpg").Convert(m_ecranVideo, true, false));
                    fogManager.setFog(true);

                    // Chargement de la scène
                    niveau = new Scene(@"..\..\scenes\niveau3.txt", m_ecranVideo, codeNiveau);
                break;
            }

            if (codeNiveau == 0)
            {
                personnage = new Personnage(m_ecranVideo, 1);
            }
            else
            {
                personnage = new Personnage(m_ecranVideo, codeNiveau);
            }

            musiqueDeVictoire = false;
            musiqueDeDefaite = false;

            niveau.positionnerPersonnage(personnage);
            niveau.positionnerCamera(personnage);
        }
  
        private static void TickEventHandler(object sender, TickEventArgs args)
        {
            // Gestion son au moment où la vie du perso est faible
            if ((personnage.sante <= 20) && (manqueVie == false))
            {
                manqueVie = true;
                soundManager.playSE("LOW_LIFE");
            }

            // Gestion musique au moment où l'oxygène du perso est faible
            if (personnage.oxygene == 0)
            {
                manqueOxy = true;
            }

            if ((personnage.oxygene > 0) && (manqueOxy == true))
            {
                manqueOxy = false;
                musiqueOxyDebute = false;

                if (m_codeNiveau == 2)
                {
                    soundManager.fadeIN("CHAN3", 1000, false);
                    soundManager.fadeIN("CHAN4", 1000, false);
                    soundManager.fadeOUT("CHAN_NIV_2_1", 1000);
                    soundManager.fadeOUT("CHAN_NIV_2_2", 1000);
                }
                else if (m_codeNiveau == 3)
                {
                    soundManager.fadeIN("CHAN5", 1000, false);
                    soundManager.fadeIN("CHAN6", 1000, false);
                    soundManager.fadeOUT("CHAN_NIV_3_1", 1000);
                    soundManager.fadeOUT("CHAN_NIV_3_2", 1000);
                }
            }

            if ((manqueOxy == true) && (musiqueOxyDebute == false))
            {
                musiqueOxyDebute = true;

                if (m_codeNiveau == 2)
                {
                    soundManager.fadeOUT("CHAN3", 1000);
                    soundManager.fadeOUT("CHAN4", 1000);
                    soundManager.fadeIN("CHAN_NIV_2_1", 1000, false);
                    soundManager.fadeIN("CHAN_NIV_2_2", 1000, false);
                }
                else if (m_codeNiveau == 3)
                {
                    soundManager.fadeOUT("CHAN5", 1000);
                    soundManager.fadeOUT("CHAN6", 1000);
                    soundManager.fadeIN("CHAN_NIV_3_1", 1000, false);
                    soundManager.fadeIN("CHAN_NIV_3_2", 1000, false);
                }
            }

            soundManager.update(gameState == "PAUSE");
            // Lorsque le jeu est mis en pause, on baisse le volume de tous les chanels
            if ((gameState == "VICTOIRE") && (m_codeNiveau == 3))
            {
                soundManager.fadeOUT("CHAN1", 500);
                soundManager.fadeOUT("CHAN2", 500);
                soundManager.fadeOUT("CHAN3", 500);
                soundManager.fadeOUT("CHAN4", 500);
                soundManager.fadeOUT("CHAN5", 500);
                soundManager.fadeOUT("CHAN6", 500);

                musiqueDeVictoire = true;
            }


            if ((gameState == "DEFAITE") && (musiqueDeDefaite == false))
            {
                musiqueDeDefaite = true;
                soundManager.fadeOUT("CHAN1", 500);
                soundManager.fadeOUT("CHAN2", 500);
                soundManager.fadeOUT("CHAN3", 500);
                soundManager.fadeOUT("CHAN4", 500);
                soundManager.fadeOUT("CHAN5", 500);
                soundManager.fadeOUT("CHAN6", 500);
            }

            /////// Gestion des sons et musique :
            /// -> Niveau 2
            if (m_codeNiveau == 2)
            {
                if ((timer.tempsEcoule() == 9) && (canalDebute == 0))
                {
                    soundManager.stopBGM("CHAN1");
                    soundManager.playBGM("CHAN3", 1.0f);
                    soundManager.playBGM("CHAN_NIV_2_1", 0.0f);
                    canalDebute = 1;
                }
                if ((timer.tempsEcoule() == 40) && (canalDebute == 1))
                {
                    soundManager.stopBGM("CHAN3");
                    soundManager.stopBGM("CHAN_NIV_2_1");
                    soundManager.playBGM("CHAN4", 1.0f);
                    soundManager.playBGM("CHAN_NIV_2_2", 0.0f);

                    canalDebute = 2;
                }
            }
            /// -> Niveau 3
            if (m_codeNiveau == 3)
            {
                if ((timer.tempsEcoule() == 9) && ((canalDebute == 1) || (canalDebute == 2)))
                {
                    soundManager.playBGM("CHAN5", 0.0f);
                    soundManager.playBGM("CHAN_NIV_3_1", 0.0f);
                    soundManager.fadeIN("CHAN5", 8000, gameState == "PAUSE");

                    canalDebute = 3;
                }
                if ((timer.tempsEcoule() == 23) && (canalDebute == 3))
                {
                    soundManager.playBGM("CHAN6", 1.0f);
                    soundManager.playBGM("CHAN_NIV_3_2", 0.0f);
                    canalDebute = 4;
                }
            }
            //Console.WriteLine(canalDebute + " " + timer.tempsEcoule() + " " + m_codeNiveau);
               
            if (gameState == "JEU")
            {
                
                if(niveau.estReussie(personnage)) 
                {
                    if (personnage.score < (niveau.scoreMax / 2)) 
                    {
                        gameState = "DEFAITE";
                        Program.soundManager.playSE("LOSE");
                    }
                    else 
                    {
                        gameState = "VICTOIRE";
                        Program.soundManager.playSE("WIN");
                    }
                    
                    //le score ne peut pas etre negatif
                    personnage.score = (personnage.score < 0) ? 0 : personnage.score;
                    sceneManager.setEndLevelScore(personnage.score,
                        niveau.scoreMax,
                        personnage.nbMauvaisTri,
                        false,
                        niveau.getCodeNiveau() == 3);
                }

                foreach (MonstreDechet ennemi in niveau.m_blocs_ennemis)
                {
                    
                    ennemi.metAZeroCollision();
                    ennemi.detecteCollision(niveau, personnage);
                    ennemi.deplacementMonstre(niveau, personnage, m_ecranVideo);

                    // Gestion bruitage lorsqu'un monstre radioactif est proche du perso
                    if ((ennemi.m_code == 34) && (ennemi.distance_perso < 90) && (ennemiRadioactifProche != 4))
                    {
                        soundManager.fadeIN("PROCHE_RAD", 500, 1.0f, false);

                        ennemiRadioactifProche = 4;
                    }

                    else if ((ennemi.m_code == 34) && (ennemi.distance_perso < 180) && (ennemiRadioactifProche != 3))
                    {
                        if (ennemiRadioactifProche == 2)
                        {
                            soundManager.fadeIN("PROCHE_RAD", 500, 0.75f, false);
                        }
                        else
                        {
                            soundManager.fadeOUT("PROCHE_RAD", 500, 0.75f);
                        }

                        ennemiRadioactifProche = 3;
                    }

                    else if ((ennemi.m_code == 34) && (ennemi.distance_perso < 270) && (ennemiRadioactifProche != 2))
                    {
                        if (ennemiRadioactifProche == 1)
                        {
                            soundManager.fadeIN("PROCHE_RAD", 500, 0.50f, false);
                        }
                        else
                        {
                            soundManager.fadeOUT("PROCHE_RAD", 500, 0.50f);
                        }

                        ennemiRadioactifProche = 2;
                    }

                    else if ((ennemi.m_code == 34) && (ennemi.distance_perso < 360) && (ennemiRadioactifProche != 1))
                    {
                        if (ennemiRadioactifProche == 0)
                        {
                            soundManager.fadeIN("PROCHE_RAD", 500, 0.25f, false);
                        }
                        else
                        {
                            soundManager.fadeOUT("PROCHE_RAD", 500, 0.25f);
                        }
                        
                        ennemiRadioactifProche = 1;
                    }

                    else if ((ennemi.m_code == 34) && (ennemi.distance_perso >= 360) && (ennemiRadioactifProche != 0))
                    {
                        soundManager.fadeOUT("PROCHE_RAD", 500);
                        ennemiRadioactifProche = 0;
                    }
                }

                personnage.metAZeroCollision();
                personnage.GetBoundingBoxCollisionStatus(niveau);

                personnage.annuleCollisionGauche(niveau, m_ecranVideo);
                personnage.annuleCollisionDroite(niveau, m_ecranVideo);
                personnage.annuleCollisionBasse(niveau);
                personnage.annuleCollisionHaute(niveau);

                if (personnage.collision_bas) personnage.a_mal_trie = false;

                personnage.testeSiPersoDescend();

                if (niveau.getCodeNiveau() != 1)
                {
                    personnage.gestionOxygene();
                }

                if (!personnage.gestionSante())
                {
                    gameState = "DEFAITE";
                    Program.soundManager.playSE("LOSE");
                    personnage.score = (personnage.score < 0) ? 0 : personnage.score;
                    sceneManager.setEndLevelScore(personnage.score,
                        niveau.scoreMax,
                        personnage.nbMauvaisTri,
                        true,
                        niveau.getCodeNiveau() == 3);
                }

                personnage.gestionDestructionDechet(niveau);

                personnage.gestionCollisionPoubelle(niveau);

                personnage.gestionToucheDroiteMaintenu(niveau, m_ecranVideo);

                personnage.gestionToucheGaucheMaintenu(niveau, m_ecranVideo);

                personnage.gestionGravite(niveau, m_ecranVideo);

                personnage.gestionSaut(niveau, m_ecranVideo);

                personnage.gestionCollisionEnnemiBas(niveau, m_ecranVideo);

                personnage.sautEnnemi(niveau, m_ecranVideo);

                m_ecranVideo.Blit(m_backgroundImage);


                niveau.dessinerScene(m_ecranVideo);

                if (personnage.clignoterPersonnage())
                {
                    personnage.show(m_ecranVideo);
                }

                if (personnage.inventaire > 0)
                {
                    Point A = personnage.positionDechet;

                    Surface s = personnage.surfaceDechet;
                    GUI.addJunk(s, A);
                }
                else
                {
                    Surface s = null;
                    Point A = new Point(0, 0);
                    GUI.addJunk(s, A);
                }


                //Params = Life,Air,JunkPath,Surface
                //if (true) GUI.addJunk();
                GUI.draw(personnage.sante, personnage.oxygene, m_ecranVideo);
              
            }

            if (gameState == "VICTOIRE") fogManager.toggleFog(false);
            if (gameState == "JEU" || gameState == "VICTOIRE") fogManager.draw(m_ecranVideo, gameState);

            sceneManager.draw(m_ecranVideo, gameState);

            m_ecranVideo.Update();
        }

        private static void MouseButtonEventHandler(object sender, MouseButtonEventArgs args)
        {
            string oldState = gameState;
            if (args.Button == MouseButton.PrimaryButton) if (gameState != "JEU") gameState = sceneManager.newState(args, gameState);
            if (gameState != oldState && gameState == "JEU")
            {
                if (oldState != "PAUSE")
                {
                    int niv = niveau.getCodeNiveau();
                    if (oldState == "VICTOIRE" && niv != 3) chargerNiveau(++niv); //NIVEAU N+1
                    else if (oldState == "MENU") chargerNiveau(1); //NIVEAU1
                    else chargerNiveau(niv); //NIVEAU N
                }
            }

            // On revient à l'écran titre à partir de pause
            if ((gameState == "MENU" && oldState == "PAUSE") || (gameState == "MENU" && oldState == "DEFAITE") || (gameState == "MENU" && oldState == "VICTOIRE"))
            {
                // On stoppe toutes les musiques et on relance celle du canal 7 (celle de l'écran titre)
                soundManager.fadeOUT("CHAN1", 2000);
                soundManager.fadeOUT("CHAN2", 2000);
                soundManager.fadeOUT("CHAN3", 2000);
                soundManager.fadeOUT("CHAN4", 2000);
                soundManager.fadeOUT("CHAN5", 2000);
                soundManager.fadeOUT("CHAN6", 2000);
                soundManager.playBGM("CHAN7", 0.0f);
                soundManager.fadeIN("CHAN7", 2000, gameState == "PAUSE");
                soundManager.fadeOUT("WIN", 2000);
            }

            if ((gameState == "JEU") && (oldState == "DEFAITE"))
            {
                // !!! PLAY SON DEFAITE ICI !!! //
                soundManager.fadeOUT("LOSE", 1000);
            }
        }

        private static void KeyboardEventHandler(object sender, KeyboardEventArgs args)
        {
            int jump = 10;

            if (gameState == "JEU")
            {
                switch (args.Key)
                {
                
                    case Key.LeftArrow:
                        personnage.mouvementAGauche(jump, niveau, m_ecranVideo, true);
                        personnage.ancienneTouche = args.Key;
                        personnage.orientation = "gauche";
                        break;

                    case Key.RightArrow:
                        personnage.mouvementADroite(jump, niveau, m_ecranVideo, true);
                        personnage.ancienneTouche = args.Key;
                        personnage.orientation = "droite";
                        break;

                    case Key.Space:
                            if (personnage.collision_bas == true)
                            {
                                personnage.temps_saut = 0;
                                personnage.saut = true;
                                Program.soundManager.playSE("JUMP");

                                if (personnage.ancienneTouche == Key.LeftArrow)
                                {
                                    personnage.saut_gauche = true;
                                    personnage.saut_droit = false;
                                }
                                else if (personnage.ancienneTouche == Key.RightArrow)
                                {
                                    personnage.saut_gauche = false;
                                    personnage.saut_droit = true;
                                }
                                else
                                {
                                    personnage.saut_gauche = false;
                                    personnage.saut_droit = false;
                                }
                            personnage.ancienneTouche = args.Key;
                        }
                    
                        break;
                    case Key.I:
                        // On simule une victoire pour pouvoir sauter de niveau à tout moment
                        gameState = "VICTOIRE";
                        if (m_codeNiveau == 3)
                        {
                            Program.soundManager.playSE("WIN");
                        }
                        else
                        {
                            Program.soundManager.playSE("WIN_LVL");
                        }
                        personnage.score = (personnage.score < 0) ? 0 : personnage.score;
                        sceneManager.setEndLevelScore(personnage.score, niveau.scoreMax, personnage.nbMauvaisTri,false, niveau.getCodeNiveau() == 3);
                    break;

                    case Key.Escape:
                        gameState = "PAUSE";
                        break;

                    default :
                        // Ne pas affecter la touche N à autre chose
                        personnage.ancienneTouche = Key.N;
                        break;
                    }

                personnage.show(m_ecranVideo);
            } 
        }
    }
}
