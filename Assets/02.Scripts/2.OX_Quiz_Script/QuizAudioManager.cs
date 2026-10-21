using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizAudioManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] audioClip;
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;

    //public AudioClip[] animalClip;
    //public AudioClip[] humanClip;
    //public AudioClip[] instrumentsClip;
    //public AudioClip[] noiselClip;
    //public AudioClip[] vehicleClip;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void playSoundEffect()
    {
        audio.clip = audioClip[Random.Range(0, audioClip.Length)];
        audio.volume = Random.Range(0.6f, 1f);
        audio.Play();
    }

    public void AnswerSoundPlay(bool check)
    {
        audio.volume =1f;
        if (check == true)
        {
            audio.clip = correctAnswerSound;
            audio.Play();
        }
        else if (check == false)
        {
            audio.clip = wrongAnswerSound;
            audio.Play();
        }
    }
}
