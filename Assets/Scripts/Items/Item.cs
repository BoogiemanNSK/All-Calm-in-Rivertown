using UnityEngine;

namespace Items {

    public abstract class Item : ScriptableObject {

        [Header("Item Attributes")]
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;
        
        [Header("Graphical Resources")]
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private GameObject _model;

        protected bool IsEquipped = false;
        
        public string Name => _name;
        public string Description => _description;
        public int Price => _price;
        public bool Equipped => IsEquipped;
        public Sprite Image => _iconSprite;
        public GameObject Model => _model;

        public abstract void Use();

    }

}