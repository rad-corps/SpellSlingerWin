using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input.Touch;
//using Android.App;
//using Android.Content.PM;
//using Android.OS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace SpellSlingerWindowsPort
{

    class Leaderboard : BASE_GAMESTATE
    {
        GameAssets gameAssets;
        ViewPort viewport;
        Factory objectFactory;
        ColliderHandler collider;
        GUI gui;
        //SpriteBatch spriteBatch;
        //SpriteFont spriteFont;
        LeaderboardSerializer serializer;
        MouseState lastMouseState;
        KbHandler kb;

        bool showSearchInput;
        public bool ShowSearchInput { get { return showSearchInput; } }
        string searchScore;
        public string SearchScore { get { return searchScore; } } 

        public List<LeaderboardRecord> Records { get { return serializer.Records; } }

        
        private string searchResultStr;
        public string SearchResultStr { get { return searchResultStr; } } 

        public Leaderboard(GameAssets gameAssets_, ViewPort viewPort_, Factory objectFactory_, ColliderHandler colliderHandler_)
        {            
            CurrentGameState = (int)GAME_STATES.LEADERBOARD;
            gameAssets = gameAssets_;
            viewport = viewPort_;
            objectFactory = objectFactory_;
            collider = colliderHandler_;

            gameAssets.FlushEntities();
            gui = new GUI(objectFactory_, viewPort_);
            gui.GUILeaderboard();
           
            serializer = new LeaderboardSerializer();
            serializer.Open();
            searchResultStr = "";

            lastMouseState = Mouse.GetState();

            kb = new KbHandler();
        }

        //this is called after the user enters a record to search for
        void OnEndShowKeyboardInput()
        {
            //string str = Guide.EndShowKeyboardInput(result);
            if (searchScore != null)
            {
                
                int num;
                bool isNum = int.TryParse(searchScore, out num);
                searchScore = "";
                if (isNum)
                {
                    LeaderboardRecord tempRecord = new LeaderboardRecord("", num);
                    List<LeaderboardRecord> records = serializer.Records;
                    int recordIndex = records.BinarySearch(tempRecord);
                    if (recordIndex >= 0)
                    {
                        LeaderboardRecord searchResult = records[recordIndex];
                        searchResultStr = "Record Found: " + searchResult.name + " - " + searchResult.score;
                    }
                    else
                    {
                        searchResultStr = "Record Not Found";
                    }
                }
                else
                {
                    searchResultStr = "Not a number";
                }
            }
                
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                
                Vector2 gesturePos = new Vector2(Mouse.GetState().X - viewport.X, Mouse.GetState().Y - viewport.Y);

                for (int i = 0; i < gameAssets.MenuListCount; i++)
                {
                    if (collider.Collider(gameAssets.MenuListItem(i), gesturePos) && gameAssets.MenuListItem(i).Identifier == 995)
                    {
                        gameAssets.FlushEntities();
                        CurrentGameState = (int)GAME_STATES.MENU;
                        return;
                    }
                    if (collider.Collider(gameAssets.MenuListItem(i), gesturePos) && gameAssets.MenuListItem(i).Identifier == 990)
                    {
                        showSearchInput = true;
                        return;
                    }
                }
            }

            if (showSearchInput)
            {                
                kb.Update();
                searchScore = kb.Str;
                if (kb.Submit)
                {
                    OnEndShowKeyboardInput();
                    showSearchInput = false;
                    kb = new KbHandler();
                }
            }
            lastMouseState = Mouse.GetState();
        }
    }
}
