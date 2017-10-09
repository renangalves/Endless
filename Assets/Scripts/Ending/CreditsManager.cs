using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour {

    float counter;
    float soundFadeTimer = 33;
    float fadeRate = 10;
    float soundStopTimer = 38;

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

        if(counter >= soundStopTimer)
        {
            credits.Stop();
            Application.LoadLevel(0);
        }

        if(counter >= soundFadeTimer)
        {
            source.volume -= Time.deltaTime / fadeRate;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Application.LoadLevel(0);
        }
	}
}
