using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;

namespace SpellSlingerWindowsPort
{
    class EnemySpawner
    {
        //the timer that controls creation
        private Timer spawnTimer;
        
        //stopTimer was a (my) dumb idea. better off just counting how many enemies to spawn
        //private Timer stopTimer;

        //private uint numEnemiesToSpawn;
        //private uint numEnemiesSpawned;
        private int pointsToSpend;


        private Timer startTimer;
        Vector2 spawnPoint;

        bool timerStopped;
        
        //this object used to create enemies when told to spawn (ordering them from the factory :)
        Factory factoryOrder; 

        //the rules that govern the chance of enemy type spawn. 
        EnemySpawnRules esr;

        //The Circle used to determine spawn location
        Circle spawnCircle;

        //constructor takes a factory (because we have already created this in Game and dont want to create a second one)
        public EnemySpawner(Factory factory_, EnemySpawnRules esr_, uint timerIntervalMs_, uint startTimerms_, Circle spawnCircle_, int pointsToSpend_)
        {
            pointsToSpend = pointsToSpend_;
            
            //numEnemiesSpawned = 0;
            //numEnemiesToSpawn = numEnemiesToSpawn_;

            //assign classwide variables
            spawnCircle = spawnCircle_;
            esr = esr_;
            factoryOrder = factory_;

            //create the timer that will control when enemies are created. 
            spawnTimer = new System.Timers.Timer(timerIntervalMs_);
            
            //the timer controlling when we stop spawning
            //stopTimer = new System.Timers.Timer(stopTimerMs_);

            //when do we kick it off? 
            startTimer = new System.Timers.Timer(startTimerms_); 

            //this line adds the SpawnEnemy envent handler (see SpawnEnemy function) that fires when the spawnTimer fires
            spawnTimer.Elapsed += SpawnEnemy;
            //stopTimer.Elapsed += StopSpawner;
            startTimer.Elapsed += Start;

            //where do we want to spawn them from? 
            spawnPoint = spawnCircle.RandomPoint();

            //kick off the timer
            startTimer.Start();
            //spawnTimer.Start();

            timerStopped = false;
        }

        //start the timer again
        private void ResetTimer()
        {
            if (!timerStopped)
            {
                spawnTimer.Stop();
                spawnTimer.Start();   //Comment out to spawn 1 enemy for testing
            }
        }

        //Callback for startTimer.Elapsed
        private void Start(Object source, ElapsedEventArgs e)
        {
            startTimer.Stop();
            spawnTimer.Start();
            //stopTimer.Start();
        }

        //Tell the factory to make us another enemy, because we only got here from spawnTimer going off!!!
        private void SpawnEnemy(Object source, ElapsedEventArgs e)
        {
            if (!timerStopped)
            {
                ENEMY_TYPE enemy_type = esr.RandomiseEnemy();

                Enemy enemy = factoryOrder.CreateEnemy(enemy_type, spawnCircle.GetPointNearLastRandomAngle());

                //if (enemy.Cost > pointsToSpend)
                //do not spawn enemy and tell the owner that the EnemySpawner has stopped
                if (enemy.Cost > pointsToSpend)
                {
                    this.StopSpawner();
                    return;
                }
                else
                {
                    pointsToSpend -= enemy.Cost;
                    this.ResetTimer();                    
                }
            }
        }

        private void StopSpawner()
        {
            timerStopped = true;
            spawnTimer.Stop();
            startTimer.Stop();
            //stopTimer.Stop();
            
            //TODO: flag for deletion
        }

        public bool HasStopped
        {
            get { return timerStopped; }
        }

        public bool Running
        {
            get { return !timerStopped; }
        }

        
    }
}
