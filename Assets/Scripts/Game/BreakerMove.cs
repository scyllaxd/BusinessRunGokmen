using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakerMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMoveY(0.7f, 0.1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
