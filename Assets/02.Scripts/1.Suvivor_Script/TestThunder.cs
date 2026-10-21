using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestThunder : MonoBehaviour
{
    public GameObject Thunder;

    int count = 10; // 번개 낙하 갯수
    private BoxCollider2D area; // 번개 낙하 범위
    public List<GameObject> ThunderList = new List<GameObject>();

    void Start()
    {
        area = GetComponent<BoxCollider2D>();
        StartCoroutine(Spawn(5)); // 5초마다 번개 생성
    }
    private IEnumerator Spawn(float delayTime)
    {
        for (int num = 0; num < count; num++) 
        {
            Vector3 spawnPos = GetRandomPosition(); 
            GameObject instance = Instantiate(Thunder, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.2f); // 각 번개 생성시 0.2초 차이를 두고 생성
            ThunderList.Add(instance);
        }
        area.enabled = false;
        yield return new WaitForSeconds(delayTime); // 번개 전부 생성 후 다음 번개 생성 시간

        for(int num=0; num<count; num++) // 번개 전부 생성 후 다음 번개 생성 시 앞서 생성 되어있는 번개들 전부 제거.
        {
            Destroy(ThunderList[num].gameObject);
        }
        ThunderList.Clear();
        area.enabled = true;
        StartCoroutine(Spawn(10));
    }
    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = transform.position;
        Vector2 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        Vector2 spwanPos = new Vector2(posX, posY);
        return spwanPos;
    }

}
