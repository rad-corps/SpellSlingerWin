using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SpellSlingerWindowsPort
{
    class EnemySpawnRules
    {
        Dice dice;
        List<ENEMY_TYPE> enemyTypes; //use this list to decide what was spawned based on dice roll.         

        public EnemySpawnRules(Dice dice_, ENEMY_TYPE defaultEnemyType_)
        {
            dice = dice_;
            enemyTypes = new List<ENEMY_TYPE>(dice.MaxRoll);
            
            //fill with default to start with
            for (int i = 0; i < dice.MaxRoll; ++i)
            {
                enemyTypes.Add(defaultEnemyType_);
            }
        }

        //allocate EnemyType results to enemyTypes List from pos, to len number of elements
        public void SetEnemyRule(ENEMY_TYPE enemyType, int pos, int len)
        {
            //fill with enemyType passed in
            for (int i = pos; i < pos + len; ++i) //this will fall over if len + pos >= enemyTypes.Count 
            {
                enemyTypes[i] = enemyType;
            }
        }


        //hopefully we dont get anything out of range here. dice.Roll() should return between 0 AND dice.MAX - 1
        public ENEMY_TYPE RandomiseEnemy()
        {            
            int dice_roll = dice.Roll();
            return enemyTypes[dice_roll];
        }
    }

    

}
