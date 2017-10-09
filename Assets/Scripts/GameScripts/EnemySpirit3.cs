using UnityEngine;
using System.Collections;

public class EnemySpirit3 : MonoBehaviour
{

    Transform target;
    Animator anim;
    GameObject attackMelee;
    public GameObject health;
    Vector3 dropRepositioning;
    Vector3 particleCorrectPosition;
    float attackCounter = 0;
    float randomTime;
    float attackTime;
    float resetTime;
    float groundPosition;
    int curHealth = 16;
    bool findPlayerPositionOnce = false;
    bool getRandomTime = false;
    bool getGroundPosition = false;
    bool isHitable = false;
    bool oneSoul = false;
    bool playAudioOnce = false;
    bool playAudioTwoOnce = false;
    bool playAttack3AudioOnce = false;
    Vector3 attackTarget;
    SwordHitFeedback swordHit; //ENEMY HIERARCHY

    public AudioClip[] attackSounds;
    public AudioClip beingHitSound;
    public AudioClip deathSound;
    AudioSource source;
    
    public ParticleSystem spiritHitParticle;
    public GameObject particle;

    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("AllPlayer").transform;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        foreach (Transform t in transform)
        {
            if (t.name == "AttackOneMelee")
            {
                t.gameObject.SetActive(false);
                attackMelee = t.gameObject;
            } 
        }
        particle = (GameObject)Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
    }
    
    // Update is called once per frame
    void Update()
    {
        particleCorrectPosition = gameObject.transform.position;
        particleCorrectPosition.y += 10f;
        if(PlayerManager.instance.lifePoints > -1){
            particle.transform.position = particleCorrectPosition;
        }
        if(isHitable == true){
            if(PlayerManager.instance.lifePoints > -1)
                particle.SetActive(true);
        } else {
            if(PlayerManager.instance.lifePoints > -1)
                particle.SetActive(false);
        }

        attackCounter += Time.deltaTime;

        if (curHealth > 0)
        {
            if (attackCounter >= 1 && attackCounter <= 1.1f && getGroundPosition == false)
            {
                groundPosition = transform.position.y;
                getGroundPosition = true;
            }

            if(attackCounter >= 2){
                anim.SetBool("Attack2", true);
                if(attackCounter >= 2.5f && attackCounter < 2.8f){
                    if(playAttack3AudioOnce == false && PlayerManager.instance.lifePoints >= 0){
                        source.PlayOneShot(attackSounds[3], 0.2f);
                        playAttack3AudioOnce = true;
                    }
                    attackMelee.SetActive(true);
                }
                if(attackCounter >= 2.8f){
                    attackMelee.SetActive(false);
                }
            }

            if (attackCounter >= 3)
            {
                anim.SetBool("Attack2", false);
                isHitable = false;
                if (getRandomTime == false)
                {
                    if(playAudioOnce == false && PlayerManager.instance.lifePoints >= 0){
                        source.PlayOneShot(attackSounds[0], 0.5f);
                        playAudioOnce = true;
                    }
                    anim.SetBool("Disappearing", true);
                    anim.SetBool("Attack1", true);
                    randomTime = Random.Range(2, 4);
                    randomTime += attackCounter;
                    attackTime = randomTime + 0.5f;
                    resetTime = attackTime + 3;
                    playAttack3AudioOnce = false;
                    getRandomTime = true;
                } else
                {
                    if (attackCounter >= randomTime)
                    {
                        if(playAudioTwoOnce == false && PlayerManager.instance.lifePoints >= 0){
                            source.PlayOneShot(attackSounds[1], 0.5f);
                            playAudioTwoOnce = true;
                        }
                        if (findPlayerPositionOnce == false)
                        {
                            attackTarget = target.position;
                            if (PlayerManager.instance.facingRight == true)
                            {
                                transform.localScale = new Vector3(1, 1f, 1f); //scale of current enemy
                                attackTarget.x -= 10;
                                attackTarget.y = groundPosition;
                            } else
                            {
                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                attackTarget.x += 10;
                                attackTarget.y = groundPosition;
                            }
                            transform.position = attackTarget;
                            anim.SetBool("Disappearing", false);
                            findPlayerPositionOnce = true;
                        }
                        isHitable = true;   
                    }
                    
                    if (attackCounter >= attackTime && attackCounter <= (attackTime + 1))
                    {
                        if(playAttack3AudioOnce == false && PlayerManager.instance.lifePoints >= 0){
                            source.PlayOneShot(attackSounds[2], 0.2f);
                            playAttack3AudioOnce = true;
                        }
                        attackMelee.SetActive(true);
                    } else
                    {
                        attackMelee.SetActive(false);
                    }
                    
                    if (attackCounter >= resetTime)
                    {
                        anim.SetBool("Attack1", false);
                        playAudioOnce = false;
                        playAudioTwoOnce = false;
                        playAttack3AudioOnce = false;
                        getRandomTime = false;
                        findPlayerPositionOnce = false;
                        attackCounter = 0;
                    }
                }
                
            } else
            {
                isHitable = true;
            }
            
            
            
            //HIERARCHY FOR ENEMY
            if (target.transform.position.x > transform.position.x)
            {
                
            }
            
            //Same as above, but for the other direction
            //ENEMY HIERARCHY
            if (target.transform.position.x < transform.position.x)
            {
                
            }
        } else
        {
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Death", true);
            attackMelee.SetActive(false);
            Destroy(gameObject, 2f);
            Destroy(particle, 2f);
            if (oneSoul == false)
            {
                source.PlayOneShot(deathSound, 0.5f);
                dropRepositioning = gameObject.transform.position;
                dropRepositioning.y += 5f;
                Instantiate(health, dropRepositioning, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                oneSoul = true;
            }
        }

        if(PlayerManager.instance.lifePoints <= -1){
            Destroy(particle, 2f);
        }
        

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (isHitable)
        {
            if (curHealth > 0)
            {
            
                if (coll.CompareTag("Attack_Spirit1"))
                {
                    PlayerDealtDamage(1);
                }

                if (coll.CompareTag("Attack_Spirit2"))
                {
                    PlayerDealtDamage(2);
                }

                if (coll.CompareTag("Attack_Spirit3"))
                {
                    PlayerDealtDamage(3);
                }

                if (coll.CompareTag("Attack_SpiritAir"))
                {
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
            
                if (coll.CompareTag("Attack_Human1"))
                {
                    if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                    {
                        PlayerDealtDamage(1);
                    }
                
                }

                if (coll.CompareTag("Attack_Human2"))
                {
                    if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                    {
                        PlayerDealtDamage(2);
                    }
                
                }

                if (coll.CompareTag("Attack_Human3"))
                {
                    if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                    {
                        PlayerDealtDamage(3);
                    }
                
                }

                if (coll.CompareTag("Attack_HumanAir"))
                {
                    if (PlayerManager.instance.isInSpecial == true || PlayerManager.instance.isInSuperSpecial)
                    {
                        PlayerDealtDamage(2);
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
        particleCorrectPosition.y += 8f;
        Instantiate(spiritHitParticle, particleCorrectPosition, gameObject.transform.rotation);
        curHealth -= damage; //I could just change the curHealth here, but maybe I will use an
        //plays an animation of the enemy flashing red indicating damage (maybe I will use this later, that's why I'm keeping this here)
        //gameObject.GetComponent<Animation>().Play("Player_RedFlash");
    }

}
