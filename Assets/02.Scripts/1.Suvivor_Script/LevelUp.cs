using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Goldmetal.UndeadSurvivor
{
    public class LevelUp : MonoBehaviour
    {
        //RectTransform rect;
        DOTweenAnimation dOTween;
        DOTweenAnimation childDOTween;
        public GameObject childPanel;
        public Item[] items;

        void Awake()
        {
         //   rect = GetComponent<RectTransform>();
            items = GetComponentsInChildren<Item>(true);
            dOTween = GetComponent<DOTweenAnimation>();
            childDOTween = childPanel.GetComponent<DOTweenAnimation>();
        }

        public void Show()
        {
            Next();
            dOTween.DOPlayById("FadeIn");
            dOTween.DORewindAllById("FadeOut");
            childDOTween.DOPlayById("LevelUP_Panel_PopUP");
            childPanel.SetActive(true);
            //rect.localScale = Vector3.one;
            GameManager.instance.Stop();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.instance.EffectBgm(true);

        }

        public void Hide()
        {
            //
            childPanel.transform.localScale = Vector3.zero;
            dOTween.DOPlayById("FadeOut");
            dOTween.DORewindAllById("FadeIn");
            childDOTween.DORewindAllById("LevelUP_Panel_PopUP");
            childPanel.SetActive(false);
            GameManager.instance.Resume();
            GameManager.instance.enemyPause(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.EffectBgm(false);
            GameManager.instance.uiJoy.transform.localScale = Vector3.one;
            GameManager.instance.csvSuvManager.csvTEXT.text = "";
            GameManager.instance.csvSuvManager.sttText.text = ""; 
        }

        public void Select(int index)
        {
            Debug.Log(items.Length);
            items[index].OnClick();
        }

        void Next()
        {
            // 1. 모든 아이템 비활성화
            foreach (Item item in items)
            {
                item.gameObject.SetActive(false);
            }

            // 2. 그 중에서 랜덤 3개 아이템 활성화
            /*int[] ran = new int[3];
            while (true) {
                ran[0] = Random.Range(0, items.Length);
                ran[1] = Random.Range(0, items.Length);
                ran[2] = Random.Range(0, items.Length);

                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                    break;
            }*/

            for (int index = 0; index < items.Length; index++)
            {
                Item showItem = items[index];

                if (showItem.level < showItem.data.damages.Length)
                {

                    showItem.gameObject.SetActive(true);
                }
                else if (showItem.level == showItem.data.damages.Length)//// 3. 만렙 아이템의 경우는 소비아이템으로 대체
                {
                    items[4].gameObject.SetActive(true);
                }
            
            }
        }
    }
}
