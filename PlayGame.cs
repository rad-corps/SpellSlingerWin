using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

using Microsoft.Xna.Framework.Input.Touch;          //Touch library

namespace SpellSlingerWindowsPort
{
    class PlayGame : BASE_GAMESTATE
    {
        public delegate void PlaySpellAudio(SPELL_TYPE st_);
        public PlaySpellAudio playSpellAudioCallback;

        const int TIME_BETWEEN_SPAWNERS_BASE = 2500;
        const float TIME_BETWEEN_SPAWNERS_MULTIPLIER = 0.9f;
        //const int TIME_BETWEEN_SPAWNERS_MULTI = 100;
        const int POINTS_TO_SPEND_BASE = 600;
        const int POINTS_TO_SPEND_MULTI = 200;
        const int SPAWNER_NUM_MULTI = 3;

        float currentTimeBetweenSpawners;

        int finalScore;
        public int FinalScore { get { return finalScore; } }

        SPELL_TYPE spellSelect = SPELL_TYPE.FIREBALL;
        PLAY_STATES playState = PLAY_STATES.ABOUT_TO_GENERATE_WAVE;
        GameAssets gameAssets_;
        ViewPort viewPort_;
        Factory objectFactory_;
        ColliderHandler colliderHandler_;
        GUI gui;
        List<EnemySpawner> currentWave;
        private Timer waveCompleteTimer;
        int waveNum;
        public int WaveNum { get { return waveNum; } }

        //bool leftMouseButtonDown = false;

        //MOUSE VARS FOR WINDOWZ
        MouseState currentMouseState;
        MouseState prevMouseState;
        //bool dragging;
        Vector2 mousePosOnClick;
        Vector2 mousePosOnRelease;

        List<int> activeSpellCDs;                                                   //Tracks cooldown time when specific spell cast
        public List<int> ActiveSpellCDs { get { return activeSpellCDs; } }

        int overwhelmedTimer;

        public PlayGame(GameAssets gameAssets, ViewPort viewPort, Factory objectFactory, ColliderHandler colliderHandler)
        {
            waveNum = 1;

            gui = new GUI(objectFactory, viewPort);
            gameAssets_ = gameAssets;
            viewPort_ = viewPort;
            objectFactory_ = objectFactory;
            colliderHandler_ = colliderHandler;
            CurrentGameState = (int)GAME_STATES.PLAY_GAME;

            gui.GUIPlayGame();                                                      //Initialise GUI

            activeSpellCDs = new List<int>();
            for (int i = 0; i < Enum.GetNames(typeof(SPELL_TYPE)).Length; i++)
            {
                activeSpellCDs.Add(0);
            }

            //remove arbitrary timer value
            waveCompleteTimer = new System.Timers.Timer(3000);
            waveCompleteTimer.Elapsed += CreateANewWave;

            overwhelmedTimer = 0;

            currentTimeBetweenSpawners = TIME_BETWEEN_SPAWNERS_BASE;

            //mouseDownTime = 0.0f;
            //dragging = false;
        }

        //triggered from the waveCompleteTimer.Elapsed
        private void CreateANewWave(Object source, ElapsedEventArgs e)
        {
            waveCompleteTimer.Stop();
            playState = PLAY_STATES.ABOUT_TO_GENERATE_WAVE;
        }

        public override void Update(GameTime gameTime)
        {
            int delta = gameTime.ElapsedGameTime.Milliseconds;
            for (int i = 0; i < gameAssets_.EnemyListCount; i++)                    //Enemy logic
            {
                gameAssets_.EnemyListItem(i).Update(delta);
            }

            for (int i = 0; i < gameAssets_.TowerListCount; i++)                    //Player logic
            {
                //pass a reference to the playstate, if the playstate is set to overwhelmed then it is a game over.
                gameAssets_.TowerListItem(i).Update(gameTime, ref playState);
            }

            if (playState == PLAY_STATES.OVERWHELMED)
            {
                //Display overwhelmed text to screen
                for (int i = 0; i < gameAssets_.GUIListCount; i++)
                {
                    if (gameAssets_.GUIListItem(i).Identifier == (int)GUI_SPRITES.OVERWHELMED_TEXT)
                    {
                        gameAssets_.GUIListItem(i).Visible = true;
                    }
                }

                overwhelmedTimer++;

                if (overwhelmedTimer >= 500)
                {
                    ResetToMenu();
                }
            }
            else
            {
                InputManagement(delta);                                                      //Spells - suggest input handler later to cover some functions already being handled by this function

                //MoveViewPort();                                                         //Viewport control
                viewPort_.Update();

                for (int i = 0; i < gameAssets_.SpellListCount; ++i)
                {
                    gameAssets_.SpellListItem(i).Update(delta);
                }

                for (int i = 0; i < gameAssets_.GUIListCount; i++)                  //Move GUI Elements with Viewport
                {
                    gameAssets_.GUIListItem(i).Update(viewPort_.X, viewPort_.Y);
                }
                
                gameAssets_.RemoveEntitiesMarkedForDelete();                            //Removing all objects marked as !active from appropriate lists            
                CollisionTesting(gameTime);                                                     //Collisions

                UpdateState();
            }
        }

        private void UpdateState()
        {
            //below playstate is only ever set for one tick. this is the tick that we generate the wave
            if (playState == PLAY_STATES.ABOUT_TO_GENERATE_WAVE)
            {
                currentTimeBetweenSpawners *= TIME_BETWEEN_SPAWNERS_MULTIPLIER;
                int pointsToSpendPerSpawner = waveNum * POINTS_TO_SPEND_MULTI + POINTS_TO_SPEND_BASE;
                int numOfSpawners = waveNum * SPAWNER_NUM_MULTI;
                int timeBetweenSpawners = (int)(waveNum * currentTimeBetweenSpawners);

                Debug.WriteLine("pointsToSpendPerSpawner: " + pointsToSpendPerSpawner);
                Debug.WriteLine("numOfSpawners: " + numOfSpawners);
                Debug.WriteLine("timeBetweenSpawners: " + timeBetweenSpawners);

                currentWave = objectFactory_.GenerateWave(pointsToSpendPerSpawner, numOfSpawners, timeBetweenSpawners, viewPort_, waveNum);

                //we need this state so we dont accidentally think the level is over before it has started (enemies may not spawn immediately)
                playState = PLAY_STATES.WAITING_FOR_WAVE_TO_START;
            }

            //as soon as an enemy has spawned, we are in progress
            if (playState == PLAY_STATES.WAITING_FOR_WAVE_TO_START && gameAssets_.EnemyListCount > 0)
            {
                playState = PLAY_STATES.WAVE_IN_PROGRESS;
            }

            //if the wave has been running, and there are no enemies left it may be the end of the wave,
            //although we also need to check that there are no spawners that are waiting to start. 
            if (playState == PLAY_STATES.WAVE_IN_PROGRESS && gameAssets_.EnemyListCount == 0)
            {
                //check that all spawners have stopped spawning
                bool running = false;
                for (int i = 0; i < currentWave.Count; ++i)
                {
                    if (currentWave[i].Running)
                    {
                        running = true;
                    }
                }

                if (!running)
                {
                    waveNum++;
                    playState = PLAY_STATES.WAVE_COMPLETE;
                    Debug.WriteLine("BEGIN WAVE" + waveNum);
                    waveCompleteTimer.Start();
                }
            }
        }



        void CollisionTesting(GameTime gameTime_)
        {
            //COLLISSION TESTING - Basic Player/Enemy Collission Test - logic here or collider can handle it?
            for (int i = 0; i < gameAssets_.EnemyListCount; i++)
            {
                if (colliderHandler_.Collider(gameAssets_.TowerListItem(0), gameAssets_.EnemyListItem(i)))
                {
                    gameAssets_.TowerListItem(0).Capacity++;
                    gameAssets_.EnemyListItem(i).Active = false;
                }
            }

            //spell - enemy
            if (gameAssets_.SpellListCount > 0)
            {
                for (int i = 0; i < gameAssets_.SpellListCount; i++)
                {
                    for (int j = 0; j < gameAssets_.EnemyListCount; j++)
                    {
                        if (colliderHandler_.Collider(gameAssets_.SpellListItem(i), gameAssets_.EnemyListItem(j)))
                        {
                            int essenceReturned = gameAssets_.EnemyListItem(j).Hit(gameAssets_.SpellListItem(i));
                            gameAssets_.TowerListItem(0).Essence += essenceReturned;
                        }
                    }
                }
            }


        }

        public void SetActiveSpell(SPELL_TYPE type_)
        {
            for (int i = 0; i < gameAssets_.GUIListCount; i++)
            {
                if (i == (int)type_)
                {
                    gameAssets_.GUIListItem(i).Active = true;
                }
                else if (i < 5) //hack temp fix
                {
                    gameAssets_.GUIListItem(i).Active = false;
                }
            }
        }


        void InputManagement(float delta_)
        {

            //Select spells 1-5.
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                spellSelect = SPELL_TYPE.FIREBALL;
                SetActiveSpell(SPELL_TYPE.FIREBALL);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                spellSelect = SPELL_TYPE.ICELANCE;
                SetActiveSpell(SPELL_TYPE.ICELANCE);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                spellSelect = SPELL_TYPE.LIGHTNING;
                SetActiveSpell(SPELL_TYPE.LIGHTNING);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                spellSelect = SPELL_TYPE.DESPAIR;
                SetActiveSpell(SPELL_TYPE.DESPAIR);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                spellSelect = SPELL_TYPE.RAPTURE;
                SetActiveSpell(SPELL_TYPE.RAPTURE);
            }
            

            currentMouseState = Mouse.GetState();

            //set all existing on screen spells Initialhit=false
            for (int i = 0; i < gameAssets_.SpellListCount; i++)
            {
                gameAssets_.SpellListItem(i).InitialHitFinished();
            }


            bool clicked = false;
            //get the mouse position on click and release
            if (prevMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                mousePosOnClick = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            if (currentMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                //dragging = false;
                mousePosOnRelease = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                if ((mousePosOnClick - mousePosOnRelease).Length() < 10)
                    clicked = true;
            }


            //bool clicked = currentMouseState.LeftButton == ButtonState.Released
            //            && prevMouseState.LeftButton == ButtonState.Pressed
            //            && !dragging;
            //            //&& mouseDownTime < DragTimeLapsus;
            bool holding = currentMouseState.LeftButton == ButtonState.Pressed
                        && prevMouseState.LeftButton == ButtonState.Pressed;
                        //&& mouseDownTime > DragTimeLapsus;



            //if (Mouse.GetState().LeftButton == ButtonState.Released) mouseDownTime = 0;
            //else mouseDownTime += delta_;

            //Debug.WriteLine("mouseDownTime: " + mouseDownTime.ToString());
            //Debug.WriteLine("DragTimeLapsus: " + DragTimeLapsus.ToString());

            //TAPPY TAP TAP
            ////http://msdn.microsoft.com/en-us/library/ff827740.aspx
            ////http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.touch.aspx
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed && !leftMouseButtonDown)
            if ( clicked )
            {
                //leftMouseButtonDown = true;
                //leftMouseButtonDown = true;
                bool GUIElementClicked = false;

                //Get gesture position in viewport
                //Vector2 gesturePos = new Vector2(gesture.Position.X - viewPort_.X, gesture.Position.Y - viewPort_.Y);
                Vector2 gesturePos = new Vector2(Mouse.GetState().X - viewPort_.X, Mouse.GetState().Y - viewPort_.Y);

                //Start at integer greater than menu screen images/textures

                for (int j = 0; j < gameAssets_.GUIListCount; j++)
                {
                    //Check if tap is on top of a GUI element - this will disable spell casting on that location if it is
                    if (colliderHandler_.Collider(gameAssets_.GUIListItem(j), gesturePos) && gameAssets_.GUIListItem(j).Visible)
                    {
                        //Logic for GUI Element depending on type
                        //Tower - Upgrades?
                        //Spellbook - spell book
                        //hotbar - select
                        switch (gameAssets_.GUIListItem(j).Identifier)
                        {
                            case 1:
                                spellSelect = SPELL_TYPE.FIREBALL;
                                SetActiveSpell(SPELL_TYPE.FIREBALL);
                                break;
                            case 2:
                                spellSelect = SPELL_TYPE.ICELANCE;
                                SetActiveSpell(SPELL_TYPE.ICELANCE);
                                break;
                            case 3:
                                spellSelect = SPELL_TYPE.LIGHTNING;
                                SetActiveSpell(SPELL_TYPE.LIGHTNING);
                                break;
                            case 4:
                                spellSelect = SPELL_TYPE.DESPAIR;
                                SetActiveSpell(SPELL_TYPE.DESPAIR);
                                break;
                            case 5:
                                spellSelect = SPELL_TYPE.RAPTURE;
                                SetActiveSpell(SPELL_TYPE.RAPTURE);
                                break;
                            default:

                                break;
                        }
                        GUIElementClicked = true;
                    }
                }

                if (!GUIElementClicked && !gameAssets_.TowerListItem(0).SpellCast && activeSpellCDs[(int)spellSelect] <= 0)
                {
                    //We must iterate through current active spell list to see whether the selected spell is currently on cooldown. (may change)
                    int spellX = (int)gesturePos.X - viewPort_.X;
                    int spellY = (int)gesturePos.Y - viewPort_.Y;

                    //Debug.WriteLine("X" + spellX + "Y" + spellY);

                    playSpellAudioCallback(spellSelect);
                    objectFactory_.CastSpell(spellSelect, gameAssets_.TowerListItem(0).SpellLevel[(int)spellSelect], spellX, spellY);
                    //Player has cast a spell - intiate global cooldown
                    gameAssets_.TowerListItem(0).SpellCast = true;

                    //Add cooldown time to list
                    activeSpellCDs[(int)spellSelect] = gameAssets_.SpellListItem((int)gameAssets_.SpellListCount - 1).SpellCooldown;
                }
            }

            ////Double Tap
            //if (gestures[i].GestureType == GestureType.DoubleTap)
            //{
            //    GestureSample gesture = gestures[i];
            //    Vector2 gesturePos = new Vector2(gesture.Position.X - viewPort_.X, gesture.Position.Y - viewPort_.Y);

            //    for (int j = 0; j < gameAssets_.GUIListCount; j++)
            //    {
            //        if (gameAssets_.GUIListItem(j).Identifier == (int)GUI_SPRITES.QUIT_BUTTON)
            //        {
            //            if (colliderHandler_.Collider(gameAssets_.GUIListItem(j), gesturePos))
            //            {
            //                Debug.WriteLine("DOUBLE TAPPED");
            //                //Going back to main menu we want to reset our game lists
            //                //Flush all lists
            //                ResetToMenu();

            //            }
            //        }
            //    }
            //}

            

            

                        

            if (holding)
            {
                //calculate how far mouse has moved
                Vector2 currentMousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                Vector2 prevMousePos = new Vector2(prevMouseState.X, prevMouseState.Y);
                Vector2 movement = currentMousePos - prevMousePos;
                
                if (prevMousePos != currentMousePos)
                {
                    //dragging = true;
                }
                viewPort_.Movement(movement.X, movement.Y);
            }
            else
            {
                viewPort_.DragComplete();
            }

            ////Move the viewport and snap it back
            //if (gestures[i].GestureType == GestureType.FreeDrag)
            //{
            //    viewPort_.Movement(gestures[i].Delta.X, gestures[i].Delta.Y);
            //}
            //if (gestures[i].GestureType == GestureType.DragComplete)
            //{
            //    viewPort_.DragComplete();
            //}
            

            //Iterate over cooldownlist - if anything > 0 we need to count it down
            for (int i = 0; i < activeSpellCDs.Count; i++)
            {
                if (activeSpellCDs[i] > 0)
                {
                    activeSpellCDs[i] -= (int)delta_;                    
                    if (activeSpellCDs[i] <= 0)
                    {
                        //    possibly add audio for spell now ready?
                    }
                }

                if (activeSpellCDs[i] < 0)
                {
                    activeSpellCDs[i] = 0;
                }
            }

            //if (Mouse.GetState().LeftButton == ButtonState.Released)
            //{
            //    leftMouseButtonDown = false;
            //}
            prevMouseState = currentMouseState;
        }

        public void ResetToMenu()
        {
            finalScore = gameAssets_.TowerListItem(0).Essence;
            gameAssets_.FlushEntities();
            gameAssets_.TowerListItem(0).Capacity = 0;
            gameAssets_.TowerListItem(0).Essence = 0;
            CurrentGameState = (int)GAME_STATES.SAVE_TO_FILE;
        }

        public PLAY_STATES CurrentPlayState
        {
            get { return playState; }
        }
    }
}
