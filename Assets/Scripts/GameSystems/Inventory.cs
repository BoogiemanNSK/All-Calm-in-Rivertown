using System.Collections.Generic;
using Items;

namespace GameSystems {

    public class Inventory {

        public List<Item> ItemsList { get; }
        
        public Inventory(IEnumerable<Item> initialList) {
            ItemsList = new List<Item>();

            foreach (var item in initialList) {
                ItemsList.Add(item);
            }
        }

    }

}