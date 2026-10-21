using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using STTSCore.Engine;
using STTSCore.Utility;
using Goldmetal.UndeadSurvivor;
public class STTS_Ctrl : MonoBehaviour, STTSCallback
{

    public CSV_Suvivor csvManager;
    public Text sttResultText;

    public Text ttsInputField;

    public Slider speedSlide;
    public Slider pitchSlide;

    private STTS stts;
    //private Mic_Ctrl micCtrl;
    // Start is called before the first frame update
    void Start()
    {
        //#if PLATFORM_ANDROID
#if PLATFORM_UNITY_WEBGL
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        TTSEvent("");
#endif
        stts = STTSFactory.GetInstance();

        if (stts != null)
        {
            stts.init(STTSLangTypes.KO, gameObject.name);

            // for ios
            STTS.onSTTEvent += STTResultCallback;
            STTS.onErrorEvent += ErrorCallback;
        }
    }

    // STT Event
    public void STTEvent()
    {
        if (stts != null)
        {
            stts.StartSpeechToText();
            //micCtrl.startMic(true) ;
        }
            
       
    }

    public void STTStopEvent()
    {
        if (stts != null)
        {
            stts.StopSpeechToText();
           // micCtrl.startMic(false);
        }
            

    }

    // TTS Event
    public void TTSEvent(string sentence)
    {
        if (stts != null)
            //stts.StartTextToSpeech(STTSLangTypes.KO, ttsInputField.text, speedSlide.value, pitchSlide.value);
            stts.StartTextToSpeech(STTSLangTypes.KO, sentence, 1, 1);
    }
    public void TTSEventForSuvivor()
    {
        if (stts != null)
            //stts.StartTextToSpeech(STTSLangTypes.KO, ttsInputField.text, speedSlide.value, pitchSlide.value);
            stts.StartTextToSpeech(STTSLangTypes.KO, csvManager.sentence, 1, 1);
    }
    private void OnDisable()
    {
        if (stts != null)
            stts.ReleaseSTTS();
    }


    // STT Result Event
    public void STTResultCallback(string result)
    {
        Debug.Log("[STT] Result: " + result);
        if (result != null)
            sttResultText.text = result;

        if (sttResultText.text != null && csvManager != null) ;
        {
            csvManager.checkResult();
        }
    }

    // Error Event
    public void ErrorCallback(string error)
    {
        Debug.Log("[Exception] Error: " + error);
    }
}
