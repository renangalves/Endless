using UnityEngine;
using System.Collections;

public class BaseForm : MonoBehaviour
{

    protected bool attackNotHeldDown = false;
    protected bool attacking = false;
    protected bool airAttacking = false;
    protected float airAttackTimer = 0;
    protected float attackCd = 0.5f;
    protected float airAttackWaitTimer = 0;
    protected float attackTimerFinal = 0;
    protected float attackTimer = 0;
    protected float attackTimer2 = 0;
    protected float attackTimer3 = 0;
    protected int lungingAttackCounter = 0;
    protected bool finishingAttack = false;
    protected bool goToFinishingAttack = false;
    protected bool goToFinishingAttack2 = false;
    protected bool nextLungeActivated = false;
    protected bool attacking2 = false;
    protected bool attacking3 = false;
    protected bool goToSecondAttack = false;
    protected bool goToThirdAttack = false;
    protected bool playAudioOnce = false;
    protected bool playJumpAudioOnce = false;
    protected bool playDeathAudioOnce = false;
    protected bool playReachGroundAudioOnce = false;
    protected bool activateParticleOnce = false;

    float reachingGroundCounter;
    protected float attackWaitTimer = 0;
    protected float attackWaitTimer2 = 0;
    protected float attackWaitTimer3 = 0;
    protected float attackWaitTimerFinal = 0;

    protected int randomJumpSound;
    protected int reachGroundCounter = 0;

    SpriteRenderer changeSpriteRenderer;

    protected Animator anim;

    public AudioSource sourceHeroVoice;
    public AudioSource sourceSword;

    public AudioClip[] audioAttacks;
    public AudioClip[] jumpSounds;
    public AudioClip[] swordSwings;
    public AudioClip[] beingHitSounds;
    public AudioClip death;

    public Collider2D attackTrigger;
    public Collider2D attackTrigger2;
    public Collider2D attackTrigger3;
    public Collider2D attackTrigger4;
    public Collider2D attackTrigger5;
    public Collider2D attackTrigger6;

    public ParticleSystem spiritCounterParticle;


    //here is controlled the layer in the spriteRenderer to control the player's layer in the scene
    void OnEnable ()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }




    void Awake()
    {
        //sourceHeroVoice = GetComponent<AudioSource>();
        anim = GetComponent<Animator>(); 
        changeSpriteRenderer = GetComponent<SpriteRenderer>();
        attackTrigger.enabled = false;
        attackTrigger2.enabled = false;
        attackTrigger3.enabled = false;
        attackTrigger4.enabled = false; //disable all attackTriggers at start
        attackTrigger5.enabled = false;
        attackTrigger6.enabled = false;

    }






    //this method is called every frame in the Human and Spirit classes
    protected void UpdateAnim()
    {
        //control every time the player got damaged to display a visual feedback in the Animation
        //and also play a random pain sound
        if(PlayerManager.instance.gotDamaged == true){
            sourceHeroVoice.PlayOneShot(beingHitSounds[Random.Range(0,3)], 1f);
            gameObject.GetComponent<Animation>().Play("Player_RedFlash");
            PlayerManager.instance.gotDamaged = false;
        }

        //control when the player activates the chakra power, or special, and show the visual feedback in the Animation
        if(PlayerManager.instance.isInSpecial == true){
            if(activateParticleOnce == false){
                Instantiate(spiritCounterParticle, gameObject.transform.position, gameObject.transform.rotation);
                activateParticleOnce = true;
            }
            gameObject.GetComponent<Animation>().Play("Player_Special");
        } else {
            activateParticleOnce = false;
        }

        PlayerManager.instance.jumpComboAttack = false;

        //changes the player's layer on the fly when he completes the first level
        if(GameManager.instance.levelOneComplete == true){
            changeSpriteRenderer.sortingLayerName = "UI";
        } else {
            changeSpriteRenderer.sortingLayerName = "Player";
        }
        
        anim.SetFloat("Velocity", Mathf.Abs(PlayerManager.instance.velocity.x)); 

        //if the player's collider detects an enemy layer then activates the combat stance
        if (PlayerManager.instance.enemyDetected == true)
        {
            anim.SetBool("Combat", true);
        } else
        {
            anim.SetBool("Combat", false); 
        }

        //controls the animation of the player when he jumps and the sounds played
        if (PlayerManager.instance.isJumping == true)
        {
            anim.SetBool("Jump", true);
            if(playJumpAudioOnce == false && attacking == false){ //plays the audio and sets the reachGroundCounter to 0 so it will then play another sound
                randomJumpSound = Random.Range(0,2);
                sourceHeroVoice.PlayOneShot(jumpSounds[randomJumpSound], 0.2f);
                playJumpAudioOnce = true;
                reachGroundCounter = 0;
            }
            anim.SetBool("ReachingGround", false);

        } else
        {   //when the player is on ground, and plays sound of the player stepping on the ground
            anim.SetBool("Jump", false);
            playJumpAudioOnce = false;
            anim.SetBool("ReachingGround", true);
            if (playReachGroundAudioOnce == false && reachGroundCounter == 0  && attacking == false)
            {
                sourceHeroVoice.PlayOneShot(jumpSounds[3], 0.1f);
                playReachGroundAudioOnce = true;
                reachGroundCounter++;
            }
            reachingGroundCounter += Time.deltaTime;
            if (reachingGroundCounter >= 0.275f) //timer to set the reaching ground animation to false (could have used the exit time in the animator)
            {
                anim.SetBool("ReachingGround", false);
                reachingGroundCounter = 0;
                playReachGroundAudioOnce = false;
            }
            
        }

        //controls the animation when the player falls from a ledge without jumping
        if (PlayerManager.instance.isOnAir == true)
        {
            anim.SetBool("IsOnAir", true);
        } else
        {
            anim.SetBool("IsOnAir", false);
        }

        //controls the animation when the player is on a wall
        if (PlayerManager.instance.isOnWall == true)
        {
            anim.SetBool("IsOnWall1", true);
        } else
        {
            anim.SetBool("IsOnWall1", false);
        }

        //controls the animation when the player is sliding on a wall
        if (PlayerManager.instance.isWallSliding == true)
        {
            anim.SetBool("IsOnWall2", true);
        } else
        {
            anim.SetBool("IsOnWall2", false);
        }

        //controls the animation when the player performs a double jump
        if (PlayerManager.instance.doubleJumpCheck == true)
        {
            anim.SetBool("DoubleJump", true);
        } else
        {
            anim.SetBool("DoubleJump", false);
        }

        anim.SetInteger("Life", PlayerManager.instance.lifePoints); //controls the animation when the player dies

        //controls if the player is actually pressing the attack button repeatelly to perform combos, instead of just holding
        if (Input.GetButtonUp("Slash"))
        {
            attackNotHeldDown = true;
        }

        //controls when the player attacks, checking to see if he is meeting the conditions to do so
        if (Input.GetButtonDown("Slash") && !attacking && PlayerManager.instance.isJumping == false && PlayerManager.instance.isOnAir == false && PlayerManager.instance.lifePoints >= 0 && GameManager.instance.inputEnabled == false && PlayerManager.instance.isBlocking == false && PlayerManager.instance.isInSuperSpecial == false)
        {
            attackNotHeldDown = false;
            anim.SetBool("Attack1", true);
            attacking = true;
            PlayerManager.instance.isAttacking = true;
            attackTimer = attackCd;
            attackTimer2 = attackCd; //these attackTimers go down during attacks, so they are reset every time a new combo starts
            attackTimer3 = attackCd + 0.2f;
            if (PlayerManager.instance.isSpiritForm == true) //one more condition is added if the player is attacking in spirit form, as he has more attacks
            {
                attackTimerFinal = attackCd - 0.2f;
            }
        }

        //ontrols when the player attacks while in the air, checking to see if he is meeting the conditions to do so
        if (Input.GetButtonDown("Slash") && !attacking && airAttacking == false && (PlayerManager.instance.isJumping == true || PlayerManager.instance.isOnAir == true) && PlayerManager.instance.wallSliding == false && PlayerManager.instance.lifePoints >= 0)
        {
            anim.SetBool("JumpAttack", true);
            playAudioOnce = false;
            airAttacking = true;
            attackNotHeldDown = false;
            PlayerManager.instance.isAirAttacking = true;
            airAttackTimer = attackCd;
        }

        //if the player is attacking on the ground, call the ComboAttackManager to manage all possible combos
        if (attacking)
        {
            ComboAttackManager();
        }

        //if the player is attacking on the air, control the timers of the animation and trigger
        if (airAttacking)
        {
            airAttackWaitTimer += Time.deltaTime;
            if (airAttackWaitTimer >= 0.3f) //activates the damaging trigger after a little while
            {
                if (playAudioOnce == false) //plays the attack sound once
                {
                    sourceSword.PlayOneShot(swordSwings[0], 0.5f);
                    playAudioOnce = true;
                }
                attackTrigger4.enabled = true;
            }
            
            if (airAttackTimer > 0) 
            {
                airAttackTimer -= Time.deltaTime;
            }

            //resets the jump attack and animation values if the attack has ended, if the player lands on the ground or if for some reason he's not in the air anymore
            if (airAttackTimer <= 0 || PlayerManager.instance.isJumping == false || PlayerManager.instance.isOnAir == false)
            {
                airAttacking = false;
                playAudioOnce = false;
                PlayerManager.instance.isAirAttacking = false;
                attackTrigger4.enabled = false;
                anim.SetBool("JumpAttack", false);
                airAttackWaitTimer = 0;
            }
        }

        //when the player respawns, use this animation to reset to his default idle state after the player chooses to continue
        if(GameManager.instance.isSpawning == true){
            Debug.Log("MUDOU RESPAWN PRA TRUE");
            anim.SetBool("Respawn", true);
        } else {
            anim.SetBool("Respawn", false);
        }

        //if the player dies
        if(PlayerManager.instance.lifePoints <= -1){
            ResetAttackCombo(true); //resets his combo attacks so he stop attacking
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlayerDead"; //change layer to make the animation fit for the game over screen

            //plays the death scream once (please Ivo teach me how to do this in a better way without bools)
            if (playDeathAudioOnce == false)
            {
                sourceHeroVoice.PlayOneShot(death, 0.5f);
                playDeathAudioOnce = true;
            }
        } else { //if the player is alive, resets the death audio and his layer to fit the stage properly
            playDeathAudioOnce = false;
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
        
    }











    //this method will control all of the possible combo attacks of the player in human and spirit form
    protected void ComboAttackManager()
    {

        //FIRST ATTACK OF THE 3-HIT COMBO
        if (attacking2 == false && attacking3 == false)
        {
            attackWaitTimer += Time.deltaTime;
            if (attackWaitTimer >= 0.3f) //activates the damage trigger after a little while
            {
                if (playAudioOnce == false) //plays the sounds only once
                {
                    Debug.Log("VAI TOCAR O SOM");
                    sourceHeroVoice.PlayOneShot(audioAttacks[0], 0.5f);
                    sourceSword.PlayOneShot(swordSwings[0], 0.5f);
                    playAudioOnce = true;
                }
                attackTrigger.enabled = true; //enables the trigger of the attack after a little while
            }

            //this timer, which goes down, will dictate the amount of time for the player to continue the combo and the time that he stays in place without moving
            if (attackTimer > 0)
            {
                PlayerManager.instance.velocity.x = 0;
                attackTimer -= Time.deltaTime;
                if (Input.GetButtonDown("Slash") && attackNotHeldDown == true) //if the player presses the attack button, the combo will go on
                {
                    attackNotHeldDown = false; //sets to false since the player pressed the button, but as he releases it will trigger the GetButtonUp and attackNotHeldDown will be true
                    goToSecondAttack = true;
                }
                
            } else if (attackTimer <= 0 && attacking2 == false) //checks if the player has not pressed the attack button, which results in cancelling the combo
            {
                ResetAttackCombo(false);
            } 

            //if the player pressed the button then goToSecondAttack is true, indicating that attacking2 will be true and the combo will go on
            if (goToSecondAttack == true && attackTimer <= 0)
            {
                attacking2 = true;
            }
        }

        //SECOND ATTACK OF THE 3-HIT COMBO, STARTS AFTER THE TIMER attackTimer ON THE FIRST ONE ENDS
        if (attacking2 == true && attackTimer <= 0 && attacking3 == false)
        {
            if (attackWaitTimer2 == 0) //controls the animation of the attacks
            {
                playAudioOnce = false;
                attackTrigger.enabled = false;
                anim.SetBool("Attack1", false);
                anim.SetBool("Attack2", true);
            }
            attackWaitTimer2 += Time.deltaTime;
            if (attackWaitTimer2 >= 0.3f) //activates the damage trigger after a little while
            {
                if (playAudioOnce == false) //plays the sounds only once
                {
                    sourceHeroVoice.PlayOneShot(audioAttacks[1], 0.5f);
                    sourceSword.PlayOneShot(swordSwings[1], 0.5f);
                    playAudioOnce = true;
                }
                attackTrigger2.enabled = true;
            }

            //this timer, which goes down, will dictate the amount of time for the player to continue the combo and the time that he stays in place without moving
            if (attackTimer2 > 0)
            {
                PlayerManager.instance.velocity.x = 0;
                attackTimer2 -= Time.deltaTime;
                if (Input.GetButtonDown("Slash") && attackNotHeldDown == true) //if the player presses the attack button, the combo will go on
                {
                    attackNotHeldDown = false; //sets to false since the player pressed the button, but as he releases it will trigger the GetButtonUp and attackNotHeldDown will be true
                    goToThirdAttack = true;
                }
                
            } else if (attackTimer2 <= 0 && attacking3 == false) //checks if the player has not pressed the attack button, which results in cancelling the combo
            {
                ResetAttackCombo(false);
            } 

            //if the player pressed the button then goToThirdAttack is true, indicating that attacking3 will be true and the combo will go on
            if (goToThirdAttack == true && attackTimer2 <= 0)
            {
                attacking3 = true;
            }
            
        }

        //THIRD ATTACK OF THE 3-HIT COMBO, STARTS AFTER THE TIMER attackTimer2 ON THE SECOND ONE ENDS
        //also checks if the player is in spirit form, which behaves differently than human form
        if (attacking3 == true && attackTimer2 <= 0 && PlayerManager.instance.isSpiritForm == false)
        {
            if (attackWaitTimer3 == 0) //controls the animation of the attacks
            {
                playAudioOnce = false;
                attackTrigger2.enabled = false;
                anim.SetBool("Attack1", false);
                anim.SetBool("Attack2", false);
                anim.SetBool("Attack3", true);
            }
            attackWaitTimer3 += Time.deltaTime;
            if (attackWaitTimer3 >= 0.3f) //activates the damage trigger after a little while
            {
                if (playAudioOnce == false) //plays the sounds only once
                {
                    sourceHeroVoice.PlayOneShot(audioAttacks[2], 0.5f);
                    sourceSword.PlayOneShot(swordSwings[2], 0.5f);
                    playAudioOnce = true;
                }
                attackTrigger3.enabled = true;
            }

            //this timer, which goes down, will dictate the amount of time for the attack to end and reset everything, since the human only can go to 3-hit combos
            if (attackTimer3 > 0)
            {
                attackTimer3 -= Time.deltaTime;
                    
            } else
            {
                ResetAttackCombo(false);
            } 
              
        }

        //THIRD ATTACK OF THE 3-HIT COMBO, STARTS AFTER THE TIMER attackTimer2 ON THE SECOND ONE ENDS
        //also checks if the player is in spirit form, which behaves differently than human form
        if (attacking3 == true && attackTimer2 <= 0 && finishingAttack == false && PlayerManager.instance.isSpiritForm == true)
        {
            if (attackWaitTimer3 == 0) //controls the animation of the attacks
            {
                playAudioOnce = false;
                attackTrigger2.enabled = false;
                anim.SetBool("Attack1", false);
                anim.SetBool("Attack2", false);
                anim.SetBool("Attack3", true);
            }
            attackWaitTimer3 += Time.deltaTime;
            if (attackWaitTimer3 >= 0.3f) //activates the damage trigger after a little while
            {
                if (playAudioOnce == false) //plays the sounds only once
                {
                    sourceHeroVoice.PlayOneShot(audioAttacks[2], 0.5f);
                    sourceSword.PlayOneShot(swordSwings[2], 0.5f);
                    playAudioOnce = true;
                }
                attackTrigger3.enabled = true;
            }
                
            //this timer, which goes down, will dictate the amount of time for the player to continue the combo and the time that he stays in place without moving
            if (attackTimer3 > 0)
            {
                attackTimer3 -= Time.deltaTime;

                //checks if the player is holding up while attacking, which triggers to go to the Armstrong Launcher attack
                if (Input.GetButtonDown("Slash") && attackNotHeldDown == true && PlayerManager.instance.playerInput.y > 0.2f)
                {
                    attackNotHeldDown = false;
                    goToFinishingAttack = true;

                //checks if the player is holding left or right while attacking, which triggers to go to the Phantom Dash attack
                } else if (Input.GetButtonDown("Slash") && attackNotHeldDown == true && (PlayerManager.instance.playerInput.x > 0.2f || PlayerManager.instance.playerInput.x < -0.2f) && PlayerManager.instance.playerInput.y < 0.2f)
                {
                    attackNotHeldDown = false;
                    goToFinishingAttack2 = true;
                }
                    
            } else if (finishingAttack == false) //checks if the player has not continued any combo, which results in cancelling the combo
            {
                ResetAttackCombo(true);
            } 
                
            //if the player executed any of the combos then it will continue on
            if ((goToFinishingAttack == true || goToFinishingAttack2 == true) && attackTimer3 <= 0)
            {
                finishingAttack = true;
            }
                
        }
          
        //ARMSTRONG LAUNCHER ATTACK, SPIRIT ONLY
        if (finishingAttack == true && attackTimer3 <= 0 && goToFinishingAttack == true)
        {
            if (attackWaitTimerFinal == 0) //controls the animation and triggers of the attacks
            {
                attackTrigger3.enabled = false;
                anim.SetBool("SpecialJumpAttack", true);
                PlayerManager.instance.isAirAttacking = true; //the player is launched in the air, allowing air movements during the attack
                attackTimerFinal = attackCd;
            }

            attackWaitTimerFinal += Time.deltaTime;

            if (attackWaitTimerFinal >= 0.3f) //activates the damage trigger after a little while
            {
                attackTrigger6.enabled = true;
                PlayerManager.instance.isAttacking = false;
                if (attackWaitTimerFinal >= 0.4f)
                {
                    PlayerManager.instance.jumpComboAttack = true; //this variable in PlayerManager triggers the jump of the character and not allowing the localScale to change
                }
            }
                
            //this timer, which goes down, will dictate the amount of time the attack takes before resetting all attack values
            if (attackTimerFinal > 0)
            {
                attackTimerFinal -= Time.deltaTime;
            } else
            {
                ResetAttackCombo(true);
            }
                

        //PHANTOM DASH ATTACK, SPIRIT ONLY
        } else if (finishingAttack == true && attackTimer3 <= 0 && goToFinishingAttack2 == true)
        {
            if (attackWaitTimerFinal == 0) //controls the animation of the attacks
            {
                attackTrigger3.enabled = false;
                anim.SetBool("DashPrepare", true);
                anim.SetBool("Attack3", false);
                anim.SetBool("DashAttack", false);
            }
            attackWaitTimerFinal += Time.deltaTime;

            //after a little while, checks if the player is holding a direction to trigger the attack
            if (attackWaitTimerFinal >= 0.2f)
            {
                if ((PlayerManager.instance.playerInput.x > 0.2f || PlayerManager.instance.playerInput.x < -0.2f))
                {
                    attackTrigger5.enabled = true;
                    anim.SetBool("DashPrepare", false);
                    anim.SetBool("DashAttack", true);
                    PlayerManager.instance.isBlocking = true; //the dash makes the player invulnerable
                    PlayerManager.instance.horizontalComboAttack = true; //this variable in PlayerManager controls the movement of the dashes and avoiding the localScale changes

                } else if (attackTrigger5.enabled == false) //if the player didn't hold to any direction, cancel the dashes
                {
                    ResetAttackCombo(true);
                }
                    
            }
                
            //this timer, which goes down, will dictate the amount of time for the player to continue the dashes before it is cancelled
            if (attackTimerFinal > 0)
            {
                attackTimerFinal -= Time.deltaTime;

                //checks if the player pressed the attack button and is holding a direction
                if (Input.GetButtonDown("Slash") && attackNotHeldDown == true && (PlayerManager.instance.playerInput.x > 0.2f || PlayerManager.instance.playerInput.x < -0.2f) && nextLungeActivated == false)
                {
                    nextLungeActivated = true; //this will dictate if the next dash will happen
                    attackNotHeldDown = false;
                }
            } else //when the timer hits 0
            {
                if (nextLungeActivated == true) //if the player continued the dashes
                {
                    lungingAttackCounter++;
                    if (lungingAttackCounter <= 2) //checks hwo many dashes were performed, not allowing more than 3 at a time
                    {
                        ResetLungingCombo(); //resets only the dash combo so it can continue
                    } else
                    {
                        PlayerManager.instance.velocity.y = 0; //resets the gravity, so the player is not shot down if he ends on a fall after the dash
                        ResetAttackCombo(true);
                    }
                } else
                {
                    ResetAttackCombo(true);
                }
            }
        }
        

    }





    //method that resets all variables for the dash attacks
    void ResetLungingCombo()
    {
        attackWaitTimerFinal = 0;
        PlayerManager.instance.isBlocking = false;
        PlayerManager.instance.horizontalComboAttack = false;
        PlayerManager.instance.lungingLeft = false;
        PlayerManager.instance.lungingRight = false;
        attackTrigger5.enabled = false;
        attackTimerFinal = attackCd - 0.2f;
        nextLungeActivated = false;
    }






    //method that resets all variables for the player's combo attacks
    protected void ResetAttackCombo(bool isSpirit)
    {
        attacking = false;
        attacking2 = false;
        attacking3 = false;
        goToSecondAttack = false;
        goToThirdAttack = false;
        PlayerManager.instance.isAttacking = false;
        attackTrigger.enabled = false; 
        attackTrigger2.enabled = false;
        attackTrigger3.enabled = false;
        attackTrigger5.enabled = false;
        attackTrigger6.enabled = false;
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        playAudioOnce = false;
        attackWaitTimer = 0;
        attackWaitTimer2 = 0;
        attackWaitTimer3 = 0;
        if (isSpirit)
        {
            finishingAttack = false;
            goToFinishingAttack = false;
            goToFinishingAttack2 = false;
            PlayerManager.instance.isAirAttacking = false;
            PlayerManager.instance.jumpComboAttack = false;
            PlayerManager.instance.horizontalComboAttack = false;
            PlayerManager.instance.isBlocking = false;
            PlayerManager.instance.lungingLeft = false;
            PlayerManager.instance.lungingRight = false;
            anim.SetBool("DashPrepare", false);
            anim.SetBool("DashAttack", false);
            anim.SetBool("SpecialJumpAttack", false);
            attackWaitTimerFinal = 0;
            lungingAttackCounter = 0;
            nextLungeActivated = false;
        }
    }



}


