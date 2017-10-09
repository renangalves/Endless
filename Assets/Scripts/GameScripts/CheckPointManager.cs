using UnityEngine;
using System.Collections;

public class CheckPointManager : MonoBehaviour {

    public Vector3 respawnPosition;

    public GameObject checkPointTrigger;

    //bool saveHasSuperSpecialPower;

    Vector3 lastPosition;

	// Use this for initialization
	void Start () {
        //at start of each level it's setup the initial respawn point to avoid any problems, sorry for the magic numbers
        if(GameManager.instance.currentStage == 1){
            respawnPosition.x = -179f;
            respawnPosition.y = 110.8f;
            lastPosition.x = -179f;
            lastPosition.y = 110.8f;
        } else if(GameManager.instance.currentStage == 2){
            respawnPosition.x = 630.6f;
            respawnPosition.y = -16.5f;
            lastPosition.x = 630.6f;
            lastPosition.y = -16.5f;
        }
	}
	
	
    //the update will control the new respawn positions and also control if the player had a super special (Soul Destruction scroll) when he died so he gets it back
	void Update () {
	    
        //the CheckPointCollider changes the respawnPosition values, triggering this if
        if(lastPosition != respawnPosition){
            GameManager.instance.respawnPreviousSoulCounter = PlayerManager.instance.soulsCounter; //saves the player's soul counter (chakra meter)
            GameManager.instance.playerHadSuperSpecial = PlayerManager.instance.hasSuperSpecial; //saves if the player has a super special (Soul Destruction scroll)
            GameManager.instance.respawnLocation = respawnPosition; //the new respawn location is set
            checkPointTrigger.SetActive(false); //the checkpoint trigger is deactivated
            lastPosition = respawnPosition;
        }

        //as soon as the player collects a scroll, it is saved so if the player dies before a checkpoint he doesn't lose it (the pickups are not reset, meaning that we would lose it
        if(PlayerManager.instance.hasSuperSpecial == true){
            GameManager.instance.playerHadSuperSpecial = true;
        }

	}
}
