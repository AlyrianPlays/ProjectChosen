using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;

namespace Game1
{
    public class Sprite
    {
        public Texture2D visual { get; set; }
        public String visualPath { get; set; }
        public Texture2D origVisual { get; set; }
        public Vector2 position { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 gameScaleFactor { get; set; }
        public Vector2 velocity { get; set; }
        public string content { get; set; }
        public float aniTime { get; set; }
        public int numCows { get; set; }
        public List<String> inventory { get; set; }

        public Sprite()
        {

        }
        public Sprite(Vector2 position, Vector2 scale, Vector2 velocity, string content, float aniTime, Microsoft.Xna.Framework.Content.ContentManager contentManager, int numCows)
        {
            this.visualPath = content;
            this.visual = contentManager.Load<Texture2D>(content);
            this.origVisual = contentManager.Load<Texture2D>(content);
            this.numCows = numCows;
            this.gameScaleFactor = scale;
            this.scale = new Vector2((scale.X/32/(this.visual.Width/32))*numCows, 0);
            this.position = position;
            //this.scale = scale;
            this.velocity = velocity;
            this.aniTime = aniTime;
        }
    }
}
