using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Level
    {
        public int height { get; set; }
        public int width { get; set; }
        public Dictionary<(int, int), Sprite> platforms { get; set; }
        public List<Character> npcs { get; set; }

        [JsonConstructor]
        public Level(int height, int width, Dictionary<(int, int), Sprite> platforms, List<Character> npcs)
        {
            this.height = height;
            this.width = width;
            this.platforms = platforms;
            this.npcs = npcs;
        }

        public Level()
        {
            height = 0;
            width = 0;
            platforms = new Dictionary<(int, int), Sprite>();
            npcs = new List<Character>();
        }

        public Level(String path)
        {
            Level store;
            try
            {
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    store = (Level)serializer.Deserialize(file, typeof(Level));
                    this.height = store.height;
                    this.width = store.width;
                    this.platforms = store.platforms;
                    this.npcs = store.npcs;
                }
            }
            catch (FileNotFoundException e)
            {
                store = null;
            }
        }

        class SerialLevel
        {
            public String saveHeight { set; get; }
            public String saveWidth { set; get; }
            public List<String> savePlatforms { set; get; }
            public List<String> saveNPCs { set; get; }

            [JsonConstructor]
            public SerialLevel(String saveHeight, String saveWidth, List<String> savePlatforms, List<String> saveNPCs)
            {
                this.saveHeight = saveHeight;
                this.saveWidth = saveWidth;
                this.savePlatforms = savePlatforms;
                this.saveNPCs = saveNPCs;
            }

            public SerialLevel(Level level)
            {
                saveHeight = level.height.ToString();
                saveWidth = level.width.ToString();
                savePlatforms = new List<String>();
                saveNPCs = new List<String>();
                String temp;
                foreach (KeyValuePair<(int, int), Sprite> entry in level.platforms)
                {
                    temp = entry.Key.Item1.ToString() + "," + entry.Key.Item2.ToString() + "," + entry.Value.visualPath + "," + entry.Value.gameScaleFactor.X;
                    savePlatforms.Add(temp);
                }
                foreach (Sprite entry in level.npcs)
                {
                    temp = entry.visualPath;
                    saveNPCs.Add(temp);
                }
            }
        }
        public static void SaveLevel(Level level, String path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                SerialLevel slevel = new SerialLevel(level);
                var serializer = new JsonSerializer();
                serializer.Serialize(file, slevel);
            }
        }
        public void LoadLevel(String path, Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            SerialLevel store;
            try
            {
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    store = (SerialLevel)serializer.Deserialize(file, typeof(SerialLevel));
                    this.height = Convert.ToInt32(store.saveHeight);
                    this.width = Convert.ToInt32(store.saveWidth);
                    String[] temp;
                    foreach(String s in store.savePlatforms)
                    {
                        temp = s.Split(',');
                        this.platforms.Add((Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1])), new Sprite(new Vector2(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1])), new Vector2(Convert.ToInt32(temp[3]), 0), new Vector2(0, 0), temp[2], 0, contentManager, 1));
                    }
                    foreach(String s in store.saveNPCs)
                    {
                        if (s != null)
                        {
                            this.npcs.Add(new Character(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), s, 0, null, null, null, null, new Vector2(0, 0), "", false, 1));
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                store = null;
            }
        }
    }
}
