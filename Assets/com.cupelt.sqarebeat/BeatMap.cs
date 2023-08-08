using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.cupelt.sqarebeat.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace com.cupelt.sqarebeat
{
    [System.Serializable]
    public class BeatMap
    {
        public BeatMap(string path)
        {
            this._path = path;
            
            using (StreamReader file = File.OpenText(path + @"\beatmap.sbm"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                this._json = (JObject)JToken.ReadFrom(reader);
            }
    
            JObject general = (JObject)this._json["General"].ToObject(typeof(JObject));
            this.clip = Util.LoadMp3(path + @"\" + general["audio"]);
    
            this.artist = general["artist"].ToString();
            this.title = general["title"].ToString();
        }
        
        private JObject _json;
        private string _path;
    
        public string artist;
        public string title;
    
        public AudioClip clip;
    
        public Stack<Note> getNotes()
        {
            JToken notes = this._json.SelectToken("Notes");

            List<Note> notelist = new List<Note>();

            return new Stack<Note>(notelist);
        }
    }
    
    public class TimeLine
    {
        public long time;
        
        public TimeLine(long time)
        {
            this.time = time;
        }
    }
    
    public class Note : TimeLine
    {
        public Note(long time) : base(time) {}
    
        public class normal : Note
        {
            public enum Direction { Up, Down, Left, Right }

            public int index;
            public Direction direction;
            
            public normal(long time, Direction direction, int index) : base(time)
            {
                this.direction = direction;
                this.index = index;
            }

            public normal(string format) : base(long.Parse(format.Split(',')[1]))
            {
                string[] data = format.Split(',');
                
                this.direction = (Direction)int.Parse(data[2]);
                this.index = int.Parse(data[3]);
            }
        }
        
        public class laser : Note
        {
            public int index;
            public long duration;
            public bool isHorizontal;

            public laser(long time, bool isHorizontal, int index, long duration) : base(time)
            {
                this.isHorizontal = isHorizontal;
                this.index = index;
                this.duration = duration;
            }
            
            public laser(string format) : base(long.Parse(format.Split(',')[1]))
            {
                string[] data = format.Split(',');
                
                this.isHorizontal = bool.Parse(data[2]);
                this.index = int.Parse(data[3]);
                this.duration = long.Parse(data[4]);
            }
        }
    }
}