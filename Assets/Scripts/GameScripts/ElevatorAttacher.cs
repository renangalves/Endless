using UnityEngine;
using System.Collections;

public class ElevatorAttacher : MonoBehaviour {



    //when the player steps on the elevator, change him to be child of the elevator and change the localscale to avoid problems
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<PlayerManager>()!=null)
        {
            coll.gameObject.transform.SetParent(transform, true);
            coll.transform.localScale = new Vector3(1/33.334f, 1/2.5f, 1);
        }   
    }
    
    
    //when the player exits, change the values back to normal
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<PlayerManager>()!=null)
        {
            coll.gameObject.transform.SetParent(null, true);
            coll.transform.localScale = Vector3.one;
        }
    }
}
