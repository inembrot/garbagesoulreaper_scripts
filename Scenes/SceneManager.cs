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
    class SceneManager
    {

        private Menu menu;
        private Aide aide;
        private Pause pause;
        private Intro intro;
        private Victoire victoire;
        private Defaite mort;

        private Surface beforePause;
        private bool endIntro = true;

        private int score = 0;
        private int scoreMax = 0;
        private int nbErreurs = 0;

        public SceneManager ()
        {
            menu = new Menu();
            aide = new Aide();
            pause = new Pause();
            intro = new Intro();
            victoire = new Victoire();
            mort = new Defaite();

        }

        public void draw (Surface s,string gameState)
        {
            if (gameState == "MENU")
            {
                if (endIntro) menu.draw(s);
                else endIntro = intro.draw(s);
            }
            else if (gameState == "AIDE")
            {
                aide.draw(s,beforePause);
            }
            else if (gameState == "PAUSE")
            {
                beforePause = pause.draw(s);
            }
            else if (gameState == "VICTOIRE")
            {
                victoire.draw(s,score,scoreMax,nbErreurs);
            }
            else if (gameState == "DEFAITE")
            {
                mort.draw(s, score, scoreMax, nbErreurs);
            }

        }

        public string newState(MouseButtonEventArgs args, string gameState)
        {
            string result = gameState;

            if (gameState == "MENU")
            {
                result = menu.newState(args);
                if (result == "AIDE") aide.wasInGame = false;
            }
            else if (gameState == "AIDE")
            {
                result = aide.newState(args);
                if (result != "AIDE") aide.wasInGame = true;
            }
            else if (gameState == "PAUSE")
            {
                result = pause.newState(args);
            }
            else if (gameState == "VICTOIRE")
            {
                result = victoire.newState(args);
            }
            else if (gameState == "DEFAITE")
            {
                result = mort.newState(args);
            }

            return result;
        }

        public void setEndLevelScore(int score,int scoreMax,int nbErreurs, bool mort, bool finiJeu)
        {
            this.score = score;
            this.scoreMax = scoreMax;
            this.nbErreurs = nbErreurs;

            this.victoire.finiJeu = finiJeu;
            this.mort.mort = mort;
        }
    }
}
