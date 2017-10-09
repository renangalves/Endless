using UnityEngine;
using System.Collections;

public class BattlePointTriggerManager : MonoBehaviour {

    public GameObject[] battlePoints;

    public GameObject[] savedBattleScenes;
    public GameObject[] cameraReferences;
    public GameObject[] spirit2References;

    int counter = 0;

    public bool[] beatenBattleScenes;
    bool redoBattleScenes = false;
    bool redoOnce = false;


	void Start () {
	    
        savedBattleScenes = new GameObject[battlePoints.Length];
        beatenBattleScenes = new bool[battlePoints.Length];

        //saves all battlezones in a temporary variable to make checks if it was cleared when the player dies and respawns
        while(counter < battlePoints.Length){
            savedBattleScenes[counter] = Instantiate(battlePoints[counter]);
            beatenBattleScenes[counter] = false;
            counter++;
        }

	}
	

	void Update () {
	    //if the player dies, then the battlescenes are redone so if the player died in a battlezone then it is reset
        if(GameManager.instance.isSpawning == true){
            redoBattleScenes = true;
            MainCamera.instance.isInFixedCombatScreen = false; //the camera will follow the player again
        }

        //if the player died then all not cleared battlezones are destroyed and re-instantiated so the player can try it again
        if(redoBattleScenes == true){
            if(redoOnce == false){ //do this process once when player spawns
                counter = 0;
                while(counter < battlePoints.Length){
                    if(beatenBattleScenes[counter] == false){ //only if the player haven't beaten the battlezone
                        Debug.Log("QUANTAS BATTLEZONES");
                        Destroy(savedBattleScenes[counter]); //destroy the battlezone
                        savedBattleScenes[counter] = Instantiate(battlePoints[counter]); //then re-instantiate it
                    }
                    counter++;
                }
                redoOnce = true;
            }
            redoBattleScenes = false;

        } else {
            redoOnce = false;
        }
	}
}
