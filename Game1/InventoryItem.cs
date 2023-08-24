using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    public class InventoryItem
    {
        public int itemID { get; set; }
        public String name { get; set; }
        public int maxDurability { get; set; }
        public int durability { get; set; }
        public int stackSize { get; set; }
        public String icon { get; set; }
        public String image { get; set; }
        public String itemType { get; set; }
        public double timeLimit { get; set; }
        public int maxRunes { get; set; }
        public int runes { get; set; }
        public Modifier modifier { get; set; }
        public InventoryItem()
        {

        }

        public InventoryItem(int itemID, String name, String icon, String image, int maxDurability, int stackSize, String itemType, int maxRunes, int runes, Modifier modifier)
        {
            this.itemID = itemID;
            this.name = name;
            this.maxDurability = maxDurability;
            durability = maxDurability;
            this.stackSize = stackSize;
            this.icon = icon;
            this.image = image;
            this.itemType = itemType;
            this.maxRunes = maxRunes;
            this.runes = runes;
            this.modifier = modifier;
        }
    }
}
