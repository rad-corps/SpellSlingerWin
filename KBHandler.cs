using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpellSlingerWindowsPort
{
    public class KbHandler
    {
        private Keys[] lastPressedKeys;

        public string Str { get { return str; } }
        string str;
        public bool Submit { get { return submit; } }
        bool submit;


        public KbHandler()
        {
            lastPressedKeys = new Keys[0];
            submit = false;
        }

        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyDown(Keys key)
        {
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                int num = ((int)key);
                num = num - (int)Keys.D0;
                str += num.ToString();
            }
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                int num = ((int)key);
                num = num - (int)Keys.NumPad0;
                str += num.ToString();
            }
            if (key == Keys.Back && str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
            }
            if (key == Keys.Enter)
            {
                submit = true;
            }

        }

        private void OnKeyUp(Keys key)
        {
            //do stuff
        }
    }
}
