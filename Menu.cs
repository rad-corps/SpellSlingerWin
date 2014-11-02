using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace MonogameAndroidProject
{
    class Menu : BASE_GAMESTATE
    {
        GUI gui;
        GameAssets gameAssets;
        ColliderHandler collider;
        ViewPort viewport;
        string name;
        MouseState lastMouseState;
        

        public Menu(GameAssets gameAssets_, ViewPort viewPort_, Factory objectFactory_, ColliderHandler colliderHandler_)
        {
            CurrentGameState = (int)GAME_STATES.MENU;
            gameAssets = gameAssets_;
            collider = colliderHandler_;
            viewport = viewPort_;

            if (gui == null)
            {
                gui = new GUI(objectFactory_, viewPort_);
                gui.GUIMenu();
            }

            lastMouseState = Mouse.GetState();
            
            
        }

        public string Name
        {
            get { return name; }
        }

        void OnEndShowKeyboardInput(IAsyncResult result)
        {
            //name = Guide.EndShowKeyboardInput(result);
            //if ( name != null ) 
            //    CurrentGameState = (int)GAME_STATES.PLAY_GAME;
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 gesturePos = new Vector2(Mouse.GetState().X - viewport.X, Mouse.GetState().Y - viewport.Y);              

                for(int i = 0; i < gameAssets.MenuListCount; i++)
                {
                    if (collider.Collider(gameAssets.MenuListItem(i), gesturePos) && gameAssets.MenuListItem(i).Identifier == 998)
                    {
                        //Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter Name", "", "Player", new AsyncCallback(OnEndShowKeyboardInput), null);                            
                        CurrentGameState = (int)GAME_STATES.PLAY_GAME;
                    }
                    if (collider.Collider(gameAssets.MenuListItem(i), gesturePos) && gameAssets.MenuListItem(i).Identifier == 997)
                    {
                        CurrentGameState = (int)GAME_STATES.END;
                    }
                    if (collider.Collider(gameAssets.MenuListItem(i), gesturePos) && gameAssets.MenuListItem(i).Identifier == 996)
                    {
                        CurrentGameState = (int)GAME_STATES.LEADERBOARD;
                    }
                }                    
            }

            lastMouseState = Mouse.GetState();
        }
    }
}
