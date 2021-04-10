using Info;
using Items;
using UnityEngine;

namespace GameSystems {
    public class Character : MonoBehaviour {
        
        [SerializeField] private string _name;
        [SerializeField] private Item[] _initialItems;
        [SerializeField] private CharInfo _characterData;
        
        public string Name => _name;
        public CharInfo CharacterData => _characterData;
        public Inventory Inventory { get; private set; }

        private void Awake() {
            Inventory = new Inventory(_name, _initialItems);
        }

    }
}