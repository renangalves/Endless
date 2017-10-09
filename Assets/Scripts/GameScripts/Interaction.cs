using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{

    public GameObject interactTarget;
    bool activateInteract = false;
    bool startTutorialCounting = false;
    bool displayForFixedTime = false;
    public bool playOneAudio = false;
    AudioSource source;
    public AudioClip audio1;
    public AudioClip audio2;
    public Animator doorLock;
    public Text text1;
    public Text text2;
    int interactCounter = 0;
    int timeToDisplay;
    float counter = 0;

    //The code here is wet and I could have done a better job without using tags.

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    //controls all interactions
    void Update()
    {
        //activate interact will be true when the player is in an interactable trigger
        if (activateInteract == true)
        {

            //if the player interacts with doors in the first level
            if (gameObject.CompareTag("LevelOneDoorTrigger"))
            {

                DoorKeyCheck(); //this method controls doors locked with keys, which will check if the player has a key and play audios


            //if the player interacts with the daughter picture
            } else if (gameObject.CompareTag("LevelOnePictureTrigger"))
            {
                //shows the picture of the daughter and freezes the player in place until he interacts again
                if (Input.GetButtonDown("Interact") && interactCounter == 0)
                {
                    interactTarget.SetActive(true);
                    PlayerManager.instance.gameIsPaused = true;
                    interactCounter++;

                //lets the player continue again
                } else if (Input.GetButtonDown("Interact") && interactCounter >= 1)
                {
                    interactTarget.SetActive(false);
                    PlayerManager.instance.gameIsPaused = false;
                    interactCounter = 0;
                }





            //if the player interacts with the radio in the room
            } else if (gameObject.CompareTag("LevelOneRadio1Trigger"))
            {
                //plays the first audio
                if (Input.GetButtonDown("Interact") && interactCounter == 0)
                {
                    if (!source.isPlaying)
                    {
                        source.clip = audio1;
                        source.Play();
                    }
                    interactCounter++;
                //here will control if an audio file is being played, if it is then it will stop it and the player can interact for another audio
                } else if (Input.GetButtonDown("Interact") && interactCounter == 1)
                {
                    if (source.isPlaying)
                    {
                        source.Stop();
                        source.clip = audio2;
                        interactCounter = 2;
                    } else
                    {
                        source.clip = audio2;
                        source.Play();
                        interactCounter = 3;
                    }

                //if the player didn't stop the previous audio then it will play normally the next one if he interacts again
                } else if (Input.GetButtonDown("Interact") && interactCounter == 2)
                {
                    source.Stop();
                    source.Play();
                    interactCounter = 3;
                }

                //after the audios have been played, disappear with the interact zone so it's not interactable anymore
                if (!source.isPlaying && interactCounter >= 3)
                {
                    GotOutOfInteractZone(); //method that disables the interact trigger so it won't be interactable anymore
                }





            //if the player interacts with the teddy bear
            } else if (gameObject.CompareTag("LevelOneTeddyBearTrigger"))
            {
                //an audio will play once and then it will not be interactable anymore
                if (Input.GetButtonDown("Interact") && interactCounter == 0 && !source.isPlaying)
                {
                    source.clip = audio1;
                    source.Play();
                    GotOutOfInteractZone();
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;

                    interactCounter++;
                } 




            //if the player touches the level complete trigger then changes a variable in the GameManager that will trigger the transitions
            } else if (gameObject.CompareTag("Level1CompleteTrigger"))
            {
                GameManager.instance.levelOneComplete = true;



            //level 2 has a special door which opens only from one side
            } else if (gameObject.CompareTag("Level2DoorTrigger1"))
            {
                //if the player interacts with the left side of the door, a panel will appear giving a hint to the player
                if (Input.GetButtonDown("Interact") && interactCounter == 0)
                {

                    GameManager.instance.tutorialInputs[14].enabled = true; //will show this panel for a brief moment
 
                    source.PlayOneShot(audio1, 0.5f); //also an audio will be played
                    displayForFixedTime = true;
                    timeToDisplay = 5;
                    interactCounter++;

                } else if(interactCounter > 0){
                    GotOutOfInteractZone(); //the door won't be interactable anymore
                }



            //if the player interacts with the right side of the door, it will open
            } else if (gameObject.CompareTag("Level2DoorTrigger2"))
            {
                if(Input.GetButtonDown("Interact") && interactCounter == 0){
                    doorLock.SetBool("LockOpen", true);
                    interactTarget.GetComponent<BoxCollider2D>().enabled = false;
                    source.PlayOneShot(audio1, 0.5f);
                    interactCounter++;
                } else if(interactCounter > 0){
                    GotOutOfInteractZone();
                }


            
            //if the player interacts with the second door in the second level, it will check if the player has the key
            } else if (gameObject.CompareTag("Level2DoorTriggerKey"))
            {
                DoorKeyCheck();


            //if the player touches the level complete trigger then changes a variable in the GameManager that will trigger the transitions
            } else if (gameObject.CompareTag("Level2CompleteTrigger"))
            {
                GameManager.instance.levelTwoComplete = true;

            }


            //this if will control the other radios scattered through the game. They have their own audio files added in the inspector and it will play here when the player interacts
            if(playOneAudio == true){
                if (Input.GetButtonDown("Interact") && interactCounter == 0 && !source.isPlaying)
                {
                    source.clip = audio1;
                    source.Play();
                    GotOutOfInteractZone();
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    
                    interactCounter++;
                } 
            }
            

        }





        //some interactions have a timer, it will be counted down here
        /*if(startTutorialCounting == true){
            counter += Time.deltaTime;
            interactCounter++;
            if (counter >= 5f)
            {
                text1.enabled = false;
                text2.enabled = false;
                startTutorialCounting = false;
            }
        }*/


        //this if will count down the visual panel when the player interacts with the special door in the second level, showing the panel for a time
        if(displayForFixedTime == true){
            counter += Time.deltaTime;
            if(counter >= timeToDisplay){
                GameManager.instance.tutorialInputs[14].enabled = false;
            }
        }

    }









    //this method will check if the player has a key, which will open the door and make audio feedbacks
    void DoorKeyCheck(){
        if (Input.GetButtonDown("Interact"))
        {
            //if the player has a key, disable the door collider and play audio feedback
            if (PlayerManager.instance.gotKey == true)
            {
                doorLock.SetBool("LockOpen", true);
                PlayerManager.instance.gotKey = false;
                source.PlayOneShot(audio2, 0.8f);
                //gameObject.SetActive(false);
                Invoke("DeactivateDoorCollider", 0.5f);
                GotOutOfInteractZone();

            //if the player doesn't have a key, only play the audio feedback
            } else
            {
                source.PlayOneShot(audio1, 0.8f);
            }
        }
    }










    //this method will only deactive the collider of the target
    void DeactivateDoorCollider(){
        interactTarget.SetActive(false);
    }






    //whenever the player touches an interactable area, show a button feedback above the character's head
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerManager.instance.isInInteractZone = true;
            activateInteract = true;
        }
    }






    //if the player leaves an interact zone, disable the feedback
    void OnTriggerExit2D(Collider2D coll)
    {
        GotOutOfInteractZone();
    }






    //this method controls the feedback appearance above the character's head
    void GotOutOfInteractZone()
    {
        PlayerManager.instance.isInInteractZone = false;
        activateInteract = false;
    }

}
