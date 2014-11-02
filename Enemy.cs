using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MonogameAndroidProject
{
    //enum ENEMY_STATUS
    //{
    //    OK,
    //    RECOVERING
    //}

    class Enemy : Entity
    {

        private int health;
        private int essence;
        private float speed;
        private ENEMY_TYPE enemyType;
        private Vector2 playerPos;
        private SPELL_TYPE resistance;
        private SPELL_TYPE weakness;
        //private ENEMY_STATUS status;
        //private Timer recoveryTimer;
        private int cost;


        public Enemy(ENEMY_TYPE enemyType_, Vector2 playerPos_, Vector2 pos_)
        {
            pos = pos_;
            //status = ENEMY_STATUS.OK;
            enemyType = enemyType_;
            InitialiseEnemyVariables();
            playerPos = playerPos_;
            Active = true;
        }

        //direction property (we dont need to store as we can calculate on the fly)
        public Vector2 Direction
        {
            get
            {
                Vector2 dir = Pos - playerPos;
                dir.Normalize();
                return dir;
            }
        }

        //happy to leave this hardcoded for now rather than using const vars
        //at least it is neat and everything is in the same spot. 
        //eventually want to database drive this. 
        private void InitialiseEnemyVariables()
        {
            switch (enemyType)
            {
                case ENEMY_TYPE.GHOUL:
                    health = 12;
                    speed = 0.016f;
                    weakness = SPELL_TYPE.FIREBALL;
                    cost = 15;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;
                    break;
                case ENEMY_TYPE.RUNNING_GHOUL:
                    health = 6;
                    speed = 0.03f;
                    weakness = SPELL_TYPE.FIREBALL;
                    cost = 20;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;
                    break;
                case ENEMY_TYPE.HEAVY_ZOMBIE:
                    health = 22;
                    speed = 0.025f;
                    weakness = SPELL_TYPE.FIREBALL;
                    resistance = SPELL_TYPE.LIGHTNING;
                    cost = 30;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;                                        
                    break;
                case ENEMY_TYPE.SKELETON_KNIGHT:
                    health = 30;
                    speed = 0.03f;
                    weakness = SPELL_TYPE.RAPTURE;
                    resistance = SPELL_TYPE.DESPAIR;
                    cost = 40;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;                    
                    break;
                case ENEMY_TYPE.OGRE:
                    health = 30;
                    speed = 0.035f;
                    weakness = SPELL_TYPE.ICELANCE;
                    resistance = SPELL_TYPE.FIREBALL;
                    cost = 80;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;   
                    break;
                case ENEMY_TYPE.WEREWOLF:
                    health = 36;
                    speed = 0.04f;
                    weakness = SPELL_TYPE.ICELANCE;
                    resistance = SPELL_TYPE.FIREBALL;
                    cost = 160;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;   
                    break;
                case ENEMY_TYPE.GREEN_DRAGON:
                    health = 50;
                    speed = 0.05f;
                    weakness = SPELL_TYPE.RAPTURE;
                    resistance = SPELL_TYPE.DESPAIR;
                    cost = 320;
                    essence = cost;
                    this.Width = 32;
                    this.Height = 32;   
                    break;
            }
        }

        //returns amount of essense received if hit results in a kill
        //else returns 0
        public int Hit(Spell spell_)
        {
            drawColour = Color.Red;

            float dmg = spell_.Damage;
            //Console.WriteLine("Damage: " + dmg);

            //do we have a resistance to this spell? if so half the damage
            if (resistance == spell_.Type)
                dmg /= 2;

            //are we weak on this spell?
            if (weakness == spell_.Type)
                dmg *= 2;

            //reduce enemy health by dmg
            health -= (int)dmg;

            //Debug.WriteLine("Dmg Received: " + dmg + ". Health: " + health);

            if (health <= 0)
            {
                Active = false;
                return essence;
            }
            //else
            //{
            //    //status = ENEMY_STATUS.RECOVERING;                    
            //    //recoveryTimer.Start();
            //}

            //TODO fix the way the essence is calculated
            return 0;
        }

        //to be called by Update
        //Move enemy towards player
        public void Update(int delta_)
        {
            //int delta = gameTime_.ElapsedGameTime.Milliseconds;
            Vector2 movementPreDelta = new Vector2();
            movementPreDelta = (Direction * speed);
            pos -= (movementPreDelta * delta_);
            Rotation = (float)(Math.Atan2(Direction.Y, Direction.X)) - ((float)Math.PI / 2);
            //Debug.WriteLine(Rotation);
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public ENEMY_TYPE EnemyType
        {
            get
            {
                return enemyType;
            }
        }

        public int Cost
        {
            get { return cost; }
        }

    }
}
