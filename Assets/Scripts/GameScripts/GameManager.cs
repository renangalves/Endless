using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject redScreen;

    Animator anim;

    protected MainCamera mC;

    public Button continueButton;
    public Button quitToTitleButton;

    public Text tutorialDoubleJump;

    public Image gotKey;
    public Image level1Transition;
    public Image gameOverScreen;
    public Image superSpecial;
    public Image textPanel;
    public Image pressToContinuePanel;
    public Image spiritVase;
    public Image loadingDone;
    public Image introPart1;
    public Image finalWhiteScreen;
    public Image[] tutorialInputs;

    Color tempColor;
    Color tempColor2;

    public Vector3 respawnLocation;
    Vector3 gameManagerPosition;
    Vector3 fixCameraPosition;

    public GameObject specialSlider;
    public GameObject allPlayer;
    public GameObject levelTwoSpawnPoint;
    public GameObject tutorialLeftWall;
    public GameObject tutorialRightWall;
    public GameObject introCube;
    public GameObject transitionCube;
    public GameObject UILife;
    public GameObject heartSoul;
    public GameObject endingRoom1;
    public GameObject endingRoom2;

    public Transform[] teleportPoints;

    public Collider2D playerCollider;

    public EventSystem es;

    MainCamera cameraVar;

    SpriteRenderer sR;

    float deathCounter;
    float levelTransitionCounter = 0;
    float respawnCounter = 0;
    public float respawnPreviousSoulCounter;
    float tutorialCounter = 0;
    float tempCounter = 0;
    float managerCounter = 0;

    public int currentStage;
    int currentCombatTutorial = 0;
    public int dummyThreeHitComboCounterHuman = 0;
    public int dummyDashAttackCounterSpirit = 0;
    public int dummyLauncherAttackCounterSpirit = 0;
    public int dummyAirAttackCounterHuman = 0;
    public int dummyBulletDestroyedCounter = 0;
    public int dummyBulletBlockedCounter = 0;

    bool activateButtons = false;
    bool justRespawned = false;
    bool playAudioOnce = false;
    bool playTransitionOnce = false;
    bool loopOnceComplete = false;
    bool playIntroOnce = false;
    bool playSuperSpecialOnce = false;
    bool playDeathOnce = false;
    public bool playerPressedBlock = false;
    public bool isSpawning = false;
    public bool levelOneComplete = false;
    public bool levelTwoComplete = false;
    public bool tutorial1;
    public bool tutorial1Complete;
    public bool tutorial2;
    public bool tutorial2Complete;
    public bool tutorial3;
    public bool tutorial3Complete;
    public bool tutorial4;
    public bool tutorial4Complete;
    public bool tutorial4Half;
    public bool tutorial4HalfComplete;
    public bool tutorial5;
    public bool tutorial5Complete;
    public bool tutorial6;
    public bool tutorial6Complete;
    public bool inputEnabled = false;
    public bool playerConfirm = false;
    public bool firstSpiritEncounter = false;
    public bool firstSuperSpecial = false;
    public bool chakraInputDone = false;
    public bool chakraInfoDone = false;
    public bool bulletsTutorial = false;
    public bool humanTutorialPause = false;
    public bool spiritTutorialPause = false;
    public bool introComplete = false;
    public bool closeLoadingScreen = false;
    public bool lastSuperSpecialComplete;
    public bool endingSuperSpecialDone = false;
    public bool playerHadSuperSpecial = false;
    public bool doNotPause = false;
    public bool lastBattleZoneActivated = false;
    public bool cannotUseSuperSpecialInTutorial = false;
    bool comboCompletion = true;
    bool resetValues = false;
    bool inputLoadingScreen;
    public bool invokeOnce = false;
    bool[] combatTutorials = new bool[6];

    public AudioClip backGroundMusic;
    public AudioClip backGroundMusicLevel2;
    public AudioClip loopOnce;
    public AudioClip introVO;
    public AudioClip superSpecialAudio;
    public AudioClip levelTransitionAudio;
    public AudioClip deathMusic;
    AudioSource source;

    public MovieTexture intro;
    public MovieTexture transition;

    public static GameManager instance;

	// Use this for initialization
	void Start () {
        GameManager.instance = this;
        source = GetComponent<AudioSource>();
        tempColor.a = 0;
        tempColor.b = 255;
        tempColor.r = 255;
        tempColor.g = 255;
        tempColor2.a = 0;
        tempColor2.b = 255;
        tempColor2.r = 255;
        tempColor2.g = 255;
        level1Transition.color = tempColor;
        level1Transition.enabled = false;
        gameOverScreen.color = tempColor;
        gameOverScreen.enabled = false;
        //introPart1.color = tempColor2;
        //introPart1.enabled = false;
        finalWhiteScreen.color = tempColor;
        finalWhiteScreen.enabled = false;
        mC = FindObjectOfType<MainCamera> ();
        cameraVar = FindObjectOfType<MainCamera>();
        sR = GetComponent<SpriteRenderer>();
        redScreen.SetActive(false);
        anim = GetComponent<Animator>();
        DeactivateButton(continueButton);
        DeactivateButton(quitToTitleButton);
        currentStage = 1;
        combatTutorials[0] = true;
	}
	
	// Update is called once per frame
	void Update () {

        //game's initial screen, the loading done screen. Will keep there until the player presses the A button to progress
        if(closeLoadingScreen == false){
            doNotPause = true; //this will stop the player from pausing the game in the wrong moments, like cutscenes
            if(Input.GetButtonDown("Submit")){
                closeLoadingScreen = true;
            }
        } else { //when the player leaves the loading screen, the intro cinematic will play
            //time the intro play time, which is near 30 seocnds
            if(managerCounter < 29.5f){

                inputEnabled = true; //inputEnabled is used when the player has to press a button to progress, but it's also handy to fully stop the player from doing any movement

                managerCounter += Time.deltaTime;
                intro.Play();
                if(playIntroOnce == false){
                    source.PlayOneShot(introVO, 1); 
                    playIntroOnce = true;
                }

                //right before ending the cutscene, enables the player to pause
                if(managerCounter > 29.2f){
                    doNotPause = false;
                }

                loadingDone.enabled = false;

            } else {
                introComplete = true;
                inputEnabled = false;
                intro.Stop();
                introCube.SetActive(false);
            }
        }
        //loadingDone.enabled = false;
        //introCube.SetActive(false); //these 3 lines are used to avoid the intro cinematic and load screen for fast tests, and comment the above ones
        //introComplete = true;

        //if the player dies, many checks are made depending on where the player is (tutorial for example) and play the death screen animations
        if(PlayerManager.instance.lifePoints <= -1){
            //stop the stage music
            if(deathCounter < 0.1f){
                source.Stop();
            }
            //then play the death music
            if(playDeathOnce == false && deathCounter > 0.2f){
                source.clip = deathMusic;
                source.volume = 0.8f;
                source.Play();
                source.loop = false;
                playDeathOnce = true;
            }
            playerCollider.enabled = false; //stop the player from receiving further hits
            PlayerManager.instance.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerDead"; //places the player in front of the death screen layer, which is in front of the stage

            GetCameraPosition();

            //if the player dies during a tutorial, then it has to be reset so it will be displayed again after he respawns
            if(tutorial4 == true){
                tutorialInputs[9].enabled = false;
                tutorialInputs[10].enabled = false;
                playerConfirm = false;
                invokeOnce = false;
                inputEnabled = false;
                humanTutorialPause = false;
                spiritTutorialPause = false;
                firstSpiritEncounter = false;
                tutorial4 = false;
            }

            //if the player dies during a tutorial, then it has to be reset so it will be displayed again after he respawns
            if(tutorial4Half== true){
                tutorialInputs[11].enabled = false;
                tutorialInputs[12].enabled = false;
                playerConfirm = false;
                chakraInfoDone = false;
                chakraInputDone = false;
                inputEnabled = false;
                tutorial4Half = false;
            }

            //if the player dies during a tutorial, then it has to be reset so it will be displayed again after he respawns
            if(tutorial6 == true){
                tutorialInputs[13].enabled = false;
                playerConfirm = false;
                invokeOnce = false;
                inputEnabled = false;
                firstSuperSpecial = false;
                tutorial6 = false;
            }

            deathCounter += Time.deltaTime;

            //after 1 second that the player dies, a blank screen is displayed. UI elements are hidden so they are not displayed during the death screen
            if(deathCounter >= 1.3f){
                mC.playerDied = true; //tells the camera to focus on the player, even if it is fixed on a position (battlezones)
                anim.SetBool("GameOverBlankScreen", true);
                gotKey.enabled = false;
                spiritVase.enabled = false;
                UILife.SetActive(false);
                heartSoul.SetActive(false);
                specialSlider.SetActive(false);
                superSpecial.enabled = false;
            }

            //the white screen is not displayed anymore and a red screen is then shown, for cool death effects
            if(deathCounter >= 1.5f){
                anim.SetBool("GameOverBlankScreen", false);
                redScreen.SetActive(true);
                redScreen.transform.position = gameObject.transform.position;
            }

            //the "you are dead" screen slowly appears after a while
            if(deathCounter >= 5f){
                gameOverScreen.enabled = true;
                tempColor.a += Time.deltaTime / 2;
                gameOverScreen.color = tempColor;

            }

            //after 8 seconds the buttons continue and quit are displayed 
            if(deathCounter >= 8f){
                if(activateButtons == false){
                    continueButton.interactable = true;
                    continueButton.GetComponentInChildren<Image>().enabled = true;
                    continueButton.Select();
                    
                    quitToTitleButton.interactable = true;
                    quitToTitleButton.GetComponentInChildren<Image>().enabled = true;

                    activateButtons = true;
                }
            }

        //if the player is not dead
        } else {

            //this method will manage the stages audios
            ManageAudio();

            //this method will teleport the player in certain areas of the game by pressing the 1-5 keys on the keyboard
            TeleportManager();

            //isSpawning is used to tell the BaseForm that the player will respawn and has to go back to the idle animation state
            if(justRespawned == true){
                respawnCounter += Time.deltaTime;
                isSpawning = true;
            }

            //a small time period is given to avoid any problem with the animation, to be sure that the animation will go from dead to idle
            if(respawnCounter >= 2){
                justRespawned = false;
                isSpawning = false;
                respawnCounter = 0;
            }

            //this method controls all tutorial triggers
            Tutorial();

            //if the player triggers the levelOneComplete
            if(levelOneComplete == true){
                inputEnabled = true; //stops any player input
                doNotPause = true; //don't allow the player to pause
                GetCameraPosition(); //stops the camera in position

                //enables the black screen to show and stops the music
                if(levelTransitionCounter <= 0.2f){
                    level1Transition.enabled = true;
                    source.Stop();
                }
                tempColor.a += Time.deltaTime / 2;
                level1Transition.color = tempColor; //slowly appear by changing the alpha
                cameraVar.levelTransition = true;
                levelTransitionCounter += Time.deltaTime;
                //anim.SetBool("Level1Complete", true);
                sR.sortingOrder = 5;
                currentStage = 2;
                PlayerManager.instance.isAttacking = true; //stops the player from being able to attack
                PlayerManager.instance.stageCompleteMovement = true; //moves the player automatically to the right

                //play the transition audio
                if(levelTransitionCounter > 1.6f){
                    if(playTransitionOnce == false){
                        source.PlayOneShot(levelTransitionAudio, 3f);
                        playTransitionOnce = true;
                    }
                }

                //plays the cutscene while also stopping the player movement in the background
                if(levelTransitionCounter >= 3f && levelTransitionCounter < 5){
                    PlayerManager.instance.stageCompleteMovement = false;
                    PlayerManager.instance.velocity.x = 0;
                    level1Transition.enabled = false;
                    transitionCube.SetActive(true);
                    transition.Play();

                }

                PlayerManager.instance.lifePoints = 5; //refills the player health

                //if the player has a key then takes it away, teleports the player to the second level position in the background while also fixing the camera
                if(levelTransitionCounter >= 3.5f && levelTransitionCounter <= 4f){
                    allPlayer.transform.position = levelTwoSpawnPoint.transform.position;
                    if(PlayerManager.instance.gotKey == true){
                        PlayerManager.instance.gotKey = false;
                    }
                    fixCameraPosition = allPlayer.transform.position;
                    fixCameraPosition.z = -1;
                    cameraVar.transform.position = fixCameraPosition;
                }

                //stops the cutscene and shows the screen again
                if(levelTransitionCounter >= 4.3f){
                    tempColor.a -= Time.deltaTime *0.93f;
                    cameraVar.levelTransition = false;
                    if(levelTransitionCounter >= 7f){ //after 7 seconds the player will move again automatically
                        PlayerManager.instance.stageCompleteMovement = true;
                        transition.Stop();
                        transitionCube.SetActive(false);

                    }
                }
            }




            //this if happens when the last super special (Soul Destruction) is executed in the end of the second stage, triggering the final white screen and level change
            if(lastSuperSpecialComplete == true){
                levelTransitionCounter += Time.deltaTime;

                //show the white screen slowly by changing alpha
                if(levelTransitionCounter >= 1 && levelTransitionCounter <= 3){
                    finalWhiteScreen.enabled = true;
                    tempColor.a += Time.deltaTime / 2;
                    finalWhiteScreen.color = tempColor;
                }

                //changes the background assets while the white screen in shown to appear that the player destroyed everything
                if(levelTransitionCounter >= 3.5f){
                    endingRoom1.SetActive(false);
                    endingRoom2.SetActive(true);
                    tempColor.a -= Time.deltaTime;
                    finalWhiteScreen.color = tempColor;

                }

                //after the super special ends, timescale is set to normal again and completes the second level
                if(levelTransitionCounter >= 4.5f){
                    Time.timeScale = 1;
                    Debug.Log("TIMESCALE " + Time.timeScale);
                    endingSuperSpecialDone = true;
                    levelTransitionCounter = 0;
                    levelTwoComplete = true;
                    lastSuperSpecialComplete = false;
                }
            }



            //controls the transition of the second level to the ending&credits screen. This works almost the same way as for the level one
            if(levelTwoComplete == true){
                source.Stop();
                doNotPause = true;
                GetCameraPosition();
                level1Transition.enabled = true; //shows the black screen slowly by changing alpha
                tempColor.a += Time.deltaTime / 2;
                level1Transition.color = tempColor;
                cameraVar.levelTransition = true;
                levelTransitionCounter += Time.deltaTime;
                //anim.SetBool("Level1Complete", true);
                sR.sortingOrder = 5;
                currentStage = 2;
                PlayerManager.instance.isAttacking = true;
                PlayerManager.instance.stageCompleteMovement = true;
                if(levelTransitionCounter >= 3f){
                    PlayerManager.instance.stageCompleteMovement = false;
                    PlayerManager.instance.velocity.x = 0;
                }
                PlayerManager.instance.lifePoints = 5;
                if(levelTransitionCounter >= 3.5f && levelTransitionCounter <= 4f){
                    allPlayer.transform.position = levelTwoSpawnPoint.transform.position;
                    fixCameraPosition = allPlayer.transform.position;
                    fixCameraPosition.z = -1;
                    cameraVar.transform.position = fixCameraPosition;
                    PlayerManager.instance.isAttacking = true;
                }

                //after 7 seconds the ending&credits screen is loaded
                if(levelTransitionCounter >= 7f){
                    PlayerManager.instance.gameIsPaused = true;
                    Application.LoadLevel(2);
                }
            }

            //whenever the transition counter reaches 8, many variables are reset so the player can keep playing and the next transition happens without problems
            if(levelTransitionCounter >= 8f){
                if(levelOneComplete == true){
                    PlayerManager.instance.isAttacking = false;
                    PlayerManager.instance.gameIsPaused = false;
                    PlayerManager.instance.stageCompleteMovement = false;
                    cameraVar.levelTransition = false;
                    level1Transition.enabled = false;
                    inputEnabled = false;
                    sR.sortingOrder = 3;
                    playTransitionOnce = false;
                    levelOneComplete = false;
                    doNotPause = false;
                    levelTransitionCounter = 0;

                //this one is kinda unnecessary, but it's for good measure and be sure nothing will show problems
                } else if(levelTwoComplete == true){
                    PlayerManager.instance.isAttacking = false;
                    PlayerManager.instance.gameIsPaused = false;
                    tempColor.a = 0;
                    cameraVar.levelTransition = false;
                    level1Transition.enabled = false;
                    sR.sortingOrder = 3;
                    levelTwoComplete = false;
                    doNotPause = false;
                    levelTransitionCounter = 0;
                }

            }
        }

	}






    //gets the camera position 
    void GetCameraPosition(){
        gameManagerPosition = cameraVar.transform.position;
        gameManagerPosition.z = -0.5f;
        gameObject.transform.position = gameManagerPosition;
    }






    //deactivates and hide the buttons
    void DeactivateButton(Button button){
        button.interactable = false;
        button.GetComponentInChildren<Image>().enabled = false;
    }

















    //this method will manage the audio played during gameplay
    void ManageAudio(){
        //this audio is the begginig part of the first stage music, which will not loop
        if(loopOnceComplete == false && introComplete == true && playAudioOnce == false){
            source.clip = loopOnce;
            source.Play();
            source.volume = 0.07f;
            playAudioOnce = true;
        }

        //after the first stage beggining part music ends, change variables so the looping part will start playing
        if(!source.isPlaying && playAudioOnce == true){
            loopOnceComplete = true;
            playAudioOnce = false;
        }

        //keeps looping the first stage music
        if(loopOnceComplete == true && playAudioOnce == false && levelOneComplete == false && currentStage == 1){
            source.loop = true;
            source.clip = backGroundMusic;
            source.Play();
            source.volume = 0.07f;
            playAudioOnce = true;
        } 

        //keeps looping the second stage music
        if(loopOnceComplete == true && playAudioOnce == false && levelOneComplete == false && currentStage == 2){
            source.loop = true;
            source.clip = backGroundMusicLevel2;
            source.Play();
            source.volume = 0.07f;
            playAudioOnce = true;
        } 
    }













    //this method manages the teleport inputs and positions that the player will go
    void TeleportManager(){

        if(Input.GetKeyDown ("1")){
            allPlayer.transform.position = teleportPoints[0].position;
        }

        if(Input.GetKeyDown ("2")){
            allPlayer.transform.position = teleportPoints[1].position;
        }

        if(Input.GetKeyDown ("3")){
            allPlayer.transform.position = teleportPoints[2].position;
        }

        if(Input.GetKeyDown ("4")){
            allPlayer.transform.position = teleportPoints[3].position;
        }

        if(Input.GetKeyDown ("5")){
            allPlayer.transform.position = teleportPoints[4].position;
        }

        if(Input.GetKeyDown ("6")){
            allPlayer.transform.position = teleportPoints[5].position;
        }

    }












    //this method will manage all the tutorials present in the game
    void Tutorial(){

        //tutorial for the jump button 
        if(tutorial1 == true){
            doNotPause = true; //stops the player from pausing during input sequences (or problems happen during battles with tutorials)
            tutorialCounter += Time.deltaTime;
            PlayerManager.instance.velocity.x = 0;
            if(tutorialCounter <= 0.5f){
                Invoke("InvokeStopMovement", 0.1f);
            }
            tutorialInputs[0].enabled = true; //show the tutorial panel

            //after a 2 second delay, wait for player input to progress
            if(tutorialCounter >= 2){

                inputEnabled = true;
                //this method will wait for player input (A button) to change the playerConfirm variable and allow progress
                WaitForPlayerInput();
            }

            //resets and changes variables to let the player continue
            if(inputEnabled == true && playerConfirm == true){
                PlayerManager.instance.gameIsPaused = false;
                tutorialCounter = 0;
                tutorialInputs[0].enabled = false;
                inputEnabled = false;
                playerConfirm = false;
                tutorial1 = false;
                tutorial1Complete = true;
                doNotPause = false;
            }

        } 

        //the second tutorial goes through the entire combat mechanics of the game (the tutorial dummy area)
        if(tutorial2 == true){
            tutorialCounter += Time.deltaTime;
            tutorialLeftWall.SetActive(true); //activates blocking walls so the player has to go through the tutorial before progressing
            tutorialRightWall.SetActive(true);

            //combatTutorial will cycle through all the tutorial moments
            if(combatTutorials[0] == true){

                tutorialInputs[1].enabled = true;

                //the tutorial dummy knows when the third attack of the combo lands on him and increments the dummyThreeHitComboCounterHuman counter
                if(dummyThreeHitComboCounterHuman >= 1){
                    tutorialInputs[1].enabled = false;
                    tutorialInputs[2].enabled = true; //goes to the next input

                    //this invoke will increment the combatTutorials counter so it goes to the next tutorial
                    Invoke("CombatTutorialProgress", 0f);
                }


            } else if(combatTutorials[1] == true){
                //reset some useful variables once
                if(resetValues == false){
                    dummyAirAttackCounterHuman = 0;
                    resetValues = true;
                    playerPressedBlock = false;
                }

                //the tutorial dummy knows when a jump attack lands on him and increments the dummyAirAttackCounterHuman counter
                if(dummyAirAttackCounterHuman >= 1){
                    tutorialInputs[2].enabled = false;
                    tutorialInputs[3].enabled = true; //shows the block tutorial panel

                    //if the player presses block then he followed the tutorial and progresses
                    if(Input.GetButtonDown("Block")){
                        playerPressedBlock = true;
                    }

                    //after the player pressed block, continues with the tutorial
                    if(playerPressedBlock == true){

                        bulletsTutorial = true;

                        tutorialInputs[3].enabled = false;
                        tutorialInputs[4].enabled = true; //block 3 bullets tutorial

                        //the tutorial dummy knows when a bullet has been blocked and increments the dummyBulletBlockedCounter counter
                        if(dummyBulletBlockedCounter >= 3){

                            tutorialInputs[4].enabled = false;
                            tutorialInputs[5].enabled = true; //attack 3 bullets tutorial

                            //the tutorial dummy knows when a a bullet has been attacked and increments the dummyBulletDestroyedCounter counter
                            if(dummyBulletDestroyedCounter >= 3){

                                Invoke("CombatTutorialProgress", 0f); //increments the combatTutorials variable so it goes to the next tutorial
                                bulletsTutorial = false;
                            }
                        } else {
                            dummyBulletDestroyedCounter = 0; //keeps reseting the variable so only when the destroy bullets tutorial is triggered it will count up
                        }

                    }

                }

            //tutorial to make the player change to spirit form
            } else if(combatTutorials[2] == true){

                tutorialInputs[5].enabled = false;
                tutorialInputs[6].enabled = true;
                pressToContinuePanel.enabled = false;

                //after 5 seconds, to make the player see and understand the change to spirit form, it keeps moving on
                if(tutorialCounter >= 5 && PlayerManager.instance.isSpiritForm == true){
                    Invoke("CombatTutorialProgress", 0f);
                }

            //tutorial for the Phantom Dash attacks
            } else if(combatTutorials[3] == true){

                //reset the values to avoid the player from dashing before this tutorial
                if(resetValues == false){
                    dummyDashAttackCounterSpirit = 0;
                    resetValues = true;
                }

                tutorialInputs[6].enabled = false;
                tutorialInputs[7].enabled = true;

                //the tutorial dummy knows when the player dashes through it and increments the dummyDashAttackCounterSpirit counter
                if(dummyDashAttackCounterSpirit >= 3){

                    tutorialInputs[7].enabled = false;
                    Invoke("CombatTutorialProgress", 0f);
                }

            //tutorial for the Armstrong Launcher
            } else if(combatTutorials[4] == true){
                //reset the values ot avoid the player from using the armstrong launcher before this tutorial
                if(resetValues == false){
                    dummyLauncherAttackCounterSpirit = 0;
                    resetValues = true;
                }
               
                tutorialInputs[8].enabled = true;

                //the tutorial dummy knows when the player uses the launcher on it and increments the dummyDashAttackCounterSpirit counter
                if(dummyLauncherAttackCounterSpirit >= 1){
                   
                    tutorialInputs[8].enabled = false;
                    Invoke("CombatTutorialProgress", 0f);
                }

            //finish the tutorial but show that the player can see the combos at any time at the combo list
            } else if(combatTutorials[5] == true){

                tutorialInputs[15].enabled = true;

                tutorialLeftWall.SetActive(false); //deactivate the walls
                tutorialRightWall.SetActive(false);
                tutorial2Complete = true; //this will trigger in the MainCamera to make the camera follow the player again

                //after showing the "press pause to see the combo list" panel, turn it off
                if(tutorialCounter >= 5){
                    tutorialInputs[15].enabled = false;
                    tutorial2 = false;
                }

            }

        }

        //this controls the tutorial when the player faces the first enemies, human and spirit. The gametime freezes using the PauseMenu class
        if(tutorial4 == true){

            //if the first spirit encounter is false, then the player is battling human enemies
            //when the spirit enemy spawns, the BattlePointTrigger changes the firstSpiritEncounter variable, triggering the next step in the tutorial
            if(firstSpiritEncounter == false){

                tutorialInputs[9].enabled = true;

                //wait for player input before letting the player continue the fight
                if(playerConfirm == false){
                    pressToContinuePanel.enabled = true;
                } else {
                    pressToContinuePanel.enabled = false;
                }

            //when the player faces the first enemy spirit
            } else {

                tutorialInputs[9].enabled = false;
                tutorialInputs[10].enabled = true;

                //stops the game again and waits for player input before progressing with the fight
                if(invokeOnce == false){
                    pressToContinuePanel.enabled = true;
                } else {
                    pressToContinuePanel.enabled = false;
                }
            }

            //if the battlezone is complete, this variable will be true. Then resets some handy variables
            if(tutorial4Complete == true){
                tutorialInputs[10].enabled = false;
                tutorial4 = false;
                playerConfirm = false;
                tutorialCounter = 0;

            }

        }

        //this tutorial shows info about the chakra power. The gametime freezes using the PauseMenu class
        if(tutorial4Half == true){

            //wait for player input to progress in the tutorial
            if(chakraInfoDone == false){
                tutorialInputs[11].enabled = true;
            } else {
                tutorialInputs[11].enabled = false;
                tutorialInputs[12].enabled = true;
            }

            //hide the panel when the player press the input
            if(playerConfirm == false){
                pressToContinuePanel.enabled = true;
            } else {
                pressToContinuePanel.enabled = false;
            }
         
            //if the battlezone is complete, this variable will be true. Then resets some handy variables
            if(tutorial4HalfComplete == true){

                tutorialInputs[12].enabled = false;
                tutorial4Half = false;
                playerConfirm = false;
                tutorialCounter = 0;
                
            }
            
        }

        //this tutorial is played after the player unlocks the first door, where he will be frozen and an audio will play talking about the super special (Soul Destruction)
        if(tutorial5 == true){

            doNotPause = true; //not letting the player pause the game in this sequence

            tutorialCounter += Time.deltaTime;

            //play the audio once
            if(playSuperSpecialOnce == false){
                source.PlayOneShot(superSpecialAudio, 10.0f);
                playSuperSpecialOnce = true;
            }

            PlayerManager.instance.velocity.x = 0;
            if(tutorialCounter <= 0.2f){
                playerConfirm = false;
                Invoke("InvokeStopMovement", 0.1f); //this invoke will stop the player movement after a little moment
            }

            //allows the player to press A to progress
            if(tutorialCounter >= 2){
                pressToContinuePanel.enabled = true;
                inputEnabled = true;
                WaitForPlayerInput();
            }

            //after pressing to progress, reset some variables to let the player move and pause
            if(inputEnabled == true && playerConfirm == true){
                PlayerManager.instance.gameIsPaused = false;
                tutorialCounter = 0;
                pressToContinuePanel.enabled = false;
                inputEnabled = false;
                playerConfirm = false;
                tutorial5 = false;
                tutorial5Complete = true; //this will trigger in the MainCamera to make the camera follow the player again
                doNotPause = false;
            }
            
        }

        //this tutorial freezes the game when the player enters the battlezone with the super special (Soul Destruction) scroll pickup, which will teach how to use it
        //The gametime freezes using the PauseMenu class
        if(tutorial6 == true){

            tutorialInputs[13].enabled = true;
            if(playerConfirm == false){
                pressToContinuePanel.enabled = true;
            } else {
                pressToContinuePanel.enabled = false;
                cannotUseSuperSpecialInTutorial = false;
            }

            //when the battlezone is complete, disable the tutorial image and resets some variables
            if(tutorial6Complete == true){
                tutorialInputs[13].enabled = false;
                tutorial6 = false;
                playerConfirm = false;
                tutorialCounter = 0;
                
            }
            
        }

    }


    //this method will progress in the combatTutorial counter so it triggers the next tutorial. Also resets some handy variables
    void CombatTutorialProgress(){
        combatTutorials[currentCombatTutorial] = false;
        currentCombatTutorial++;
        combatTutorials[currentCombatTutorial] = true;
        resetValues = false;
        invokeOnce = false;
        playerConfirm = false;
        inputEnabled = false;
        tutorialCounter = 0;
    }









    //this method is purely to only wait for player input, which is used many times during the tutorial sequence
    void WaitForPlayerInput(){
        if(Input.GetButtonDown("Submit")){
            playerConfirm = true;
            PlayerManager.instance.gameIsPaused = false;
        }
    }









    //this method is invoked some times during the tutorial to stop the player from moving while tutorial is being shown
    void InvokeStopMovement(){
        PlayerManager.instance.gameIsPaused = true;
    }









    //this method is called when the player chooses the "continue" option after he dies
    public void ContinueGame(){
        Invoke("LoadLastCheckpoint", 2f); //after he chooses the option, there is a small delay so it's not too abrupt
        es.enabled = false; //turn of the EventSystem so the player can't move or choose other option

    }
    
    







    //this method is called when the player chooses the "quit" option after he dies
    public void QuitGame(){
        Application.LoadLevel(0);
    }









    //this method takes care of many variables that will be reset when the player respawns
    void LoadLastCheckpoint(){
        PlayerManager.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; //change the player back to his layer
        source.Stop(); //stop the death music
        playAudioOnce = false;
        playDeathOnce = false; 
        isSpawning = true;
        justRespawned = true;
        es.enabled = true;
        activateButtons = false;
        deathCounter = 0;
        tempColor.a = 0;
        DeactivateButton(continueButton);
        DeactivateButton(quitToTitleButton);
        playerCollider.enabled = true;
        mC.playerDied = false;
        redScreen.SetActive(false);
        gameOverScreen.enabled = false;
        gameOverScreen.color = tempColor;
        gotKey.enabled = true;
        UILife.SetActive(true);
        heartSoul.SetActive(true); //activate all UI elements again
        spiritVase.enabled = true;
        specialSlider.SetActive(true);
        superSpecial.enabled = true;
        PlayerManager.instance.lifePoints = 5;
        PlayerManager.instance.soulsCounter = respawnPreviousSoulCounter; //restore the previous amount of souls the player had before
        PlayerManager.instance.hasSuperSpecial = playerHadSuperSpecial; //restore if the player had a super special scroll
        allPlayer.transform.position = respawnLocation;
    }

}
