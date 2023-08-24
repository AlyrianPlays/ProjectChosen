using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Game1
{
    public class SaveState
    {
        public String positionX { get; set; }
        public String positionY { get; set; }
        public List<String> inventory { get; set; }

        public SaveState(Vector2 position)
        {
            positionX = position.X.ToString();
            positionY = position.Y.ToString();
            inventory.Add("none");
        }
        [JsonConstructor]
        public SaveState(Vector2 position, List<String> inventory)
        {
            positionX = position.X.ToString();
            positionY = position.Y.ToString();
            this.inventory = inventory;
        }
    }
}
