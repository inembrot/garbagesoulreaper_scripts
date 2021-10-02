using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;
using Tao.Sdl;

namespace GarbageSoulReaper.Sources.Scenes
{
    class FogManager
    {

        private Surface[] m_FogSurfacePartielle;
        private Surface tempGame;
        private bool actif = true;
        private bool victoire = false;
        private int count = 20;


        public FogManager()
        {
            m_FogSurfacePartielle = new Surface[20];
            for (int i = 0; i < 20; i++)
            {
                m_FogSurfacePartielle[i] = new Surface(@"..\..\images\autres\fog\"+(i+1)+".png");
                m_FogSurfacePartielle[i].AlphaBlending = true;
            }
                
            
        }

        public void toggleFog(bool actif)
        {
            this.actif = actif;
        }

        public void setFog(bool actif)
        {
            if (actif) count = 20;
            else count = 0;
            this.actif = actif;
        }

        public bool isActif()
        {
            return actif;
        }

        public void draw(Surface s,string gameState)
        {

            if (tempGame == null) tempGame = new Surface(s.Width, s.Height);


            if (gameState == "VICTOIRE")
            {
                if (!victoire)
                {        
                    victoire = true;
                    s.Blit(tempGame);
                }
                else
                {
                    if (count != 0) s.Blit(tempGame);
                }
            }
            else
            {
                victoire = false;
                tempGame.Blit(s);
            }

            if (count != 0)
            {
                s.Blit(m_FogSurfacePartielle[count - 1], new Point(0, 0));
            }


            if (actif)
            {
                if (count < 20) count++;
            }
            else
            {
                if (count > 0) count--;
            }

        }

    }

}
