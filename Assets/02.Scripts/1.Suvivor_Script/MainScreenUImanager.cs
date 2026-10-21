using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class MainScreenUImanager : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollrect;
    public Scrollbar scrollbar;
    public DOTweenAnimation dotAnim;

    public GameObject selectGameObject;
    public GameObject leftArrow;
    public GameObject rightArrow;

    public Transform centerIcon;
    public Transform[] pos;
    public Transform movePos;

    public Button startButton;

    public bool moveIcon;
    public float speed;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        GameSceneManager.sceneNum = 1;
        moveIcon = false;
        scrollbar = scrollrect.horizontalScrollbar;
    }

    void FixedUpdate()
    {
        if (moveIcon == true)
        {
            centerIcon.position = Vector3.Lerp(centerIcon.position, movePos.position, speed * Time.deltaTime);
            distance = Vector3.Distance(centerIcon.position, movePos.position);
            if (distance <= 0.1f)
            {
                centerIcon.position = movePos.position;
                moveIcon = false;
                movePos = null;
            }
        }
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        moveIcon = false;
        movePos = null;
        selectGameObject=null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //0번 씬은 메인메뉴
        //1번은 ox 퀴즈
        //2번은 삼지선다 퀴즈
        //3번은 서바이벌 게임
        if (moveIcon == false)
        {
            if (scrollbar.value <= 0.25f)
            {
                movePos = pos[0];
                selectGameObject = pos[0].gameObject;
                GameSceneManager.sceneNum = 0 + 1;
                moveIcon = true;
            }
            else if (scrollbar.value >= 0.25f && scrollbar.value <= 0.75f)
            {
                movePos = pos[1];
                selectGameObject = pos[1].gameObject;
                GameSceneManager.sceneNum = 1 + 1;
                moveIcon = true;
            }
            else if (scrollbar.value > 0.75f)
            {
                movePos = pos[2];
                selectGameObject = pos[2].gameObject;
                GameSceneManager.sceneNum = 2 + 1;
                moveIcon = true;
            }
            // Debug.Log("스크롤 값 : " + scrollbar.value + "  현재 타겟 포지션 :" + movePos);
        }
        ActiveArrow();
    }

    public void ActiveArrow()
    {

        if (selectGameObject == pos[0].gameObject)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
        else if (selectGameObject == pos[1].gameObject)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }
        else if (selectGameObject == pos[2].gameObject)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
    }

    public void ActiveButton()
    {
        startButton.enabled = true;
    }

    public void endAnim()
    {
        dotAnim.DORewind();
    }


}
