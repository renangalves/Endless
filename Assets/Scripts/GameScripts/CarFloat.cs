using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CarFloat : MonoBehaviour
{
    
    Elevator el;
    
    //sets the tweener for the car to make it look like it's floating
    void Start()
    {
        Tweener t = transform.DOBlendableMoveBy(new Vector2(0, 5), 8);
        t.SetLoops(-1, LoopType.Yoyo);
        t.SetEase(Ease.InOutSine);
        el = GetComponent<Elevator>();
    }
    

    void Update()
    {
        
    }

    //sets the player as the child to fix him on the platform
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.gameObject.transform.SetParent(el.transform, true);
        }   
    }
    
    
    //if the player leaves, set him back to the root position
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.gameObject.transform.SetParent(coll.gameObject.transform.root.parent, true);
        }
    }
}
