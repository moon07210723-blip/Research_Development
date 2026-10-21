using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Goldmetal.UndeadSurvivor;
using DG.Tweening;

public class OX_GameManager : MonoBehaviour
{
    public static OX_GameManager Instance;
    public QuizAudioManager oxAudioManager;

    [Header("Player")]
    public OX_Quiz_Player player;
    public int playerId;

    [Header("Quiz")]
    public int QuizCorrectCount = 0;
    public int AllQuizCount = 20;
   
    
    [Header("플레이어가 맞힌 횟수")]
    public int CheckCorrectaAnswer = 0;

    [Header("현재 플레이어가 선택한 정답")]
    public int currentPlayerQuizAnswer = 0;

    [Header("퀴즈 정답 0은 o, 1은 X")]
    public int corretQuizNum = 2; // 정답 랜덤 위치 0.true, 1.false
    [Header("남은 문제 수")]
    public int remaningQuizNum = 5; //문항수


    public GameObject EndGame;
    public TextMeshProUGUI corretAnswerText;
    public TextMeshProUGUI wrongAnswerText;

    public bool quizCorrectCheck;
    public bool quizPlaying;
    private bool Pause;
    public float nextQuizTimer;
    public int CountRound;

    //GameObject Retry;
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
    public TextMeshPro quiz_Text;
    public TextMeshPro text_timer;
    public TextMeshPro quiz_board;

    public float LimitTime;
    private float DelTime;
    // public Text text_timer;

    [Header("Ground")]
    public GameObject QuizArea;

    WaitForSecondsRealtime wait;
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        wait = new WaitForSecondsRealtime(3);
        quizPlaying = false;
        quizCorrectCheck = false;
        nextQuizTimer = 2.5f;
    }

    public void GameStart(int id)
    {
        playerId = id;
        player.gameObject.SetActive(true);
        sceneButton.SetActive(false);
        pasueButton.SetActive(true);

        UiJoy.localScale = Vector3.one;
        text_timer.text = "게임을 " + "\n" + "시작합니다!";
        StartCoroutine(TimeDelay(3f));

    }




    IEnumerator TimeDelay(float x) //0.5�� ����� StartCoroutine(??());
    {
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

            QuizGameUIText(remaningQuizNum);
            LimitTime -= Time.deltaTime;
            text_timer.text = Mathf.Round(LimitTime).ToString();

            if (LimitTime <= 0)
            {
                quizPlaying = false;
            }
        }
        else if (quizPlaying == false && LimitTime <= 0)
        {
            UiJoy.localScale = Vector3.zero;
            QuizCorrect();
            nextQuizTimer -= Time.deltaTime;


            if (nextQuizTimer <= 0)
            {
                remaningQuizNum--;
                if (remaningQuizNum <= 0)
                {
                    remaningQuizNum = 0;
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
            }

        }
    }




    void QuizGameStart()
    {
        LimitTime = 10;
        text_timer.gameObject.SetActive(true); // 타이머
        QuizArea.gameObject.SetActive(true);
        quizCorrectCheck = false;
        corretAnswerUI.SetActive(false);
        wrongAnsWerUI.SetActive(false);
    }
    void QuizGameEnd()
    {

        corretAnswerText.text = "맞춘 문항 :" + CheckCorrectaAnswer;
        wrongAnswerText.text = "틀린 문항 : " + (5 - CheckCorrectaAnswer).ToString();


        EndGame.gameObject.SetActive(true);
        pasueButton.SetActive(false);

    }
    public void LoadScene(int sceneNum)
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
                if (corretQuizNum - currentPlayerQuizAnswer == 0)
                {
                    text_timer.gameObject.SetActive(false); ; // 타이머
                    player.speed = 0;
                    corretAnswerUI.SetActive(true);
                    CheckCorrectaAnswer++;
                    oxAudioManager.AnswerSoundPlay(true);
                }
                else
                {
                    text_timer.gameObject.SetActive(false); ; // 타이머
                    player.speed = 0;
                    wrongAnsWerUI.SetActive(true);
                    oxAudioManager.AnswerSoundPlay(false);
                }
            }
            quizCorrectCheck = true;
        }
    }
    void QuizRandomNumber()
    {
        int isSoundEnabled = Random.Range(0, 2); // 0 : 소리가 안들렸나요?, 1 : 소리가 들렸나요?
        int isSoundMute = Random.Range(0, 5); // 0 : 음소거, 1 : 볼륨 100

        if (isSoundEnabled == 0) // 소리가 안들렸나요?
        {
            quiz_Text.text = "소리가 안들렸나요?";

            if (isSoundMute == 0) // 음소거
            {
                corretQuizNum = 0;
            }
            else if (isSoundMute != 0) // 볼륨 100
            {
                oxAudioManager.playSoundEffect();
                corretQuizNum = 1;
            }
        }
        else if (isSoundEnabled == 1)// 소리가 들렸나요?
        {
            quiz_Text.text = "소리가 들렸나요?";
            if (isSoundMute == 0) // 음소거
            {
                corretQuizNum = 1;
            }
            else if (isSoundMute != 0) // 볼륨 100
            {
                oxAudioManager.playSoundEffect();
                corretQuizNum = 0;
            }
        }
        //corretQuizNum = Random.Range(0, 2); //( OX 자리에 들어가게  값을 줌)
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
    public void QuizGameUIText(int index)
    {
        quiz_board.text = "남은 문제 갯수:" + index.ToString(); 
    }
}
