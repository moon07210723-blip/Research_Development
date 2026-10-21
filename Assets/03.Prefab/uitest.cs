using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class uitest : MonoBehaviour
{
    public DOTweenAnimation popup;
    public DOTweenAnimation close;
    // Start is called before the first frame update


    public void DOTrewind()
    {
        popup.DORewind();
        close.DORewind();
        Debug.Log("«Ï¿Ã");
    }
}
