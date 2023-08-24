using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    public class Blade : InventoryItem
    {
        public int damage { get; set; }
        public int maxRunes { get; set; }
        public int runes { get; set; }
        public Blade(int itemID, String name, String icon, String image, int maxDurability, int stackSize, int damage, int maxRunes)
        {
            this.itemID = itemID;
            this.name = name;
            this.maxDurability = maxDurability;
            durability = maxDurability;
            this.stackSize = stackSize;
            this.icon = icon;
            this.image = image;
            this.itemType = itemType;
            this.damage = damage;
            this.maxRunes = maxRunes;
        }
    }
}
