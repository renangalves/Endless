using UnityEngine;
using System.Collections;

public class EnemySpirit : MonoBehaviour
{

	int curHealth = 10;
    int randomDrop;
	
	public float distance;
	public float wakeRange;
	float shootInterval = 5;
	public float bulletSpeed = 100;
	float bulletTimer = 0;

	public bool lookingRight = true;
	bool oneSoul = false;
    public bool playerDetected = false;
    bool hasShot = false;
    bool playSoundOnce = false;
    bool playDeathSoundOnce = false;
	
	public GameObject bullet;
	public GameObject soul;
    public GameObject health;
    GameObject particle;
	Transform target;
    public Transform shootPoint;
	SwordHitFeedback swordHit;
	Vector3 enemyPosition;
    Vector3 particleCorrectPosition;
	
	Animator anim;

    public AudioClip beingHitSound;
    public AudioClip attack;
    public AudioClip death;
    AudioSource source;

    public ParticleSystem spiritHitParticle;
    public GameObject spiritEnergyParticle;
	
	
	void Awake ()
	{
		anim = GetComponentInChildren<Animator> ();
		target = GameObject.Find("AllPlayer").transform;
        source = GetComponent<AudioSource>();
	}
	
	
    void Start(){
        particleCorrectPosition = gameObject.transform.position;
        particleCorrectPosition.y += 8f;
        particle = (GameObject)Instantiate(spiritEnergyParticle, particleCorrectPosition, gameObject.transform.rotation);
    }
	
	void Update ()
	{	

		enemyPosition.x = gameObject.transform.position.x;
		enemyPosition.y = gameObject.transform.position.y;
		//Checks if the player position is higher than the position of the enemy, to turn him around
		if (target.transform.position.x > transform.position.x && curHealth > 0) {
			lookingRight = true;
			transform.localScale = new Vector3 (7.538133f, 7.538133f, 7.538133f); //scale of current enemy
		}
		
		//Same as above, but for the other direction
        if (target.transform.position.x < transform.position.x && curHealth > 0) {
			lookingRight = false;
			transform.localScale = new Vector3 (-7.538133f, 7.538133f, 7.538133f);
		}
		
		
		
		//checks if the player killed the enemy to play a little animation and destroy the gameobject
		if (curHealth <= 0) {
			anim.SetBool ("Death", true);
            if(playDeathSoundOnce == false){
                source.PlayOneShot(death, 1.0f);
                playDeathSoundOnce = true;
            }
			Destroy (gameObject, 1.5f);
            Destroy(particle);
			if(oneSoul == false){
                randomDrop = Random.Range(0,3);
                if(randomDrop == 0){
                    Instantiate(soul, enemyPosition, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                    oneSoul = true;
                } else {
                    Instantiate(health, enemyPosition, gameObject.transform.rotation); //when instantiating from a prefab, use these 3 arguments to correctly place the prefab
                    oneSoul = true;
                }
				
			}
			
		}

        if(PlayerManager.instance.lifePoints <= -1){
            Destroy(particle, 2f);
        }

        if(playerDetected == true){
            Attack();
        }
		
	}

	
	
	
	//controls the attacking of the enemy when the player is in range
	public void Attack ()
	{

		bulletTimer += Time.deltaTime;
		//show an animation of the enemy readying to attack
		if (bulletTimer >= 4.3f) {
			anim.SetBool ("ShootWarn", true);
		}

		//activates the attack
		if (bulletTimer >= shootInterval) {
            if(hasShot == false){
                Vector2 direction = target.transform.position - transform.position;
                direction.Normalize ();

                if(playSoundOnce == false && PlayerManager.instance.lifePoints >= 0 && curHealth > 0){
                    source.PlayOneShot(attack, 0.5f);
                    playSoundOnce = true;
                }

                GameObject bulletClone;
                if (curHealth > 0) {
                    bulletClone = Instantiate (bullet, shootPoint.transform.position, shootPoint.transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;
                    playerDetected = false;
                }
                hasShot = true;
            }

				
			
		}

        if(bulletTimer >= 6){
            hasShot = false;
            bulletTimer = 0;
            playSoundOnce = false;
            anim.SetBool ("ShootWarn", false);
        }
		
	}
	
	
	
	//if the player hits the enemy with a sword attack in the spirit form, it will deal damage and play the slashing animation
	void OnTriggerEnter2D (Collider2D coll)
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
