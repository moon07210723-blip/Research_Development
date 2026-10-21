using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour // 업적을 어치브
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;
    WaitForSecondsRealtime wait;
    enum Achive
    {
        UnlockPotato,
        UnlockBean
    }

    Achive[] achives;
    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData")) // 업적 클리어 정보 저장
        {
            Init();
        }
            
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }
    private void Start()
    {
        UnlockCharacter();
    }
    void UnlockCharacter()
    {
        for(int index =0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;  //업적 달성이면 트루
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }
    private void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }
    void CheckAchive (Achive achive)  // 업적완료 판단 함수
    {
        bool isAchive = false;
        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = OX_GameManager.Instance.QuizCorrectCount >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = OX_GameManager.Instance.QuizCorrectCount == OX_GameManager.Instance.AllQuizCount;
                break;
        }
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) 
        {
            PlayerPrefs.SetInt (achive.ToString(), 1);
            for(int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
                
        }
    }
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        yield return wait;
        uiNotice.SetActive(false);
    }
}
