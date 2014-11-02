using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MonogameAndroidProject
{
    
    public class Circle
    {
        Vector2 centre;
        public double radius;
        //Random random;
        private static Random random;
        private double lastRandomAngle;

        public Circle(Vector2 centre_, double radius_)
        {
            centre = centre_;
            radius = radius_;
            
            if (random == null)
            {
                int seed = unchecked(DateTime.Now.Ticks.GetHashCode());
                random = new Random(seed); 
            }

        }

        public Vector2 GetPoint(double angle_)
        {
            Vector2 ret = new Vector2();
            float sine = (float) (Math.Sin(angle_) * radius);
            float cosine = (float)(Math.Cos(angle_) * radius);
            ret.X = centre.X + cosine;
            ret.Y = centre.Y + sine;                       
            return ret;
        }

        public Vector2 RandomPoint()
        {
            lastRandomAngle = random.NextDouble() * (Math.PI * 2);
            return GetPoint(lastRandomAngle);
        }

        public Vector2 GetPointNearLastRandomAngle(double tolleranceInRadians_ = 0.65)
        {
            double maxAngle = lastRandomAngle + (tolleranceInRadians_ / 2.0);
            double minAngle = lastRandomAngle - (tolleranceInRadians_ / 2.0);
            double randomAngle = random.NextDouble() * (maxAngle - minAngle) + minAngle;
            return GetPoint(randomAngle);
        }
    }
}