using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpellSlingerWindowsPort
{
    enum GAME_STATES
    {
        INTRO,
        MENU,
        LEADERBOARD,
        PLAY_GAME,
        SAVE_TO_FILE, //set to this state on end of play game
        END
    }

    enum PLAY_STATES
    {
        ABOUT_TO_GENERATE_WAVE,
        WAITING_FOR_WAVE_TO_START,
        WAVE_IN_PROGRESS,
        WAVE_COMPLETE,
        PAUSE,
        OVERWHELMED
    }

    class BASE_GAMESTATE
    {
        int currentGameState;

        public virtual void Update(GameTime gameTime_) { ;}
        
        public int CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

    }
}
