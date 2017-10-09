using UnityEngine;
using System.Collections;

public class TutorialDummy : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    int numberSpawned;
    public float distance;
    public float wakeRange;
    float shootInterval = 4;
    float bulletSpeed = 15;
    float bulletTimer = 0;
    bool attackOnce = false;
    bool playAudioOnce = false;
    public GameObject bullet;
    GameObject bulletClone;
    public Transform target;
    public Transform shootPoint;
    SwordHitFeedback swordHit;
    EnemyManager em;
    public ParticleSystem dummyAttacked;
    AudioSource source;
    public AudioClip dummyHit;
    public AudioClip dummyHitSpirit;
    public AudioClip shoot;

    void Start(){
        source = GetComponent<AudioSource>();
    }

    
    void Update()
    {
      

        //when the bulletsTutorial is true in GameManager it will trigger here the dummy to start shooting bullets
        if(GameManager.instance.bulletsTutorial == true){

            bulletTimer += Time.deltaTime;

            //every 1.5 seconds it will spawn a bullet with the initial animation
            if (bulletTimer >= 1.5f)
            {
                //play an audio every time it shoots
                if (playAudioOnce == false && GameManager.instance.tutorial2Complete == false)
                {
                    source.PlayOneShot(shoot, 0.2f);
                }

                //only spawn one at a time
                if (numberSpawned < 1)
                {
                    bulletClone = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation) as GameObject;
                    numberSpawned++;
                }

                playAudioOnce = true;


            }
            
            //after 2 seconds, the bullet will leave the dummy towards the shoot point
            if (bulletTimer >= 2f && attackOnce == false)
            {
                attackOnce = true;
                Attack();
            }

     

        }
        
    }

    
    //this method will make the bullet move towards a shootPoint target
    void Attack ()
    {
        Vector2 direction = target.transform.position - transform.position; //gets the direction the player is
        direction.Normalize (); //and normalizes it

        //make sure that a bullet exists
        if (bulletClone != null)
        {
            bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }

        attackOnce = false;
        bulletTimer = 0; //resets the variables for another bullet
        numberSpawned = 0;
        playAudioOnce = false;
    }


    
    //if the player hits the enemy with a sword attack in the human form, it will deal damage, play the slashing animation and shake the camera
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.CompareTag ("Attack_Human1")) {
            DummyAttacked();
            source.PlayOneShot(dummyHit, 0.4f);
        }
        
        if (coll.CompareTag ("Attack_Human2")) {
            DummyAttacked();
            source.PlayOneShot(dummyHit, 0.4f);
        }
        
        if (coll.CompareTag ("Attack_Human3")) {
            DummyAttacked();
            source.PlayOneShot(dummyHit, 0.4f);
            GameManager.instance.dummyThreeHitComboCounterHuman++;
        }

        if (coll.CompareTag ("Attack_HumanAir")) {
            DummyAttacked();
            source.PlayOneShot(dummyHit, 0.4f);
            GameManager.instance.dummyAirAttackCounterHuman++;

        }
        
        if (coll.CompareTag ("Attack_Spirit1")) {
            DummyAttacked();
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }
        
        if (coll.CompareTag ("Attack_Spirit2")) {
            DummyAttacked();
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }
        
        if (coll.CompareTag ("Attack_Spirit3")) {
            DummyAttacked();
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }

        if (coll.CompareTag ("Attack_SpiritAir")) {
            DummyAttacked();
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }

        if (coll.CompareTag ("Attack_SpiritDash")) {
            DummyAttacked();
            GameManager.instance.dummyDashAttackCounterSpirit++;
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }

        if (coll.CompareTag ("Attack_SpiritLauncher")) {
            DummyAttacked();
            GameManager.instance.dummyLauncherAttackCounterSpirit++;
            source.PlayOneShot(dummyHitSpirit, 0.4f);
        }
           
    }
    


    //this method manages when the dummy is hit, showing visual and audio feedback
    void DummyAttacked(){
        swordHit = FindObjectOfType<SwordHitFeedback>();
        swordHit.hitEnemy = true;
        PlayerManager.instance.playerHitEnemy = true;
        PlayerManager.instance.comboCounter++;
        Instantiate(dummyAttacked, gameObject.transform.position, gameObject.transform.rotation);
    }
    
}
