using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHP : MonoBehaviour {

	public float range;
	public GameObject healthbox;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("CreateHP", Random.Range (15, 20), Random.Range (20, 25)); // generate a Healer Box every 20-30 seconds
	}
	
	private void CreateHP(){
		
		Vector2 hpLocale = new Vector2 (7.50f - (range * Random.value), 5.55f); // Random (horizontal) X placement

		GameObject newHP = Instantiate(healthbox, hpLocale, Quaternion.identity);
		newHP.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,-75));

	}

}
