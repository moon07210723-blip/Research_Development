using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Goldmetal.UndeadSurvivor;
using System.Linq;
using System.Globalization;

public class Explosion_Ctrl : MonoBehaviour
{
    public List<GameObject> explosionList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] newTargets;

    public GameObject explosionPrefab;

    private Animator explosionAnimator;

    public bool enemyFound; // 적을 찾았는가?
    public bool checkingEnemy; //적을 체크하였는가?
    public bool startExplosion; // 폭발이 시작 됐는가?

    public int ExplosionCount;

    public float ExploisionTimer;
    public float enemyFindTimer;
    public float ExploisionDeley;

    // Start is called before the first frame update
    void Start()
    {
        startExplosion = true;
        checkingEnemy = false;
        scanRange = 5f;
        explosionPrefab = GameManager.instance.pool.prefabs[4];
        targetLayer = LayerMask.GetMask("Enemy");
 
        addExploser(5);
        ExploisionDeley = 2f;

    }

    public void InitExplosion()
    {
        foreach (GameObject exploser in explosionList)
        {
            exploser.SetActive(false);
        }
        //enemyList.Clear();
        startExplosion = true;
        checkingEnemy = false;
        enemyFound = false;
        ExploisionDeley = 2f;
        enemyFindTimer = 0;
    }

    public void SetExplosionDamage(float weaponDamage)
    {
        for(int i = 0; i < explosionList.Count; i++)
        {
            explosionList[i].GetComponent<Bullet>().damage = weaponDamage;
        }
    }

    public void ChechExplosionList() //폭발 오브젝트가 전부 비활성화 됐는지 확인하는 함수
    {
        int count = 0;
        foreach (GameObject exploser in explosionList)
        {
            if (exploser.activeSelf == false)
            {
                //startExplosion = true;
                count++;
            }
        }
        if (count >= explosionList.Count)
        {
            CheckEnemy(); //적을 찾는 함수
        }
    }

    public void CheckEnemy() //에너미를 탐지하는 함수
    {
        newTargets = Physics2D.CircleCastAll(GameManager.instance.player.transform.position, scanRange, Vector2.zero, 0, targetLayer);
        enemyList.Clear();
        if (newTargets != null)
        {
            foreach (RaycastHit2D target in newTargets)
            {
                float targetHP = target.transform.GetComponent<Enemy>().health;
                if (targetHP >= targetHP / 3)
                {
                    enemyList.Add(target.transform.gameObject);
                }
            }
            enemyFound = true;//적을 찾았으니 enemyfound를 true로 전환
        }
        enemyList = enemyList.OrderByDescending(x => Vector2.Distance(transform.position, x.transform.position)).Distinct().ToList();

    } 

    // Update is called once per frame
    void Update()
    {

        if (enemyFound == false)//적을 찾지 못했으니 1초마다 적을 탐지한다.
        {
            enemyFindTimer += Time.deltaTime;

            if (enemyFindTimer >= 1f) //1초마다 폭발 오브젝트가 전부 비활성화 되어있는지 확인하고 적을 찾는다.
            {
                enemyFindTimer = 0;
                ChechExplosionList();
            }
        }
        else if (enemyFound == true)//적을 탐지했으니 폭발함수를 실행한다
        {
            if (startExplosion == true)//폭발함수 조건이 참인가?
            {
                ExploisionTimer -= Time.deltaTime * GameManager.instance.worldSpeed;
                int extraExplosion = explosionList.Count - enemyList.Count;

                if (explosionList.Count > enemyList.Count && ExploisionTimer <= 0f) //폭발 오브젝트가 탐지한 적보다 많을 때
                {
                    TurnOnExplosion(ExplosionCount); //0.25초마다 폭발 오브젝트를 활성화한다.
                    if (ExplosionCount == enemyList.Count) //탐지한 적수만큼 폭발 함수를 실행했으면 초기화한다.
                    {
                        ExplosionCount = 0;
                        startExplosion = false;
                    }
                }
                else if (explosionList.Count <= enemyList.Count && ExploisionTimer <= 0f) //폭발 오브젝트가 탐지한 적보다 적을 떄 (적이 더 많을 때)
                {
                    TurnOnExplosion(ExplosionCount);
                    if (ExplosionCount >= explosionList.Count) //폭발 오브젝트 수 만큼 폭발 함수를 실행했으면 초기화한다.
                    {
                        ExplosionCount = 0;
                        startExplosion = false;
                    }
                }
            }
            else if (startExplosion == false)//폭발함수 실행이 종료되면 ExploisionDeley만큼 있다가 초기화를한다.
            {
                ExploisionDeley -= Time.deltaTime * GameManager.instance.worldSpeed;
                if (ExploisionDeley <= 0)
                {
                    enemyFound = false;
                    startExplosion = true;
                    ExploisionDeley = 2f;
                   // enemyList.Clear(); //에너미 리스트를 초기화한다.
                }
            }

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(GameManager.instance.player.transform.position, 5f);
    }

    public void addExploser(int addcount) //폭발 오브젝트 생성
    {
        for (int i = 0; i < addcount; i++)
        {
            GameObject exploser = Instantiate(explosionPrefab);
            exploser.transform.parent = GameManager.instance.pool.transform;
            exploser.transform.localScale = new Vector3(6, 6, 6);
            exploser.name = "explosion_" + explosionList.Count;
            exploser.SetActive(false);
            explosionList.Add(exploser);
        }
    }

    public void TurnOnExplosion(int i)
    {
        if (enemyList.Count > 0)
        {
            ExploisionTimer = 0.25f;
            explosionList[i].SetActive(true);
            explosionAnimator = explosionList[i].GetComponent<Animator>();
            explosionList[i].transform.position = enemyList[i].transform.position;
            explosionAnimator.SetTrigger("Explosion");
            ExplosionCount++;
        }
    }
}
