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
    class Pause
    {
        private Surface m_ReprendreSurface;
        private Surface m_ReprendreSurfaceS;
        private Point p_ReprendreSurface;
        private Point p_ReprendreSurfaceS;

        private Surface m_AideSurface;
        private Surface m_AideSurfaceS;
        private Point p_AideSurface;
        private Point p_AideSurfaceS;

        private Surface m_TitreSurface;
        private Surface m_TitreSurfaceS;
        private Point p_TitreSurface;
        private Point p_TitreSurfaceS;

        private Surface m_QuitterSurface;
        private Surface m_QuitterSurfaceS;
        private Point p_QuitterSurface;
        private Point p_QuitterSurfaceS;

        private Surface m_ShadowSurface;
        private int count;

        public Surface before;

        public Pause()
        {
            count = 0;

            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 42);
            m_ReprendreSurface = font.Render("Reprendre", Color.White);
            m_ReprendreSurfaceS = font.Render("Reprendre", Color.FromArgb(128, 128, 128));
            m_AideSurface = font.Render("Aide", Color.White);
            m_AideSurfaceS = font.Render("Aide", Color.FromArgb(128, 128, 128));
            m_TitreSurface = font.Render("Titre", Color.White);
            m_TitreSurfaceS = font.Render("Titre", Color.FromArgb(128, 128, 128));
            m_QuitterSurface = font.Render("Quitter", Color.White);
            m_QuitterSurfaceS = font.Render("Quitter", Color.FromArgb(128, 128, 128));
        }

        public Surface draw (Surface s)
        {
            if (count == 0)
            {
                m_ShadowSurface = (Surface)s.Clone();
                before = (Surface)s.Clone(true);
                m_ShadowSurface.Fill(Color.Black);
                m_ShadowSurface.Alpha = 125;
                m_ShadowSurface.AlphaBlending = true;
                before.Blit(m_ShadowSurface);
                m_ShadowSurface.Alpha = 25;
            }


            if (count < 5)
            {
                s.Blit(m_ShadowSurface);
                count++;
            }

            if (count == 5)
            {
                s.Blit(before);
            }
            

            p_ReprendreSurfaceS = new Point(s.Width / 2 - m_ReprendreSurfaceS.Width / 2 + 2,
                           s.Height / 5 - m_ReprendreSurfaceS.Height / 2 + 2);
            s.Blit(m_ReprendreSurfaceS, p_ReprendreSurfaceS);

            p_ReprendreSurface = new Point(s.Width / 2 - m_ReprendreSurface.Width / 2,
                           s.Height / 5 - m_ReprendreSurface.Height / 2);
            s.Blit(m_ReprendreSurface, p_ReprendreSurface);

            p_AideSurfaceS = new Point(s.Width / 2 - m_AideSurfaceS.Width / 2 + 2,
                           s.Height*2 / 5 - m_AideSurfaceS.Height / 2 + 2);
            s.Blit(m_AideSurfaceS, p_AideSurfaceS);

            p_AideSurface = new Point(s.Width / 2 - m_AideSurface.Width / 2,
                           s.Height * 2 / 5 - m_AideSurface.Height / 2);
            s.Blit(m_AideSurface, p_AideSurface);

            p_TitreSurfaceS = new Point(s.Width / 2 - m_TitreSurfaceS.Width / 2 + 2,
                           s.Height * 3 / 5 - m_TitreSurfaceS.Height / 2 + 2);
            s.Blit(m_TitreSurfaceS, p_TitreSurfaceS);

            p_TitreSurface = new Point(s.Width / 2 - m_TitreSurface.Width / 2,
                           s.Height * 3 / 5 - m_TitreSurface.Height / 2);
            s.Blit(m_TitreSurface, p_TitreSurface);

            p_QuitterSurfaceS = new Point(s.Width / 2 - m_QuitterSurfaceS.Width / 2 + 2,
                           s.Height * 4 / 5 - m_QuitterSurfaceS.Height / 2 + 2);
            s.Blit(m_QuitterSurfaceS, p_QuitterSurfaceS);

            p_QuitterSurface = new Point(s.Width / 2 - m_QuitterSurface.Width / 2,
                           s.Height * 4 / 5 - m_QuitterSurface.Height / 2);
            s.Blit(m_QuitterSurface, p_QuitterSurface);

            return before;
        }

        public string newState(MouseButtonEventArgs args)
        {
            string result = "PAUSE";

            if ((args.X > p_ReprendreSurface.X) && (args.X < (p_ReprendreSurfaceS.X + m_ReprendreSurfaceS.Width)))
            {
                if ((args.Y > p_ReprendreSurface.Y) && (args.Y < (p_ReprendreSurfaceS.Y + m_ReprendreSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    count = 0;
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

            if ((args.X > p_TitreSurface.X) && (args.X < (p_TitreSurfaceS.X + m_TitreSurfaceS.Width)))
            {
                if ((args.Y > p_TitreSurface.Y) && (args.Y < (p_TitreSurfaceS.Y + m_TitreSurfaceS.Height)))
                {
                    Program.soundManager.playSE("CLICK");
                    count = 0;
                    result = "MENU";
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
