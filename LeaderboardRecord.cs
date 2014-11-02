using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;



namespace MonogameAndroidProject
{
    [Serializable]
    class LeaderboardRecord : IComparable<LeaderboardRecord>
    {
        //[NonSerialized]
        //private static int CURRENT_ID;

        //readonly int id;
        //public int ID { get { return id; } }
        public string name;
        public int score;

        public LeaderboardRecord(string name_, int score_)
        {
            name = name_;
            score = score_;
            //id = CURRENT_ID++;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            LeaderboardRecord lr = obj as LeaderboardRecord;
            if (lr == null) return false;
            else return Equals(lr);
        }

        public bool Equals(LeaderboardRecord other)
        {
            if (other == null) return false;
            return (this.score.Equals(other.score));
        }

        // Default comparer for Part type. 
        public int CompareTo(LeaderboardRecord lr_)
        {
            // A null value means that this object is greater. 
            if (lr_ == null)
                return 1;

            else
                return lr_.score.CompareTo(this.score);
        }


    }
}