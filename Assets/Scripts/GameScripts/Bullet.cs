using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Bullet : MonoBehaviour
{

	SwordHitFeedback swordHit;
    public ParticleSystem explosion;
    GameObject particle;
    CircleCollider2D triggerBox;
    SpriteRenderer sR;
    bool invokeOnce = false;
    AudioSource source;
    public AudioClip bulletDestroyed;

    void Awake(){
        triggerBox = GetComponent<CircleCollider2D>();
        triggerBox.enabled = false;
        sR = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    void Update(){
        //use invoke to make the bullet hurt the player only after a delay, after it spawns from the enemy gun
        if(invokeOnce == false){
            Invoke("ActivateCollider", 0.5f);
            invokeOnce = true;
        }

        Destroy(gameObject, 6f);

    }
	



    //activates the collider of the bullet so it can hurt the player or destroyed
    void ActivateCollider(){
        triggerBox.enabled = true;
    }





    //checks if the bullet collided with the player to hurt him or with the sword to destroy it
	void OnTriggerEnter2D (Collider2D coll)
	{
		//if buller hits the players then deals damage
		if (coll.isTrigger != true) {
			if (coll.CompareTag ("Player")) {

                //checks if the player is not blocking or it's not a tutorial dummy bullet
                if(PlayerManager.instance.isBlocking == false && !gameObject.CompareTag("DummyBullet")){

                    //also checks if the player is not invulnerable from taking an earlier hit
                    if(PlayerManager.instance.isInvulnerable == false){
                        PlayerManager.instance.lifePoints -= 1;
                        PlayerManager.instance.isInvulnerable = true;
                    }

                //if the player is blocking then changes the variable attackBlocked in PlayerManager which will trigger a sound effect and particle effect
                } else if(PlayerManager.instance.isBlocking == true){
                    PlayerManager.instance.attackBlocked = true;
                }

                //checks if the player is blocking and if it is a tutorial dummy bullet
                if(PlayerManager.instance.isBlocking == true && gameObject.CompareTag("DummyBullet")){
                    GameManager.instance.dummyBulletBlockedCounter++; //this variable counts how many bullets were blocked to progress in the tutorial
                    PlayerManager.instance.attackBlocked = true;
                }
                Destroy (gameObject);
			}
		}

		//if the bullet is hit by the one of the attack triggers of the player then destroys the bullet
		if (coll.isTrigger == true) {
            if (coll.CompareTag ("Attack_Human1") || coll.CompareTag ("Attack_Human2") || coll.CompareTag ("Attack_Human3") || coll.CompareTag ("Attack_HumanAir")) {
                GameManager.instance.dummyBulletDestroyedCounter++; //this variable counts how many bullets were attacked to progress in the tutorial
                triggerBox.enabled = false;
				Destroy (gameObject, 1f); //destroys after a while to give enough time for an audio and visual feedback
                source.PlayOneShot(bulletDestroyed, 0.5f);
                sR.enabled = false;
                Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation); //instantiate a particle feedback for destroying the bullet

			}
		}
	}
	

}
