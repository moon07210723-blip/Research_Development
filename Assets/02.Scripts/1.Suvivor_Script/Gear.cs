using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goldmetal.UndeadSurvivor.ItemData;

namespace Goldmetal.UndeadSurvivor
{
    public class Gear : MonoBehaviour
    {
        public ItemData.ItemType type;
        public float rate;
        private int countBomb=0;
        public void Init(ItemData data)
        {
            // Basic Set
            name = "Gear " + data.itemId;
            transform.parent = GameManager.instance.player.transform;
            transform.localPosition = Vector3.zero;

            // Property Set
            type = data.itemType;
            rate = data.damages[0];
            ApplyGear();
        }

        public void LevelUp(float rate)
        {
            this.rate = rate;
 
            ApplyGear();
            
        }

        public void ApplyGear()
        {
            /*
             * 1. 레벨업 후 아이템 선택창에서 각 기어1,2,3 을 1번 이상 선택
             * 2. 이 후 다시 레벨업 후 아이템 선택창에서 무기1,2를 1번이상 클릭시 기어들이 모두 실행됨
             * 해당 함수(ApplyGear)에서 switch문을 사용하였으나 case 1,2,3이 전부 실행됨 그러나 기어의 레벨이 올라가는 것은 아님
             * 해당 기어 데이터들을 위에 Init()함수에서 보관중이므로 2중으로 해당 기어 레벨이 증가하는 것은아님
             * 폭탄 같은 경우는 레벨업 방식이 아닌 발동형 Gear여서 해당문제를 발생시키는 것으로 보임
             */
      
            switch (type) {
                case ItemData.ItemType.Glove:
                    Debug.Log("ApplyGear__GLove");
                    RateUp();
                    break;
                case ItemData.ItemType.Shoe:
                    Debug.Log("ApplyGear__Shoe");
                    SpeedUp();
                    break;
                case ItemData.ItemType.Boom:
                  
                    Debug.Log(countBomb);
                    if (countBomb < 2)
                    {
                        Debug.Log("ApplyGear__Boom");
                        type = ItemType.Boom;
                        StartCoroutine(Boom());
                    }
                    countBomb++;
                    break;
            }
        }

        void RateUp()
        {
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

            foreach (Weapon weapon in weapons) {
                switch (weapon.id) {
                    case 0:
                        float speed = 150 * Character.WeaponSpeed;
                        weapon.speed = speed + (speed * rate);
                        break;
                    default:
                        speed = 0.5f * Character.WeaponRate;
                        weapon.speed = speed * (1f - rate);
                        break;
                }
            }
        }

        void SpeedUp()
        {
            float speed = 3 * Character.Speed;
            GameManager.instance.player.speed = speed + speed * rate;
        }
        
        IEnumerator Boom()
        {
            GameManager.instance.enemyCleaner.SetActive(true);
 
            yield return new WaitForSeconds(0.5f);

            GameManager.instance.enemyCleaner.SetActive(false);
            type = ItemData.ItemType.None;
        }
    }
}