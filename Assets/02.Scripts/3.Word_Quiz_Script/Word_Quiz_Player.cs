using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Word_Quiz_Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 5.0f;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    public RuntimeAnimatorController[] animCon;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()  // 활성화 되었을때 실행
    {
        anim.runtimeAnimatorController = animCon[Word_Quiz_GameManager.Instance.playerId]; //�÷��̾� id���� �ѹ��� ������� �÷��̾�[id]�� ���� 

    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // ���������� 1����ŭ �Ҹ�� �ð�
        rigid.MovePosition(rigid.position + nextVec);


    }
    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Area0"))
        {
            Word_Quiz_GameManager.Instance.QuizChooseText.text = "Area0";
            Word_Quiz_GameManager.Instance.currentAnswerNumber = 0;


        }
        else if (collision.CompareTag("Area1"))
        {
            Word_Quiz_GameManager.Instance.QuizChooseText.text = "Area1";
            Word_Quiz_GameManager.Instance.currentAnswerNumber = 1;

        }

        else if (collision.CompareTag("Area2"))
        {
            Word_Quiz_GameManager.Instance.QuizChooseText.text = "Area2";
            Word_Quiz_GameManager.Instance.currentAnswerNumber = 2;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area0"))
        {
            Word_Quiz_GameManager.Instance.currentAnswerNumber = 10;
        }
        else if (collision.CompareTag("Area1"))
        {
             Word_Quiz_GameManager.Instance.currentAnswerNumber = 10;
        }
        else if (collision.CompareTag("Area2"))
        {
            Word_Quiz_GameManager.Instance.currentAnswerNumber = 10;
        }
    }
}

