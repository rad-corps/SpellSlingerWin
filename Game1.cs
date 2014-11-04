#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;

//using Microsoft.Xna.Framework.Input.Touch;          //Touch library
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
//using Android.Media;

#endregion

namespace SpellSlingerWindowsPort
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //SpriteFont font;

        GameAssets gameAssets;
        ViewPort viewPort;
        //EnemySpawner enemySpawner;
        ColliderHandler colliderHandler;

        SpriteManager spriteManager;
        Factory objectFactory;

        public static int SCREEN_WIDTH;
        public static int SCREEN_HEIGHT;
        public static uint waveTimer = 1000;
        public static float FONT_MARGIN = 30.0f;

        BASE_GAMESTATE gameState;
        int currentGameState;

        //Text to screen
        SpriteFont myFont;
        Vector2 fontPos;
        Vector2 scoreSearchPos;

        //Text to screen
        SpriteFont waveStateFont;
        Vector2 leaderboardFont;
        Vector2 waveCapacityFontPos;
        Vector2 nameFontPos;

        //Ground & Trees
        Texture2D groundTexture;
        Texture2D treesTexture;
        Vector2 g_tposition;
        Rectangle groundAndTreeDest;

        //MediaPlayer menuMusic;
        //MediaPlayer inGameMusic;
        //MediaPlayer confirmSound;

        List<SoundEffect> spellSounds; //sound effect
        string name;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            groundAndTreeDest = new Rectangle();

            //menuMusic = MediaPlayer.Create(Activity, Resource.Raw.one_in_2);           
            //menuMusic.Looping = true;
            //menuMusic.SetVolume(0.25f, 0.25f);
            //menuMusic.Start();

            //inGameMusic = MediaPlayer.Create(Activity, Resource.Raw.LastHotel);
            //inGameMusic.SetVolume(0.25f, 0.25f);
            //inGameMusic.Looping = true;

            //confirmSound = MediaPlayer.Create(Activity, Resource.Raw.CONFIRM);

            //spellSounds = new List<MediaPlayer>();
            //spellSounds.Add(MediaPlayer.Create(Activity, Resource.Raw.FIREBALL));
            //spellSounds.Add(MediaPlayer.Create(Activity, Resource.Raw.ICELANCE3));
            //spellSounds.Add(MediaPlayer.Create(Activity, Resource.Raw.THUNDER2));
            //spellSounds.Add(MediaPlayer.Create(Activity, Resource.Raw.DESPAIR));
            //spellSounds.Add(MediaPlayer.Create(Activity, Resource.Raw.RAPTURE3));

            //for (int i = 0; i < spellSounds.Count; ++i)
            //{
            //    spellSounds[i].Start();
            //    spellSounds[i].Pause();
            //    spellSounds[i].Completion += SpellSoundCompletion;
            //}

            //randomly select a name
            List<string> playerNames = new List<string>();
            playerNames.Add("");

            playerNames.Add("Biqis The Undead");
            playerNames.Add("Reiwix The Gorish");
            playerNames.Add("Zanirn Kane");
            playerNames.Add("Siothir The Living");
            playerNames.Add("Wabrix The Soulreaper");
            playerNames.Add("Gudrex The Experi-Mentor");
            playerNames.Add("Wriocrux Solace");
            playerNames.Add("Eboxas The Paranoid");
            playerNames.Add("Krixis Molder");
            playerNames.Add("Gobres Magnus");
            playerNames.Add("Wrauness Hex");
            playerNames.Add("Drocilia Hex");
            playerNames.Add("Estrirotia Rane");
            playerNames.Add("Iwausin Diction");
            playerNames.Add("Striomish Crane");
            playerNames.Add("Wroris The Corruptor");
            playerNames.Add("Stroris The Soulkeeper");
            playerNames.Add("Tiocia Bane");
            playerNames.Add("Ceness The Desecrator");
            playerNames.Add("Staerina Morte");
            playerNames.Add("Grourael Diction");
            playerNames.Add("Zighor The Abominable");
            playerNames.Add("Braelazar The Corruptor");
            playerNames.Add("Werael Deville");
            playerNames.Add("Gebrum The Demise");
            playerNames.Add("Yothik Alure");
            playerNames.Add("Kriolazar Bloodworth");
            playerNames.Add("Toumon The Dcotor");
            playerNames.Add("Vroutic The Wraith");
            playerNames.Add("Fekras Sanguine");

            //select a random name
            var r = new Random();
            // print random integer >= 0 and  < 100
            int index = r.Next(playerNames.Count);
            name = playerNames[index];
        }

        void SpellSoundCompletion(object sender, EventArgs e)
        {
            //MediaPlayer player = (MediaPlayer)sender;
            //player.SeekTo(0);
            //player.Pause();
        }

        public void PlaySpellSound(SPELL_TYPE type_)
        {
            spellSounds[(int)type_].Play(0.3f, 0.0f, 0.0f);
        }

        void MyDrawString(Vector2 pos_, string text_, float scale_ = 1.5f)
        {
            spriteBatch.DrawString(waveStateFont, text_, pos_, Color.White, 0, new Vector2(), scale_, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;
            gameAssets = new GameAssets();
            objectFactory = new Factory(gameAssets);
            spriteManager = new SpriteManager();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            viewPort = new ViewPort(spriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT);
            colliderHandler = new ColliderHandler();
            gameState = new Intro();

            leaderboardFont = new Vector2(viewPort.ResRect.Width * 0.3f, viewPort.ResRect.Height * 0.3f);

            currentGameState = -1;

            //TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.DoubleTap;   //Enable gestures here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //load the tower texture
            for (int i = 0; i < SpriteManager.playerNumTextures; i++)
            {
                gameAssets.TextureList.Add(Content.Load<Texture2D>(spriteManager.GetPlayerSpriteFileName(i)));
            }

            //load the enemy textures
            for (int i = 0; i < SpriteManager.enemyNumTextures; i++)
            {
                gameAssets.EnemyTextureList.Add(Content.Load<Texture2D>(spriteManager.GetEnemySpriteFileName(i)));
            }

            //load the spell bar textures
            for (int i = 0; i < SpriteManager.spellNumTextures; i++)
            {
                gameAssets.SpellTextureList.Add(Content.Load<Texture2D>(spriteManager.GetSpellSpriteFileName(i)));
            }
            
            //load the gui textures
            for (int i = 0; i < SpriteManager.GUINumTextures; i++)
            {
                gameAssets.GUITextureList.Add(Content.Load<Texture2D>(spriteManager.GetGUISpriteFileName(i)));
            }

            //load the spell sprite sheet.
            gameAssets.spellTexture = Content.Load<Texture2D>("particlefx_02.png");

            //environment textures
            groundTexture = this.Content.Load<Texture2D>("grass.png");
            treesTexture = this.Content.Load<Texture2D>("tree_overlay.png");

            //CreatePlayer relies on the gameAssets being initialised
            objectFactory.CreatePlayer();

            //Text to screen - initiliase font & position
            myFont = Content.Load<SpriteFont>("myFont");
            fontPos = new Vector2(20, 80);

            //JEREMY!! where is "myFont" defined??? - "Content" -> myFont.xnb
            waveStateFont = Content.Load<SpriteFont>("myFont");            
            //waveStateFontPos = new Vector2(20, 100);
            waveCapacityFontPos = new Vector2(20, 20);
            nameFontPos = new Vector2(630, 130);
            scoreSearchPos = new Vector2(150, 600);

            gameAssets.leaderboardBG = Content.Load<Texture2D>("leaderboard_bg.png");

            //sounds
            //menuMusic = Content.Load<SoundEffect>("one_in_2.mp3");
            //menuMusic = Content.Load<Song>("one_in_2.mp3");

            spellSounds = new List<SoundEffect>();
            spellSounds.Add(Content.Load<SoundEffect>("FIREBALL2.wav"));
            spellSounds.Add(Content.Load<SoundEffect>("ICELANCE4.wav"));
            spellSounds.Add(Content.Load<SoundEffect>("THUNDER3.wav"));
            spellSounds.Add(Content.Load<SoundEffect>("DESPAIR2.wav"));
            spellSounds.Add(Content.Load<SoundEffect>("RAPTURE4.wav"));

        }

        //gameState MUST BE a PlayGame object for SaveToFile() to work.
        private void SaveToFile()
        {
            int finalScore = ((PlayGame)gameState).FinalScore;
            LeaderboardRecord record = new LeaderboardRecord(name, finalScore);
            LeaderboardSerializer ls = new LeaderboardSerializer();
            ls.Open();
            ls.Add(record);
            ls.Save();
        }

        void CheckChangeMusic()
        {
            ////gameState.CurrentGameState is the actual currentGameState
            ////currentGameState is the previous game state (nice name :)
            ////currentGameState = gameState.CurrentGameState;

            ////we are going back to the menu from PLAY_GAME
            //if (gameState.CurrentGameState != (int)GAME_STATES.PLAY_GAME && currentGameState == (int)GAME_STATES.PLAY_GAME)
            //{
            //    inGameMusic.SeekTo(0);
            //    inGameMusic.Pause();
                
            //    menuMusic.Start();                
            //}

            //if (gameState.CurrentGameState == (int)GAME_STATES.PLAY_GAME && currentGameState != (int)GAME_STATES.PLAY_GAME)
            //{
            //    inGameMusic.Start();

            //    menuMusic.SeekTo(0);
            //    menuMusic.Pause();
            //}

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (currentGameState != gameState.CurrentGameState)
            {
                switch (gameState.CurrentGameState)
                {
                    case (int)GAME_STATES.INTRO:
                        //Not used at this time
                        break;
                    case (int)GAME_STATES.MENU:
                        gameState = new Menu(gameAssets, viewPort, objectFactory, colliderHandler);
                        ((Menu)gameState).Name = name;
                        //MediaPlayer.Play(menuMusic);
                        break;
                    case (int)GAME_STATES.SAVE_TO_FILE:
                        SaveToFile();                        
                        break;
                    case (int)GAME_STATES.PLAY_GAME:
                        //need to store the name before we delete the menu
                        name = ((Menu)gameState).Name;
                        gameState = new PlayGame(gameAssets, viewPort, objectFactory, colliderHandler);
                        ((PlayGame)gameState).playSpellAudioCallback = this.PlaySpellSound;
                        break;
                    case (int)GAME_STATES.LEADERBOARD:
                        gameState = new Leaderboard(gameAssets, viewPort, objectFactory, colliderHandler);
                        break;
                    case (int)GAME_STATES.END:
                        Exit();
                        break;
                    default:
                        break;
                    
                }
                CheckChangeMusic();
                currentGameState = gameState.CurrentGameState;
                
                
                
                //DODGY HAX
                if (gameState.CurrentGameState == (int)GAME_STATES.SAVE_TO_FILE)
                {
                    gameState.CurrentGameState = (int)GAME_STATES.MENU;
                    base.Update(gameTime);
                    return;
                }
            }

            gameState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int i = 0; i < gameAssets.MenuListCount; i++)
            {
                if (gameAssets.MenuListItem(i).Active && currentGameState == (int)GAME_STATES.MENU)
                {
                    viewPort.Draw(gameAssets.MenuListItem(i));
                    MyDrawString(nameFontPos, name);
                }
            }
            
            //g_tposition = new Vector2(-250 + viewPort.X, -150 + viewPort.Y);
            g_tposition = new Vector2(viewPort.X - 250, viewPort.Y - 150);
            groundAndTreeDest.X = (int)g_tposition.X;
            groundAndTreeDest.Y = (int)g_tposition.Y;
            groundAndTreeDest.Width = viewPort.ResRect.Width + 500;
            groundAndTreeDest.Height = viewPort.ResRect.Height + 300;

            //Draw ground first
            if (currentGameState == (int)GAME_STATES.PLAY_GAME)
            {
                spriteBatch.Draw(groundTexture, groundAndTreeDest, Color.White);
            }

            for (int i = 0; i < gameAssets.DrawListCount; i++)
            {
                if (gameAssets.DrawListItem(i).Active && currentGameState == (int)GAME_STATES.PLAY_GAME)
                {
                    viewPort.Draw(gameAssets.DrawListItem(i));
                }
            }

            //Draw Trees
            if (currentGameState == (int)GAME_STATES.PLAY_GAME)
            {
                spriteBatch.Draw(treesTexture, groundAndTreeDest, Color.White);
            }

            //Add GUI to separate drawlist - Always draw on top of everything else
            for (int i = 0; i < gameAssets.GUIListCount; i++)
            {
                if (gameAssets.GUIListItem(i).Visible && currentGameState == (int)GAME_STATES.PLAY_GAME)
                {
                    viewPort.Draw(gameAssets.GUIListItem(i));
                }
            }




            if (currentGameState == (int)GAME_STATES.PLAY_GAME)
            {
                string capacityString =  "CAPACITY " + gameAssets.TowerListItem(0).Capacity.ToString() + " / 15";
                MyDrawString(waveCapacityFontPos, capacityString);

                string currentScoreStr = "SCORE    " + gameAssets.TowerListItem(0).Essence.ToString();                                          
                MyDrawString(new Vector2(waveCapacityFontPos.X, waveCapacityFontPos.Y + FONT_MARGIN), currentScoreStr);

                if (gameState is PlayGame)
                {
                    Vector2 gameStateTextPos = new Vector2(waveCapacityFontPos.X, waveCapacityFontPos.Y + FONT_MARGIN * 2);
                    if (((PlayGame)gameState).CurrentPlayState == PLAY_STATES.WAVE_IN_PROGRESS)
                    {                        
                        string waveString = "WAVE     " + ((PlayGame)gameState).WaveNum.ToString();
                        MyDrawString(gameStateTextPos, waveString);
                    }
                    if (((PlayGame)gameState).CurrentPlayState == PLAY_STATES.WAITING_FOR_WAVE_TO_START)
                    {
                        MyDrawString(gameStateTextPos, "GET READY");
                    }
                    if (((PlayGame)gameState).CurrentPlayState == PLAY_STATES.WAVE_COMPLETE)
                    {                        
                        string waveString = "WAVE " + (((PlayGame)gameState).WaveNum - 1).ToString() + " COMPLETE";
                        MyDrawString(new Vector2(420, viewPort.ViewPortHeight / 2 + 100), waveString, 3.0f);
                    }
                    if (((PlayGame)gameState).CurrentPlayState == PLAY_STATES.OVERWHELMED)
                    {
                        MyDrawString(gameStateTextPos, "YOU HAVE BEEN OVERWHELMED");
                    }

                    //display spell timers to screen
                    List<int> activeSpellCDs  = ((PlayGame)gameState).ActiveSpellCDs;

                    //Add GUI to separate drawlist - Always draw on top of everything else
                    for (int i = 0; i < gameAssets.GUIListCount; i++)
                    {
                        //thanks for the majic numbers brudda!
                        if (gameAssets.GUIListItem(i).Identifier >= 1 && gameAssets.GUIListItem(i).Identifier <=5 )
                        {                            
                            int TIME_DIVIDER = 100;
                            //find position of spell counter
                            Vector2 guiItemPos = new Vector2(gameAssets.GUIListItem(i).X + viewPort.FocusAreaX, gameAssets.GUIListItem(i).Y + viewPort.FocusAreaY);
                            Vector2 tempPos = new Vector2(guiItemPos.X, guiItemPos.Y - 112);
                            //string to draw
                            int tempActiveSpellTime = (activeSpellCDs[i] / TIME_DIVIDER);
                            if (tempActiveSpellTime > 0)
                            {
                                string activeSpellTime = tempActiveSpellTime.ToString();

                                //if single digit, pad it. 
                                if (activeSpellTime.Length == 1)
                                    activeSpellTime = " " + activeSpellTime;

                                //draw it
                                MyDrawString(tempPos, activeSpellTime, 2.0f);
                            }
                        }
                    }
                }

            }

            if (currentGameState == (int)GAME_STATES.LEADERBOARD)
            {                
                spriteBatch.Draw(gameAssets.leaderboardBG, viewPort.ResRect, Color.White);
                Leaderboard lb = (Leaderboard)gameState;
                List<LeaderboardRecord> records = lb.Records;

                const int RECORD_Y_SPACING = 30;

                for (int i = 0; i < records.Count; ++i)
                {
                    MyDrawString(new Vector2(leaderboardFont.X, leaderboardFont.Y + i * RECORD_Y_SPACING), records[i].name, 1.5f);
                    MyDrawString(new Vector2(leaderboardFont.X + 500, leaderboardFont.Y + i * RECORD_Y_SPACING), records[i].score.ToString(), 1.5f);
                    if ( i >= 9 ) break;
                }

                //show search results.
                if ( lb.SearchResultStr != "" ) 
                {
                    Vector2 resultPos = new Vector2(leaderboardFont.X, viewPort.ViewPortHeight - 100);
                    MyDrawString(resultPos, lb.SearchResultStr, 1.0f);
                }
                
                //draw gui for leaderboards
                for (int i = 0; i < gameAssets.MenuListCount; i++)
                {
                    if (gameAssets.MenuListItem(i).Visible && currentGameState == (int)GAME_STATES.LEADERBOARD)
                    {
                        viewPort.Draw(gameAssets.MenuListItem(i));
                    }
                }
                if (lb.ShowSearchInput)
                {
                    Vector2 resultPos = new Vector2(leaderboardFont.X, viewPort.ViewPortHeight - 120);
                    MyDrawString(resultPos, "Type score and press enter: " + lb.SearchScore, 1.0f);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
