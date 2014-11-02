using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;

namespace SpellSlingerWindowsPort
{
    

    class ViewPort
    {


        int viewPortWidth;
        int viewPortHeight;

        int aimAreaX;
        int aimAreaY;

        public int FocusAreaX { get { return focusAreaX; } }
        public int FocusAreaY { get { return focusAreaY; } }
        int focusAreaX;
        int focusAreaY;

        const int VIEW_PORT_SPEED = 15;

        SpriteBatch spriteBatch;
        Vector2 snapPosition;
        Rectangle drawPos;
        Rectangle resRect;

        //PC: is the user currently holding W,A,S or D down?
        bool viewLeftSnappedState;
        bool viewRightSnappedState;
        bool viewUpSnappedState;
        bool viewDownSnappedState;

        bool movementHeld;

        

        public ViewPort(SpriteBatch spriteBatch_, int viewPortWidth_, int viewPortHeight_)
        {
            spriteBatch = spriteBatch_;
            viewPortWidth = viewPortWidth_;
            viewPortHeight = viewPortHeight_;
            focusAreaX = 0;
            focusAreaY = 0;
            aimAreaX = 0;
            aimAreaY = 0;

            viewLeftSnappedState = false;
            viewRightSnappedState = false;
            viewUpSnappedState = false;
            viewDownSnappedState = false;

            snapPosition = new Vector2(0.0f, 0.0f);
            drawPos = new Rectangle(0, 0, 0, 0);
            movementHeld = false;
            resRect = new Rectangle(0, 0, viewPortWidth_, viewPortHeight_);
        }


        public void Draw(Entity entity_)
        {
            int xPos = (int)entity_.X + focusAreaX;
            int yPos = (int)entity_.Y + focusAreaY;

            //dont create a new rectangle each time, thanks JK
            drawPos.Width = entity_.Width;
            drawPos.Height = entity_.Height;
            drawPos.X = xPos;
            drawPos.Y = yPos;

            Rectangle destination = new Rectangle(drawPos.X - drawPos.Width / 2,
                                                    drawPos.Y - drawPos.Height / 2,
                                                    drawPos.Width,
                                                    drawPos.Height);

            //spriteBatch.Draw(entity_.Texture, null, drawPos, null, entity_.Origin, entity_.Rotation, null, entity_.DrawColor, SpriteEffects.None, 0f);
            spriteBatch.Draw(entity_.Texture, destination, entity_.SourceRectangle, entity_.DrawColor, entity_.Rotation, new Vector2(), SpriteEffects.None, 0.0f);

            if (entity_ is Enemy)
                entity_.ResetDrawColour();
        }

        public void DragComplete()
        {
            movementHeld = false;
        }

        public void Movement(float x_, float y_)
        {
            focusAreaX += (int)x_;
            focusAreaY += (int)y_;

            if (focusAreaX >= 250)
                focusAreaX = 250;
            if (focusAreaX <= -250)
                focusAreaX = -250;
            if (focusAreaY >= 150)
                focusAreaY = 150;
            if (focusAreaY <= -150)
                focusAreaY = -150;

            movementHeld = true;
        }

        public void Update()
        {
            if (movementHeld == false)
            {
                if (focusAreaX < aimAreaX)
                    focusAreaX += VIEW_PORT_SPEED;

                if (focusAreaX > aimAreaX)
                    focusAreaX -= VIEW_PORT_SPEED;

                if (focusAreaY < aimAreaY)
                    focusAreaY += VIEW_PORT_SPEED;

                if (focusAreaY > aimAreaY)
                    focusAreaY -= VIEW_PORT_SPEED;
            }
        }

        public int X
        {
            get { return focusAreaX; }
        }

        public int Y
        {
            get { return focusAreaY; }
        }

        public int ViewPortWidth
        {
            get { return viewPortWidth; }
            set { viewPortWidth = value; }
        }

        public int ViewPortHeight
        {
            get { return viewPortHeight; }
            set { viewPortHeight = value; }
        }

        public Rectangle ResRect
        {
            get
            {
                return resRect;
            }
        }
    }
}
