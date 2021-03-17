using UnityEngine;

namespace Items {

    [CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable", order = 53)]
    public class Consumable : Item {

        public enum ConsumableType {

            Healing,
            HealthEnhance,
            DamageEnhance,
            ArmorEnhance

        }

        [Header("Consumable Attributes")]
        [SerializeField] private ConsumableType _type;
        [SerializeField] private int _enhancementValue;

        public ConsumableType Type => _type;
        public int EnhancementValue => _enhancementValue;

        public override void Use() {
            throw new System.NotImplementedException();
        }

    }

}