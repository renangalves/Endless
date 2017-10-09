using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    Animator anim;

    public Button startButton;
    public Button creditsButton;
    public Button quitButton;
    public Image gameTitle;
    public Image loadingScreen;
    public SpriteRenderer background;
    public EventSystem es;

    public AudioClip menuAudio;
    public AudioClip[] buttonsAudio;
    AudioSource source;

    public GameObject rain;

    Color tempColor;

    bool activateButtons = false;
    bool newGame = false;

    float counter;
    float loadingCounter;

	
	void Start () {
	
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        source.clip = menuAudio;

        startButton.interactable = false;
        startButton.GetComponentInChildren<Image>().enabled = false;
        //startButton.GetComponentInChildren<Text>().color = Color.clear;

        creditsButton.interactable = false;
        creditsButton.GetComponentInChildren<Image>().enabled = false;

        quitButton.interactable = false;
        quitButton.GetComponentInChildren<Image>().enabled = false;
        //quitButton.GetComponentInChildren<Text>().color = Color.clear;

        gameTitle.enabled = false;

        tempColor.a = 0;
        tempColor.b = 255;
        tempColor.r = 255;
        tempColor.g = 255;
        loadingScreen.color = tempColor;
        loadingScreen.enabled = false;
	}
	
	//after the VFS logo appears, it will show the background and the buttons
	void Update () {
	
        counter += Time.deltaTime;

        if(counter > 5){
            background.enabled = false;

            //activate the buttons and play the main menu music
            if(activateButtons == false){
                source.Play();
                source.loop = true;
                source.volume = 0.30f;

                startButton.interactable = true;
                startButton.GetComponentInChildren<Image>().enabled = true;
                startButton.Select();

                creditsButton.interactable = true;
                creditsButton.GetComponentInChildren<Image>().enabled = true;

                quitButton.interactable = true;
                quitButton.GetComponentInChildren<Image>().enabled = true;

                gameTitle.enabled = true;

                activateButtons = true;
            }
           

        }

        //after a little delay, to not be too abrupt, the loading screen will slowly appear in front
        if(newGame == true){
            loadingCounter += Time.deltaTime;
            if(loadingCounter >= 1.5f){
                rain.SetActive(false);
                loadingScreen.enabled = true;
                tempColor.a += Time.deltaTime;
                loadingScreen.color = tempColor;

            }
        }


 
	}





    //this will trigger the event trigger which will play the sound when the player selects the button
    public void PlayButtonSound(){
        source.PlayOneShot(buttonsAudio[0], 2.5f);
    }




    //this will trigger the event trigger which will play the sound when the player chooses the button
    public void PlayButtonSoundSelect(){
        source.PlayOneShot(buttonsAudio[1], 1f);
    }





    //if the player selects this button, it will load the game after 3 seconds, to show the loading screen first
    public void NewGame(){
        Invoke("StartGame", 3f);
        newGame = true;
        es.enabled = false;
    }





    //if the player selects this button, it will show the credits screen
    public void Credits(){
        Invoke("ShowCredits", 2f);
        es.enabled = false;
    }





    //if the player selects this button, it will quit the game
    public void QuitGame(){
        Debug.Log("QUITTING TO DESKTOP");
        Application.Quit();
    }





    void StartGame(){
        Application.LoadLevel(1);
    }


    void ShowCredits(){
        Application.LoadLevel(3);
    }

}
