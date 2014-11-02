using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace SpellSlingerWindowsPort
{



    class Spell : Entity
    {
        SPELL_TYPE type;
        int spellLevel;
        float damage;
        float damagePerTick;
        //Timer activeTimer;
        float currentTimeActive;
        int activeTime;
        int spellCooldown;

        float initialDamage;
        //Timer frameSwapTimer;
        int currentFrame;
        GameAssets assets;

        public Spell(SPELL_TYPE type_, int spellLevel_, float x_, float y_, GameAssets assets_)
        {
            assets = assets_;
            type = type_;
            spellLevel = spellLevel_;
            Active = true;
            initialDamage = 0;

            X = x_;
            Y = y_;

            //            Debug.WriteLine("spellX" + X + "spellY" + Y);

            //Set spell to be 'active' - currently being used to control draw time on screen

            //Split into own functions later
            switch (type)
            {
                case SPELL_TYPE.FIREBALL:
                    Width = 64;
                    Height = 64;
                    //Width = 128;
                    //Height = 128;
                    initialDamage = 50;
                    damagePerTick = initialDamage * 0.01f;
                    //initialDamage = 100 * (1 + spellLevel * 0.1f);                    
                    spellCooldown = 10;
                    activeTime = 800;
                    drawColour = Color.Red;
                    break;
                case SPELL_TYPE.ICELANCE:
                    Width = 128;
                    Height = 128;
                    initialDamage = 20 * (1 + spellLevel * 0.1f);
                    damagePerTick = initialDamage * 0.01f; 
                    spellCooldown = 600;
                    activeTime = 800;
                    drawColour = Color.BlueViolet;
                    break;
                case SPELL_TYPE.LIGHTNING:
                    Width = 128;
                    Height = 128;
                    initialDamage = 30 * (1 + spellLevel * 0.1f);
                    damagePerTick = initialDamage * 0.01f;
                    spellCooldown = 1000;
                    activeTime = 1400;
                    drawColour = Color.Green;
                    break;
                case SPELL_TYPE.DESPAIR:
                    Width = 256;
                    Height = 256;
                    initialDamage = 50 * (1 + spellLevel * 0.1f);
                    damagePerTick = initialDamage * 0.01f;
                    spellCooldown = 7500;
                    activeTime = 2000;
                    drawColour = Color.DarkOrange;
                    break;
                case SPELL_TYPE.RAPTURE:
                    Width = 384;
                    Height = 384;
                    initialDamage = 60;
                    damagePerTick = initialDamage * 0.01f;
                    spellCooldown = 10000;
                    activeTime = 3000;
                    drawColour = Color.DarkTurquoise;
                    break;
                default:
                    Width = 64;
                    Height = 64;
                    initialDamage = 0;
                    damagePerTick = 0;
                    spellCooldown = 10;
                    activeTime = 500;
                    break;
            }
            currentTimeActive = 0.0f;
            damage = initialDamage;
            currentFrame = SpriteManager.SPELL_SPRITE_ANIMATIONS - 1;
            //this.texture = assets.AnimatedSpellTextureList[type][currentFrame];
            this.texture = assets.spellTexture;
        }

        public void InitialHitFinished()
        {
            damage = damagePerTick;
        }

        public void Update(float delta_)
        {
            if (Active)
            {
                currentTimeActive += delta_;
                if (currentTimeActive > activeTime)
                {
                    Active = false;
                }
                SwapFrame();                
            }
        }

        private void SwapFrame()
        {
            float fframe = MathHelper.Lerp(0.0f, (float)SpriteManager.SPELL_SPRITE_ANIMATIONS, (activeTime - currentTimeActive) / activeTime);
            int iFrame = (int)fframe;
            if (iFrame != currentFrame)
            {                
                --currentFrame;
                if (currentFrame < 0 )
                {
                    Active = false;
                }
                //else
                //{
                //    this.texture = assets.AnimatedSpellTextureList[type][currentFrame];
                //}
            }
        }

        

        //private void OnTimedEventDamage(object source, ElapsedEventArgs e)
        //{
        //    damage = damagePerTick;
        //}

        public int SpellLevel
        {
            get { return spellLevel; }
            set { spellLevel = value; }
        }

        internal SPELL_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }

        public override Rectangle? SourceRectangle
        {
            get 
            {             
                //use current frame to mathematically work out the spell coords.
                //63 = bottom right
                //62 = on left of bottom right
                //55 = one above bottom right
                //7 = top right
                //0 = top left
                //work out row and column
                int row = currentFrame / 8;
                int col = currentFrame % 8;

                //work out rectangle based on row col and size 128x128
                Rectangle rect = new Rectangle(col * 128, row * 128, 128, 128);
                return rect; 
            }
        }

        public float Damage
        {
            //make sure the first time we get initial damage. 
            //every time after, damagePerTick
            get
            {
                return damage;
            }
            set { damage = value; }
        }



        public int SpellCooldown
        {
            get { return spellCooldown; }
            set { spellCooldown = value; }
        }

        public float DamagePerTick
        {
            get { return damagePerTick; }
            set { damagePerTick = value; }
        }

    }
}