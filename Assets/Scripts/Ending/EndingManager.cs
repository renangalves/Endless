using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {

    public Image choiceBackground;
    public Image choicesLeft;
    public Image choicesRight;

    float endingCounter;

    bool goodEnding;
    bool badEnding;
    bool playSoundOnce = false;
    bool playBattleOnce = false;
    bool playGoodBadEnding = false;
    bool playGoodBadDaughterEnding = false;
    bool choiceDone = false;

    public GameObject endingCube;
    public GameObject creditsCube;
    public GameObject daughterEndingCube;
    public GameObject goodEndingText;
    public GameObject badEndingText;

    Color tempColor;

    public Animator leftArrow;
    public Animator rightArrow;

    public AudioClip creditsSound;
    public AudioClip monsterBattleSound;
    public AudioClip goodEndingSound;
    public AudioClip badEndingAudio;
    public AudioClip goodEndingDaughter;
    public AudioClip badEndingDaughter;
    AudioSource source;

    public MovieTexture ending;
    public MovieTexture credits;
    public MovieTexture daughter;

	// Use this for initialization
	void Start () {
	
        source = GetComponent<AudioSource>();
        tempColor.a = 0;
        tempColor.b = 255;
        tempColor.r = 255; //setting alpha to 0 to animate a fade in by code
        tempColor.g = 255;
        choiceBackground.color = tempColor;
        choiceBackground.enabled = false;
        choicesLeft.color = tempColor;
        choicesLeft.enabled = false;
        choicesRight.color = tempColor;
        choicesRight.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
        endingCounter += Time.deltaTime;

        //enable background and both choices at the beggining of the ending screen
        if(goodEnding == false && badEnding == false){
            choiceBackground.enabled = true;
            choicesLeft.enabled = true;
            choicesRight.enabled = true;
        }

        //show the background and choices smoothly by increasing alpha
        tempColor.a += Time.deltaTime / 2;
        choiceBackground.color = tempColor;
        choicesLeft.color = tempColor;
        choicesRight.color = tempColor;

        //checks if the player pressed the X button, given a 3 second period to avoid careless inputs, and after the choice 
        //the variable choiceDone switches to avoid changes after the input
        if (Input.GetButtonDown("Slash") && endingCounter >= 3 && choiceDone == false)
        {
            leftArrow.SetBool("LeftSelected", true);
            rightArrow.SetBool("RightNotSelected", true);
            goodEnding = true; //good ending has been selected
            choiceDone = true; //avoid any other choices
            endingCounter = 0; //reset endingCounter for a new countdown now that the choice has been done
            Invoke("ChoiceSelected", 3f); //invoke ChoiceSelected to disappear with the background, play the ending and choices after 3 seconds
            Debug.Log("GOOD ENDING");
        }

        //checks if the player pressed the B button, given a 3 second period to avoid careless inputs, and after the choice 
        //the variable choiceDone switches to avoid changes after the input
        if (Input.GetButtonDown ("Block") && endingCounter >= 3 && choiceDone == false) 
        {
            leftArrow.SetBool("LeftNotSelected", true);
            rightArrow.SetBool("RightSelected", true);
            badEnding = true; //bad ending has been selected
            choiceDone = true; //avoid any other choices
            endingCounter = 0; //reset endingCounter for a new countdown now that the choice has been done
            Invoke("ChoiceSelected", 3f); //invoke ChoiceSelected to disappear with the background, play the ending and choices after 3 seconds
            Debug.Log("BAD ENDING");
        }

        //after 8 seconds, call the method PlayEnding to display the good ending final text and the credits
        if(goodEnding == true && endingCounter >= 8)
        {
            if(endingCounter > 5 && playGoodBadEnding == false){
                source.PlayOneShot(goodEndingSound, 1);
                playGoodBadEnding = true;
            }
            PlayEnding(true);

        }

        //after 8 seconds, call the method PlayEnding to display the bad ending final text and the credits
        if(badEnding == true && endingCounter >= 8)
        {
            if(endingCounter > 5 && playGoodBadEnding == false){
                source.PlayOneShot(badEndingAudio, 1);
                playGoodBadEnding = true;
            }
            PlayEnding(false);
            
        }

	}



    //disappear with the background and the choices, then invoke the method that will play the ending after 2 seconds
    void ChoiceSelected(){
        choiceBackground.enabled = false;
        choicesLeft.enabled = false;
        choicesRight.enabled = false;
        Invoke("ActivateEndingCube", 2f);
        if(playBattleOnce == false){
            source.PlayOneShot(monsterBattleSound, 1);
            playBattleOnce = true;
        }
    }


    //activates the cube that plays the ending cinematic, and also resets the endingCounter for further use
    void ActivateEndingCube(){
        endingCube.SetActive(true);
        ending.Play();
        endingCounter = 0;
    }


    //this method will control the final text appearance, play the credits and play the daughter cinematic
    void PlayEnding(bool choice){
        ending.Stop();
        endingCube.SetActive(false);
        if(choice == true){
            goodEndingText.SetActive(true); //show the good ending text
            Debug.Log("I WILL SAVE MY DAUGHTER");
        } else {
            badEndingText.SetActive(true); //show the bad ending text
            Debug.Log("I WILL KILL THEM ALL");
        }

        //after 15 seconds, play the credits
        if(endingCounter >= 15){
            creditsCube.SetActive(true);
            credits.Play();
            if(playSoundOnce == false){ //play the ending music only once
                source.clip = creditsSound;
                source.volume = 0.50f;
                source.Play();
                playSoundOnce = true;
            }
        }

        //after 47 seconds, slowly lower the volume of the music for the last scene
        if(endingCounter >= 47){
            source.volume -= Time.deltaTime / 10;
        }

        //after 53 seconds, stop the credits and then play the daughter final scene
        if(endingCounter >= 53){
            credits.Stop();
            creditsCube.SetActive(false);
            daughterEndingCube.SetActive(true);
            daughter.Play();
            if(endingCounter >= 56f && choice == false && playGoodBadDaughterEnding == false){
                source.Stop();
                source.clip = badEndingDaughter;
                source.volume = 1f;
                source.Play();
                playGoodBadDaughterEnding = true;
            } else if(endingCounter >= 59 && choice == true && playGoodBadDaughterEnding == false){
                source.Stop();
                source.clip = goodEndingDaughter;
                source.volume = 1f;
                source.Play();
                playGoodBadDaughterEnding = true;
            }
            if(endingCounter >= 62){
                daughter.Stop();
                Application.LoadLevel(0);
            }
        }
    }
}
