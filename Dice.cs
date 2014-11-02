using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Models N number of dice with S number of sides

namespace SpellSlingerWindowsPort
{
    class Dice
    {
        private int dice;
        private int sides;
        Random random;

        public Dice(int numOfDice_, int numOfSides_)
        {
            dice = numOfDice_;
            sides = numOfSides_;
            random = new Random();
            //to do error checking. 
            //can not have 0  or <0 dice
            //can not have 0 or <0 sides
            //upper bounds? 
        }

        public int Roll()
        {
            int result = 0;

            for (int i = 0; i < dice; ++i)
            {
                result += random.Next(1, sides);
            }

            return result; 
        }

        public int MaxRoll
        {
            get
            {
                return dice * sides;
            }
        }

    }
}
