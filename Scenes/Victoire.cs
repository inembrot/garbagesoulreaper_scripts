using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;
using Tao.Sdl;

namespace MaPremiereApplication.Sources.Scenes
{
    class Victoire
    {
        private Surface m_VictoireSurface;
        private Surface m_VictoireSurfaceS;
        private Point p_VictoireSurface;
        private Point p_VictoireSurfaceS;

        private Surface m_ScoreSurface;
        private Surface m_ScoreSurfaceS;
        private Point p_ScoreSurface;
        private Point p_ScoreSurfaceS;

        private Surface m_CommentaireSurface;
        private Surface m_CommentaireSurfaceS;
        private Point p_CommentaireSurface;
        private Point p_CommentaireSurfaceS;

        private Surface m_NextLevelSurface;
        private Surface m_NextLevelSurfaceS;
        private Point p_NextLevelSurface;
        private Point p_NextLevelSurfaceS;

        private SdlDotNet.Graphics.Font font;

        private Surface m_ShadowSurface;
        private int count;

        public Surface before;

        public bool finiJeu = false;

        public Victoire()
        {
            count = 0;

            font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 42);

            m_VictoireSurface = font.Render("Vous avez gagné !", Color.White);
            m_VictoireSurfaceS = font.Render("Vous avez gagné !", Color.FromArgb(128, 128, 128));
            m_VictoireSurface.Alpha = 25;
            m_VictoireSurface.AlphaBlending = true;
            m_VictoireSurfaceS.Alpha = 25;
            m_VictoireSurfaceS.AlphaBlending = true;
            
        }

        public Surface draw (Surface s, int score,int scoreMax,int nbErreurs)
        {


            if (count == 0)
            {

                m_ScoreSurface = font.Render("Votre score : " + score + " / " +scoreMax + " points", Color.White);
                m_ScoreSurfaceS = font.Render("Votre score : " + score + " / " + scoreMax + " points", Color.FromArgb(128, 128, 128));
                m_ScoreSurface.Alpha = 25;
                m_ScoreSurface.AlphaBlending = true;
                m_ScoreSurfaceS.Alpha = 25;
                m_ScoreSurfaceS.AlphaBlending = true;

                string str = "";
                if (nbErreurs == 0)
                {
                    str = "C'est un score parfait !";
                }
                else
                {
                    str = "Vous avez fait " + nbErreurs + " erreurs.";
                }
                m_CommentaireSurface = font.Render(str, Color.White);
                m_CommentaireSurfaceS = font.Render(str, Color.FromArgb(128, 128, 128));
                m_CommentaireSurface.Alpha = 25;
                m_CommentaireSurface.AlphaBlending = true;
                m_CommentaireSurfaceS.Alpha = 25;
                m_CommentaireSurfaceS.AlphaBlending = true;

                

                if (finiJeu)
                {
                    str = "Retour titre";
                }
                else
                {
                    str = "Niveau suivant";
                }

                
                m_NextLevelSurface = font.Render(str, Color.White);
                m_NextLevelSurfaceS = font.Render(str, Color.FromArgb(128, 128, 128));
                m_NextLevelSurface.Alpha = 25;
                m_NextLevelSurface.AlphaBlending = true;
                m_NextLevelSurfaceS.Alpha = 25;
                m_NextLevelSurfaceS.AlphaBlending = true;
                

                m_ShadowSurface = (Surface)s.Clone();
                before = (Surface)s.Clone(true);
                m_ShadowSurface.Fill(Color.Black);
                m_ShadowSurface.Alpha = 125;
                m_ShadowSurface.AlphaBlending = true;
                before.Blit(m_ShadowSurface);
                m_ShadowSurface.Alpha = 13;
            }


            if (count < 20)
            {
                count++;
            }
            else if (count < 30)
            {
                s.Blit(m_ShadowSurface);
                count++;
            }
            else if (count < 40)
            {
                p_VictoireSurfaceS = new Point(s.Width / 2 - m_VictoireSurfaceS.Width / 2 + 2,
                           s.Height / 5 - m_VictoireSurfaceS.Height / 2 + 2);
                s.Blit(m_VictoireSurfaceS, p_VictoireSurfaceS);

                p_VictoireSurface = new Point(s.Width / 2 - m_VictoireSurface.Width / 2,
                               s.Height / 5 - m_VictoireSurface.Height / 2);
                s.Blit(m_VictoireSurface, p_VictoireSurface);
                count++;
            }
            else if (count < 50)
            {
                p_ScoreSurfaceS = new Point(s.Width / 2 - m_ScoreSurfaceS.Width / 2 + 2,
                           s.Height * 2 / 5 - m_ScoreSurfaceS.Height / 2 + 2);
                s.Blit(m_ScoreSurfaceS, p_ScoreSurfaceS);

                p_ScoreSurface = new Point(s.Width / 2 - m_ScoreSurface.Width / 2,
                               s.Height * 2 / 5 - m_ScoreSurface.Height / 2);
                s.Blit(m_ScoreSurface, p_ScoreSurface);
                count++;
            }
            else if (count < 60)
            {
                p_CommentaireSurfaceS = new Point(s.Width / 2 - m_CommentaireSurfaceS.Width / 2 + 2,
                        s.Height * 3 / 5 - m_CommentaireSurfaceS.Height / 2 + 2);
                s.Blit(m_CommentaireSurfaceS, p_CommentaireSurfaceS);

                p_CommentaireSurface = new Point(s.Width / 2 - m_CommentaireSurface.Width / 2,
                                s.Height * 3 / 5 - m_CommentaireSurface.Height / 2);
                s.Blit(m_CommentaireSurface, p_CommentaireSurface);
                count++;
            }
            else if (count < 70)
            {
                p_NextLevelSurfaceS = new Point(s.Width / 2 - m_NextLevelSurfaceS.Width / 2 + 2,
                        s.Height * 4 / 5 - m_NextLevelSurfaceS.Height / 2 + 2);
                s.Blit(m_NextLevelSurfaceS, p_NextLevelSurfaceS);

                p_NextLevelSurface = new Point(s.Width / 2 - m_NextLevelSurface.Width / 2,
                                s.Height * 4 / 5 - m_NextLevelSurface.Height / 2);
                s.Blit(m_NextLevelSurface, p_NextLevelSurface);  
                count++;
            }
            
            /*
            p_CommentaireSurfaceS = new Point(s.Width / 2 - m_CommentaireSurfaceS.Width / 2 + 2,
                           s.Height * 3 / 5 - m_CommentaireSurfaceS.Height / 2 + 2);
            s.Blit(m_CommentaireSurfaceS, p_CommentaireSurfaceS);

            p_CommentaireSurface = new Point(s.Width / 2 - m_CommentaireSurface.Width / 2,
                           s.Height * 3 / 5 - m_CommentaireSurface.Height / 2);
            s.Blit(m_CommentaireSurface, p_CommentaireSurface);
             * */

            return before;
        }

        public string newState(MouseButtonEventArgs args)
        {
            string result = "VICTOIRE";

            if ((args.X > p_NextLevelSurface.X) && (args.X < (p_NextLevelSurfaceS.X + m_NextLevelSurfaceS.Width)))
            {
                if ((args.Y > p_NextLevelSurface.Y) && (args.Y < (p_NextLevelSurfaceS.Y + m_NextLevelSurfaceS.Height)))
                {
                    if (count > 60)
                    {
                        Program.soundManager.playSE("CLICK");
                        if (finiJeu) result = "MENU";
                        else result = "JEU";
                        count = 0;
                        finiJeu = false;
                    }
                }
            }

            return result;
        }
    }
}
