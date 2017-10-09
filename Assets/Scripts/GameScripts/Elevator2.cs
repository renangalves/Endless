using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Elevator2: MonoBehaviour 
{

    //These elevator scripts could have been avoided or grouped in one, but I didn't have enough time to fix this
    //sets the second elevator in the second level to go up and down
    void Start () 
    {
        Tweener t = transform.DOBlendableMoveBy (new Vector2 (0,-20),4);
        t.SetLoops (-1, LoopType.Yoyo);
        t.SetEase (Ease.InOutSine);
    }
    

    void Update () 
    {
        
    }
}
