using UnityEngine;
using System.Collections;

public class AttackCone : MonoBehaviour
{

    public Enemy enemyAI;
    public EnemySpirit enemySpiritAI;

    //get the component inside the gameobject so I can identufy which enemy is on the OnTriggerStay2D method
    //both soldier and sludge spirit have similar behaviour, and I should have used inheritance...
    void Awake()
    {
        enemyAI = gameObject.GetComponentInParent<Enemy>();
        enemySpiritAI = gameObject.GetComponentInParent<EnemySpirit>();
    }


    //when the player enter the enemy's view collider call this method
    void OnTriggerStay2D(Collider2D coll)
    {
        //if the enemy is not spirit, then it is a soldier and will act differently
        if (enemyAI != null && enemySpiritAI == null)
        {
            //checks if the collider is detecting the small point reference in the player 
            //and if the player is not touching the enemy to shoot him instead of melee attacking
            if (coll.CompareTag("PlayerPositionReference") && enemyAI.touchingPlayer == false)
            {
                //below is all the checks done to verify which of the vision cones the player currently is,
                //then change the variable to the position he actually is
                if (gameObject.CompareTag("AttackCone0"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 0;
                } else if (gameObject.CompareTag("AttackCone30"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 30;
                } else if (gameObject.CompareTag("AttackCone60"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 60;
                } else if (gameObject.CompareTag("AttackCone90"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 90;
                } else if (gameObject.CompareTag("AttackCone120"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 120;
                } else if (gameObject.CompareTag("AttackCone150"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 150;
                } else if (gameObject.CompareTag("AttackCone180"))
                {
                    enemyAI.playerDetected = true;
                    enemyAI.playerDirection = 180;
                }
                    
            }
            
            

          //if the enemy is not a soldier, then it is a spirit, which behaves a little different
        } else if (enemyAI == null && enemySpiritAI != null)
        {
            //if the enemy detects the player then change the variable playerDetected to true so he can attack
            if (coll.CompareTag("Player"))
            {
                enemySpiritAI.playerDetected = true;
            }
        }


    }


    //if the player leaves the enemy's vision zone, then stop attacking
    void OnTriggerExit2D(Collider2D coll) {
        if (enemyAI != null && enemySpiritAI == null){
            enemyAI.playerDetected = false;
        }
    }

}
