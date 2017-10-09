using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    int curHealth = 8;
    public int playerDirection;
    int randomSound;
    int randomBloodParticleLocation;
    int shootLocation;
    int numberSpawned = 0;
    public float distance;
    public float wakeRange;
    float shootInterval = 4;
    public float bulletSpeed = 100;
    public float bulletTimer = 0;
    float nextDetectionCounter = 0;
    float meleeAttackTimer = 0;
    public bool lookingRight = true;
    public bool isNotInBattleArea;
    public bool playerDetected;
    bool oneSoul = false;
    bool setAnimationOnce = false;
    bool attackOnce = false;
    public bool touchingPlayer = false;
    bool dealtMeleeDamage;
    bool playAudioOnce = false;
    bool playWarnOnce = false;
    bool willShoot = false;
    public GameObject bullet;
    public GameObject spawnEnemySpirit;
    GameObject bulletClone;
    public ParticleSystem bloodParticle;
    Transform target;
    public Transform shootPoint0;
    public Transform shootPoint30;
    public Transform shootPoint60;
    public Transform shootPoint90;
    public Transform shootPoint120;
    public Transform shootPoint150;
    public Transform shootPoint180;
    public Transform[] bloodParticleReferences;
    Transform chosenShootPoint;
    SwordHitFeedback swordHit;
    MainCamera cameraVar;
    Animator anim;
    Vector3 enemyPosition;
    EnemyManager em;
    AudioSource source;
    public AudioClip[] deaths;
    public AudioClip[] stabs;
    public AudioClip shoot;
    public AudioClip daggerAttack;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        cameraVar = FindObjectOfType<MainCamera>(); //gets the MainCamera component to shake the camera when needed
        target = GameObject.Find("AllPlayer").transform;
        em = GetComponent<EnemyManager>();
        anim.SetInteger("AimDirection", 23);
    }

    void Update()
    {
        enemyPosition.x = gameObject.transform.position.x;
        enemyPosition.y = gameObject.transform.position.y;
        

        if (curHealth > 0)
        {
            if (playerDirection == 0)
            {
                shootLocation = 0;
                chosenShootPoint = shootPoint0;
            } else if (playerDirection == 30)
            {
                shootLocation = 30;
                chosenShootPoint = shootPoint30;
            } else if (playerDirection == 60)
            {
                shootLocation = 60;
                chosenShootPoint = shootPoint60;
            } else if (playerDirection == 90)
            {
                shootLocation = 90;
                chosenShootPoint = shootPoint90;
            } else if (playerDirection == 120)
            {
                shootLocation = 120;
                chosenShootPoint = shootPoint120;
            } else if (playerDirection == 150)
            {
                shootLocation = 150;
                chosenShootPoint = shootPoint150;
            } else if (playerDirection == 180)
            {
                shootLocation = 180;
                chosenShootPoint = shootPoint180;
            }

            //Checks if the player position is higher than the position of the enemy, to turn him around
            if (target.transform.position.x > transform.position.x)
            {
                lookingRight = true;
                transform.localScale = new Vector3(-4.5f, 4.5f, 4.5f); //scale of current enemy
            }
            
            //Same as above, but for the other direction
            if (target.transform.position.x < transform.position.x)
            {
                lookingRight = false;
                transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
            }

            if (playerDetected == true)
            {
                willShoot = true;
            }

            if (willShoot == true && touchingPlayer == false)
            {
                bulletTimer += Time.deltaTime;
                if (bulletTimer >= 1.0f && bulletTimer <= 2.0f)
                {
                    anim.SetBool("Reload", true);
                } else
                {
                    anim.SetBool("Reload", false);
                }

                if (bulletTimer >= 4.0f)
                {
                    anim.SetInteger("AimDirection", shootLocation);
                }

                if (bulletTimer >= 4.5f)
                {
                    if (playAudioOnce == false && PlayerManager.instance.lifePoints >= 0)
                    {
                        source.PlayOneShot(shoot, 0.2f);
                        if (numberSpawned < 1)
                        {
                            bulletClone = Instantiate(bullet, chosenShootPoint.transform.position, chosenShootPoint.transform.rotation) as GameObject;
                            numberSpawned++;
                        }
                        playAudioOnce = true;
                    }
                    if (bulletClone != null)
                    {
                        bulletClone.transform.position = chosenShootPoint.position;
                        bulletClone.transform.rotation = chosenShootPoint.rotation;
                    }
                }

                //show an animation of the enemy readying to attack
                if (bulletTimer >= 5f && attackOnce == false)
                {
                    attackOnce = true;
                    Attack();
                }
            } else if (touchingPlayer == true)
            {
                anim.SetInteger("AimDirection", 23);
                anim.SetBool("Reload", false);
                if(meleeAttackTimer >= 0){
                    anim.SetBool("Melee", true);
                }
                meleeAttackTimer += Time.deltaTime;
                if (meleeAttackTimer >= 1.2f && dealtMeleeDamage == false)
                {
                    if (PlayerManager.instance.isBlocking == false)
                    {
                        if (PlayerManager.instance.isInvulnerable == false && PlayerManager.instance.isInSuperSpecial == false)
                        {
                            PlayerManager.instance.lifePoints -= 1;
                            if (PlayerManager.instance.lifePoints >= 0)
                            {
                                source.PlayOneShot(daggerAttack, 0.5f);
                            } else if (PlayerManager.instance.lifePoints <= -1)
                            {
                                Destroy(gameObject, 2f);
                            }
                            PlayerManager.instance.isInvulnerable = true;
                        }
                    } else if(PlayerManager.instance.isBlocking == true){
                        PlayerManager.instance.attackBlocked = true;
                    }
                    dealtMeleeDamage = true;
                }
            }

            if (meleeAttackTimer >= 2.1f)
            {
                anim.SetBool("Melee", false);
                meleeAttackTimer = -2;
                dealtMeleeDamage = false;
            }

        } else
        {

            if (playAudioOnce == false)
            {
                randomSound = Random.Range(0, deaths.Length);
                source.PlayOneShot(deaths [randomSound], 0.5f);
                playAudioOnce = true;
            }
            if(bulletClone != null && attackOnce == false){
                Destroy(bulletClone);
            }
            anim.SetInteger("AimDirection", 23);
            anim.SetBool("Melee", false);
            anim.SetBool("Reload", false);
            anim.SetBool("Death", true);
            Destroy (gameObject, 2f);
            if (isNotInBattleArea == true)
            {
                if (oneSoul == false)
                {
                    Instantiate(spawnEnemySpirit, enemyPosition, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                    oneSoul = true;
                }
            }
        }
    
    }

    










    //controls the attacking of the enemy when the player is in range
    void Attack()
    {
    
        //activates the attack
        //if (bulletTimer >= shootInterval) {
        Vector2 direction = target.transform.position - transform.position; //gets the direction the player is
        direction.Normalize(); //and normalizes it

            
            
        if (curHealth > 0)
        {
                
            if (bulletClone != null)
            {
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
        }
                
            
        attackOnce = false;
        setAnimationOnce = false;
        nextDetectionCounter = 0;
        bulletTimer = 0;
        numberSpawned = 0;
        playWarnOnce = false;
        playAudioOnce = false;
        willShoot = false;
        anim.SetInteger("AimDirection", 23);
        //}

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
            if (coll.CompareTag("Attack_Human1"))
            {
                PlayerDealtDamage(1, 0.35f, 0.05f);
                if (playAudioOnce == false && PlayerManager.instance.isInSuperSpecial == false)
                {
                    source.PlayOneShot(stabs [0], 0.5f);
                    playAudioOnce = true;
                }
            }

            if (coll.CompareTag("Attack_Human2"))
            {
                PlayerDealtDamage(2, 0.45f, 0.10f);
                if (playAudioOnce == false)
                {
                    source.PlayOneShot(stabs [1], 0.5f);
                    playAudioOnce = true;
                }
            }

            if (coll.CompareTag("Attack_Human3"))
            {
                PlayerDealtDamage(3, 0.55f, 0.15f);
                if (playAudioOnce == false)
                {
                    source.PlayOneShot(stabs [2], 0.5f);
                    playAudioOnce = true;
                }
            }

            if (coll.CompareTag("Attack_HumanAir"))
            {
                PlayerDealtDamage(2, 0.45f, 0.10f);
                if (playAudioOnce == false)
                {
                    source.PlayOneShot(stabs [1], 0.5f);
                    playAudioOnce = true;
                }
            }
            
            if (coll.CompareTag("Attack_Spirit1"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(1, 0.35f, 0.05f);
                }
                
            }

            if (coll.CompareTag("Attack_Spirit2"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(2, 0.45f, 0.10f);
                }
                
            }

            if (coll.CompareTag("Attack_Spirit3"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(3, 0.55f, 0.15f);
                }
                
            }

            if (coll.CompareTag("Attack_SpiritAir"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(2, 0.45f, 0.10f);
                }
                
            }

            if (coll.CompareTag("Attack_SpiritDash"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(2, 0.45f, 0.10f);
                }
                
            }

            if (coll.CompareTag("Attack_SpiritLauncher"))
            {
                if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                {
                    PlayerDealtDamage(3, 0.55f, 0.15f);
                }
                
            }
            playAudioOnce = false;
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("VIADO TA FUGINDO");
            anim.SetBool("Melee", false);
            touchingPlayer = false;
            meleeAttackTimer = 0;
            dealtMeleeDamage = false;
        }
    }

    void PlayerDealtDamage(int damage, float shakeLength, float shakePower)
    {
        swordHit = FindObjectOfType<SwordHitFeedback>();
        swordHit.hitEnemy = true;
        PlayerManager.instance.playerHitEnemy = true;
        PlayerManager.instance.comboCounter++;
        cameraVar.Shake(shakeLength, shakePower);
        Instantiate(bloodParticle, bloodParticleReferences [Random.Range(0, 4)].position, bloodParticleReferences [Random.Range(0, 4)].rotation);
        curHealth -= damage; //I could just change the curHealth here, but maybe I will use an
        //plays an animation of the enemy flashing red indicating damage (maybe I will use this later, that's why I'm keeping this here)
        //gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }


}
