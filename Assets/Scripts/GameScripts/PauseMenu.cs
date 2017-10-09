using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    bool isPaused = false;
    bool activateButtons = false;
    bool isInComboList = false;

    Animator anim;

    public Image comboList;

    public Button continueButton;
    public Button comboListButton;
    public Button quitButton;

    public GameObject pauseTitle;

    float positioningCounter = 0;

    Vector3 pausePosition;

    MainCamera cameraVar;

    public AudioClip[] buttonsAudio;
    AudioSource source;

    public EventSystem eS;


	void Start () {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        cameraVar = FindObjectOfType<MainCamera>();
        DeactivateButton(continueButton);
        DeactivateButton(comboListButton);
        DeactivateButton(quitButton);
        pauseTitle.SetActive(false);
	}
	
	
	void Update () {

        //when the player presses the pause button it will set the timescale to 0, if all conditions are met
        if(Input.GetButtonDown("Pause") && PlayerManager.instance.lifePoints > -1 && GameManager.instance.introCube == true && isInComboList == false && GameManager.instance.doNotPause == false){

            isPaused = !isPaused;
            PlayerManager.instance.gameIsPaused = isPaused; //stops all player movement if the game is paused

            //set the camera to stop and the timescale to 0
            if(isPaused){
                pausePosition = cameraVar.transform.position;
                pausePosition.z = -0.5f;
                gameObject.transform.position = pausePosition;
                anim.SetBool("isPaused", true);
                Time.timeScale = 0;
            } else {
                anim.SetBool("isPaused", false);
                Time.timeScale = 1;
            }
        }

        TutorialPauses(); //all pauses during the tutorials are managed here

        //checks if the player leaves the combo list, so the eventSystem is activated again and the button is selected
        if(isInComboList == true){
            if (Input.GetButtonDown ("Block")) 
            {
                comboList.enabled = false;
                isInComboList = false;
                eS.enabled = true;
                comboListButton.Select();
            }
        }

        //activates the buttons when it's paused
        if(isPaused){
            if(activateButtons == false){
                ActivateButton(continueButton, true);
                ActivateButton(comboListButton, false);
                ActivateButton(quitButton, false);
                activateButtons = true;
                pauseTitle.SetActive(true);
            }

        //deactivates the buttons when it's not paused
        } else {
            DeactivateButton(continueButton);
            DeactivateButton(comboListButton);
            DeactivateButton(quitButton);
            activateButtons = false;
            pauseTitle.SetActive(false);
        }
	
	}









    //during the tutorial stops the player will need to press A which will enable them to continue playing
    void SetInputToFalse(){
        GameManager.instance.inputEnabled = false;
    }








    //this method only activates the buttons and make them appear when it's paused
    void ActivateButton(Button button, bool isSelected){
        if(isSelected == true){
            button.interactable = true;
            button.GetComponentInChildren<Image>().enabled = true;
            button.Select();
        } else {
            button.interactable = true;
            button.GetComponentInChildren<Image>().enabled = true;
        }
    }










    //this method only deactivates the buttons when the player unpause the game
    void DeactivateButton(Button button){
        button.interactable = false;
        button.GetComponentInChildren<Image>().enabled = false;
    }








    //if the player selects this button, it will continue the game normally
    public void ContinueButton(){
        isPaused = false;
        anim.SetBool("isPaused", false);
        PlayerManager.instance.gameIsPaused = isPaused;
        PlayerManager.instance.pauseNotJumpCounter = 0;
        Time.timeScale = 1;
    }








    //if the player chooses the combo list, show the combo list panel
    public void ComboListButton(){
        comboList.enabled = true;
        isInComboList = true;
        eS.enabled = false;
    }







    //if the player chooses the quit game, go back to the main menu
    public void QuitGame(){
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }








    //this will trigger the event trigger which will play the sound when the player selects the button
    public void PlayButtonSound(){
        if(isPaused){
            source.PlayOneShot(buttonsAudio[0], 0.5f);
        }
    }







    //this will trigger the event trigger which will play the sound when the player chooses the button
    public void PlayButtonSoundSelect(){
        if(isPaused){
            source.PlayOneShot(buttonsAudio[1], 0.5f);
        }
    }









    //this method manages all pauses during tutorials
    void TutorialPauses(){


        //The comment on the first one applies to the others

        //if the player triggers the tutorial
        if(GameManager.instance.tutorial4 == true && GameManager.instance.humanTutorialPause == false){
            positioningCounter += Time.deltaTime;

            //after a small delay it will stop time until the player press the A input
            if(positioningCounter >= 0.5f){
                PlayerManager.instance.isAttacking = true;
                GameManager.instance.inputEnabled = true;
                Time.timeScale = 0;
            }

            //after the player presses the A input, it will let the player move again and the time will go back to normal
            if(Input.GetButtonDown("Submit") && positioningCounter >= 0.5f && isPaused == false){
                Time.timeScale = 1;
                GameManager.instance.playerConfirm = true;
                PlayerManager.instance.isAttacking = false;
                Invoke("SetInputToFalse", 0.2f);
                positioningCounter = 0;
                GameManager.instance.humanTutorialPause = true;
            }
        }
        
        if(GameManager.instance.tutorial4 == true && GameManager.instance.firstSpiritEncounter == true && GameManager.instance.spiritTutorialPause == false){
            positioningCounter += Time.deltaTime;
            if(positioningCounter >= 0.5f){
                PlayerManager.instance.isAttacking = true;
                GameManager.instance.inputEnabled = true;
                Time.timeScale = 0;
            }
            if(Input.GetButtonDown("Submit") && positioningCounter >= 0.5f && isPaused == false){
                Time.timeScale = 1;
                GameManager.instance.invokeOnce = true;
                PlayerManager.instance.isAttacking = false;
                Invoke("SetInputToFalse", 0.2f);
                positioningCounter = 0;
                GameManager.instance.spiritTutorialPause = true;
            }
        }
        
        if(GameManager.instance.tutorial4Half == true && GameManager.instance.chakraInfoDone == false && GameManager.instance.chakraInputDone == false){
            positioningCounter += Time.deltaTime;
            if(positioningCounter >= 0.5f){
                PlayerManager.instance.isAttacking = true;
                GameManager.instance.inputEnabled = true;
                Time.timeScale = 0;
            }
            if(Input.GetButtonDown("Submit") && isPaused == false){
                GameManager.instance.chakraInfoDone = true;
            }
            
            if(GameManager.instance.chakraInfoDone == true){
                if(Input.GetButtonDown("Submit") && isPaused == false){
                    Time.timeScale = 1;
                    PlayerManager.instance.isAttacking = false;
                    Invoke("SetInputToFalse", 0.2f);
                    positioningCounter = 0;
                    GameManager.instance.playerConfirm = true;
                    GameManager.instance.chakraInputDone = true;
                }
            }
        }
        
        if(GameManager.instance.tutorial6 == true && GameManager.instance.firstSuperSpecial == false){
            positioningCounter += Time.deltaTime;
            if(positioningCounter >= 0.5f){
                PlayerManager.instance.isAttacking = true;
                GameManager.instance.inputEnabled = true;
                Time.timeScale = 0;
            }
            if(Input.GetButtonDown("Submit") && positioningCounter >= 0.5f && isPaused == false){
                Time.timeScale = 1;
                GameManager.instance.playerConfirm = true;
                PlayerManager.instance.isAttacking = false;
                Invoke("SetInputToFalse", 0.2f);
                positioningCounter = 0;
                GameManager.instance.firstSuperSpecial = true;
            }
        }

    }



}
