using UnityEngine;
using System.Collections;

public class BattlePointTrigger : MonoBehaviour
{

    bool isInBattleArea;
    protected BattlePointTriggerManager bPointTriggerManager;
    public GameObject[] enemyList;
    GameObject[] enemies = new GameObject[13];
    GameObject[] invisibleWalls = new GameObject[2];
    int counter;
    int counter2;
    int soundCounter;
    int soundCheck;
    int enemyCounter;
    bool enemiesSpawned = false;
    bool spiritSpawn1 = false;
    bool spiritSpawn2 = false;
    bool spiritSpawn3 = false;
    bool spiritSpawn4 = false;
    bool spiritSpawn5 = false;
    bool spiritSpawn6 = false;
    bool playAudioOnce = false;
    AudioSource source;
    public AudioClip[] playerSpotted;
    public Transform[] spawnPoints;


    void Start()
    {
        bPointTriggerManager = FindObjectOfType<BattlePointTriggerManager>();
        source = GetComponent<AudioSource>();
        isInBattleArea = false;

        //find the invisible walls in the battlezone and deactivate them
        foreach (Transform t in transform)
        {
            if (t.name == "LeftWall")
            {
                t.gameObject.SetActive(false);
                invisibleWalls [0] = t.gameObject;
            } else if (t.name == "RightWall")
            {
                t.gameObject.SetActive(false);
                invisibleWalls [1] = t.gameObject;
            }
        }
    }
    

    void Update()
    {

        //These battlezones are wet and I could have done a way better work with them, but time constraints didn't let me fix them so I had to go for safe code.
        //The battlezone work in the same way, with the exception of certain ones like tutorial and ending, so the commenting of the first one applies to the others
//-----------------------------------------------------------------------BATTLE POINT 1------------------------------------------------------------------------
        //Checks which battlezone is being managed
        if (gameObject.tag == "CameraStop1")
        {

            //spawn the enemies right after the player goes through a trigger
            if (isInBattleArea && counter == 1)
            {
                
                SpawnEnemies(enemyList [0], spawnPoints [0], false); //calls a method which will spawn the enemies, false if the enemy is human and true if it is a spirit
                SpawnEnemies(enemyList [0], spawnPoints [1], false); 
                
                SpawnEnemies(enemyList [1], spawnPoints [1], true); //spirits spawn in the same spawnPoint as some enemies after they die

                GameManager.instance.tutorial4 = true; //this battlezone triggers a tutorial in the GameManager
                isInBattleArea = false; //this variable is used to spawn the enemies only once
                enemiesSpawned = true; //this variable is used to start checking which enemies are defeated to spawn spirits or clear the battlezone
                
            } 

            //after the enemies spawn, start checking which ones are defeated
            if (enemiesSpawned == true)
            {
                //play two random soldier sounds
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                //checks if the enemy which will spawn a spirit is defeated, which will then activate the spirit
                if (enemies [1] == null && spiritSpawn1 == false)
                {
                    enemies [2].SetActive(true);
                    spiritSpawn1 = true;
                    GameManager.instance.firstSpiritEncounter = true; //tutorial trigger in GameManager about the first spirit the player faces
                }

                //checks if all enemies in the battlezone have been defeated to clear the battlezone and let the player progress
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    GameManager.instance.tutorial4Complete = true; //trigger in GameManager that the tutorial has been completed
                    MainCamera.instance.isInFixedCombatScreen = false; //makes the camera follow the player again
                    invisibleWalls [0].SetActive(false); //set the walls to false to let the player progress
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [0] = true; //add the battlezone to the cleared list so the player won't need to clear it again if he dies
                    gameObject.SetActive(false);
                }


            }



//-----------------------------------------------------------------------BATTLE POINT 2------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop2")
        {

            if (isInBattleArea && counter == 1)
            {

                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                SpawnEnemies(enemyList [0], spawnPoints [4], false);

                SpawnEnemies(enemyList [1], spawnPoints [0], true);
                SpawnEnemies(enemyList [1], spawnPoints [1], true);

                GameManager.instance.tutorial4Half = true; //this battlezone triggers a tutorial in the GameManager
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn1 = true;
                }

                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false; 
                    GameManager.instance.tutorial4HalfComplete = true; //trigger in GameManager that the tutorial has been completed
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [1] = true;
                    gameObject.SetActive(false);
                }
            }




//-----------------------------------------------------------------------BATTLE POINT 3------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop3")
        {
            
            if (isInBattleArea && counter == 1)
            {
                
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                SpawnEnemies(enemyList [0], spawnPoints [4], false);
                
                SpawnEnemies(enemyList [1], spawnPoints [0], true);
                SpawnEnemies(enemyList [1], spawnPoints [1], true);
                SpawnEnemies(enemyList [1], spawnPoints [4], true);

                GameManager.instance.tutorial6 = true; //this battlezone triggers a tutorial in the GameManager
                GameManager.instance.cannotUseSuperSpecialInTutorial = true; //avoid the player from using the super special while frozen
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn2 = true;
                }

                if (enemies [4] == null && spiritSpawn3 == false)
                {
                    enemies [7].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    GameManager.instance.tutorial6Complete = true; //trigger in GameManager that the tutorial has been completed
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [2] = true;
                    gameObject.SetActive(false);
                }
            }




//-----------------------------------------------------------------------BATTLE POINT 4------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop4")
        {
            
            if (isInBattleArea && counter == 1)
            {

                Debug.Log("SPAWNIN ENEMIES");
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                
                SpawnEnemies(enemyList [2], spawnPoints [0], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [0].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [3] = true;
                    gameObject.SetActive(false);
                }
            }



//-----------------------------------------------------------------------BATTLE POINT 5------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop5")
        {
            
            if (isInBattleArea && counter == 1)
            {
                
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);

                SpawnEnemies(enemyList [2], spawnPoints [0], true);
                SpawnEnemies(enemyList [2], spawnPoints [2], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [1].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [3].SetActive(true);
                    spiritSpawn1 = true;
                }

                if (enemies [2] == null && spiritSpawn2 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [4] = true;
                    gameObject.SetActive(false);
                }
            }



//-----------------------------------------------------------------------BATTLE POINT 6------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop6")
        {
            
            if (isInBattleArea && counter == 1)
            {
                
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                
                SpawnEnemies(enemyList [1], spawnPoints [0], true);
                SpawnEnemies(enemyList [2], spawnPoints [1], true);
                SpawnEnemies(enemyList [1], spawnPoints [2], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [2].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [3].SetActive(true);
                    spiritSpawn1 = true;
                }

                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn2 = true;
                }

                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [5] = true;
                    gameObject.SetActive(false);
                }
            }




//-----------------------------------------------------------------------BATTLE POINT 7------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop7")
        {
            
            if (isInBattleArea && counter == 1)
            {
                
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                SpawnEnemies(enemyList [0], spawnPoints [4], false);

                SpawnEnemies(enemyList [2], spawnPoints [0], true);
                SpawnEnemies(enemyList [1], spawnPoints [1], true);
                SpawnEnemies(enemyList [1], spawnPoints [2], true);
                SpawnEnemies(enemyList [1], spawnPoints [3], true);
                SpawnEnemies(enemyList [2], spawnPoints [4], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [3].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [7].SetActive(true);
                    spiritSpawn3 = true;
                }

                if (enemies [3] == null && spiritSpawn4 == false)
                {
                    enemies [8].SetActive(true);
                    spiritSpawn4 = true;
                }

                if (enemies [4] == null && spiritSpawn5 == false)
                {
                    enemies [9].SetActive(true);
                    spiritSpawn5 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [6] = true;
                    gameObject.SetActive(false);
                }
            }


//-----------------------------------------------------------------------BATTLE POINT 8------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop8")
        {
            
            if (isInBattleArea && counter == 1)
            {
                Debug.Log("SPAWNOU NO CAMERASTOP8");
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                
                SpawnEnemies(enemyList [3], spawnPoints [0], true);
                
                
                isInBattleArea = false;
                enemiesSpawned = true;
                
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [2].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [7] = true;
                    gameObject.SetActive(false);
                }
                
                
            }
 

//-----------------------------------------------------------------------BATTLE POINT 9------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop9")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                
                SpawnEnemies(enemyList [1], spawnPoints [0], true);
                SpawnEnemies(enemyList [1], spawnPoints [1], true);
                SpawnEnemies(enemyList [2], spawnPoints [2], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [4].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [3].SetActive(true);
                    spiritSpawn1 = true;
                }

                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn2 = true;
                }

                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [8] = true;
                    gameObject.SetActive(false);
                }
                
                
            }
      


//-----------------------------------------------------------------------BATTLE POINT 10------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop10")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                
                SpawnEnemies(enemyList [3], spawnPoints [0], true);
                SpawnEnemies(enemyList [3], spawnPoints [1], true);
                SpawnEnemies(enemyList [1], spawnPoints [2], true);
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [3].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [9] = true;
                    gameObject.SetActive(false);
                }
                
                
            }
         


//-----------------------------------------------------------------------BATTLE POINT 11------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop11")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                
                SpawnEnemies(enemyList [1], spawnPoints [0], true);
                SpawnEnemies(enemyList [3], spawnPoints [1], true);
                SpawnEnemies(enemyList [1], spawnPoints [2], true);
                SpawnEnemies(enemyList [2], spawnPoints [3], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [5].transform.position;

                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn3 = true;
                }

                if (enemies [3] == null && spiritSpawn4 == false)
                {
                    enemies [7].SetActive(true);
                    spiritSpawn4 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [10] = true;
                    gameObject.SetActive(false);
                }
                
                
            }




//-----------------------------------------------------------------------BATTLE POINT 12------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop12")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                
                SpawnEnemies(enemyList [2], spawnPoints [0], true);
                SpawnEnemies(enemyList [3], spawnPoints [1], true);
                SpawnEnemies(enemyList [2], spawnPoints [2], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [6].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [3].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [11] = true;
                    gameObject.SetActive(false);
                }
                
                
            }



//-----------------------------------------------------------------------BATTLE POINT 13------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop13")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                
                SpawnEnemies(enemyList [3], spawnPoints [0], true);
                SpawnEnemies(enemyList [3], spawnPoints [1], true);
                SpawnEnemies(enemyList [2], spawnPoints [3], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [7].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [4].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [12] = true;
                    gameObject.SetActive(false);
                }
                
                
            }




//-----------------------------------------------------------------------BATTLE POINT 14------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStop14")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                SpawnEnemies(enemyList [0], spawnPoints [4], false);
                
                SpawnEnemies(enemyList [3], spawnPoints [0], true);
                SpawnEnemies(enemyList [2], spawnPoints [1], true);
                SpawnEnemies(enemyList [3], spawnPoints [2], true);
                SpawnEnemies(enemyList [1], spawnPoints [4], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [8].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [5].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [6].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [7].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (enemies [4] == null && spiritSpawn4 == false)
                {
                    enemies [8].SetActive(true);
                    spiritSpawn4 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [13] = true;
                    gameObject.SetActive(false);
                }
                
                
            }




//-----------------------------------------------------------------------BATTLE POINT FINAL------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStopFinal")
        {
            
            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                SpawnEnemies(enemyList [0], spawnPoints [3], false);
                SpawnEnemies(enemyList [0], spawnPoints [4], false);
                SpawnEnemies(enemyList [0], spawnPoints [5], false);
                SpawnEnemies(enemyList [0], spawnPoints [6], false);
                
                SpawnEnemies(enemyList [3], spawnPoints [0], true);
                SpawnEnemies(enemyList [2], spawnPoints [1], true);
                SpawnEnemies(enemyList [3], spawnPoints [2], true);
                SpawnEnemies(enemyList [3], spawnPoints [4], true);
                SpawnEnemies(enemyList [1], spawnPoints [5], true);
                SpawnEnemies(enemyList [2], spawnPoints [6], true);

                //the spirit2 enemy (Flying Head) requires a reference point in battlezones to spawn to, so it is set in the PlayerManager the reference,
                //and when he spawns he gets that reference to go to
                PlayerManager.instance.spirit2Reference = bPointTriggerManager.spirit2References [9].transform.position;
                
                isInBattleArea = false;
                enemiesSpawned = true;
            } 
            
            if (enemiesSpawned == true)
            {
                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }
                
                if (enemies [0] == null && spiritSpawn1 == false)
                {
                    enemies [7].SetActive(true);
                    spiritSpawn1 = true;
                }
                
                if (enemies [1] == null && spiritSpawn2 == false)
                {
                    enemies [8].SetActive(true);
                    spiritSpawn2 = true;
                }
                
                if (enemies [2] == null && spiritSpawn3 == false)
                {
                    enemies [9].SetActive(true);
                    spiritSpawn3 = true;
                }
                
                if (enemies [3] == null && spiritSpawn4 == false)
                {
                    enemies [10].SetActive(true);
                    spiritSpawn4 = true;
                }

                if (enemies [5] == null && spiritSpawn5 == false)
                {
                    enemies [11].SetActive(true);
                    spiritSpawn5 = true;
                }

                if (enemies [6] == null && spiritSpawn6 == false)
                {
                    enemies [12].SetActive(true);
                    spiritSpawn6 = true;
                }
                
                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [14] = true;
                    gameObject.SetActive(false);
                }
                
                
            }



//-----------------------------------------------------------------------BATTLE POINT ENDING------------------------------------------------------------------------
        } else if (gameObject.tag == "CameraStopEnding")
        {


            if (isInBattleArea && counter == 1)
            {
                SpawnEnemies(enemyList [0], spawnPoints [0], false);
                SpawnEnemies(enemyList [0], spawnPoints [1], false);
                SpawnEnemies(enemyList [0], spawnPoints [2], false);
                
                isInBattleArea = false;
                enemiesSpawned = true;
                GameManager.instance.lastBattleZoneActivated = true;
            } 
            
            if (enemiesSpawned == true)
            {
                //slow down time for the final moment in the game
                if(PlayerManager.instance.lifePoints >= 0){
                    Time.timeScale = 0.2f;
                } else {
                    Time.timeScale = 1.0f;
                }

                //when the player uses a super special (Soul Destruction) in this battlezone, it triggers the final moment in the game in the GameManager
                if(PlayerManager.instance.isInSuperSpecial == true){
                    GameManager.instance.lastSuperSpecialComplete = true;
                }

                if(!source.isPlaying && soundCounter < 2){
                    playAudioOnce = false;
                    PlayPlayerSpottedAudio();
                    soundCounter++;
                }

                if (AllEnemiesDead())
                {
                    Debug.Log("DESATIVOU AS PAREDES");
                    MainCamera.instance.isInFixedCombatScreen = false;
                    invisibleWalls [0].SetActive(false);
                    invisibleWalls [1].SetActive(false);
                    bPointTriggerManager.beatenBattleScenes [14] = true;
                    gameObject.SetActive(false);
                }
                
                
            }
            
        }


    }





    //checks if all enemies are dead in the current triggered battlezone
    bool AllEnemiesDead()
    {
        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                return false;
            }

        }
        return true;
    }




    //method that will spawn enemies depending on the type, position and if it is a spirit or not
    void SpawnEnemies(GameObject enemy, Transform enemyPosition, bool isSpirit)
    {
        enemies [enemyCounter] = Instantiate(enemy);
        enemies [enemyCounter].transform.position = enemyPosition.position;
        enemies [enemyCounter].transform.parent = gameObject.transform;

        //if it is a spirit, then spawn and deactivate it
        if (isSpirit == true)
        {
            enemies [enemyCounter].transform.rotation = gameObject.transform.rotation;
            enemies [enemyCounter].SetActive(false);
        }
        enemyCounter++;
        Debug.Log("ENEMY COUNTER " + enemyCounter);
    }





    //This method looks horrible and I know it, could have done way better
    //this method will control the position that the camera will fix on depending on the current triggered battlezone
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.isTrigger != true)
        {
            if (coll.CompareTag("Player") && counter == 0)
            {
                isInBattleArea = true;
                counter += 1;
                invisibleWalls [0].SetActive(true); //sets the walls to true to stop the player from progressing
                invisibleWalls [1].SetActive(true);

                //checks which battlezone is currently triggered to get the point reference in the BattlePointTriggerManager to fix the camera
                if (gameObject.tag == "CameraStop1")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [0];
                } else if (gameObject.tag == "CameraStop2")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [1];
                } else if (gameObject.tag == "CameraStop3")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [2];
                } else if (gameObject.tag == "CameraStop4")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [3];
                } else if (gameObject.tag == "CameraStop5")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [4];
                } else if (gameObject.tag == "CameraStop6")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [5];
                } else if (gameObject.tag == "CameraStop7")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [6];
                } else if (gameObject.tag == "CameraStop8")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [7];
                } else if (gameObject.tag == "CameraStop9")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [8];
                } else if (gameObject.tag == "CameraStop10")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [9];
                } else if (gameObject.tag == "CameraStop11")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [10];
                } else if (gameObject.tag == "CameraStop12")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [11];
                } else if (gameObject.tag == "CameraStop13")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [12];
                } else if (gameObject.tag == "CameraStop14")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [13];
                } else if (gameObject.tag == "CameraStopFinal")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [14];
                } else if (gameObject.tag == "CameraStopEnding")
                {
                    MainCamera.instance.fixedCameraPosition = bPointTriggerManager.cameraReferences [15];
                }
                MainCamera.instance.isInFixedCombatScreen = true;
            }
        }
    }




    //this method controls the audio to play from the soldiers
    void PlayPlayerSpottedAudio()
    {

        int randomSound;

        if (playAudioOnce == false)
        {
            randomSound = Random.Range(0, playerSpotted.Length);
            while(soundCheck == randomSound){
                randomSound = Random.Range(0, playerSpotted.Length);
            }
            soundCheck = randomSound;
            source.clip = playerSpotted[randomSound];
            source.Play();
            playAudioOnce = true;
        }
    
    }
}
