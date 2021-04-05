using Items;
using UnityEngine;

namespace GameSystems {

    public class Player : MonoBehaviour {

        public Inventory Inventory { get; private set; }

        [SerializeField] private Item[] InitialItems;

        private void Awake() {
            Inventory = new Inventory(InitialItems);
        }

    }

}