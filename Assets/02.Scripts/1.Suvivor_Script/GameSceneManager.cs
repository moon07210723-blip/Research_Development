using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static int sceneNum;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    public void loadScene()
    {
        SceneManager.LoadScene(sceneNum);
    }



}
