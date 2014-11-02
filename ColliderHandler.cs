using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MonogameAndroidProject
{
    class ColliderHandler
    {

        //public bool Collider(Entity entity_a, Entity entity_b)
        //{
        //    float entity_a_LH = entity_a.X - entity_a.Width / 2; 	                //LH
        //    float entity_a_RH = entity_a.X + entity_a.Width / 2;	//RH
        //    float entity_a_T = entity_a.Y - entity_a.Height / 2;	                    //Top
        //    float entity_a_B = entity_a.Y + entity_a.Height / 2;	//Btm

        //    //Let's get centre of entity_b for a little more realism/accuracy
        //    float entity_b_X = entity_b.X;// +(entity_b.Width * 0.5f);
        //    float entity_b_Y = entity_b.Y;// +(entity_b.Height * 0.5f);

        //    if (entity_b_X >= entity_a_LH && entity_b_X <= entity_a_RH && entity_b_Y <= entity_a_B && entity_b_Y >= entity_a_T)
        //    {
        //        //On enemy collission with tower increase capacity of tower - or this can be ran in main game loop
        //        if (entity_a.GetType() == typeof(Tower))
        //        {
        //            //Temporary for testing - set object to inactive for list clean up - Pass responsibility to enemy directly
        //            entity_b.Active = false;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public bool Collider(Entity r1_, Entity r2_)
        {
            if (r1_.Right < r2_.Left
                || r2_.Right < r1_.Left
                || r1_.Bottom > r2_.Top
                || r1_.Top < r2_.Bottom)
            {
                return false;
            }
            return true;
        }

        public bool Collider(Entity entity_a, Vector2 mousePos_)
        {
             float entity_a_LH = entity_a.X - entity_a.Width/2; 	                //LH
            float entity_a_RH = entity_a.X + entity_a.Width/2;	//RH
            float entity_a_T = entity_a.Y - entity_a.Height/2;	                    //Top
            float entity_a_B = entity_a.Y +entity_a.Height/2;	//Btm

            if (mousePos_.X >= entity_a_LH && mousePos_.X <= entity_a_RH && mousePos_.Y <= entity_a_B && mousePos_.Y >= entity_a_T)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
