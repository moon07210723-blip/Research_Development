using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
namespace Goldmetal.UndeadSurvivor
{

    public class CSV_Suvivor : MonoBehaviour
    {
        public Text csvTEXT;
        public Text sttText;
        public Text ResultText;

        private string path;
        private string csv;
        private string stt;
        public string sentence;

        public InputField ttsInputField;
        public GameObject sttUI;
        public GameManager gameManager;
        public LevelUp levelUP;

        public string[] CSV_Name;
        public List<Dictionary<string, object>> data_Dialog;

        private int firstSenteceCount;
        private int secondSenteceCount;
        private int thirdSenteceCount;

        public bool sentenceResult;
        public DOTweenAnimation csvTextTween;

        // Start is called before the first frame update

        void Start()
        {
            path = "CSV_Suvivor/";
            CSV_Name = new string[2] { "CSV_Weather", "CSV_Food" };
            int csvRandomSelect = Random.Range(0, 2);

            data_Dialog = CSVReader.Read(path + CSV_Name[csvRandomSelect]);
            SentenceCounter();
            //  RandomSentenceMaker();
            sentenceResult = false;
        }
        void SentenceCounter()
        {
            firstSenteceCount = (int)data_Dialog[0]["count"];
            secondSenteceCount = (int)data_Dialog[1]["count"];
            thirdSenteceCount = (int)data_Dialog[2]["count"];
        }

        public void RandomSentenceMaker()
        {
            //"";
            sentence = data_Dialog[Random.Range(0, firstSenteceCount)]["Sentence_0"] + " " +
                  data_Dialog[Random.Range(0, secondSenteceCount)]["Sentence_1"] + " " +
                  data_Dialog[Random.Range(0, thirdSenteceCount)]["Sentence_2"];
            // csvTEXT.text = sentence;


            csvTEXT.DOText(sentence, 1.5f);
        }


        public void checkResult()
        {
            sentenceResult = false;

            csv = csvTEXT.text;
            stt = sttText.text;

            csv = csv.Replace(" ", string.Empty);
            stt = stt.Replace(" ", string.Empty);

            if (csv == stt)
            {
                // ResultText.text = "정답이에요";
                sentenceResult = true;
                sttUI.SetActive(false);
                levelUP.Show();
                GameManager.instance.level++;
                GameManager.instance.exp = 0;
                GameManager.instance.playBgm(true);
                GameManager.instance.enemyPause(false);
            }
            else
            {
                //    ResultText.text = "틀렸어요";
                sttUI.SetActive(false);
                levelUP.Hide();
                GameManager.instance.exp = GameManager.instance.exp * 0.75f;
                GameManager.instance.playBgm(true);
                GameManager.instance.enemyPause(false);
            }
        }




        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RandomSentenceMaker();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                //ResultText.text = "정답이에요";
                sentenceResult = true;
                sttUI.SetActive(false);
                levelUP.Show();
                GameManager.instance.level++;
                GameManager.instance.exp = 0;
                GameManager.instance.playBgm(true);
                GameManager.instance.enemyPause(false);

            }


        }

    }
}
