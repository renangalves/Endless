using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller))]

public class EnemyManager : MonoBehaviour
{ 

    //This script was more intended on being used by the final boss, which had to be cut
    //With this script I could control movement on enemies, at first the soldiers were going to move, but that was changed

	float jumpHeight = 9f; //used to tune the jumpVelocity on the inspector (how high want to jump)
	public float timeToJump = .5f; //used to tune the gravity on the inspector (how fast to hit the high of the jump) 

	public Vector3 velocity;
	public float accelerationTimeOnAir = .2f; //for tune the aceleration while in the air
	public float accelerationTimeOnGround = .1f; //for tune the aceleration while in the ground
	public float moveSpeed = 6;
	public bool isJumping = false;
    public bool jumpTrigger = false;
	//public bool enemyDetected = false;
	//public bool isAttacking = false;
	
	float velocityOnXSmoother;
	public Controller mainController;
	
	//Isolates the acelleration (our gravity)
	float gravity; //used to the velocity when falling
	
	//velocityFinal = velocityInitial + (acceleration*time)  // (velocity = velocityFinal - velocityInicial)
	float jumpVelocity; //used to the velocity when jumping
	
	//protected Animator animator;
	
	
	
	
	
	
	
	
	void Start ()
	{
		
		//instead of using Unity's physics we calculate the gravity and velocities
		gravity = -(5 * jumpHeight) / Mathf.Pow (timeToJump, 2);
		
		jumpVelocity = Mathf.Abs (gravity) * timeToJump;
		
		//animator = GetComponent<Animator> ();
		
		mainController = GetComponent<Controller> ();
		
	}
	
	void Update ()
	{  

			
		//GetAxisRaw can only be 0,1,-1 and pure getAxis have a sensitivity, so it takes a while to be 1 or -1 
		Vector2 enemyInput = new Vector2 (0f, 0f); 
			
			
		float targetVelocityOnX = enemyInput.x * moveSpeed;
			
		/*current position on X, the target position on X, a reference of the current movement 
         and the smooth time (how long it takes to go to the current velocity to the target velocity)
         so the smooth time will be float accelerationTimeOnGround if colliders are on ground 
         otherwise smooth time will be accelerationTimeOnAir */
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityOnX, ref velocityOnXSmoother, (mainController.allCollisions.below) ? accelerationTimeOnGround : accelerationTimeOnAir); 
			
			
		//dont acumulate gravity when is on the ground
		if (mainController.allCollisions.above || mainController.allCollisions.below) { 
			isJumping = false;
			velocity.y = 0; //set "gravity" to 0
		}
			
		if (jumpTrigger == true) {
			//if the player is on the wall, checks which direction he is pressing to accordingly jump away from the wall
				
			if (mainController.allCollisions.below) { //checks is on the ground
				isJumping = true;
				velocity.y = jumpVelocity; //makes the jump
			} 
				
		}
			
			
		//the jump
		velocity.y += gravity * Time.deltaTime; 
			
		//move the player according to the velocity, but first checks if the player is attacking, which will stop the player from moving

		mainController.Move (velocity * Time.deltaTime);
		
	}
	
}
