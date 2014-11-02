using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpellSlingerWindowsPort
{
    class GUI_Component : Entity
    {
        float offsetX;
        float offsetY;

        public GUI_Component(float x_, float y_, int width_, int height_, bool active_, bool visible_, int identifier_)
        {
            Width = width_;
            Height = height_;
            X = x_;
            Y = y_;
            offsetX = X;
            offsetY = Y;
            Active = active_;
            Visible = visible_;
            Identifier = identifier_;
        }

        public void Update(float x_, float y_)
        {
            X = offsetX - x_;
            Y = offsetY - y_;

            if (Active)
            {
                drawColour = Color.White;
            }
            else
            {
                drawColour = Color.OrangeRed;
            }

        }

    }
}
