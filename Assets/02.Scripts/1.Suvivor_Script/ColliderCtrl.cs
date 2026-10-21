using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCtrl : MonoBehaviour
{
    // Start is
    // called before the first frame update
    public GameObject circleCollider;
    public ArrowDirCtrl ArrowDirCtrl;
    void Start()
    {
        if (circleCollider == null)
        {
            circleCollider = transform.gameObject.GetComponentInChildren<GameObject>(true);
        }
        
    }
    public void colliderCtrl()
    {
        if (circleCollider.activeSelf == false)
        {
            circleCollider.SetActive(true);
        }
        else if (gameObject.activeSelf == true)
        {
            circleCollider.SetActive(false);
        }
    }

    //public void spwawnSword()
    //{
    //    ArrowDirCtrl.SpawnSwordEffect();
    //}
    public void SwordDir()
    {
        ArrowDirCtrl.SetSwordDir();
    }

    // Update is called once per frame

}
