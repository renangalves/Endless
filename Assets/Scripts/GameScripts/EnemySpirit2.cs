using UnityEngine;
using System.Collections;

public class EnemySpirit2 : MonoBehaviour {

	Transform target; //ENEMY HIERARCHY

	int curHealth = 6;
    int randomDrop;

	float spawnCounter = 0;
	float attackCounter = 0;
	float lerpCounter = 0;
	float goBackToPositionCounter = 0;
	float SPAWN_LERPING_TIME = 1.1f;
	float ATTACK_LERPING_TIME = 0.45f;
	float MAX_LEFT_DISTANCE = 38f;
	float MAX_RIGHT_DISTANCE = 38f;
    float RANDOM_ATTACK_TIME;

	bool getLastPlayerPosition = false;
	bool initialLerping;
	bool findPlayerPositionOnce = false;
	bool lerpFromLeftToRight = false;
	bool lerpFromRightToLeft = false;
	bool setMovementPosition = false;
	bool attacking = false;
    bool hurtPlayer = false;
    bool setToHurtOnce = false;
    bool playAudioOnce = false;
    bool playAudioOnce2 = false;
    bool playDeathAudioOnce = false;
    bool oneSoul = false;

	Vector3 initialPosition;
	Vector3 finalInitialLerping;
	Vector3 startPosition;
	Vector3 endPlayerTarget;
	Vector3 lastSavedPosition;
	Vector3 maxLeftPosition;
	Vector3 maxRightPosition;
	Vector3 definitiveLeftPosition;
	Vector3 definitiveRightPosition;
    Vector3 particleCorrectPosition;
	SwordHitFeedback swordHit; //ENEMY HIERARCHY

    public AudioClip beingHitSound;
    public AudioClip attack;
    public AudioClip bite;
    public AudioClip death;
    AudioSource source;

    Animator anim;
    
    public ParticleSystem spiritHitParticle;

    public GameObject soul;
    public GameObject health;
    public GameObject particle;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("AllPlayer").transform;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
		initialLerping = true;
		initialPosition = gameObject.transform.position;
		finalInitialLerping = PlayerManager.instance.spirit2Reference;
		initialLerping = true;
        RANDOM_ATTACK_TIME = Random.Range(4,7);
        Debug.Log("ATTACK: " + RANDOM_ATTACK_TIME);
        particle = (GameObject)Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		spawnCounter += Time.deltaTime;
        if(curHealth > 0){
            if (spawnCounter >= 2) {
                attackCounter += Time.deltaTime;
                if(setMovementPosition == false){
                    maxLeftPosition = transform.position;
                    maxRightPosition = transform.position;
                    maxLeftPosition.x += MAX_LEFT_DISTANCE;
                    definitiveLeftPosition = maxLeftPosition;
                    maxRightPosition.x -= MAX_RIGHT_DISTANCE; 
                    definitiveRightPosition = maxRightPosition;
                    setMovementPosition = true;
                }
            }
            if (initialLerping == true) {
                Lerp(initialPosition, finalInitialLerping, SPAWN_LERPING_TIME);
                if(transform.position == finalInitialLerping){
                    initialLerping = false;
                    lerpCounter = 0;
                }
            }
            
            if (curHealth > 0) {
                if (attackCounter < RANDOM_ATTACK_TIME && spawnCounter >= 2) {
                    attacking = false;
                    lastSavedPosition = transform.position;
                    if (initialLerping == true) {
                        if (attackCounter < RANDOM_ATTACK_TIME) {
                            Lerp (lastSavedPosition, maxLeftPosition, SPAWN_LERPING_TIME);
                        }
                        if (transform.position == maxLeftPosition) {
                            lerpFromRightToLeft = true;
                            lerpFromLeftToRight = false;
                            lerpCounter = 0;
                            initialLerping = false;
                        }
                    } else if (lerpFromRightToLeft == false && initialLerping == false) {
                        if (attackCounter < RANDOM_ATTACK_TIME) {
                            Lerp (maxRightPosition, maxLeftPosition, SPAWN_LERPING_TIME);
                        }
                        if (transform.position == maxLeftPosition) {
                            lerpFromRightToLeft = true;
                            lerpFromLeftToRight = false;
                            maxRightPosition = definitiveRightPosition;
                            lerpCounter = 0;
                            initialLerping = false;
                        }
                    } else if (lerpFromLeftToRight == false && initialLerping == false) {
                        if (attackCounter < RANDOM_ATTACK_TIME) {
                            Lerp (maxLeftPosition, maxRightPosition, SPAWN_LERPING_TIME);
                        }
                        if (transform.position == maxRightPosition) {
                            lerpFromRightToLeft = false;
                            lerpFromLeftToRight = true;
                            maxLeftPosition = definitiveLeftPosition;
                            lerpCounter = 0;
                            initialLerping = false;
                        }
                    }
                }
                
                if (attacking == false) {
                    if(lerpCounter <= 1){
                        maxLeftPosition.y += Time.deltaTime * 5;
                        maxRightPosition.y += Time.deltaTime * 5;
                    } else if (lerpCounter > 1 && lerpCounter <= 2){
                        maxLeftPosition.y -= Time.deltaTime * 50;
                        maxRightPosition.y -= Time.deltaTime * 50;
                    } else if (lerpCounter > 2 && lerpCounter <= 3){
                        maxLeftPosition.y += Time.deltaTime * 5;
                        maxRightPosition.y += Time.deltaTime * 5;
                    } else if (lerpCounter > 3 && lerpCounter <= 4){
                        maxLeftPosition.y -= Time.deltaTime * 50;
                        maxRightPosition.y -= Time.deltaTime * 50;
                    }
                }
                
                
                if (attackCounter >= RANDOM_ATTACK_TIME) {
                    attacking = true;
                    anim.SetBool("Attack1", true);
                    if(playAudioOnce == false  && PlayerManager.instance.lifePoints >= 0){
                        source.PlayOneShot(attack, 0.2f);
                        playAudioOnce = true;
                    }
                    if(setToHurtOnce == false){
                        hurtPlayer = true;
                        setToHurtOnce = true;
                    }
                    
                    if(findPlayerPositionOnce == false){
                        startPosition = transform.position;
                        findPlayerPositionOnce = true;
                        lerpCounter = 0;
                    }
                    if(attackCounter >= RANDOM_ATTACK_TIME+1.5f){
                        anim.SetBool("Attack2", true);
                        if(getLastPlayerPosition == false){
                            endPlayerTarget = target.position;
                            endPlayerTarget.y += 8f;
                            getLastPlayerPosition = true;
                        }
                        Lerp(startPosition, endPlayerTarget, ATTACK_LERPING_TIME);
                    }
                    if(transform.position == endPlayerTarget){
                        hurtPlayer = false;
                        goBackToPositionCounter += Time.deltaTime;
                        if(goBackToPositionCounter >= 0.2f){
                            anim.SetBool("Attack3", true);
                        }
                        if(playAudioOnce2 == false && PlayerManager.instance.lifePoints >= 0){
                            source.PlayOneShot(bite, 0.5f);
                            playAudioOnce2 = true;
                        }
                        if(goBackToPositionCounter >= 2){
                            startPosition = endPlayerTarget;
                            endPlayerTarget = lastSavedPosition;
                            lerpCounter = 0;

                        }
                        
                    }
                    if(transform.position == lastSavedPosition){
                        attackCounter = 0;
                        setToHurtOnce = false;
                        getLastPlayerPosition = false;
                        findPlayerPositionOnce = false;
                        anim.SetBool("Attack1", false);
                        anim.SetBool("Attack2", false);
                        anim.SetBool("Attack3", false);
                        playAudioOnce = false;
                        playAudioOnce2 = false;
                        goBackToPositionCounter = 0;
                        if(lerpFromLeftToRight == false){
                            maxLeftPosition = lastSavedPosition;
                        } else if(lerpFromRightToLeft == false){
                            maxRightPosition = lastSavedPosition;
                        }
                    }
                }
                
                
                

                //HIERARCHY FOR ENEMY
                if (target.transform.position.x > transform.position.x && PlayerManager.instance.lifePoints > -1) {
                    transform.localScale = new Vector3 (1, 1f, 1f); //scale of current enemy
                    Vector3 particleFix = gameObject.transform.position;
                    particleFix.x -= 3;
                    particle.transform.position = particleFix;
                }
                
                //Same as above, but for the other direction
                //ENEMY HIERARCHY
                if (target.transform.position.x < transform.position.x && PlayerManager.instance.lifePoints > -1) {
                    transform.localScale = new Vector3 (-1f, 1f, 1f);
                    Vector3 particleFix = gameObject.transform.position;
                    particleFix.x += 3;
                    particle.transform.position = particleFix;
                }
            }
        } else {
            Destroy(particle);
            if(oneSoul == false){
                randomDrop = Random.Range(0,3);
                if(randomDrop == 0){
                    Instantiate(soul, gameObject.transform.position, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                    oneSoul = true;
                } else {
                    Instantiate(health, gameObject.transform.position, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                    oneSoul = true;
                }
                
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if(playDeathAudioOnce == false){
                source.PlayOneShot(death, 0.5f);
                playDeathAudioOnce = true;
            }
            Destroy(gameObject, 2f);
        }
        if(PlayerManager.instance.lifePoints <= -1){
            Destroy(particle, 2f);
        }
		

	}


	void Lerp(Vector3 initialPos, Vector3 endPos, float lerpingSpeed){
		lerpCounter += Time.deltaTime;
		transform.position = Vector3.Lerp (initialPos, endPos, lerpCounter / lerpingSpeed);
	}


	





	//if the player hits the enemy with a sword attack in the spirit form, it will deal damage and play the slashing animation
	void OnTriggerEnter2D (Collider2D coll)
	{
        if (curHealth > 0) {
            
            if (coll.CompareTag ("Attack_Spirit1")) {
                PlayerDealtDamage(1);
            }
            
            if (coll.CompareTag ("Attack_Spirit2")) {
                PlayerDealtDamage(2);
            }
            
            if (coll.CompareTag ("Attack_Spirit3")) {
                PlayerDealtDamage(3);
            }

            if (coll.CompareTag ("Attack_SpiritAir")) {
                PlayerDealtDamage(2);
            }

            if (coll.CompareTag("Attack_SpiritDash"))
            {
                PlayerDealtDamage(2);
            }

            if (coll.CompareTag("Attack_SpiritLauncher"))
            {
                PlayerDealtDamage(3);
            }
            
            if (coll.CompareTag ("Attack_Human1")) {
                if(PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial){
                    PlayerDealtDamage(1);
                }
                
            }
            
            if (coll.CompareTag ("Attack_Human2")) {
                if(PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial){
                    PlayerDealtDamage(2);
                }
                
            }
            
            if (coll.CompareTag ("Attack_Human3")) {
                if(PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial){
                    PlayerDealtDamage(3);
                }
                
            }

            if (coll.CompareTag ("Attack_HumanAir")) {
                if(PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial){
                    PlayerDealtDamage(2);
                }
                
            }

            if (coll.isTrigger != true) {
                if (coll.CompareTag ("Player")) {
                    if(PlayerManager.instance.isBlocking == false && PlayerManager.instance.isInSuperSpecial == false){
                        if(hurtPlayer == true){
                            PlayerManager.instance.lifePoints -= 1;
                        }
                        
                    } else if(PlayerManager.instance.isBlocking == true){
                        PlayerManager.instance.attackBlocked = true;
                    }
                }
            }
        }
		
		
	}



    void PlayerDealtDamage(int damage)
    {
        swordHit = FindObjectOfType<SwordHitFeedback>();
        swordHit.hitEnemy = true;
        PlayerManager.instance.playerHitEnemy = true;
        PlayerManager.instance.comboCounter++;
        source.PlayOneShot(beingHitSound, 0.8f);
        particleCorrectPosition = gameObject.transform.position;
        particleCorrectPosition.y += 3f;
        Instantiate(spiritHitParticle, particleCorrectPosition, gameObject.transform.rotation);
        curHealth -= damage; //I could just change the curHealth here, but maybe I will use an
        //plays an animation of the enemy flashing red indicating damage (maybe I will use this later, that's why I'm keeping this here)
        //gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }
}
