using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
//using Android.Content;
////using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MonogameAndroidProject
{
    class LeaderboardSerializer //: Activity
    {
        private List<LeaderboardRecord> records;

        public List<LeaderboardRecord> Records { get { return records; } }

        public LeaderboardSerializer(List<LeaderboardRecord> records_)
        {
            records = records_;
        }

        public LeaderboardSerializer()
        {
            records = new List<LeaderboardRecord>();
        }

        public void Add(LeaderboardRecord record_)
        {
            records.Add(record_);
        }

        public void Save()
        {
            //make sure records are in order before saving them
            records.Sort();


            IFormatter formatter = new BinaryFormatter();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "lb.ss");
            FileStream fStream = File.Open(filePath, FileMode.Create, FileAccess.Write);

            //serialize the number of records as an int so we know how many to retreive
            formatter.Serialize(fStream, records.Count);

            for (int i = 0; i < records.Count; ++i)
            {
                formatter.Serialize(fStream, records[i]);
            }

            fStream.Close();
            fStream.Dispose();
        }

        
        public void Open()
        {
            //clear any records in memory before loading from file
            records.Clear();

            //create the stream from file
            IFormatter formatter = new BinaryFormatter();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "lb.ss");
            FileStream fStream = File.Open(filePath, FileMode.OpenOrCreate);

            //if we just created the file it will be empty
            if (fStream.Length > 0)
            {
                //how many records do we want to load? 
                int num = (int)formatter.Deserialize(fStream);

                //load the records
                for (int i = 0; i < num; ++i)
                {
                    records.Add((LeaderboardRecord)formatter.Deserialize(fStream));
                }
            }
            //dispose of the stream
            fStream.Close();
            fStream.Dispose();

            //make sure records are in order
            records.Sort();
        }

    }
}