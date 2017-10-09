using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	Vector3 lastPosition;
	public Transform target;
	//Transform cameraPos;
	public GameObject fixedCameraPosition;
	public bool isInFixedCombatScreen = false;
    public bool playerDied = false;
    public bool levelTransition = false;
	Camera camera;
	float shakeAmount = 0;
	public static MainCamera instance;
	float posY;
	

    //There's a lot of magic numbers here, sorry about that. All of it is used for positioning, but I should avoid it anyway


	void Start ()
	{
		MainCamera.instance = this;
		camera = GetComponent<Camera> ();

	}

	//Update is for now used to follow the target (player) with in a smooth way with SmoothDamp
	void Update ()
	{
		//does all the calculations for the smoothing of the camera
		if (isInFixedCombatScreen == false) {
			if (target && levelTransition == false) {
				CameraSmoothDamping(target);
			}
		} else {
			CameraSmoothDamping(fixedCameraPosition.gameObject.transform); //smooths the camera movement
		}

        //if the player dies, positions the camera back to the player without smoothing
        if(playerDied == true){
            DeathCameraPositioning(target);
        }
	}



    //this method will smooth the camera when it's following the player
	void CameraSmoothDamping(Transform pos){

		Vector3 point = camera.WorldToViewportPoint (pos.position);
		Vector3 delta = pos.position - camera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z)); 
		Vector3 destination = transform.position + delta;
		
        //these ifs and elses are used to trigger certain events like tutorials
        //when the player reaches the end of the first level, will fix the camera
        if (destination.x <= 390 && destination.x >= 305 && destination.y < -126 && destination.y >= -152) {
			destination.y = -140f;
            destination.x = 306f;

        //when the player reaches the door which will trigger the first tutorial
        } else if (destination.x >= -130f  && destination.x <= -120f && destination.y < 130f && destination.y >= 120f && GameManager.instance.tutorial1Complete == false) {
            destination.x = -108.7f;
            destination.y = 125f;
            GameManager.instance.tutorial1 = true;

        //when the player reaches the door with the tutorial dummy
        } else if (destination.x >= -61f  && destination.x <= 3f && destination.y < 150f && destination.y >= 107f && GameManager.instance.tutorial2Complete == false) {
            destination.x = -29f;
            destination.y = 121f;
            GameManager.instance.tutorial2 = true;

        //when the player reaches the door with the first super special (Soul Destruction) pickup near 
        } else if (destination.x >= 48f  && destination.x <= 60f && destination.y < 20f && destination.y >= 0f && GameManager.instance.tutorial5Complete == false) {
            destination.x = 85.8f;;
            destination.y = 5.7f;
            GameManager.instance.tutorial5 = true;
        }

		transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);

	}


    //places the camera focused on the character when he dies
    void DeathCameraPositioning(Transform pos){
        Vector3 lastPosition = pos.position;
        lastPosition.z = -1;
        transform.position = lastPosition;
    }



	//this method controls the camera shake effect when the player hits an enemy on human form
	public void Shake (float amt, float length)
	{
		shakeAmount = amt;
		InvokeRepeating ("BeginShake", 0, 0.01f);
		Invoke ("StopShake", length);
		lastPosition = camera.transform.position;
	}


	//shakes the camera in a direction depending on the random values set in the offsetX and offsetY
	void BeginShake ()
	{
		if (shakeAmount > 0) {
			Vector3 camPos = camera.transform.position;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x += offsetX;
			camPos.y += offsetY;

			camera.transform.position = camPos;
		}
	}


	//stops the shaking of the camera and resets back to the original position
	void StopShake ()
	{
		CancelInvoke ("BeginShake");
		camera.transform.localPosition = lastPosition;
	}

	
}
