using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Goldmetal.UndeadSurvivor;


public enum GameMode
{
    None,
    OX,
    Quiz,
    Survivor
}
public class ArrowDirCtrl : MonoBehaviour
{
    public GameMode Mode;
    private Player player; //서바이벌 게임
    private OX_Quiz_Player Q_player;  // OX 게임 
    private Word_Quiz_Player player3; // 퀴즈 게임

    public Transform playerPos;
    //public Transform arrowPos;
    public Transform effectObject;
    public Transform weaponCollider;
    public GameObject weaponChild;

    public float arrowDitance;
    public float effectDitance;
    public float colliderDitance;
    public Quaternion lookRotation;
    public Vector3 v3;

    private float distance;
    private float saveX;
    private float saveY;

    private bool rotLock;
    void Start()
    {
        switch (Mode)
        {
            case GameMode.None:
                break;
            case GameMode.OX:
                Q_player = transform.GetComponentInParent<OX_Quiz_Player>();
                playerPos = Q_player.transform;
                break;
            case GameMode.Quiz:
                player3 = transform.GetComponentInParent<Word_Quiz_Player>();
                playerPos = player3.transform;
                break;
            case GameMode.Survivor:
                player =transform.GetComponentInParent<Player>();
                playerPos = player.transform;
                break;

        }
        saveX = 0f;
        saveY = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mode)
        {
            case GameMode.None:
                break;
            case GameMode.OX:
                playerPos = Q_player.transform;
                if (Q_player.inputVec.x != 0 || Q_player.inputVec.y != 0)
                {
                    transform.position = new Vector3(playerPos.position.x + Q_player.inputVec.x * arrowDitance, playerPos.position.y + Q_player.inputVec.y * arrowDitance, playerPos.position.z);
                    transform.rotation = Quaternion.LookRotation(transform.position - playerPos.position);
                    saveX = playerPos.position.x + Q_player.inputVec.x * colliderDitance;
                    saveY = playerPos.position.y + Q_player.inputVec.y * colliderDitance;
                }
                break;
            case GameMode.Quiz:
                playerPos = player3.transform;
                if (player3.inputVec.x != 0 || player3.inputVec.y != 0)
                {
                    transform.position = new Vector3(playerPos.position.x + player3.inputVec.x * arrowDitance, playerPos.position.y + player3.inputVec.y * arrowDitance, playerPos.position.z);
                    transform.rotation = Quaternion.LookRotation(transform.position - playerPos.position);
                    saveX = playerPos.position.x + player3.inputVec.x * colliderDitance;
                    saveY = playerPos.position.y + player3.inputVec.y * colliderDitance;
                }
                break;
            case GameMode.Survivor:
                
                if (player.inputVec.x != 0 || player.inputVec.y != 0)
                {
                    transform.position = new Vector3(playerPos.position.x + player.inputVec.x * arrowDitance, playerPos.position.y + player.inputVec.y * arrowDitance, playerPos.position.z);
                    transform.rotation = Quaternion.LookRotation(transform.position - playerPos.position);
                    saveX = playerPos.position.x + player.inputVec.x * colliderDitance;
                    saveY = playerPos.position.y + player.inputVec.y * colliderDitance;
                }
                if (rotLock == false)
                {
                    effectObject.position = new Vector3(playerPos.position.x + player.inputVec.x * effectDitance, playerPos.position.y + player.inputVec.y * effectDitance, playerPos.position.z);
                    weaponCollider.position = new Vector3(saveX, saveY, playerPos.position.z);
                    distance = Vector3.Distance(effectObject.position, playerPos.position);
                    if (distance == 0)
                    {
                        effectObject.rotation = Quaternion.LookRotation(weaponCollider.position - playerPos.position, v3);
                    }
                    else
                    {
                        effectObject.rotation = Quaternion.LookRotation(effectObject.position - playerPos.position, v3);
                    }
                }
                break;

        }

    }

    public void SpawnSwordEffect()
    {
        //  effectObject.position = new Vector3(saveX, saveY, playerPos.position.z);
    }
    public void SetSwordDir()
    {
        if (rotLock == false)
        {
            rotLock = true;
        }
        else if (rotLock == true)
        {
            rotLock = false;
        }


    }
}
