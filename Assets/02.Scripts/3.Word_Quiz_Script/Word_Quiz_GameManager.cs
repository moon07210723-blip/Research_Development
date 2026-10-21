using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using DG.Tweening;

public class Word_Quiz_GameManager : MonoBehaviour  //삼지선다 퀴즈 게임 매니저
{
    public static Word_Quiz_GameManager Instance;
    public STTS_Ctrl STTmanager;
    public CSV_Quiz CSVQuizManager;
    public QuizAudioManager QuizAudioManager;

    [Header("Player")]
    public Word_Quiz_Player player;
    public int playerId;

    [HideInInspector]
    public int QuizCorrectCount; //업적해제용 카운트
    public int AllQuizCount = 20;
    public Text QuizChooseText;

    [Header("플레이어가 정답을 맞힌 횟수")]
    public int countCorrectAnswer = 0; //플레이어가 정답을 맞힌 횟수
    [Header("플레이어가 현재 선택한 정답 번호")]
    public int currentAnswerNumber = 0;
    [Header("퀴즈 정답 번호")]
    public int corretAnswerNumber = 2; // 퀴즈 정답 변수 값은 랜덤하게 변경 됨 0.true, 1.false
    [Header("퀴즈 문항 수")]
    public int QuestionCount = 5; //문항수

    public bool quizCorrectCheck; // 타이머가 0이되면 정답을 체크하기 위한 BOOL 변수
    public bool quizPlaying; // 현재 타이머가 동작중인지 파악하기 위한 BOOL 변수
    private bool Pause;
    public float nextQuizTimer; // 정답 체크 후 다음 문제 출력까지 걸리는 시간


    [Header("퀴즈 UI")]
    public TextMeshPro QuizRemainingText; // 남은 문항수 표시 텍스트
    public TextMeshPro restQuizText;
    public TextMeshPro text_timer;

    [Header("게임 결과창")]
    public GameObject gameReusltUI;

    public TextMeshProUGUI Correct_Answer_Text;
    public TextMeshProUGUI Wrong_Answer_Text;


    public GameObject corretAnswerUI;
    public GameObject wrongAnsWerUI;
    public GameObject CharactorGroup;
    public GameObject Title;
    public Transform UiJoy;

    public GameObject sceneButton;

    [Header("일시정지 UI")]
    public GameObject pasueButton;
    public GameObject pausePanel;
    public DOTweenAnimation pauseDOTween;
    public DOTweenAnimation PauseBtnDOTween;

    [Header("Time")]
    public float LimitTime;


    [Header("Ground")]
    public GameObject[] QuizArea;

    private void Awake()
    {
        Pause = false;
        Instance = this;
        Application.targetFrameRate = 60;
        
        quizPlaying = false;
        quizCorrectCheck = false;
        nextQuizTimer = 2.5f;
        CSVQuizManager.CsvQuizStart();
        
    }

    public void GameStart(int id)
    {
        playerId = id;
        restQuizText.text = "들린 낱말을" + "\n" + "선택하세요 ";
        StartCoroutine(TimeDelay(2f));
    }

    IEnumerator TimeDelay(float x) //0.5초 딜레이 StartCoroutine(??());QuestionCount
    {
        sceneButton.SetActive(false);
        pasueButton.SetActive(true);
        player.gameObject.SetActive(true);
        UiJoy.localScale = Vector3.one;
        yield return new WaitForSeconds(x);
        QuizRandomNumber();
        quizPlaying = true;
        nextQuizTimer = 2.5f;
        QuizGameStart();
    }

    void Update()
    {
        if (quizPlaying == true)
        {
            QuizGameUIText(QuestionCount);

            LimitTime -= Time.deltaTime;
            text_timer.text = " 남은 시간: " + Mathf.Round(LimitTime);

            if (LimitTime <= 0)
            {
                quizPlaying = false;
            }
        }
        else if (quizPlaying == false && LimitTime <= 0)
        {
            UiJoy.localScale = Vector3.zero;
            QuizCorrect();
            AreaColorChage();
            nextQuizTimer -= Time.deltaTime;


            if (nextQuizTimer <= 0)
            {
                QuestionCount--;
                if (QuestionCount <= 0)
                {

                    QuestionCount = 0;
                    corretAnswerUI.SetActive(false);
                    wrongAnsWerUI.SetActive(false);
                    QuizGameEnd();
                }
                else
                {
                    quizPlaying = true;
                    QuizGameStart();
                    QuizRandomNumber();
                    nextQuizTimer = 2.5f;
                    player.speed = 5f;
                    UiJoy.localScale = Vector3.one;
                }
                AreaColorChage();
            }
        }
    }

    void QuizGameStart()
    {
        LimitTime = 20;
        text_timer.gameObject.SetActive(true); // 타이머
        quizCorrectCheck = false;
        corretAnswerUI.SetActive(false);
        wrongAnsWerUI.SetActive(false);
    }
    void QuizGameEnd()
    {
        
        gameReusltUI.SetActive(true);
        gameReusltUI.transform.localScale = Vector3.one;

        Correct_Answer_Text.text = "맞춘 문항 :" + countCorrectAnswer;
        Wrong_Answer_Text.text = "틀린 문항 : " + (5 - countCorrectAnswer).ToString();

        pasueButton.gameObject.SetActive(false);
    }


    public void LoadScnen(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    void QuizCorrect()
    {
        if (quizCorrectCheck == false)
        {
            if (Mathf.Round(LimitTime) <= 0)
            {
                if (corretAnswerNumber - currentAnswerNumber == 0) //정답
                {
                    text_timer.gameObject.SetActive(false); ; // 타이머
                    player.speed = 0;
                    corretAnswerUI.SetActive(true);
                    countCorrectAnswer++;
                    QuizAudioManager.AnswerSoundPlay(true);
                }
                else // 오답
                {
                    text_timer.gameObject.SetActive(false); ; // 타이머
                    player.speed = 0;
                    wrongAnsWerUI.SetActive(true);
                    QuizAudioManager.AnswerSoundPlay(false);
                }
            }
            quizCorrectCheck = true;
        }
    }

    public void gamePause()
    {
        if (Pause == false)
        {
            Pause = true;
            Time.timeScale = 0;
            UiJoy.localScale = Vector3.zero;

            pauseDOTween.DOPlayById("Pause_Panel_FadeIn");
            PauseBtnDOTween.DOPlayById("Pause_Panel_PopUP");
        }
        else if (Pause == true)
        {
            Pause = false;
            Time.timeScale = 1;
            UiJoy.localScale = Vector3.one;

            pauseDOTween.DOPlayById("Pause_Panel_FadeOut");
            PauseBtnDOTween.DOPlayById("Pause_Panel_Close");
        }
    }
    public void SetPausePanel()
    {
        if (pausePanel.activeSelf == false)
        {
            pausePanel.SetActive(true);
        }
        else if (pausePanel.activeSelf == true)
        {
            pausePanel.SetActive(false);
        }
    }

    void AreaColorChage()
    {
        SpriteRenderer spriteRenderer;
        //
        if (quizPlaying == false)// 정답 체크일때
        {
            for (int i = 0; i < 3; i++)
            {
                if (corretAnswerNumber != i)
                {
                    spriteRenderer = QuizArea[i].GetComponent<SpriteRenderer>();
                    spriteRenderer.color = new Color32(219, 80, 76, 255);
                }
                else if (corretAnswerNumber == i)
                {
                    spriteRenderer = QuizArea[i].GetComponent<SpriteRenderer>();
                    spriteRenderer.color = new Color32(46, 177, 226, 255);
                }
            }
        }
        else if (quizPlaying == true) // 퀴즈풀때
        {
            for (int i = 0; i < 3; i++)
            {
                spriteRenderer = QuizArea[i].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color32(37, 37, 40, 255);
            }
        }
    }
    void QuizRandomNumber()
    {
        corretAnswerNumber = Random.Range(0, 3); //0,1,2 중 랜덤하게 1개 선택
        if (CSVQuizManager.TextMeshPros[corretAnswerNumber].text != null)
        {
            CSVQuizManager.makeRandomWord();
            STTmanager.TTSEvent(CSVQuizManager.TextMeshPros[corretAnswerNumber].text);
        
        }
        else if (CSVQuizManager.TextMeshPros[corretAnswerNumber].text == null)
        {
            CSVQuizManager.makeRandomWord();
            STTmanager.TTSEvent(CSVQuizManager.TextMeshPros[corretAnswerNumber].text);
        }
    }

    public void QuizGameUIText(int index)
    {
        QuizRemainingText.text = "남은 문제: " + index;
    }
}
