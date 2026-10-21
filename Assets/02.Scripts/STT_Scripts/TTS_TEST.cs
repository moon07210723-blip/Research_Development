using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTS_TEST : MonoBehaviour
{
    public Text readKoreanText;

    private string korea = "Ko";

    private string readUrl = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    IEnumerator playSpeak(string str)
    {
        WWW www = new WWW(str);
        yield return www;

        audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        audioSource.Play();
    }

    private string getString(string text, string stateName)
    {
        return text + "&tl=" + stateName + "-gb";
    }

    public void KoreaButton()
    {
        StartCoroutine(playSpeak(readUrl + getString(readKoreanText.text, korea)));
    }
}
