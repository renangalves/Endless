using UnityEngine;
using System.Collections;

public class InteractionFeedback : MonoBehaviour {

    Transform target;

    Vector3 correctPosition;

    Animator anim;

	
	void Start () {
        target = GameObject.Find("AllPlayer").transform;
        anim = GetComponent<Animator>();
	}
	
	




    //the button will always keep following the player, but is shown when he is in an interact zone
	void Update () {
	    
        ButtonPositioning();

        if(PlayerManager.instance.isInInteractZone == true){
            anim.SetBool("Interact", true);
        } else {
            anim.SetBool("Interact", false);
        }



	}






    //this method will keep repositioning the button feedback above the character's head
    void ButtonPositioning(){
        correctPosition = target.position;
        
        correctPosition.y += 13;
        
        transform.position = correctPosition;
    }

}
