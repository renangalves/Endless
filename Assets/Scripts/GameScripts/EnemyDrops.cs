using UnityEngine;
using System.Collections;

public class EnemyDrops : MonoBehaviour {

    SpriteRenderer sR;
    BoxCollider2D trigger;
    AudioSource source;
    public AudioClip sound;
    public ParticleSystem collectFeedback;
    Vector3 particlePositionFix;


	void Start () {
        source = GetComponent<AudioSource>();
        sR = GetComponent<SpriteRenderer>();
        trigger = GetComponent<BoxCollider2D>();
	}
	

	void Update () {
	
	}

    //when the player collides with a pickup
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.isTrigger != true) {
			if (coll.CompareTag ("Player")) {
                if(gameObject.CompareTag("Soul")){ //if it's a soul, play the particles and sound, then increases the counter 
                    CollectChanges();
                    PlayerManager.instance.soulsCounter += 0.4f;
                    PlayerManager.instance.acquiredSoul = true; //tells the PlayerManager to play the particle of the jar once

                //if it's health, only collects if the player health is below 5
                } else if(gameObject.CompareTag("HealthPickUp")){
                    if(PlayerManager.instance.lifePoints < 5){
                        CollectChanges();
                        PlayerManager.instance.lifePoints++;
                    }

                }
				
			}
		}
		
	}



    //controls the changes when the player collects a pickup, playing particle and sound before it is destroyed
    void CollectChanges(){
        sR.enabled = false; //disables the renderer so the sprite is not shown anymore
        trigger.enabled = false; //the trigger is disabled so the player will not collect again
        source.PlayOneShot(sound, 0.8f);
        Destroy(gameObject, 2f); 
        particlePositionFix = gameObject.transform.position; 
        particlePositionFix.y += 2; //correct the positioning of the particle
        Instantiate(collectFeedback, particlePositionFix, gameObject.transform.rotation);
    }
}
