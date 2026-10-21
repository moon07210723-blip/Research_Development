using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Goldmetal.UndeadSurvivor
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [Header("# Game Control")]
        public bool isLive;
        public float gameTime;
        public float maxGameTime = 2 * 10f;
        [Header("# Player Info")]
        public int playerId;
        public float health;
        public float maxHealth = 100;
        public int level;
        public int kill;
        public float exp;
        public float[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

        [Header("# 월드 스피드")]
        public float worldSpeed;

        [Header("# Game Object")]
        public PoolManager pool;
        public AudioManager audioManager;
        //public Scanner scanner;
        public Player player;
        public LevelUp uiLevelUp;
        public Result uiResult;
        public Transform uiJoy;
        public GameObject enemyCleaner;
        public CSV_Suvivor csvSuvManager;

        [Header("# 일시정지 UI")]
        public GameObject pauseButton;
        public GameObject pausePanel;
        public DOTweenAnimation pauseDOTween;
        public DOTweenAnimation PauseBtnDOTween;
        
        [Header("# STT UI")]
        public GameObject sttUI;
        public DOTweenAnimation sttPanelTween;
        void Awake()
        {
            instance = this;
            Application.targetFrameRate = 60;
        }

        public void GameStart(int id)
        {
            playerId = id;
            health = maxHealth;

            player.gameObject.SetActive(true);
            pauseButton.SetActive(true);
            uiLevelUp.Select(playerId % 2);
            Resume();

            AudioManager.instance.PlayBgm(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        }

        public void GameOver()
        {
            Debug.Log("GAMEOVER");
            StartCoroutine(GameOverRoutine());
        }
        IEnumerator GameVictroyRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }
        IEnumerator GameOverRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();

            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        }

        public void GameVictroy()
        {
            StartCoroutine(GameVictroyRoutine());
        }

        

        public void GameRetry()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(3);
        }

        public void GameQuit()
        {
            Application.Quit();
        }

        void Update()
        {
            if (!isLive)
                return;

            gameTime += Time.deltaTime * GameManager.instance.worldSpeed;

            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictroy();
            }

        }

        public void GetExp()
        {
            if (!isLive)
                return;

            exp++;

            //������
            if (exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
            {

                float saveExp = exp;


                //exp = 0;
                playBgm(false);
                //Stop();
                enemyPause(true);
              
                sttUI.SetActive(true);
                sttPanelTween.DOPlay();
            }
        }
        public void playBgm(bool isPlay)
        {
            audioManager.PlayBgm(isPlay);
        }
        public void showLevelUpUI()
        {
            Debug.Log("sss");
            uiLevelUp.Show();
        }


        public void SetPause()
        {
            if (isLive == true)
            {
             //   SetPausePanel();
                pauseDOTween.DOPlayById("Pause_Panel_FadeIn");
                PauseBtnDOTween.DOPlayById("Pause_Panel_PopUP");
              
                Stop();
            }
            else if (isLive == false)
            {
                pauseDOTween.DOPlayById("Pause_Panel_FadeOut");
                PauseBtnDOTween.DOPlayById("Pause_Panel_Close");
                Resume();
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

        public void enemyPause(bool enemyStop)
        {
            if (enemyStop == true)
            {
                isLive = false;
                worldSpeed = 0;
                uiJoy.localScale = Vector3.zero;
                pool.enemyAnimPause(true);
            }
            else if (enemyStop == false)
            {
                isLive = true;
                worldSpeed = 1;
                pool.enemyAnimPause(false);
                
            }
        }
 

        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;
            uiJoy.localScale = Vector3.zero;
        }

        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;
            uiJoy.localScale = Vector3.one;
        }

        public void LoadScnen(int sceneNum)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneNum);
        }

    }
}
