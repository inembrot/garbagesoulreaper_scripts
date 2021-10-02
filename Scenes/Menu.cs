using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;
using System.Runtime.InteropServices;
using Tao.Sdl;

namespace MaPremiereApplication.Sources.Scenes
{
    class Menu
    {

        private Surface m_JouerSurface;
        private Surface m_JouerSurfaceS;
        private Point p_JouerSurface;
        private Point p_JouerSurfaceS;

        private Surface m_AideSurface;
        private Surface m_AideSurfaceS;
        private Point p_AideSurface;
        private Point p_AideSurfaceS;

        private Surface m_QuitterSurface;
        private Surface m_QuitterSurfaceS;
        private Point p_QuitterSurface;
        private Point p_QuitterSurfaceS;

        private Surface m_LogoTitleSurface;
        private Point p_LogoTitleSurface;

        private Surface m_BackSurface;

        public Menu()
        {
            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 42);
            m_JouerSurface = font.Render("Jouer", Color.White);
            m_JouerSurfaceS = font.Render("Jouer", Color.FromArgb(128, 128, 128));
            m_AideSurface = font.Render("Aide", Color.White);
            m_AideSurfaceS = font.Render("Aide", Color.FromArgb(128, 128, 128));
            m_QuitterSurface = font.Render("Quitter", Color.White);
            m_QuitterSurfaceS = font.Render("Quitter", Color.FromArgb(128, 128, 128));

            m_LogoTitleSurface = new Surface(@"..\..\images\autres\logoTitle.png");
            m_BackSurface = new Surface(@"..\..\images\autres\air-pollution-for-health-powerpoint-backgrounds.png").CreateScaledSurface(0.80);
        }

        public void draw (Surface s)
        {
            s.Fill( Color.Black);
            s.Blit(m_BackSurface);
            
            p_LogoTitleSurface = new Point(s.Width / 2 - m_LogoTitleSurface.Width / 2,
                           s.Height * 2 / 8 - m_LogoTitleSurface.Height / 4);
            s.Blit(m_LogoTitleSurface, p_LogoTitleSurface);

            p_JouerSurfaceS = new Point(s.Width / 2 - m_JouerSurfaceS.Width / 2 + 2,
                           s.Height*5 / 8 - m_JouerSurfaceS.Height / 2 + 2);
            s.Blit(m_JouerSurfaceS, p_JouerSurfaceS);

            p_JouerSurface = new Point(s.Width / 2 - m_JouerSurface.Width / 2,
                           s.Height*5 / 8 - m_JouerSurface.Height / 2);
            s.Blit(m_JouerSurface, p_JouerSurface);

            p_AideSurfaceS = new Point(s.Width / 2 - m_AideSurfaceS.Width / 2 + 2,
                           s.Height*6 / 8 - m_AideSurfaceS.Height / 2 + 2);
            s.Blit(m_AideSurfaceS, p_AideSurfaceS);

            p_AideSurface = new Point(s.Width / 2 - m_AideSurface.Width / 2,
                           s.Height*6 / 8 - m_AideSurface.Height / 2);
            s.Blit(m_AideSurface,p_AideSurface);

            p_QuitterSurfaceS = new Point(s.Width / 2 - m_QuitterSurfaceS.Width / 2 + 2,
                           s.Height*7 / 8 - m_QuitterSurfaceS.Height / 2 + 2);
            s.Blit(m_QuitterSurfaceS,p_QuitterSurfaceS);

            p_QuitterSurface = new Point(s.Width / 2 - m_QuitterSurface.Width / 2,
                           s.Height*7 / 8 - m_QuitterSurface.Height / 2);
            s.Blit(m_QuitterSurface,p_QuitterSurface);
        }

        public string newState (MouseButtonEventArgs args)
        {
            string result = "MENU";

            if ((args.X > p_JouerSurface.X) && (args.X < (p_JouerSurfaceS.X + m_JouerSurfaceS.Width)))
            {
                if ((args.Y > p_JouerSurface.Y) && (args.Y < (p_JouerSurfaceS.Y + m_JouerSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    result = "JEU";
                }
            }

            if ((args.X > p_AideSurface.X) && (args.X < (p_AideSurfaceS.X + m_AideSurfaceS.Width)))
            {
                if ((args.Y > p_AideSurface.Y) && (args.Y < (p_AideSurfaceS.Y + m_AideSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    result = "AIDE";
                }
            }

            if ((args.X > p_QuitterSurface.X) && (args.X < (p_QuitterSurfaceS.X + m_QuitterSurfaceS.Width)))
            {
                if ((args.Y > p_QuitterSurface.Y) && (args.Y < (p_QuitterSurfaceS.Y + m_QuitterSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    Events.QuitApplication();
                }
            }

            return result;
        }

    }
}