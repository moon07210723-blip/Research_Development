using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject[] prefabs;

        public List<GameObject>[] pools;
        private int countNum;
        void Awake()
        {
            countNum = 0;
            pools = new List<GameObject>[prefabs.Length];

            for (int index = 0; index < pools.Length; index++)
            {
                pools[index] = new List<GameObject>();
            }
        }

        public GameObject Get(int index)
        {
            GameObject select = null;

            foreach (GameObject item in pools[index])
            {
                if (!item.activeSelf)
                {
                    select = item;

                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(prefabs[index], transform);

                select.name = select.name + "_" + (countNum - 3);
                countNum++;
                pools[index].Add(select);
            }

            return select;
        }
        public void enemyAnimPause(bool setBool)
        {
            Enemy animator;
            Rigidbody2D rb2d;
            foreach (GameObject item in pools[0])
            {
                animator = item.GetComponent<Enemy>();
                rb2d = item.GetComponent<Rigidbody2D>();
                rb2d.velocity = Vector3.zero;
                animator.stopAnim(setBool);
                
            }
         


        }
    }
}
