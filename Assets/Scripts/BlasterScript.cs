using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterScript : MonoBehaviour {

	public GameObject triggerunit;

	void Start(){
		GameObject blastertrigger = Instantiate (triggerunit, transform.position, Quaternion.identity);
		blastertrigger.transform.parent = transform;
	}

	/*
	 * If Alien Blaster collides with Tank Bullet, destroy Blaster in 0.15 seconds
	 * */
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.CompareTag ("bullet")) {
			Destroy (gameObject, 0.15f);
			Destroy (coll.gameObject, 1f);
		}
	}

	/*
	 * Destroy Gameobjects when they disappear off-screen.
	 * */
	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}

}