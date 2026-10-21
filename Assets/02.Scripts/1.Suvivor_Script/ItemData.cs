using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType {None, Melee, Range, Glove, Shoe, Heal, Boom, Explosion, Sword }

        [Header("# Main Info")]
        public ItemType itemType;
        public int itemId;
        public string itemName;
        [TextArea]
        public string itemDesc;
        public Sprite[] itemIcon;

        [Header("# Level Data")]
        public float baseDamage;
        public int baseCount;
        public float[] damages;
        public int[] counts;

        [Header("# Weapon")]
        public GameObject[] projectile; 
        public Sprite[] hand;
    }
}
