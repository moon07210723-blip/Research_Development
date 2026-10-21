using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum gameMode
{
    none, OX, QUIZ
}
public class AreaColorChanger : MonoBehaviour
{
    public gameMode gameMode;
    SpriteRenderer spriteRenderer;
    public Color32 selectColor;
    public Color32 notSelectColor;
    public bool playerInside;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            spriteRenderer.color = selectColor;
            playerInside=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            spriteRenderer.color = notSelectColor;
            playerInside = false;
        }
    }

    private void Update()
    {
        if (gameMode == gameMode.QUIZ)
        {
            if (Word_Quiz_GameManager.Instance.quizPlaying == true && playerInside == true)
            {
                spriteRenderer.color = selectColor;
            }
        }
        else if (gameMode == gameMode.OX)
        {
            if (OX_GameManager.Instance.quizPlaying == true && playerInside == true)
            {
                spriteRenderer.color = selectColor;
            }

        }
        
    }


}
