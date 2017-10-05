using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ricochet : MonoBehaviour {

	public static int ricochetCount = 0;
	private int hit;



	void Start () {
		hit = 0;
	}

	/*
	 * Update the Ricochet count if this single bullet hits 2 or more aliens.
	 * Also, increment gravity scale upon impact.
	 * */
	void OnCollisionEnter2D(Collision2D coll){

		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0.1f;

		if (coll.collider.CompareTag ("alien")) {
			hit++;
			if (hit > 1 && !coll.gameObject.GetComponent<MonsterBehavior> ().isSpinning) {
				//if you've already bounced off an alien and you're striking an alien, increment ricochetCount
				ricochetCount++;
			}
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