using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpellSlingerWindowsPort
{
    class Intro : BASE_GAMESTATE
    {
        int tempTimer;
        int introTime;
        bool introActive;

        public Intro()
        {
            CurrentGameState = (int)GAME_STATES.INTRO;
            tempTimer = 0;
            introTime = 5000;
            introActive = false;
        }

        public override void Update(GameTime gameTime_)
        {
            if (introActive)
            {
                Debug.WriteLine("INTRO");
                tempTimer += gameTime_.ElapsedGameTime.Milliseconds;
                if (tempTimer >= introTime)
                {
                    Debug.WriteLine("INTRO HAS BEEN PLAYED - SWITCH TO PLAYGAME (WILL BE MENU FIRST) - EXAMPLE ONLY");
                    CurrentGameState = (int)GAME_STATES.MENU;
                }
            }
            else
            {
                Debug.WriteLine("[NOTIFICATION] INTRO IS CURRENTLY OFF");
                CurrentGameState = (int)GAME_STATES.MENU;
            }
        }
    }
}
