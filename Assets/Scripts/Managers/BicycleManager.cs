using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BicycleManager : MonoBehaviour
{
    public enum BicycleParts
    {
        Pedals,
        FrontWheel,
        BackWheel
    }

    public BicycleParts Part;
    void Start()
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, 180f), 0.1f).SetLoops(-1);
        if (BicycleParts.Pedals == Part) transform.DOLocalRotate(new Vector3(0f, 0, -90f), 0.25f).SetLoops(-1, LoopType.Incremental);
    }

    
    void Update()
    {
        
    }
}
