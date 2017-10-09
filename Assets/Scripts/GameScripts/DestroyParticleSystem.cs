using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour {


    Transform target;

    Vector3 particleCorrect;
    public int correctAmount;

    public bool followPlayer;
    public bool dontDestroy;

	//finds the player for some player-following particles
	void Start () {
	
        target = GameObject.Find("AllPlayer").transform;

	}
	
	
	void Update () {

        //if followPlayer is checked on the inspector then the particle will follow the player and not destroyed immediatelly
        if(followPlayer == true){
            particleCorrect = target.position;
            particleCorrect.y += correctAmount;
            gameObject.transform.position = particleCorrect;

        }

        //controls the particle when the player is in spirit form, destroying the particle when he goes back to human form
        if(gameObject.CompareTag("BodySpiritParticle")){
            particleCorrect = target.position;
            particleCorrect.y += 3f;
            gameObject.transform.position = particleCorrect;
            if(PlayerManager.instance.isSpiritForm == false){
                Destroy(gameObject);
            }

        //same for the BodySpiritParticle, but it's spawned in a different position and destroyed when the player leaves the soul counter power (or chakra power)
        } else if(gameObject.CompareTag("SoulCounterPowerParticle")){
            particleCorrect = target.position;
            particleCorrect.y += 5f;
            gameObject.transform.position = particleCorrect;
            if(PlayerManager.instance.isInSpecial == false){
                Destroy(gameObject);
            }

        //dontDestroy is changed in the inspector, it's purpose is to avoid automatically destroying certain particles
        } else {
            if(dontDestroy == false){
                Destroy(gameObject, 2f);
            }
        }
	}
}
