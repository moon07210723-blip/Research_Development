using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D Rigidbody2D;
    public Animator Animator;

    [SerializeField]
    private int movementCount;

    public float speed;
    private bool canMove;
    public bool isMove;
    public bool isDash;

    public GameObject[] targetArea;
    public Transform target;
    public Transform cube;
    private Vector3 targetPos;
    void Start()
    {
        movementCount = 5;
        speed = 4f;
        target = null;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        target = targetArea[Random.Range(0, targetArea.Length)].transform;
        setRandomArea();
    }

    IEnumerator chagnePosion()
    {
        float timer = Random.Range(1f, 5f);
        yield return new WaitForSeconds(timer);
        setRandomArea();
    }
    void setRandomArea()
    {
        canMove = true;
        movementCount--;
        SpriteRenderer targetSpriteRender = target.GetComponent<SpriteRenderer>();

        //targetPos = new Vector3(Random.Range((-(targetSpriteRender.size.x) / 2) + 1, ((targetSpriteRender.size.x) / 2) - 1),
        //                        Random.Range((target.transform.position.y - targetSpriteRender.size.y / 2) + 0.75f, (target.transform.position.y + targetSpriteRender.size.y / 2)) + 0.75f,
        //                        target.transform.position.z);

        //간소화한 버전
        targetPos = new Vector3(Random.Range(-7.5f + 1, 7.5f - 1),
                        Random.Range((target.transform.position.y - 2f) + 0.75f, (target.transform.position.y + 2f)) - 0.75f,
                        target.transform.position.z);
        //   cube.position = targetPos;

        //Debug.Log((-(targetSpriteRender.size.x) / 2) + 1);

        //Debug.Log(((targetSpriteRender.size.x) / 2) - 1);

        //Debug.Log((target.transform.position.y - targetSpriteRender.size.y / 2) + 0.75f);
        //Debug.Log((target.transform.position.y + targetSpriteRender.size.y / 2) - 0.75f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isMove == false)
            {
                isMove = true;
                Animator.SetBool("Move", isMove);
            }
            else if (isMove == true)
            {
                isMove = false;
                Animator.SetBool("Move", isMove);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isMove = false;
            Animator.SetBool("Move", isMove);
            if (isDash == false)
            {
                isDash = true;
                Animator.SetBool("Dash", isDash);
            }
            else if (isDash == true)
            {
                isDash = false;
                Animator.SetBool("Dash", isDash);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            setRandomArea();
        }
    }

    private void FixedUpdate()
    {
        if (target != null && canMove == true)
        {

            Vector2 dirVec = targetPos - transform.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            Rigidbody2D.MovePosition(Rigidbody2D.position + nextVec);
            Rigidbody2D.velocity = Vector2.zero;

            float distance = Vector2.Distance(transform.position, targetPos);

            if (distance >= 2f)
            {
                speed = 4;
                isDash = true;
                Animator.SetBool("Dash", isDash);
                isMove = false;
                Animator.SetBool("Move", isMove);
            }
            else if (distance <= 2f && distance >= 0.25f)
            {
                speed = 1.5f;
                isDash = false;
                Animator.SetBool("Dash", isDash);
                isMove = true;
                Animator.SetBool("Move", isMove);
            }
            else if (distance <= 0.25f)
            {
                canMove = false;

                Rigidbody2D.velocity = Vector2.zero;
                isDash = false;
                Animator.SetBool("Dash", isDash);
                isMove = false;
                Animator.SetBool("Move", isMove);

                if (movementCount > 0)
                {
                    StartCoroutine("chagnePosion");
                }

            }
        }
    }

    void LateUpdate()
    {
        SpriteRenderer.flipX = targetPos.x < Rigidbody2D.position.x;
    }

}
