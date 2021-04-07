using System.Collections.Generic;
using Items;

namespace GameSystems {

    public class Inventory {

        public readonly string InventoryName;
        public List<Item> ItemsList { get; }
        
        public Inventory(string inventoryName, IEnumerable<Item> initialList) {
            InventoryName = inventoryName;
            ItemsList = new List<Item>(initialList);
        }

        public void TransferItem(Item item, Inventory newInventory) {
            ItemsList.Remove(item);
            newInventory.ItemsList.Add(item);
        }

    }

}