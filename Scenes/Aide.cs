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
    class Aide
    {
        

        private Surface m_LBarTxtSurface;
        private Surface m_LBarTxtSurfaceS;
        private Point p_LBarTxtSurface;
        private Point p_LBarTxtSurfaceS;

        private Surface m_OBarTxtSurface;
        private Surface m_OBarTxtSurfaceS;
        private Point p_OBarTxtSurface;
        private Point p_OBarTxtSurfaceS;

        private Surface m_JunkBoxTxtSurface;
        private Surface m_JunkBoxTxtSurfaceS;
        private Point p_JunkBoxTxtSurface;
        private Point p_JunkBoxTxtSurfaceS;

        private Surface m_SuivantSurface;
        private Surface m_SuivantSurfaceS;
        private Point p_SuivantSurface;
        private Point p_SuivantSurfaceS;

        private Surface m_RetourSurface;
        private Surface m_RetourSurfaceS;
        private Point p_RetourSurface;
        private Point p_RetourSurfaceS;

        private Surface m_LBarSurface;
        private Point p_LBarSurface;

        private Surface m_OBarSurface;
        private Point p_OBarSurface;

        private Surface m_LMiniBarSurface;
        private Surface m_OMiniBarSurface;

        private Surface m_JunkBoxSurface;
        private Point p_JunkBoxSurface;

        private Surface m_1AideSurface;
        private Surface m_2AideSurface;
        private Surface m_3AideSurface;
        private Point p_AideSurface;

        private Surface m_BackSurface;

        public bool wasInGame = true;
        private int nbAide = 0;

        public Aide()
        {
            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 42);
            SdlDotNet.Graphics.Font fontLittle = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 18);

            //Image

            m_1AideSurface = new Surface(@"..\..\images\autres\aide1.png");
            m_2AideSurface = new Surface(@"..\..\images\autres\aide2.png");
            m_3AideSurface = new Surface(@"..\..\images\autres\aide3.png");
            m_BackSurface = new Surface(@"..\..\images\autres\air-pollution-for-health-powerpoint-backgrounds.png").CreateScaledSurface(0.80);

            //Legende
            
            m_LBarTxtSurface = fontLittle.Render("Jauge de Santé", Color.White);
            m_LBarTxtSurfaceS = fontLittle.Render("Jauge de Santé", Color.FromArgb(128, 128, 128));
            m_OBarTxtSurface = fontLittle.Render("Jauge d'Oxygène", Color.White);
            m_OBarTxtSurfaceS = fontLittle.Render("Jauge d'Oxygène", Color.FromArgb(128, 128, 128));
            m_JunkBoxTxtSurface = fontLittle.Render("Déchet transporté", Color.White);
            m_JunkBoxTxtSurfaceS = fontLittle.Render("Déchet transporté", Color.FromArgb(128, 128, 128));

            //Boutons

            m_RetourSurface = font.Render("Retour", Color.White);
            m_RetourSurfaceS = font.Render("Retour", Color.FromArgb(128, 128, 128));
            m_SuivantSurface = font.Render("Suivant", Color.White);
            m_SuivantSurfaceS = font.Render("Suivant", Color.FromArgb(128, 128, 128));

            //GUI

            m_LBarSurface = new Surface(153, 18);
            m_OBarSurface = new Surface(153, 18);
            m_LMiniBarSurface = new Surface(2, 14);
            m_LMiniBarSurface.Fill(Color.Red);
            m_OMiniBarSurface = new Surface(2, 14);
            m_OMiniBarSurface.Fill(Color.LightSkyBlue);

            m_JunkBoxSurface = new Surface(80, 80);
            m_JunkBoxSurface.Fill(Color.Black);
            Surface s = new Surface(78, 78);
            s.Fill(Color.FromArgb(220, 220, 220));
            m_JunkBoxSurface.Blit(s, new Point(1, 1));
            s = new Surface(72, 72);
            s.Fill(Color.FromArgb(0, 0, 0));
            m_JunkBoxSurface.Blit(s, new Point(4, 4));

            for (int i = 0; i < 20; i++)
            {
                s = new Surface(70 - (2 * i), 70 - (2 * i));
                s.Fill(Color.FromArgb(100 - (5 * i), 100 - (5 * i), 100 - (5 * i)));
                m_JunkBoxSurface.Blit(s, new Point(5 + i, 5 + i));
            }
        }
        public void draw(Surface s,Surface beforePause)
        {
            if (wasInGame) s.Blit(beforePause);
            else s.Blit(m_BackSurface);

            //Image

            p_AideSurface = new Point((s.Width*4 / 50) + 1,
                           s.Height*6 / 30);
            if (nbAide == 0)
            {
                s.Blit(m_1AideSurface, p_AideSurface);
            }
            else if (nbAide == 1)
            {
                s.Blit(m_2AideSurface, p_AideSurface);
            }
            else if (nbAide == 2)
            {
                s.Blit(m_3AideSurface, p_AideSurface);
            }

            //Legende

            p_LBarTxtSurfaceS = new Point((s.Width*10 / 50) + 2,
                           (s.Height / 30 - m_LBarTxtSurfaceS.Height / 2) + 2);
            s.Blit(m_LBarTxtSurfaceS, p_LBarTxtSurfaceS);

            p_LBarTxtSurface = new Point(s.Width*10/ 50,
                           s.Height / 30 - m_LBarTxtSurface.Height / 2);
            s.Blit(m_LBarTxtSurface, p_LBarTxtSurface);

            p_OBarTxtSurfaceS = new Point((s.Width * 10 / 50) + 2,
                           (s.Height * 2 / 30 - m_OBarTxtSurfaceS.Height / 2) + 2);
            s.Blit(m_OBarTxtSurfaceS, p_OBarTxtSurfaceS);

            p_OBarTxtSurface = new Point(s.Width * 10 / 50,
                           s.Height * 2 / 30 - m_OBarTxtSurface.Height / 2);
            s.Blit(m_OBarTxtSurface, p_OBarTxtSurface);


            p_JunkBoxTxtSurfaceS = new Point((s.Width * 36 / 50) + 2,
                           (s.Height * 2 / 30 - m_JunkBoxTxtSurfaceS.Height / 2) + 2);
            s.Blit(m_JunkBoxTxtSurfaceS, p_JunkBoxTxtSurfaceS);

            p_JunkBoxTxtSurface = new Point(s.Width * 36 / 50,
                           s.Height * 2 / 30 - m_JunkBoxTxtSurface.Height / 2);
            s.Blit(m_JunkBoxTxtSurface, p_JunkBoxTxtSurface);

            //Boutons


            p_RetourSurfaceS = new Point(s.Width*2 / 10 - m_RetourSurfaceS.Width / 2 + 2,
                           s.Height * 7 / 8 - m_RetourSurfaceS.Height / 2 + 2);
            s.Blit(m_RetourSurfaceS, p_RetourSurfaceS);

            p_RetourSurface = new Point(s.Width*2 / 10 - m_RetourSurface.Width / 2,
                           s.Height * 7 / 8 - m_RetourSurface.Height / 2);
            s.Blit(m_RetourSurface, p_RetourSurface);

            p_SuivantSurfaceS = new Point(s.Width*8 / 10 - m_SuivantSurfaceS.Width / 2 + 2,
                           s.Height * 7 / 8 - m_SuivantSurfaceS.Height / 2 + 2);
            s.Blit(m_SuivantSurfaceS, p_SuivantSurfaceS);

            p_SuivantSurface = new Point(s.Width*8 / 10 - m_SuivantSurface.Width / 2,
                           s.Height * 7 / 8 - m_SuivantSurface.Height / 2);
            s.Blit(m_SuivantSurface, p_SuivantSurface);

            //GUI

            p_LBarSurface = new Point(s.Width / 50,
                           s.Height / 30 - m_LBarSurface.Height / 2);
            m_LBarSurface.Fill(Color.Black);
            for (int i = 0; i < 100 / 2; i++)
            {
                m_LBarSurface.Blit(m_LMiniBarSurface, new Point(2 + (3 * i), 2));
            }

            s.Blit(m_LBarSurface, p_LBarSurface);

            p_OBarSurface = new Point(s.Width / 50,
                           s.Height * 2 / 30 - m_OBarSurface.Height / 2);
            m_OBarSurface.Fill(Color.Black);
            for (int i = 0; i < 100 / 2; i++)
            {
                m_OBarSurface.Blit(m_OMiniBarSurface, new Point(2 + (3 * i), 2));
            }

            s.Blit(m_OBarSurface, p_OBarSurface);


            p_JunkBoxSurface = new Point(s.Width * 45 / 50,
                           p_LBarSurface.Y);
            s.Blit(m_JunkBoxSurface, p_JunkBoxSurface);
        }

        public string newState(MouseButtonEventArgs args)
        {
            string result = "AIDE";

            if ((args.X > p_RetourSurface.X) && (args.X < (p_RetourSurfaceS.X + m_RetourSurfaceS.Width)))
            {
                if ((args.Y > p_RetourSurface.Y) && (args.Y < (p_RetourSurfaceS.Y + m_RetourSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    if (wasInGame) result = "PAUSE";
                    else result = "MENU";
                    nbAide = 0;
                    
                }
            }

            if ((args.X > p_SuivantSurface.X) && (args.X < (p_SuivantSurfaceS.X + m_SuivantSurfaceS.Width)))
            {
                if ((args.Y > p_SuivantSurface.Y) && (args.Y < (p_SuivantSurfaceS.Y + m_SuivantSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    if (nbAide == 2) nbAide = 0;
                    else nbAide++;
                }
            }

            return result;
        }
    }
}
