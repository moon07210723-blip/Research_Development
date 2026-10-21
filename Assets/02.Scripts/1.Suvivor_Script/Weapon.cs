using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Weapon : MonoBehaviour
    {
        public int id;
        public int prefabId;
        public float damage;
        public int count;
        public float speed;
        public float closeAttackDeley;
        public Transform closeWeaponOrigin;
        float timer;
        Player player;
        public Animator animator;
        public Animator effectAnimator;
        
       // public Explosion_Ctrl explosionCtrl;

        void Awake()
        {
            player = GameManager.instance.player;
        }


        void Update()
        {
            if (!GameManager.instance.isLive)
                return;

            switch (id)
            {
                case 0:
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                    break;
                case 1:
                    timer += Time.deltaTime;

                    if (timer > speed)
                    {
                        timer = 0f;
                        Fire();
                    }
                    break;
                case 7:

                    timer += Time.deltaTime;

                    if (player.inputVec.x > 0)
                    {
                        closeWeaponOrigin.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (player.inputVec.x < 0)
                    {
                        closeWeaponOrigin.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    if (timer > closeAttackDeley)
                    {
                        timer = 0f;
                        animator.SetTrigger("SwordAttack");
                        effectAnimator.SetTrigger("Sword_Slash");
                    }
                    break;
                default:
                    break;
            }

            // .. Test Code..
            if (Input.GetButtonDown("Jump"))
            {
                LevelUp(10, 1);
            }
        }

        public void LevelUp(float damage, int count)
        {
            this.damage = damage * Character.Damage;
        

            if (id == 0)
            {
                this.count += count;
                Batch();
            }
            else if (id == 6) // 폭발 아이템을 레벨업을 했을 때
            {
                this.count = count;
                Explosion_Ctrl explosion_Ctrl = gameObject.GetComponent<Explosion_Ctrl>();
                explosion_Ctrl.addExploser(1);
                explosion_Ctrl.SetExplosionDamage(damage);
            }
            else if (id == 7) //Sword 아이템을 레벨업을 했을 때
            {
                this.count += count;
                closeAttackDeley -= 0.25f;
                Bullet swordBullet = effectAnimator.transform.GetChild(0).GetComponent<Bullet>();
                swordBullet.damage = damage;
            }

            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        public void Init(ItemData data) // UI에서 스킬아이콘을 터치 클릭하면 실행되는 초기화 함수
        {
            // Basic Set
            name = "Weapon " + data.itemId;
            transform.parent = player.transform;
            transform.localPosition = Vector3.zero;
            
            // Property Set
            id = data.itemId;
            damage = data.baseDamage * Character.Damage;
            count = data.baseCount + Character.Count;

            for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
            {
                if (data.projectile[0] == GameManager.instance.pool.prefabs[index])
                {
                    prefabId = index;
                    break;
                }
            }

            switch (id)
            {
                case 0:
                    speed = 150 * Character.WeaponSpeed;
                    Batch();
                    break;

                case 6:
                    gameObject.AddComponent<Explosion_Ctrl>().InitExplosion();
                    Rigidbody2D rigid = gameObject.AddComponent<Rigidbody2D>();
                    rigid.gravityScale = 0;
                    break;
                case 7:
                    GameManager.instance.pool.prefabs[3].gameObject.SetActive(true);
                    GameObject Sword = GameManager.instance.pool.prefabs[3];
                    animator = Sword.GetComponent<Animator>();
                    effectAnimator = GameObject.FindWithTag("Effect").gameObject.GetComponent<Animator>();
                    Bullet swordBullet = effectAnimator.transform.GetChild(0).GetComponent<Bullet>();
                    swordBullet.damage = damage;

                    // transform.parent = effectAnimator.transform.GetChild(0).transform;
                    closeWeaponOrigin = Sword.transform.parent.GetComponent<Transform>();
                    closeAttackDeley = 1.5f;
                    break;

                default:
                    speed = 0.5f * Character.WeaponRate;
                    break;
            }

            // Hand Set

            if ((int)data.itemType <= 2)
            {
                Hand hand = player.hands[(int)data.itemType - 1];
                hand.spriter.sprite = data.hand[0]; // 해당 구문에서 hand스프라이트 연결 후 변경 시켜줌
                hand.gameObject.SetActive(true);
            }

            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }


        void Batch()
        {
            for (int index = 0; index < count; index++)
            {
                Transform bullet;

                if (index < transform.childCount)
                {
                    bullet = transform.GetChild(index);
                }
                else
                {
                    bullet = GameManager.instance.pool.Get(prefabId).transform;
                    bullet.parent = transform;
                }

                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.
            }
        }

        void Fire()
        {

            if (!player.scanner.nearestTarget)
                return;

            if (id != 7) // 검이 아닐경우에만 실행
            {
                Vector3 targetPos = player.scanner.nearestTarget.position;
                Vector3 dir = targetPos - transform.position;
                dir = dir.normalized;

                Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.position = transform.position;
                bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                bullet.GetComponent<Bullet>().Init(damage, count, dir);

                AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
            }

        }
    }
}
