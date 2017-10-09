using UnityEngine;
using System.Collections;

public class Human : BaseForm
{
	
    public ParticleSystem dustParticle;
    public Transform dustPositionReference;

    float dustCounter = 0;

	// Update function for now is mostly controlling the character animations by checking bools and floats from the player manager instance
	void Update ()
	{
        //don't let any animation happen when the game is paused or spirit form is activated
        if(PlayerManager.instance.gameIsPaused == false && PlayerManager.instance.isSpiritForm == false){
            
            UpdateAnim(); //this method is called every frame in BaseForm for the attacks and other animations

            dustCounter += Time.deltaTime; //will control the quantity of dust particle spawned 

            //controls the block, checking if the player is not jumping, on the air or dead
            if (Input.GetButtonDown ("Block") && PlayerManager.instance.isJumping == false && PlayerManager.instance.isOnAir == false && PlayerManager.instance.lifePoints >= 0 && GameManager.instance.inputEnabled == false) {
                PlayerManager.instance.isBlocking = true; //this variable set to true will make the player not move anymore
                anim.SetBool("Blocking", true);
                ResetAttackCombo(false); //when the player blocks keep resets the player's combo variables to stop in the middle of any attack
            }

            //when the player let go of the block button, change the blocking values
            if(Input.GetButtonUp ("Block")){
                PlayerManager.instance.isBlocking = false; //this variable set to false will let the player move freely again
                anim.SetBool("Blocking", false);
            }

            //reset the player's attack animations if he dies while attacking, so the animator will go for the death animation without oddities
            if(PlayerManager.instance.lifePoints == -1){
                anim.SetBool ("Attack1", false);
                anim.SetBool("Attack2", false);
            }

            //when the player moves on the ground it will create small dust particles after a little while
            if((PlayerManager.instance.velocity.x >= 10 || PlayerManager.instance.velocity.x <= -10) && PlayerManager.instance.isOnAir == false && PlayerManager.instance.isJumping == false && dustCounter > 0.2f){
                Instantiate(dustParticle, dustPositionReference.position, dustPositionReference.rotation);
                dustCounter = 0;
            }
        }

	}
	
}










































