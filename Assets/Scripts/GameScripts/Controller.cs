using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider2D))]
//requiring a boxcollider on the script

public class Controller : MonoBehaviour
{

	//this script and the PlayerManager was used in our Hat Jam Legend of Paulo game, and I'm reusing it here

	//all movement checks will be on objects with "Terrain" layer
	public LayerMask terrainMask;
	public LayerMask invisibleTerrainMask;
	
	private float skinWidth = 1.0f;
	BoxCollider2D mainCollider;
	
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	
	RayCastOrigins rayCastOrigin;
	
	public CollisionsInfo allCollisions; //creating a public reference to the CollisionsInfo
	
	public float directionOnX;
	
	float horizontalRaySpace;
	float verticalRaySpace;

	
	void Start ()
	{
		mainCollider = GetComponent<BoxCollider2D> ();
		//calculate the spacing right after getting the Collider
		allCollisions.faceDirection = 1;
	}
	
	
	void Update ()
	{
		CalculateRaySpacing ();
		UpdateRayCastOrigins ();
		
		
	}


	//move the character depending on the velocity set by the player input in the PlayerManager script
	public void Move (Vector3 velocity)
	{

		allCollisions.Reset (); //reseting all colisions on the begining
		
		if (velocity.x != 0) {
			//checks the last direction and store the information on the faceDirection the (int) transform the velocity.x (a float) into a int
			//Mathf.Sign -> Return value is 1 when f is positive or zero, -1 when f is negative.
			allCollisions.faceDirection = (int)Mathf.Sign (velocity.x); 
		}
		
		HorizontalCollision (ref velocity);//if the velocity changes, this also changes on the fly
		
		if (velocity.y != 0) {
			VerticalCollision (ref velocity); //if the velocity changes, this also changes on the fly
		}
		

		transform.Translate (velocity);
	}
	


	//controls all the horizontal collision and horizontal raycasts, also ref creates a copy off a variable
	void HorizontalCollision (ref Vector3 velocity)
	{
		directionOnX = allCollisions.faceDirection; //if moving right direcionOnX will be equal to -1, if left it will be 1
		float rayLength = Mathf.Abs (velocity.x) + skinWidth; //turn the velocity.x value into a absolute value (always positive)
		
		
		//give some more distance to the collider to detect the wall even if the player is not moving to the direction of the wall
		if (Mathf.Abs (velocity.x) < skinWidth) {
			rayLength = 2 * skinWidth; 
		}
		
		for (int i =0; i<horizontalRayCount; i++) {
			//the question mark act like a if and the collum acts like a else
			Vector2 rayOrigin = (directionOnX == -1) ? rayCastOrigin.bottomLeft : rayCastOrigin.bottomRight;  //checks if the player is going down and sets the raycast to the botton
			rayOrigin += Vector2.up * (horizontalRaySpace * i);
			
			//if the player hits the terrain that is on the layer terrainMask, hit will be true
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionOnX, rayLength, terrainMask); 
			RaycastHit2D hitInvWall = Physics2D.Raycast (rayOrigin, Vector2.right * directionOnX, rayLength, invisibleTerrainMask); 

			//shows the raycast for reference
			Debug.DrawRay (rayOrigin, Vector2.right * directionOnX, Color.red);
			
			if (hit) { //if the raycast hit something
				/*
                     the vertical velocity will be the distance of the object that the raycast finds - the skinwidth (that is the "skin")
                      multiplied by the direction of the actual movment (up(1) or down(-1))
                 */
				velocity.x = (hit.distance - skinWidth) * directionOnX; 
				
				//this makes sure that if at least one raycast is on a terrain it sets the velocity to be this hit distance, so it will be 0
				rayLength = hit.distance; 
				
				allCollisions.left = directionOnX == -1; //if we hit something and colide with anything, collision left will be true
				allCollisions.right = directionOnX == 1; // same as above but for the right position
				
			}

			if (hitInvWall) { //if the raycast hit something
				/*
                     the vertical velocity will be the distance of the object that the raycast finds - the skinwidth (that is the "skin")
                      multiplied by the direction of the actual movment (up(1) or down(-1))
                 */
				velocity.x = (hitInvWall.distance - skinWidth) * directionOnX; 
				
				//this makes sure that if at least one raycast is on a terrain it sets the velocity to be this hit distance, so it will be 0
				rayLength = hitInvWall.distance; 
			}
		}
	}
	
	
	//controls all the vertical collision and vertical raycasts, also ref creates a copy off a variable
	void VerticalCollision (ref Vector3 velocity)
	{
		float directionOnY = Mathf.Sign (velocity.y); //if moving down direcionOnY will be equal to -1, if moving up it will be 1
		float rayLength = Mathf.Abs (velocity.y) + skinWidth + 0.1f; //turn the velocity.y value into a absolute value (always positive)
		
		for (int i =0; i<verticalRayCount; i++) {
			//the question mark act like a if and the collum acts like a else
			Vector2 rayOrigin = (directionOnY == -1) ? rayCastOrigin.bottomLeft : rayCastOrigin.topLeft;  //checks if the player is going down and sets the raycast to the botton
			rayOrigin += Vector2.right * (verticalRaySpace * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionOnY, rayLength, terrainMask); //if hit the terrain that is on the layer terrainMask, hit will be true

			//shows the raycast for reference
			Debug.DrawRay (rayOrigin, Vector2.up * directionOnY, Color.red);
			
			if (hit) { //if the raycast hit something
				/*
                     the vertical velocity will be the distance of the object that the raycast finds - the skinwidth (that is the "skin")
                      multiplied by the direction of the actual movment (up(1) or down(-1))
                 */
				velocity.y = (hit.distance - skinWidth) * directionOnY; 
				
				//this makes sure that if at least one raycast is on a terrain it sets the velocity to be this hit thistance, so it will be 0
				rayLength = hit.distance; 
				
				allCollisions.below = directionOnY == -1; //below is equal true if collide with something on top
				allCollisions.above = directionOnY == 1; //above is equal true if collide with something on the ground
				
			}
		}
	}
	
	
	
	
	//controls where the raycasting initiate
	void UpdateRayCastOrigins ()
	{
		Bounds boundarys = mainCollider.bounds; //getting the value of the bounds of the box collider
		boundarys.Expand (skinWidth * -2); //going even futher into only getting the boundarys of the collider by multyplying by a small width
		
		//setting the values of the raycast to the edge of the colliders
		rayCastOrigin.bottomLeft = new Vector2 (boundarys.min.x, boundarys.min.y); 
		rayCastOrigin.bottomRight = new Vector2 (boundarys.max.x, boundarys.min.y);
		rayCastOrigin.topLeft = new Vector2 (boundarys.min.x, boundarys.max.y);
		rayCastOrigin.topRight = new Vector2 (boundarys.max.x, boundarys.max.y);
		
	}



	//controls the spacing beteween the raycasts
	void CalculateRaySpacing ()
	{
		Bounds boundarys = mainCollider.bounds; //getting the value of the bounds of the box collider
		boundarys.Expand (skinWidth * -2);
		
		//the minimum spacing should be 2 (one on each corner) and the max is any value, that is what int.MaxValue means
		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue); 
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);
		
		/*
            setting what is the space based on how many Rays we have, EX: if we select the minimum amount of raycast (2), 
            the horizontalRaySpace will be the lenght of the colider on Y divided by (2(minimum value) - 1), so it will create 2 raycastings, one on each borde
         */
		
		horizontalRaySpace = boundarys.size.y / (horizontalRayCount - 1);
		verticalRaySpace = boundarys.size.x / (verticalRayCount - 1);
	}


	//control the bools that checks with direction the player is colliding
	public struct CollisionsInfo
	{
		public bool above, below;
		public bool left, right;
		
		public int faceDirection; //1 if facing right and -1 if facing left
		
		public void Reset ()
		{
			above = below = false;
			left = right = false;
		}
	}
	

	//struct is a function that hold values that don`t change often
	struct RayCastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
