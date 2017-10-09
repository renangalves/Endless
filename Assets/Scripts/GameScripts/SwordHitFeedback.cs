using UnityEngine;
using System.Collections;

public class SwordHitFeedback : MonoBehaviour
{

	Animator anim;

	public bool hitEnemy;
	float counter;


	void Start ()
	{

		anim = GetComponent<Animator> ();

	}

	//The reason for this script is to active the sword hit animation
	void Update ()
	{

		if (hitEnemy == true) {
			anim.SetBool ("Hit", true);
			counter += Time.deltaTime;
		}

        //after a very short while, deactivate the sword hit animation
		if (counter >= 0.04f) {
			hitEnemy = false;
			anim.SetBool ("Hit", false);
			counter = 0;
		}
	}
}
