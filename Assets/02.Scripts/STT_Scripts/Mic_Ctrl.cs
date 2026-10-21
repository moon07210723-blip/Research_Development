using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic_Ctrl : MonoBehaviour
{

    //public AudioClip aud;
    //private int sampleRate = 44100;
    //private float[] samples;
    //public float rmsValue;
    //public float modulate = 10000;
    //public int resultValue;
    //public int cutValue;

    //public bool recording;
    //public GameObject obj;

    //void Start()
    //{
    //    samples = new float[sampleRate];
    //    aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    getMicValue();
    //}

    //public void startMic(bool startRecording)
    //{
    //    string micName = Microphone.devices[0].ToString();
    //    if (startRecording == true)
    //    {
    //        aud = Microphone.Start(micName, true, 1, sampleRate);
    //        recording = true;
    //    }
    //    else if (startRecording == false)
    //    {
    //        Microphone.End(micName);
    //        recording = false;
    //    }
     
    //}
    //public void getMicValue()
    //{
    //    if (recording == true)
    //    {
    //        aud.GetData(samples, 0);
    //        float sum = 0;
    //        for (int i = 0; i < samples.Length; i++)
    //        {
    //            sum += samples[i] * samples[i];
    //        }
    //        rmsValue = Mathf.Sqrt(sum / samples.Length);
    //        rmsValue = rmsValue * modulate;
    //        rmsValue = Mathf.Clamp(rmsValue, 0, 100);
    //        resultValue = Mathf.RoundToInt(rmsValue);

    //        if (resultValue < cutValue)
    //        {
    //            resultValue = 0;
    //        }

    //        obj.transform.localScale = new Vector3(resultValue, resultValue, resultValue) * 0.01f;
    //    }
    //    else if (recording == false)
    //    {
    //        rmsValue = 0;
    //        resultValue = 0;
    //        if (obj.transform.localScale.x > 0)
    //        {
    //            float minusScale = obj.transform.localScale.x;
    //            minusScale = minusScale - Time.deltaTime;
    //            obj.transform.localScale = new Vector3(minusScale, minusScale, minusScale);
    //        }
    //        else if (obj.transform.localScale.x <= 0)
    //        {
    //            obj.transform.localScale = new Vector3(0, 0, 0);
    //        }
    //    }
    //}
}
