using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpellSlingerWindowsPort
{

    class Factory
    {
        GameAssets gameAssets;
        List<EnemySpawnRules> spawnRulesList;
        Dice spawnRulesSelector;

        public Factory(GameAssets gameAssets_)                                        //Default Constructor
        {
            gameAssets = gameAssets_;
            spawnRulesList = new List<EnemySpawnRules>();

            InitEnemySpawnRules();
            spawnRulesSelector = new Dice(1, spawnRulesList.Count);

        }

        #region TEST WAVES

        private void InitEnemySpawnRules()
        {
            Dice dice = new Dice(1, 100);

            {
                //50% Ghoul, 50% Running Ghoul
                EnemySpawnRules rules = new EnemySpawnRules(dice, ENEMY_TYPE.GHOUL);
                rules.SetEnemyRule(ENEMY_TYPE.RUNNING_GHOUL, 50, 50);
                spawnRulesList.Add(rules);
            }

            {
                //70% SKELETON_KNIGHT, 30% WEREWOLF
                EnemySpawnRules rules = new EnemySpawnRules(dice, ENEMY_TYPE.SKELETON_KNIGHT);
                rules.SetEnemyRule(ENEMY_TYPE.WEREWOLF, 70, 30);
                spawnRulesList.Add(rules);
            }

            {
                //10% Green Dragon, 40% HEAVY_ZOMBIE, 40% SKELETON KNIGHT
                EnemySpawnRules rules = new EnemySpawnRules(dice, ENEMY_TYPE.HEAVY_ZOMBIE);
                rules.SetEnemyRule(ENEMY_TYPE.SKELETON_KNIGHT, 70, 30);
                rules.SetEnemyRule(ENEMY_TYPE.GREEN_DRAGON, 70, 30);
                spawnRulesList.Add(rules);
            }

            {
                //100% RUNNING GHOUL
                EnemySpawnRules rules = new EnemySpawnRules(dice, ENEMY_TYPE.RUNNING_GHOUL);
                spawnRulesList.Add(rules);
            }

        }

        //public void CreateTestWave()
        //{
        //    uint spawnNumber = 0;
        //    const uint SPAWN_INTERVAL = 5000;
        //    const uint TIMER_INTERVAL = 350;
        //    const float ENEMIES_WAVE_ONE = 5.0f;
        //    const float ENEMIES_INCREMENTER = 0.6f;
        //    const float ENEMIES_WAVE_END = 50.0f;

        //    //const float ENEMIES_WAVE_ONE = 1.0f;
        //    //const float ENEMIES_INCREMENTER = 0.6f;
        //    //const float ENEMIES_WAVE_END = 1.5f;

        //    //had to move CreatePlayer here as the creation of the spawn circle needs it to exist.
        //    CreatePlayer();

        //    //even though these enemySpawner instances instantly go out of scope. they are not destroyed while their timers are running. 
        //    for (float i = ENEMIES_WAVE_ONE; i < ENEMIES_WAVE_END; i += ENEMIES_INCREMENTER)
        //    {
        //        //grab a random rules                
        //        EnemySpawnRules rules = spawnRulesList[spawnRulesSelector.Roll()];

        //        Circle circle = new Circle(new Vector2(gameAssets.TowerListItem(0).X, gameAssets.TowerListItem(0).Y), 400.0);
        //        EnemySpawner enemySpawner = new EnemySpawner(this, rules, TIMER_INTERVAL, (uint)(spawnNumber * SPAWN_INTERVAL) + 1, (uint)i * 2, circle);
        //        ++spawnNumber;
        //    }            
        //}

        public List<EnemySpawner> GenerateWave(int pointsToSpendPerSpawner_, int numOfSpawners_, int timeBetweenSpawners_, ViewPort vp_, int waveNum_)
        {
            List<EnemySpawner> wave = new List<EnemySpawner>();
            uint spawnNumber = 0;
            const uint TIMER_INTERVAL = 350;

            //even though these enemySpawner instances instantly go out of scope. they are not destroyed while their timers are running. 
            for (float i = 1; i < numOfSpawners_; ++i)
            {
                //grab a random rules                
                EnemySpawnRules rules = spawnRulesList[spawnRulesSelector.Roll()];


                Circle circle = new Circle(new Vector2(gameAssets.TowerListItem(0).X, gameAssets.TowerListItem(0).Y), (vp_.ResRect.Width / 2) + 200);
                uint startTimerMS = (uint)(spawnNumber * timeBetweenSpawners_) + 1;
                EnemySpawner enemySpawner = new EnemySpawner(this, rules, TIMER_INTERVAL, startTimerMS, circle, pointsToSpendPerSpawner_, waveNum_);
                wave.Add(enemySpawner);
                ++spawnNumber;
            }
            return wave;
        }



        #endregion

        //By the time we get to CreateEnemy the EnemySpawner, Dice and EnemySpawnRules have done their job i.e. decided what enemy to spawn. 
        public Enemy CreateEnemy(ENEMY_TYPE enemyType_, Vector2 enemyPos_, int wave_)
        {
            //TODO: 0 hardcoded in next line for now, will be safe unless multiple towers introduced
            Enemy enemy = new Enemy(enemyType_, gameAssets.TowerListItem(0).Pos, enemyPos_, wave_);

            enemy.Texture = gameAssets.EnemyTextureList[(int)enemyType_];
            gameAssets.EnemyListAdd(enemy);

            return enemy;
        }

        public void CreatePlayer()
        {
            Tower entity = new Tower();
            entity.Texture = gameAssets.TextureList[(int)PLAYER_SPRITES.TOWER];
            gameAssets.TowerListAdd(entity);
        }

        public void CastSpell(SPELL_TYPE spellType_, int level_, float x_, float y_)
        {
            Spell spell = new Spell(spellType_, level_, x_, y_, gameAssets);
            //spell.Texture = gameAssets.SpellTextureList[(int)spellType_];
            gameAssets.SpellListAdd(spell);
        }

        public void CreateGUIComponent(GUI_SPRITES sprite_, float x_, float y_, int width_, int height_, bool active_, bool visible_, int identifier_)
        {
            GUI_Component button = new GUI_Component(x_, y_, width_, height_, active_, visible_, identifier_);
            button.Texture = gameAssets.GUITextureList[(int)sprite_];
            if (identifier_ < 900)      //magic numbers tut tut
            {
                gameAssets.GUIListAdd(button);
            }
            else
            {
                gameAssets.MenuListAdd(button);
            }
        }

    }
}
