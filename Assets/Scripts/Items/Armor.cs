using UnityEngine;

namespace Items {

    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 52)]
    public class Armor : Item {

        [Header("Armor Attributes")]
        [SerializeField] private int _armorPoints;

        public int ArmorPoints => _armorPoints;

        public override void Use() {
            throw new System.NotImplementedException();
        }

    }

}