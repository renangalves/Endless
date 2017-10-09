using UnityEngine;
using System.Collections;

public class CheckPointCollider : MonoBehaviour {

    public GameObject respawnPoint;

    protected CheckPointManager checkManager;

	
	void Start () {
	    
        checkManager = FindObjectOfType<CheckPointManager> ();

	}
	
	
	void Update () {
	    
	}


    //when the player collides with the checkpoint collider then send to the manager the new respawn position
    void OnTriggerEnter2D (Collider2D coll)
    {

        if (coll.isTrigger != true) {
            if (coll.CompareTag ("Player")) {
                checkManager.respawnPosition.x = respawnPoint.transform.position.x;
                checkManager.respawnPosition.y = respawnPoint.transform.position.y;
                checkManager.checkPointTrigger = respawnPoint;
            }
        }
    }


}
