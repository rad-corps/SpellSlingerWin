using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpellSlingerWindowsPort
{

    class Entity
    {
        protected Texture2D texture;
        protected Vector2 pos;
        private int width;
        private int height;
        private float rotation;
        private bool visible;
        private int identifier;                         //Use for MOR object control (Required for GUI & spell select)
        protected Color drawColour = Color.White;
        protected float scale = 1.0f;
        protected Rectangle sourceRect;                     //Used for UV's


        //Will allow for iteration through lists and if false object will be removed - ie dead enemies, spells cast
        private bool active;
    
        public void ResetDrawColour()
        {
            drawColour = Color.White;
        }

        public virtual Rectangle? SourceRectangle
        {
            get { return null; } 
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Vector2 Origin
        {
            get
            {
                float width_div2 = width / 2;
                float height_div2 = height / 2;
                return new Vector2(width_div2, height_div2);
                //return new Vector2(0.0f, 0.0f);
            }
        }

        public float X
        {
            get { return pos.X; }
            set
            {
                pos.X = value;
            }

        }

        public float Y
        {
            get { return pos.Y; }
            set
            {
                pos.Y = value;
            }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Left
        {
            get { return pos.X - width / 2; } 
        }
                
        public float Right
        {
            get { return pos.X + width / 2; } 
        }

        public float Top
        {
            get { return pos.Y + height / 2; } 
        }

        public float Bottom
        {
            get { return pos.Y - height / 2; }
        }

        public Color DrawColor
        {
            get { return drawColour; }
            //set { drawColour = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }



    }
}
