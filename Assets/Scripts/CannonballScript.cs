using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour {

	/*
	 * Increment gravity scale upon impact.
	 * */
	void OnCollisionEnter2D(Collision2D coll){
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0.2f;
	}

}
