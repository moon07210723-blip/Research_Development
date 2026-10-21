using Goldmetal.UndeadSurvivor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OX_Quiz_Player : MonoBehaviour
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
        anim.runtimeAnimatorController = animCon[OX_GameManager.Instance.playerId]; //플레이어 id에서 넘버를 가지고와 플레이어[id]를 실행 
        
    }
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 물리프레임 1개만큼 소모된 시간
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
        if (collision.CompareTag("True"))
        {
            OX_GameManager.Instance.currentPlayerQuizAnswer = 0;
        }
        else if (collision.CompareTag("False"))
        {
            OX_GameManager.Instance.currentPlayerQuizAnswer = 1;
        }

        //else if (collision.CompareTag("Middle"))
        //{
        //    OX_GameManager.Instance.currentPlayerQuizAnswer = 2;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OX_GameManager.Instance.currentPlayerQuizAnswer = 2;
    }

}

