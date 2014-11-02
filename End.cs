using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpellSlingerWindowsPort
{
    class End : BASE_GAMESTATE
    {
        public End()
        {
            CurrentGameState = (int)GAME_STATES.END;
        }

        public override void Update(GameTime gameTime)
        {
            Debug.WriteLine("END");
        }
    }
}
