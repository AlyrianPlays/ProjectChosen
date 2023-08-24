using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    class Rune : InventoryItem
    {

        public Rune(String name, String icon, String image, int maxDurability, int stackSize, int damage, int maxRunes)
        {
            this.itemID = itemID;
            this.name = name;
            this.maxDurability = maxDurability;
            durability = maxDurability;
            this.stackSize = stackSize;
            this.icon = icon;
            this.image = image;
            this.itemType = itemType;
            
        }
    }
}
