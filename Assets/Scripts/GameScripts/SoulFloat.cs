using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoulFloat : MonoBehaviour
{
    
    //makes the pickups float slowly 
    void Start()
    {
        Tweener t = transform.DOBlendableMoveBy(new Vector2(0, 1), 3);
        t.SetLoops(-1, LoopType.Yoyo);
        t.SetEase(Ease.InOutSine);
    }
    

    void Update()
    {
        
    }

}
