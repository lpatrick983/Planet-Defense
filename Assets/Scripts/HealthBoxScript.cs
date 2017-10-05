using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxScript : MonoBehaviour {

	public GameObject orbOfHealth;


	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("bullet")) {
			GameObject orbAzure = Instantiate (orbOfHealth, gameObject.transform.position, Quaternion.identity);
			orbAzure.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range(-50,50), 25f));
			Destroy (gameObject); //Destroy this Medical box
		}
	}

}