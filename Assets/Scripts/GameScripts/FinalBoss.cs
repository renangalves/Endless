using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    public int playerDirection;
    int randomAttack;

    float nextAttackCounter = 0;
    float meleeAttackTimer = 0;
    float jumpTimer = 0;
    public float bulletSpeed = 20;
    float shootInterval = 0;
    float lungeTimer = 0;

    bool setAnimationOnce = false;
    bool attackOnce = false;
    public bool touchingPlayer = false;
    bool playAudioOnce = false;
    bool jumpOnce = false;
    bool shootOnce = false;
    bool checkPlayerPositionOnce = false;
    bool lungeLeft = false;
    bool lungeRight = false;
    bool oneAttack = false;

    public GameObject bullet;
    public GameObject spawnEnemySpirit;
    Transform target;
    public Transform[] wallJumpShootAttackPoints;
    SwordHitFeedback swordHit;
    MainCamera cameraVar;
    Animator anim;
    Vector3 enemyPosition;
    EnemyManager em;
    AudioSource source;
    public AudioClip[] deaths;


    //*****************************THIS SCRIPT IS COMPLETELY UNUSED, THE BOSS HAD TO BE CUT SO ALL THE CODING BELOW IS NOT USED ANYWHERE IN THE GAME**************************
    //(I should have deleted it, but I want to keep it for possible future uses)

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        cameraVar = FindObjectOfType<MainCamera>(); //gets the MainCamera component to shake the camera when needed
        target = GameObject.Find("AllPlayer").transform;
        em = GetComponent<EnemyManager>();
    }
    
    void Start()
    {
        curHealth = maxHealth;
        LookAtPlayer();
        checkPlayerPositionOnce = false;
    }
    
    void Update()
    {
        //Debug.Log("PLAYER DIRECTION " + playerDirection);
        enemyPosition.x = gameObject.transform.position.x;
        enemyPosition.y = gameObject.transform.position.y;
        
        nextAttackCounter += Time.deltaTime;

        if (attackOnce == true)
        {
            nextAttackCounter = 0;
            attackOnce = false;
            oneAttack = false;
        }

        if (nextAttackCounter >= 3)
        {
            if (attackOnce == false && oneAttack == false)
            {
                randomAttack = Random.Range(0, 2);
                oneAttack = true;
                LookAtPlayer();
                checkPlayerPositionOnce = false;
            }
            if (randomAttack == 0)
            {
                WallShootJumpAttack();
            } else if (randomAttack == 1)
            {
                LungingAttack();
            }
        }

        
    }
    
    
    
    
    
    //controls the attacking of the enemy when the player is in range
    void WallShootJumpAttack()
    {
        jumpTimer += Time.deltaTime;
        shootInterval += Time.deltaTime;
        if (jumpOnce == false)
        {
            em.jumpTrigger = true;
            jumpOnce = true;
        }
       
        if (jumpTimer >= 0.5f && jumpTimer <= 3)
        {
            em.jumpTrigger = false;
            em.velocity.y = 0;
        }

        Vector2 direction1 = wallJumpShootAttackPoints [3].transform.position - transform.position; //gets the direction the player is
        Vector2 direction2 = wallJumpShootAttackPoints [4].transform.position - transform.position;
        Vector2 direction3 = wallJumpShootAttackPoints [5].transform.position - transform.position;
        direction1.Normalize(); //and normalizes it
        direction2.Normalize();
        direction3.Normalize();
        
        GameObject bulletClone1;
        GameObject bulletClone2;
        GameObject bulletClone3;

        if (jumpTimer < 3)
        {
            if (shootInterval >= 0.5f)
            {
                bulletClone1 = Instantiate(bullet, wallJumpShootAttackPoints [0].transform.position, wallJumpShootAttackPoints [0].transform.rotation) as GameObject;
                bulletClone1.GetComponent<Rigidbody2D>().velocity = direction1 * bulletSpeed;
                
                bulletClone2 = Instantiate(bullet, wallJumpShootAttackPoints [1].transform.position, wallJumpShootAttackPoints [1].transform.rotation) as GameObject;
                bulletClone2.GetComponent<Rigidbody2D>().velocity = direction2 * bulletSpeed;
                
                bulletClone3 = Instantiate(bullet, wallJumpShootAttackPoints [2].transform.position, wallJumpShootAttackPoints [2].transform.rotation) as GameObject;
                bulletClone3.GetComponent<Rigidbody2D>().velocity = direction3 * bulletSpeed;
                shootInterval = 0;
            }
        }
        
        if (jumpTimer >= 5)
        {
            attackOnce = true;
            jumpTimer = 0;
            jumpOnce = false;
            shootInterval = 0;
            setAnimationOnce = false;
            checkPlayerPositionOnce = false;
        }
        
    }

    void LungingAttack()
    {
        lungeTimer += Time.deltaTime;
        bool lookDirection = LookAtPlayer();
        if(lookDirection){
            lungeRight = true;
        } else {
            lungeLeft = true;
        }

        if(lungeTimer > 2.1f && lungeTimer < 2.4f){
            if(lungeLeft == true){
                em.mainController.Move(new Vector3(-200, 0, 0) * Time.deltaTime);
            } else if(lungeRight == true) {
                em.mainController.Move(new Vector3(200, 0, 0) * Time.deltaTime);
            }
        }

        Debug.Log(em.velocity.x);

        if (lungeTimer >= 4)
        {
            attackOnce = true;
            lungeTimer = 0;
            lungeRight = false;
            lungeLeft = false;
            checkPlayerPositionOnce = false;
        }
    }




    bool LookAtPlayer(){

        if (checkPlayerPositionOnce == false)
        {
            
            if (target.position.x > gameObject.transform.position.x)
            {
                transform.localScale = new Vector3(2.954418f, 3.053683f, 7.538133f);
                checkPlayerPositionOnce = true;
                return true;
            } else if (target.position.x < gameObject.transform.position.x)
            {
                transform.localScale = new Vector3(-2.954418f, 3.053683f, 7.538133f);
                checkPlayerPositionOnce = true;
                return false;
            }
        }

        return true;

    }



    
    //if the player hits the enemy with a sword attack in the human form, it will deal damage, play the slashing animation and shake the camera
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (curHealth > 0)
        {
            if (coll.CompareTag("Player"))
            {
                Debug.Log("ATACA ELE MANO");
                touchingPlayer = true;
            }
            if (coll.CompareTag("Attack_Human"))
            {
                swordHit = FindObjectOfType<SwordHitFeedback>();
                swordHit.hitEnemy = true;
                PlayerManager.instance.playerHitEnemy = true;
                PlayerManager.instance.comboCounter++;
                cameraVar.Shake(0.55f, 0.15f);
                curHealth -= 20; //I could just change the curHealth here, but maybe I will use an
                //plays an animation of the enemy flashing red indicating damage (maybe I will use this later, that's why I'm keeping this here)
                //gameObject.GetComponent<Animation>().Play("Player_RedFlash");
            }
            
            if (coll.CompareTag("Attack_Spirit"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    swordHit = FindObjectOfType<SwordHitFeedback>();
                    swordHit.hitEnemy = true;
                    PlayerManager.instance.playerHitEnemy = true;
                    PlayerManager.instance.comboCounter++;
                    curHealth -= 20; //I could just change the curHealth here, but maybe I will use an
                    //plays an animation of the enemy flashing red indicating damage (maybe I will use this later, that's why I'm keeping this here)
                    //gameObject.GetComponent<Animation>().Play("Player_RedFlash");
                }
                
            }
        }   
    }
    
    
    /*void OnTriggerExit2D(Collider2D coll) {
        if (coll.CompareTag("Player")){
            Debug.Log("VIADO TA FUGINDO");
            anim.SetBool("Melee", false);
            touchingPlayer = false;
            meleeAttackTimer = 0;
            dealtMeleeDamage = false;
        }
    }*/
}
