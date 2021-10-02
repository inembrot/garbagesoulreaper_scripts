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
    class Defaite
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

        private Surface m_TitleSurface;
        private Surface m_TitleSurfaceS;
        private Point p_TitleSurface;
        private Point p_TitleSurfaceS;

        private Surface m_RetrySurface;
        private Surface m_RetrySurfaceS;
        private Point p_RetrySurface;
        private Point p_RetrySurfaceS;

        private SdlDotNet.Graphics.Font font;

        private Surface m_ShadowSurface;
        private int count;

        public bool mort = false;

        public Surface before;

        public Defaite()
        {
            count = 0;

            font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 42);
            

            m_TitleSurface = font.Render("Retour titre", Color.White);
            m_TitleSurfaceS = font.Render("Retour titre", Color.FromArgb(128, 128, 128));
            m_TitleSurface.Alpha = 25;
            m_TitleSurface.AlphaBlending = true;
            m_TitleSurfaceS.Alpha = 25;
            m_TitleSurfaceS.AlphaBlending = true;

            m_RetrySurface = font.Render("Recommencer", Color.White);
            m_RetrySurfaceS = font.Render("Recommencer", Color.FromArgb(128, 128, 128));
            m_RetrySurface.Alpha = 25;
            m_RetrySurface.AlphaBlending = true;
            m_RetrySurfaceS.Alpha = 25;
            m_RetrySurfaceS.AlphaBlending = true;

        }

        public Surface draw(Surface s, int score, int scoreMax, int nbErreurs)
        {

            if (count == 0)
            {
                string str = "";
                if (mort)
                {
                    str = "Vous êtes mort !";
                }
                else
                {
                    str = "Vous avez fait trop d'erreurs !";
                }
                m_VictoireSurface = font.Render(str, Color.White);
                m_VictoireSurfaceS = font.Render(str, Color.FromArgb(128, 128, 128));
                m_VictoireSurface.Alpha = 25;
                m_VictoireSurface.AlphaBlending = true;
                m_VictoireSurfaceS.Alpha = 25;
                m_VictoireSurfaceS.AlphaBlending = true;

                m_ScoreSurface = font.Render("Votre score : " + score + " / " + scoreMax + " points", Color.White);
                m_ScoreSurfaceS = font.Render("Votre score : " + score + " / " + scoreMax + " points", Color.FromArgb(128, 128, 128));
                m_ScoreSurface.Alpha = 25;
                m_ScoreSurface.AlphaBlending = true;
                m_ScoreSurfaceS.Alpha = 25;
                m_ScoreSurfaceS.AlphaBlending = true;

                if (nbErreurs == 0)
                {
                    str = "Vous n'avez pas fait d'erreurs.";
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

                m_ShadowSurface = (Surface)s.Clone();
                before = (Surface)s.Clone(true);
                m_ShadowSurface.Fill(Color.Black);
                m_ShadowSurface.Alpha = 125;
                m_ShadowSurface.AlphaBlending = true;
                before.Blit(m_ShadowSurface);
                m_ShadowSurface.Alpha = 13;
            }


            if (count < 10)
            {
                s.Blit(m_ShadowSurface);
                count++;
            }
            else if (count < 20)
            {

                p_VictoireSurfaceS = new Point(s.Width / 2 - m_VictoireSurfaceS.Width / 2 + 2,
                           s.Height  / 5 - m_VictoireSurfaceS.Height / 2 + 2);
                s.Blit(m_VictoireSurfaceS, p_VictoireSurfaceS);

                p_VictoireSurface = new Point(s.Width / 2 - m_VictoireSurface.Width / 2,
                               s.Height / 5 - m_VictoireSurface.Height / 2);
                s.Blit(m_VictoireSurface, p_VictoireSurface);
                count++;
            }
            else if (count < 30)
            {
                p_ScoreSurfaceS = new Point(s.Width / 2 - m_ScoreSurfaceS.Width / 2 + 2,
                           s.Height * 2 / 5 - m_ScoreSurfaceS.Height / 2 + 2);
                s.Blit(m_ScoreSurfaceS, p_ScoreSurfaceS);

                p_ScoreSurface = new Point(s.Width / 2 - m_ScoreSurface.Width / 2,
                               s.Height * 2 / 5 - m_ScoreSurface.Height / 2);
                s.Blit(m_ScoreSurface, p_ScoreSurface);
                count++;
            }
            else if (count < 40)
            {
                p_CommentaireSurfaceS = new Point(s.Width / 2 - m_CommentaireSurfaceS.Width / 2 + 2,
                        s.Height * 3 / 5 - m_CommentaireSurfaceS.Height / 2 + 2);
                s.Blit(m_CommentaireSurfaceS, p_CommentaireSurfaceS);

                p_CommentaireSurface = new Point(s.Width / 2 - m_CommentaireSurface.Width / 2,
                                s.Height * 3 / 5 - m_CommentaireSurface.Height / 2);
                s.Blit(m_CommentaireSurface, p_CommentaireSurface);
                count++;
            }
            else if (count < 50)
            {

                p_TitleSurfaceS = new Point(s.Width * 2 / 10 - m_TitleSurfaceS.Width / 2 + 2,
                               s.Height * 4 / 5 - m_TitleSurfaceS.Height / 2 + 2);
                s.Blit(m_TitleSurfaceS, p_TitleSurfaceS);

                p_TitleSurface = new Point(s.Width * 2 / 10 - m_TitleSurface.Width / 2,
                               s.Height * 4 / 5 - m_TitleSurface.Height / 2);
                s.Blit(m_TitleSurface, p_TitleSurface);

                p_RetrySurfaceS = new Point(s.Width * 8 / 10 - m_RetrySurfaceS.Width / 2 + 2,
                               s.Height * 4 / 5 - m_RetrySurfaceS.Height / 2 + 2);
                s.Blit(m_RetrySurfaceS, p_RetrySurfaceS);

                p_RetrySurface = new Point(s.Width * 8 / 10 - m_RetrySurface.Width / 2,
                               s.Height * 4 / 5 - m_RetrySurface.Height / 2);
                s.Blit(m_RetrySurface, p_RetrySurface);
                count++;
            }

            return before;
        }

        public string newState(MouseButtonEventArgs args)
        {
            string result = "DEFAITE";

            if ((args.X > p_RetrySurface.X) && (args.X < (p_RetrySurfaceS.X + m_RetrySurfaceS.Width)))
            {
                if ((args.Y > p_RetrySurface.Y) && (args.Y < (p_RetrySurfaceS.Y + m_RetrySurfaceS.Height)))
                {
                    if (count > 40)
                    {
                        Program.soundManager.playSE("CLICK");
                        result = "JEU";
                        count = 0;
                        mort = false;
                    }
                }
            }

            if ((args.X > p_TitleSurface.X) && (args.X < (p_TitleSurfaceS.X + m_TitleSurfaceS.Width)))
            {
                if ((args.Y > p_TitleSurface.Y) && (args.Y < (p_TitleSurfaceS.Y + m_TitleSurfaceS.Height)))
                {
                    if (count > 40)
                    {
                        Program.soundManager.playSE("CLICK");
                        result = "MENU";
                        count = 0;
                        mort = false;
                    }
                }
            }

            return result;
        }
    }
}
