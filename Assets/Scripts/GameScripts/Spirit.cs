using UnityEngine;
using System.Collections;

public class Spirit : BaseForm
{
	
    bool activateSpiritParticle;
    bool isSuperSpecialAttacking = false;
    bool superSpecialFinishAnimation = false;
    bool playScreamOnce = false;
    bool playEffectOnce = false;
    bool playGlassBreak = false;
	
    float specialAttackCounter = 0;
    float specialHitZoneActivate;
    float hitZoneCounter = 0.2f;

    MainCamera cameraVar;

    public Collider2D superAttackTrigger;
    public Collider2D playerCollider;

    public ParticleSystem superSpecialParticle;
    public ParticleSystem spiritFormParticle;

    AudioSource source;
    public AudioClip superSpecialScreamPoorKillian;
    public AudioClip superSpecialFeedback;
    public AudioClip superSpecialFinalBattleFeedback;
    public AudioClip glassBreaking;

	
	void Start ()
	{
        cameraVar = FindObjectOfType<MainCamera> ();
        superAttackTrigger.enabled = false;
        superSpecialFinishAnimation = true;
        source = GetComponent<AudioSource>();
	}
	
	// Update function for now is mostly controlling the character animations by checking bools and floats from the player manager instance
	void Update ()
	{

        //if the game is paused it will not allow any animation or special attacks to happen
        if(PlayerManager.instance.gameIsPaused == false && PlayerManager.instance.isSpiritForm == true){

            //activates the particle when the player is in spirit form
            if(activateSpiritParticle == false){
                Instantiate(spiritFormParticle, gameObject.transform.position, gameObject.transform.rotation);
                activateSpiritParticle = true;
            }


            UpdateAnim(); //this method is called every frame in BaseForm for the attacks and other animations

            //makes a lot of checks to be sure the player is on the ground and has a super special pickup (Scroll) to use this attack
            if (Input.GetButtonDown ("SuperSpecial") && !attacking && airAttacking == false && PlayerManager.instance.hasSuperSpecial == true && PlayerManager.instance.isJumping == false && PlayerManager.instance.isOnAir == false && PlayerManager.instance.wallSliding == false && PlayerManager.instance.lifePoints >= 0 && GameManager.instance.cannotUseSuperSpecialInTutorial == false) {
                if(playEffectOnce == false){
                    if(GameManager.instance.lastBattleZoneActivated == false){
                        source.PlayOneShot(superSpecialFeedback, 0.5f);
                        playEffectOnce = true;
                    } else {
                        source.PlayOneShot(superSpecialFinalBattleFeedback, 0.5f);
                        playEffectOnce = true;
                    }
                }
                PlayerManager.instance.hasSuperSpecial = false;
                isSuperSpecialAttacking = true;
                anim.SetBool ("Transform", true);
                superSpecialFinishAnimation = false;
                PlayerManager.instance.isAttacking = true;
                GameManager.instance.inputEnabled = true; //will not let the player do anything else while it's using the super special (Soul Destruction)
                PlayerManager.instance.isInSuperSpecial = true;
                playerCollider.enabled = false;
                cameraVar.Shake (0.55f, 1.75f);
            }

            //here it will be managed the time of the attack and also the damage triggers
            if(isSuperSpecialAttacking == true){
                Instantiate(superSpecialParticle, gameObject.transform.position, gameObject.transform.rotation);
                specialAttackCounter += Time.deltaTime;

                //play the sound feedback
                if(specialAttackCounter >= 1.25f && playScreamOnce == false){
                    source.PlayOneShot(superSpecialScreamPoorKillian, 1f);
                    playScreamOnce = true;
                }

                //here it will be invokeRepeating the damage triggers on and off to hit the enemies a certain amount of times
                if(specialAttackCounter >= 1.75f){
                    InvokeRepeating("EnableSuperSpecialTrigger", 0.10f, 0.10f);
                    if(specialAttackCounter >= 1.85f){
                        InvokeRepeating("DisableSuperSpecialTrigger", 0.10f, 0.10f);
                    }
                }

                if(specialAttackCounter >= 2f){
                    if(playGlassBreak == false && GameManager.instance.lastBattleZoneActivated == true){
                        source.PlayOneShot(glassBreaking, 0.15f);
                        playGlassBreak = true;
                    }
                }

                //after a while, stop the damage trigger invokes
                if(specialAttackCounter >= 3f){
                    CancelInvoke("EnableSuperSpecialTrigger");
                    CancelInvoke("DisableSuperSpecialTrigger");
                    isSuperSpecialAttacking = false;

                }

            //after the super special is done, reset all variables so it can be used again
            } else {
                if(superSpecialFinishAnimation == false){
                    specialAttackCounter += Time.deltaTime;
                }
                specialHitZoneActivate = 0;
                hitZoneCounter = 0.2f;
                superAttackTrigger.enabled = false;
                if(specialAttackCounter >= 4f){
                    playerCollider.enabled = true;
                    PlayerManager.instance.isAttacking = false;
                    GameManager.instance.inputEnabled = false;
                    PlayerManager.instance.isInSuperSpecial = false;
                    anim.SetBool ("Transform", false);
                    specialAttackCounter = 0;
                    playScreamOnce = false;
                    playEffectOnce = false;
                    superSpecialFinishAnimation = true;
                }
            }

            //reset the player's attack animations if he dies while attacking, so the animator will go for the death animation without oddities
            if(PlayerManager.instance.lifePoints == -1){
                anim.SetBool ("Attack1", false);
                anim.SetBool("Attack2", false);
            }

        //if the player is not in spirit form, then do not activate the spirit particle
        } else {
            activateSpiritParticle = false;
        }

	}
	



    //enables the damage trigger during the super special
    void EnableSuperSpecialTrigger(){
        superAttackTrigger.enabled = true;
    }


    //disables the damage trigger during the super special
    void DisableSuperSpecialTrigger(){
        superAttackTrigger.enabled = false;
    }
	
	
}
