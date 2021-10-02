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
    class GUI
    {
        /*
        private Surface m_TextSurface;
        private Surface m_TextSurfaceS;
        private Point p_TextSurface;
        private Point p_TextSurfaceS;
        */

        private Surface m_LBarSurface;
        private Point p_LBarSurface;

        private Surface m_OBarSurface;
        private Point p_OBarSurface;

        private Surface m_LMiniBarSurface;
        private Surface m_OMiniBarSurface;

        private Surface m_JunkBoxSurface;
        private Point p_JunkBoxSurface;

        private Surface m_JunkSurface;
        private Point p_JunkSurface;

        private SdlDotNet.Graphics.Font font;

        private int count = 0;
        private bool junk = false;

        public GUI()
        {
            font = new SdlDotNet.Graphics.Font(@"..\..\font\Arial.ttf", 18);
            m_LBarSurface = new Surface(153,18);
            m_OBarSurface = new Surface(153,18);
            m_LMiniBarSurface = new Surface(2, 14);
            m_LMiniBarSurface.Fill(Color.Red);
            m_OMiniBarSurface = new Surface(2, 14);
            m_OMiniBarSurface.Fill(Color.LightSkyBlue);

            m_JunkBoxSurface = new Surface(80, 80);
            m_JunkBoxSurface.Fill(Color.Black);
            Surface s = new Surface(78, 78);
            s.Fill(Color.FromArgb(220, 220, 220));
            m_JunkBoxSurface.Blit(s,new Point(1,1));
            s = new Surface(72, 72);
            s.Fill(Color.FromArgb(0, 0, 0));
            m_JunkBoxSurface.Blit(s, new Point(4, 4));
            
            for (int i = 0; i < 20; i++)
            {
                s = new Surface(70 - (2*i), 70 - (2*i));
                s.Fill(Color.FromArgb(100 - (5 * i), 100 - (5 * i), 100 - (5 * i)));
                m_JunkBoxSurface.Blit(s, new Point(5 + i, 5 + i));
            }

        }

        public void draw(int life, int air , Surface s)
        {

            p_LBarSurface = new Point(s.Width / 50,
                           s.Height / 30 - m_LBarSurface.Height / 2);
            m_LBarSurface.Fill(Color.Black);
            for (int i = 0; i < life / 2; i++)
            {
                m_LBarSurface.Blit(m_LMiniBarSurface,new Point(2 + (3 * i),2));
            }

            s.Blit(m_LBarSurface, p_LBarSurface);

            p_OBarSurface = new Point(s.Width / 50,
                           s.Height*2 / 30 - m_OBarSurface.Height / 2);
            m_OBarSurface.Fill(Color.Black);
            for (int i = 0; i < air / 2; i++)
            {
                m_OBarSurface.Blit(m_OMiniBarSurface, new Point(2 + (3 * i), 2));
            }

            s.Blit(m_OBarSurface, p_OBarSurface);


            p_JunkBoxSurface = new Point(s.Width*45 / 50,
                           p_LBarSurface.Y);
            s.Blit(m_JunkBoxSurface, p_JunkBoxSurface);

            if (junk)
            {
                Point p =  new Point(
                    ((count * (p_JunkBoxSurface.X + ((m_JunkBoxSurface.Width - m_JunkSurface.Width) / 2))) + ((10 - count) * p_JunkSurface.X)) / 10,
                    ((count * (p_JunkBoxSurface.Y + ((m_JunkBoxSurface.Height - m_JunkSurface.Height) / 2))) + ((10 - count) * p_JunkSurface.Y)) / 10);
                s.Blit(m_JunkSurface,p);
                if (count < 10) count++;
            }


            /*
            string message = "HP : " + life + "  Air : " + air;
            m_TextSurface = font.Render(message, Color.White);
            m_TextSurfaceS = font.Render(message, Color.FromArgb(128, 128, 128));

            p_TextSurfaceS = new Point(s.Width / 30 + 2,
                           s.Height * 19 / 20 - m_TextSurfaceS.Height / 2 + 2);
            s.Blit(m_TextSurfaceS, p_TextSurfaceS);

            p_TextSurface = new Point(s.Width / 30,
                           s.Height * 19 / 20 - m_TextSurface.Height / 2);
            s.Blit(m_TextSurface, p_TextSurface);
            */

        }

        public void addJunk(Surface dechet, Point positionDechet)
        {
            if (dechet == null)
            {
                count = 0;
                junk = false;
            } 
            else if (!junk) 
            {
                count = 0;
                junk = true;
                m_JunkSurface = dechet;
                p_JunkSurface = positionDechet;
            }
        }
    }
}