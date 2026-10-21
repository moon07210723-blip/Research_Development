using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goldmetal.UndeadSurvivor
{
    public class Item : MonoBehaviour
    {
        public ItemData data;
        public int level;
        public Weapon weapon;
        public Gear gear;

         Image icon;

        Text textLevel;
        Text textName;
        Text textDesc;

        void Awake()
        {

            
            icon = GetComponentsInChildren<Image>()[1];
            
            icon.sprite = data.itemIcon[0]; 



            icon = GetComponentsInChildren<Image>()[1];

            icon.sprite = data.itemIcon[0];


            Text[] texts = GetComponentsInChildren<Text>();
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
            textName.text = data.itemName;
        }

        void OnEnable()
        {
            textLevel.text = "Lv." + (level + 1);

            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                case ItemData.ItemType.Boom:
                    textDesc.text = string.Format(data.itemDesc, data.counts[level]);
                  
                    break;
                case ItemData.ItemType.Explosion:
                    textDesc.text = string.Format(data.itemDesc, data.counts[level], data.damages[level] * 100);
              
                    break;
                case ItemData.ItemType.Sword:
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                default:
                    textDesc.text = string.Format(data.itemDesc);
                    break;
            }
        }

        public void OnClick()
        {
            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;
                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];
                        weapon.LevelUp(nextDamage, nextCount);
                    }

                    level++;
                    break;
                case ItemData.ItemType.Range: //총기
                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;

                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];

                        weapon.LevelUp(nextDamage, nextCount);
                       
                    }

                    level++;
                    
                    if (level == 5) // 총기 레벨 5 되면 레벨업 ui의 엽총 아이콘을 샷건 아이콘으로 변경
                    {
                        icon.sprite = data.hand[1];
                    }
                    else if (level >= 6) // 총기 레벨 6 되면 플레이어 캐릭터의 엽총 스프라이트를 샷건 스프라이트로 업데이트
                    {
                        Player player = GameManager.instance.player;
                        Hand hand = player.hands[(int)data.itemType-1];
                        hand.spriter.sprite = data.hand[1];
                    }
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    if (level == 0)
                    {
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        float nextRate = data.damages[level];
                        gear.LevelUp(nextRate);
                    }
                    level++;
                    break;

                case ItemData.ItemType.Boom:
                    if (level == 0)
                    {
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else 
                    {
                        //float nextRate = data.damages[level];
                        gear.Init(data);
                        
                    }
                    level++;
                    break;

                case ItemData.ItemType.Explosion: //

                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        int nextCount = data.counts[level];

                        nextDamage += data.baseDamage * data.damages[level];
                        weapon.LevelUp(nextDamage, nextCount);
                    }

                    level++;
                    break;
                case ItemData.ItemType.Heal:
                    GameManager.instance.health = GameManager.instance.maxHealth;
                    break;
                case ItemData.ItemType.Sword:
                    Debug.Log("소드");
                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
//                        weapon.animator = data.projectile
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        
                        int nextCount = 0;
                        nextDamage += data.baseDamage * data.damages[level];
                        
                        weapon.LevelUp(nextDamage, nextCount);
                    }

                    level++;

                    break;
            }

            if (level == data.damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
