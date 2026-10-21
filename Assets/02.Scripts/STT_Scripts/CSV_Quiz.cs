using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class CSV_Quiz : MonoBehaviour
{


    public string[] CSV_Name;
    public TextMeshPro[] TextMeshPros;
    public List<Dictionary<string, object>> data_Dialog;
    private string path;

    public string[] Words;
    private string[] csvRow; //CSV의 가로행


    // Start is called before the first frame update
    public void CsvQuizStart()
    {
        path = "CSV_Quiz/";

        Words = new string[3];
        csvRow = new string[3];

        csvRow[0] = "Word_1";
        csvRow[1] = "Word_2";
        csvRow[2] = "Word_3";

        CSV_Name = new string[2] { "CSV_Quiz_01", "CSV_Quiz_02" };

        int csvRandomSelect = Random.Range(0, CSV_Name.Length);

        data_Dialog = CSVReader.Read(path + CSV_Name[csvRandomSelect]);

   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            makeRandomWord();

        }
    }

    public void makeRandomWord()
    {
        int col = Random.Range(0, 10);
        for (int i = 0; i < Words.Length; i++)
        {
            Words[i] = (string)data_Dialog[col][csvRow[i]];
            TextMeshPros[i].text = Words[i];
        }
        wordShuffle();
    }

    public void wordShuffle() //선택문항 섞기
    {
        for (int i = 0; i < Words.Length; i++)
        {
            int ranInt = Random.Range(0, Words.Length);
            string temp = Words[i];
            if (i != ranInt)
            {
                Words[i] = Words[ranInt];
                Words[ranInt] = temp;
            }
        }
    }
}

