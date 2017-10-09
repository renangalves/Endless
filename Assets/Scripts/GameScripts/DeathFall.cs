using UnityEngine;
using System.Collections;

public class DeathFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //controls when the player collides with the trigger of death falls
    void OnTriggerEnter2D(Collider2D coll){
        if (coll.isTrigger != true) {
            if (coll.CompareTag ("Player")) {
                PlayerManager.instance.lifePoints = -1;
            }
        }
    }
}
