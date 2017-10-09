using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Elevator : MonoBehaviour
{

    Elevator el;
    
    //These elevator scripts could have been avoided or grouped in one, but I didn't have enough time to fix this
    //sets the first elevator in the second level to move up and down 
    void Start()
    {
        Tweener t = transform.DOBlendableMoveBy(new Vector2(0, 70), 4);
        t.SetLoops(-1, LoopType.Yoyo);
        t.SetEase(Ease.InOutSine);
        el = GetComponent<Elevator>();
    }
    

    void Update()
    {
        
    }


}
