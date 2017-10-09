using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.UI;

[RequireComponent (typeof(Controller))]

public class PlayerManager : MonoBehaviour
{
	public float jumpHeight = 4f; //used to tune the jumpVelocity on the inspector (how high want to jump)
	public float timeToJump = .5f; //used to tune the gravity on the inspector (how fast to hit the high of the jump) 
	
	public Vector3 velocity;
	
	public float accelerationTimeOnAir = .2f; //for tune the aceleration while in the air
	public float accelerationTimeOnGround = .1f; //for tune the aceleration while in the ground
	public float moveSpeed = 6;
    float wallJumpCounter = 0;

	public LayerMask enemy;

	public float wallSlideMaxSpeed = 6; //for tune the volocity of the character slliding down
	public Vector2 wallJumpClimb; //jump a little to the top while on the wall and pressing to the side of the wall
	public Vector2 wallJumpOff; //jump off of the wall just by pressing jump
	public Vector2 wallLeap; //jump off of the wall on the direction pressed
	public Vector2 playerInput;
    public Vector3 spirit2Reference;

	public Transform characterCombat;
	
	public float wallStickTime = 0.25f; //a time to allow the player to peform wall jump to the oposite direction whitouth falling fast
	public float timeToWallUnstick;
	public float enemyDetectRadius;
	public float transformationCounter = 0;
	public float soulsCounter = 0;
    float vibrateCounter = 0;
    float comboCounterReset;
    float invulnerabilityTimer;
    float redScreenCounter = 0;
    public float MAX_TIME_INVULNERABLE;

    public float pauseNotJumpCounter = 0;

	Physics2D enemyCheck;

	GameObject currentForm;
    public GameObject soulJarParticle;
    public GameObject comboCounterParticle;
    public GameObject soulGotParticle;

    public ParticleSystem spiritChangeParticle;
    public ParticleSystem humanChangeParticle;
    public ParticleSystem blockParticle;
    public ParticleSystem superSpecialParticle;

    Rigidbody2D rb;

	public int lifePoints = 0;
    int lastLifeCheck;
    public int comboCounter;
    int onlyOneCombo= 0;
    int trackPlayerHealth;
    int consecutiveHits = 0;
    int randomChoice = 0;

	//most of all these bools are for checkings associated with animations in the Human and Spirit scripts
    public bool isSpiritForm = false;
	public bool facingRight;
	public bool facingLeft;
	bool wallSlidingRotateBack = false;
	public bool isJumping = false;
	public bool doubleJump = false;
	public bool isOnWall = false;
    public bool wallSliding;
	public bool isWallSliding = false;
	public bool enemyDetected = false;
	public bool doubleJumpCheck = false;
	public bool transformation = false;
	public bool isAttacking = false;
	public bool isAirAttacking = false;
    public bool isBlocking = false;
    public bool attackBlocked = false;
	public bool isOnAir = false;
	public bool isInSpecial = false;
    public bool isInSuperSpecial = false;
	public bool jumpComboAttack = false;
    public bool isInInteractZone = false;
    public bool horizontalComboAttack = false;
    public bool lungingLeft = false;
    public bool lungingRight = false;
    public bool playerHitEnemy = false;
    public bool gameIsPaused = false;
    public bool isControllerVibrating = false;
    public bool gotKey = false;
    public bool acquiredSoul = false;
    public bool gotDamaged = false;
    bool checkCombo = false;
    bool enableJump;
    bool playSoundOnce = false;
    bool playParticleOnce = false;
    bool playHumanParticleOnce = false;
    bool tookDamage = false;
    bool stopWhile = false;
    bool[] redScreensChecks = new bool[3];
    public bool isInvulnerable;
    public bool hasSuperSpecial = false;
    public bool stageCompleteMovement = false;

	float velocityOnXSmoother;
	public Controller mainController;

	//Isolates the acelleration (our gravity)
	float gravity; //used to the velocity when falling
	
	//velocityFinal = velocityInitial + (acceleration*time)  // (velocity = velocityFinal - velocityInicial)
	public float jumpVelocity; //used to the velocity when jumping
	
    public Animator[] redScreens;
	protected Animator animator;
    public Animator heartUIPulses;
    public Animator heartUISoul;

    AudioSource source;
    public AudioClip key;
    public AudioClip blockSound;

//    public Image UIlife;
    public Image UIKey;
    public Image UISuperSpecial;
    public Sprite superSpecial;

    public Text comboText;

    Slider specialSlider;

	public static PlayerManager instance;
	
   

	
	void Start ()
	{
		PlayerManager.instance = this;

        isSpiritForm = false;

		//instead of using Unity's physics we calculate the gravity and velocities
		gravity = -(5 * jumpHeight) / Mathf.Pow (timeToJump, 2);

		jumpVelocity = Mathf.Abs (gravity) * timeToJump;
		
		mainController = GetComponent<Controller> ();

        rb = GetComponent<Rigidbody2D>();

        lastLifeCheck = lifePoints;

        UIKey.enabled = false;

        UISuperSpecial.enabled = false;

        specialSlider = GameObject.Find("SpecialSlider").GetComponent<Slider>();

        heartUIPulses.SetInteger("PlayerLife", lifePoints);
        heartUISoul.SetInteger("PlayerLife", lifePoints);
        
        source = GetComponent<AudioSource>();

        trackPlayerHealth = 5;
	}






	void FixedUpdate ()
	{
		//sets a bool to identify when the player is near an enemy
		if (gameObject.tag == "Player") {
			enemyDetected = Physics2D.OverlapCircle (characterCombat.position, enemyDetectRadius, enemy);
		}

	}







	void Update ()
	{  

        if(GameManager.instance.introComplete == true){


            if (lifePoints >= 0 && gameIsPaused == false) {
                
                //GetAxisRaw can only be 0,1,-1 and pure getAxis have a sensitivity, so it takes a while to be 1 or -1 
                playerInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")); 
                
                //if the collision on the left is on a wall the wall direction is -1, else is 1)
                int wallDirX = (mainController.allCollisions.left) ? -1 : 1; 
                
                //checks which way the player is facing
                if (playerInput.x > 0.2f) {
                    facingRight = true;
                    facingLeft = false;
                } 
                
                if (playerInput.x < -0.2f) {
                    facingRight = false;
                    facingLeft = true;
                }   
                
                float targetVelocityOnX = playerInput.x * moveSpeed;
                
                /*current position on X, the target position on X, a reference of the current movement 
         and the smooth time (how long it takes to go to the current velocity to the target velocity)
         so the smooth time will be float accelerationTimeOnGround if colliders are on ground 
         otherwise smooth time will be accelerationTimeOnAir */
                velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityOnX, ref velocityOnXSmoother, (mainController.allCollisions.below) ? accelerationTimeOnGround : accelerationTimeOnAir); 
                
                
                //rotate the character around, but only if he is not transforming or attacking
                if (playerInput.x < 0 && isAttacking == false && isAirAttacking == false && isBlocking == false && jumpComboAttack == false) {
                    transform.localScale = new Vector3 (-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                if (playerInput.x > 0 && isAttacking == false && isAirAttacking == false && isBlocking == false && jumpComboAttack == false) {
                    transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                
                
                wallSliding = false;
                
                //check if the players is on a right or left wall and not in the ground at the same time and the player is moving down
                if ((mainController.allCollisions.left || mainController.allCollisions.right) && !mainController.allCollisions.below && velocity.y < 0) {
                    
                    wallSliding = true;
                    doubleJump = false;
                    //so the velocity.y will never be bigger than the wall slide speed
                    if (velocity.y < -wallSlideMaxSpeed) {
                        velocity.y = -wallSlideMaxSpeed;
                    }
                    
                    //control the player on the wall
                    if (timeToWallUnstick > 0) {
                        velocityOnXSmoother = 0;
                        velocity.x = 0; //player not moving in X
                        //checks if the player is not pressing the same direction of the wall and is pressing any another direction
                        if (playerInput.x != wallDirX && playerInput.x != 0) {
                            timeToWallUnstick -= Time.deltaTime; //begin to count the timer to unstick from the wall
                        } else {
                            timeToWallUnstick = wallStickTime;
                        }
                    } else {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                
                //dont acumulate gravity when is on the ground
                if(mainController.allCollisions.above){
                    velocity.y = 0; //set "gravity" to 0
                }
                
                if (mainController.allCollisions.below) { 
                    isJumping = false;
                    isOnAir = false;
                    doubleJumpCheck = false;
                    velocity.y = 0; //set "gravity" to 0
                } else if(!mainController.allCollisions.below){
                    isOnAir = true;
                }
                
                //STOP THE PLAYER FROM JUMPING WHEN HE PRESS A TO CONTINUE THE GAME FROM THE PAUSE SCREEN
                pauseNotJumpCounter += Time.deltaTime;
                if(pauseNotJumpCounter == 0){
                    enableJump = false;
                    
                } else if(pauseNotJumpCounter >= 0.1f){
                    enableJump = true;
                }

                wallJumpCounter += Time.deltaTime;
                
                if ((Input.GetButtonDown ("Jump") || jumpComboAttack == true) && stageCompleteMovement == false && GameManager.instance.inputEnabled == false && isBlocking == false) {
                    //if the player is on the wall, checks which direction he is pressing to accordingly jump away from the wall
                    //rb.AddForce(transform.position, ForceMode2D.Impulse);
                    if(enableJump == true){
                        if (wallSliding) {
                            if (wallDirX == playerInput.x) { //the direction that the player is pressing is the same as the wall direction
                                velocity.x = -wallDirX * wallJumpClimb.x; //climb the wall
                                velocity.y = wallJumpClimb.y;
                            } else if (playerInput.x == 0) {  //not pressing left of right
                                velocity.x = -wallDirX * wallJumpOff.x; //just jump off of the wall
                                velocity.y = wallJumpOff.y;
                            } else {
                                velocity.x = -wallDirX * wallLeap.x; //jump to the other side
                                velocity.y = wallLeap.y;
                            }
                            isJumping = true;
                            doubleJump = false;
                            wallJumpCounter = 0;
                        }
                        
                        if (mainController.allCollisions.below) { //checks is on the ground
                            isJumping = true;
                            doubleJump = false;
                            velocity.y = jumpVelocity; //makes the jump
                        } 
                        

                    }
                    
                }

                //checks if the player is not on the ground and if he has double jump available then he double jumps
                if (!mainController.allCollisions.below && Input.GetButtonDown ("Jump") && doubleJump == false && wallJumpCounter >= 0.25f && GameManager.instance.inputEnabled == false) {
                    velocity.y = jumpVelocity;
                    doubleJump = true;
                    Debug.Log("DEU DOUBLE JUMP");
                    doubleJumpCheck = true;
                }
                
                //checks if the player is currently sticking on a wall to change some variables to control animations in Human and Spirit scripts, and double jumping
                if ((mainController.allCollisions.left || mainController.allCollisions.right) && !mainController.allCollisions.below) {
                    
                    isOnWall = true;
                    doubleJumpCheck = false;
                } else {
                    isOnWall = false;
                }
                
                //same as above, but checking if the player is pressing the other direction from the wall to show the correct animation in the Human and Spirit scripts
                if ((mainController.allCollisions.right || mainController.allCollisions.left) && !mainController.allCollisions.below && !((wallDirX < 0 && playerInput.x < 0) || (wallDirX > 0 && playerInput.x > 0))) {
                    if (playerInput.x == 0) {
                        isOnWall = true;
                        doubleJumpCheck = false;
                    } else {
                        isWallSliding = true;
                        doubleJumpCheck = false;
                    }
                    
                } else {
                    isWallSliding = false;
                }
                
                

                //if the player presses the transform button it first checks if he can at the moment
                if (Input.GetButtonDown ("Transform")) {
                    if(isAttacking == false && jumpComboAttack == false && isAirAttacking == false && isBlocking == false){
                        transformation = !transformation;
                        //then it switches the form he currently is on
                        if(transformation == true){
                            SwitchSpirit ();
                        } else {
                            SwitchHuman ();
                        }
                        
                    }
                }
                

                //if the player uses the chakra power it will change the isInSpecial variable which lets the player hit any type of enemy
                if(Input.GetButtonDown("Special")){
                    if(soulsCounter >= 0.1f && isInSpecial == false){
                        isInSpecial = true;
                        soulJarParticle.SetActive(true);
                    } else if(soulsCounter >= 0.1f && isInSpecial == true){
                        isInSpecial = false;
                        soulJarParticle.SetActive(false);
                    }
                }


                //the amount of chakra (or souls) that the player can have is always a maximum of 10
                if(soulsCounter > 10){
                    soulsCounter = 10;
                }
                
                specialSlider.value = soulsCounter; //keep updating the slider


                //the special (or chakra) bar will slowly drain until the player press the button again or it depletes
                if(isInSpecial == true){
                    soulsCounter -= Time.deltaTime / 2;
                    specialSlider.value = soulsCounter;
                    if(soulsCounter <= 0){
                        isInSpecial = false;
                        soulJarParticle.SetActive(false);
                        soulsCounter = 0;
                    }
                }
                
                
                //keep updating velocity.y with the players gravity
                velocity.y += gravity * Time.deltaTime; 



                
                //move the player according to the velocity, but first checks if the player is attacking, which will stop the player from moving
                if (!isAttacking && !isBlocking) {
                    mainController.Move (velocity * Time.deltaTime);

                    //always keep checking if the player is in human or spirit form
                    if(transformation == true && isAirAttacking == false){
                        SwitchSpirit();
                    } else if(transformation == false && isAirAttacking == false){
                        SwitchHuman();
                    }
                }



                //this will make the character move automatically to the right when a stage is complete
                if(stageCompleteMovement == true){
                    velocity.x = 20;
                    transform.localScale = new Vector3 (1, 1, 1);
                    mainController.Move (velocity * Time.deltaTime);
                }



                //this will control the dashes performed in spirit form in the Phantom Dash ability
                if(horizontalComboAttack == true){
                    //if the player is holding left before a dash, the dash will be to the left
                    if(lungingLeft == true){
                        transform.localScale = new Vector3 (-1, 1, 1);
                        mainController.Move (new Vector3(-200,0,0) * Time.deltaTime);
                    }

                    //if the player is holding right before a dash, the dash will be to the right
                    if(lungingRight == true){
                        transform.localScale = new Vector3 (1, 1, 1);
                        mainController.Move (new Vector3(200,0,0) * Time.deltaTime);
                    }

                //between each dash this will check what direction the player is holding
                } else {
                    if (playerInput.x < -0.2) {
                        lungingLeft = true;
                        lungingRight = false;
                    }
                    if (playerInput.x > 0.2 ) {
                        lungingRight = true;
                        lungingLeft = false;
                    }
                }
                
                
            } 
            
            
            
            //PLAYER ACTIONS END HERE



            //this will track if the player took damage and how many times during a certain time period
            //first it checks if the player health has decreased
            if(lifePoints < trackPlayerHealth && lifePoints > -1){
                trackPlayerHealth = lifePoints;
                tookDamage = true;
                redScreenCounter = 0;
                gotDamaged = true;
                stopWhile = false;
                consecutiveHits++;
            //it also checks if the player health has increased to update the trackPlayerHealth variable
            } else if(lifePoints > trackPlayerHealth){
                trackPlayerHealth = lifePoints;
            }


            //if the player takes damage, it will randomly choose between 3 spots on the screen to show a red blood mark
            //the chosen random spot cannot be one that is already chosen and it will never be more than 3
            if(tookDamage == true){
                redScreenCounter += Time.deltaTime;
                if(consecutiveHits == 1){
                    if(stopWhile == false){
                        randomChoice = Random.Range(0,3);
                        redScreens[randomChoice].SetBool("TookHit", true);
                        redScreensChecks[randomChoice] = true;
                        stopWhile = true;
                    }
                } else if(consecutiveHits == 2){
                    while(stopWhile == false){
                        randomChoice = Random.Range(0,3);
                        if(redScreensChecks[randomChoice] == false){
                            redScreens[randomChoice].SetBool("TookHit", true);
                            redScreensChecks[randomChoice] = true;
                            stopWhile = true;
                        }
                    }
                } else if(consecutiveHits == 3){
                    while(stopWhile == false){
                        randomChoice = Random.Range(0,3);
                        if(redScreensChecks[randomChoice] == false){
                            redScreens[randomChoice].SetBool("TookHit", true);
                            redScreensChecks[randomChoice] = true;
                            stopWhile = true;
                        }
                    }
                }

                //after 5 seconds without taking damage the red spots will go away and the variables will be reset
                if(redScreenCounter >= 5){
                    redScreenCounter = 0;
                    redScreens[0].SetBool("TookHit", false);
                    redScreens[1].SetBool("TookHit", false);
                    redScreens[2].SetBool("TookHit", false);
                    redScreensChecks[0] = false;
                    redScreensChecks[1] = false;
                    redScreensChecks[2] = false;
                    consecutiveHits = 0;
                    tookDamage = false;
                }
            } 



            //every time the player blocks an attack it will display a visual and audio feedback
            if(attackBlocked == true){
                Vector3 fixPosition = gameObject.transform.position;
                fixPosition.y += 5f;
                Instantiate(blockParticle, fixPosition, gameObject.transform.rotation);
                source.PlayOneShot(blockSound, 0.5f);
                attackBlocked = false;
            }

            //everytime the player collects a soul, it will show feedback on the UI vase
            if(acquiredSoul == true){
                soulGotParticle.SetActive(false);
                soulGotParticle.SetActive(true);
                acquiredSoul = false;
            }
            
            //enables the key UI when the player gets a key
            if(gotKey == false){
                UIKey.enabled = false;
            }

            //enables the super special (scroll) UI when the player gets one
            if(hasSuperSpecial == false){
                UISuperSpecial.enabled = false;
            }
            

            //this if will manage when the player gets damaged and the controller will vibrate
            if(lastLifeCheck != lifePoints && lifePoints >= -1){
                LifeUIUpdate();
                vibrateCounter += Time.deltaTime;
                if(vibrateCounter < 0.5){
                    GamePad.SetVibration(PlayerIndex.One, 0, 1);
                    isControllerVibrating = true;
                } else {
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                    vibrateCounter = 0;
                    lastLifeCheck = lifePoints;
                    isControllerVibrating = false;
                }
                
            }

            //if the player dies the comboText will be nothing
            if(lifePoints <= -1){
                comboText.text = "";
            }
            
            comboCounterReset += Time.deltaTime;

            //if the player doesn't deal damage in 5 seconds the combo counter will show a COOL or NICE or not show anything depending on the amount of hits
            if(comboCounterReset >= 5){
                if(comboCounter > 5 && comboCounter <= 9 && lifePoints >= -1){
                    comboText.text = "COOL!";
                    comboCounter = 0;
                } else if(comboCounter >= 10 && lifePoints >= -1){
                    comboText.text = "NICE!!";
                    comboCounter = 0;
                } else {
                    comboCounter = 0;
                }
                if(comboCounterReset >= 7){
                    comboText.text = "";
                    comboCounterParticle.SetActive(false);
                }
                
            }
            
            if(checkCombo == true && onlyOneCombo == 1){
                checkCombo = false;
            }

            //when the player hits an enemy the combo starts or increments, showing the amount of hits on the screen
            if(playerHitEnemy == true){
                comboCounterParticle.SetActive(true);
                if(comboCounter == 0){
                    comboCounter++;
                }
                comboText.text = comboCounter + " HITS!";
                comboCounterReset = 0;
                VibrateController();
                checkCombo = true;
                onlyOneCombo++;
            } else {
                onlyOneCombo = 0;
            }


            //when the player gets damaged, this if will count the amount of time he is invulnerable before returning back to normal
            if(isInvulnerable == true){
                invulnerabilityTimer += Time.deltaTime;
                if(invulnerabilityTimer >= MAX_TIME_INVULNERABLE){
                    invulnerabilityTimer = 0;
                    isInvulnerable = false;
                }
            }
            
            //if the player pauses when the controller is vibrating it will stop the vibration
            if(gameIsPaused == true){
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                lastLifeCheck = lifePoints;
                vibrateCounter = 0.3f;
            }

        }
		 

	}






	//disables the spirit gameobject to enable the human gameobject
	void SwitchHuman ()
	{
        transform.Find ("Spirit").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);//.gameObject.SetActive (false);
        SetAllCollider(transform.Find ("Spirit").gameObject, false);
		currentForm = transform.Find ("Human").gameObject;
        if(playHumanParticleOnce == false){
            Instantiate(humanChangeParticle, gameObject.transform.position, gameObject.transform.rotation);
            playHumanParticleOnce = true;
        }
        currentForm.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);//.SetActive (true);
        SetAllCollider(currentForm, true);
		isSpiritForm = false;
        playParticleOnce = false;
	}
	
	




	//disables the human gameobject to enable the spirit gameobject
	void SwitchSpirit ()
	{
        transform.Find ("Human").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);//.gameObject.SetActive (false);
        SetAllCollider(transform.Find ("Human").gameObject, false);
		currentForm = transform.Find ("Spirit").gameObject;
        if(playParticleOnce == false){
            Instantiate(spiritChangeParticle, gameObject.transform.position, gameObject.transform.rotation);
            playParticleOnce = true;
        }
        currentForm.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);//.SetActive (true);
        SetAllCollider(currentForm, true);
		isSpiritForm = true;
        playHumanParticleOnce = false;
	}






    //when changing forms, the attack colliders are set again
    void SetAllCollider(GameObject obj, bool value) {
        foreach(Collider2D c in obj.GetComponentsInChildren<Collider2D>(true)) {
            c.gameObject.SetActive(value);
        }
    }




    //this method controls the vibration of the controller
    public void VibrateController(){
        vibrateCounter += Time.deltaTime;
        if(vibrateCounter < 0.3){
            GamePad.SetVibration(PlayerIndex.One, 1, 1);
            isControllerVibrating = true;
        } else {
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
            vibrateCounter = 0;
            lastLifeCheck = lifePoints;
            playerHitEnemy = false;
            isControllerVibrating = false;
        }

    }
    



    //this method will keep updating the animator of the UI for the health and the soul behind the heart
    void LifeUIUpdate(){
        heartUIPulses.SetInteger("PlayerLife", lifePoints);
        heartUISoul.SetInteger("PlayerLife", lifePoints);
    }

 




    //if the player is hit by an EnemyAttack tag, it will be blocked or damage the player
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.CompareTag ("EnemyAttack")) {
            if(isBlocking == true){
                attackBlocked = true;
            } else {
                if(isInvulnerable == false && isInSuperSpecial == false){
                    lifePoints -= 1;
                    isInvulnerable = true;
                }
                //GamePad.SetVibration(PlayerIndex.One, 1, 1);
            }
		}

        //if the player collides with a key, he gets a key
        if(coll.CompareTag ("Key")){
            gotKey = true;
            UIKey.enabled = true;
            Destroy(coll.gameObject);
            source.PlayOneShot(key, 0.8f);
        }


        //if the player collides with a scroll, it will be collected
        if(coll.CompareTag ("SuperSpecialPickUp")){
            hasSuperSpecial = true;
            UISuperSpecial.enabled = true;
            Instantiate(superSpecialParticle, coll.gameObject.transform.position, coll.gameObject.transform.rotation);
            coll.GetComponent<SpriteRenderer>().enabled = false;
            coll.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(coll.gameObject, 2f);
        }

	}



    //if the player leaves an interact trigger, it changes the isInInteractZone variable so InteractionFeedback will display the button input above the character
    void OnTriggerExit2D(Collider2D coll) {
        if (coll.CompareTag("InteractTrigger"))
            isInInteractZone = false;
        
    }


}
