using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour {

    float counter;

    public GameObject creditsCube;

    public AudioClip creditsSound;
    AudioSource source;

    public MovieTexture credits;

	// Use this for initialization
	void Start () {
	
        source = GetComponent<AudioSource>();
        source.clip = creditsSound;
        source.volume = 0.50f;
        source.Play();

	}
	
	// Update is called once per frame
	void Update () {

        counter += Time.deltaTime;

        creditsCube.SetActive(true);
        credits.Play();

        if(counter >= 38){
            credits.Stop();
            Application.LoadLevel(0);
        }

        if(counter >= 33){
            source.volume -= Time.deltaTime / 10;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Application.LoadLevel(0);
        }
	}
}
