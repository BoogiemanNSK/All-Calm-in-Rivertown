using UnityEngine;

namespace Items {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 51)]
    public class Weapon : Item {

        [Header("Weapon Attributes")]
        [SerializeField] private int _damage;

        public int Damage => _damage;

        public override void Use() {
            throw new System.NotImplementedException();
        }

    }

}