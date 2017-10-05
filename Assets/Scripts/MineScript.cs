using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {

	public GameObject smokeExplosion;
	private GameObject[] smokeParticles = new GameObject[5];

	private GameObject opponent;



	void Update () {
		if (opponent != null) {
			opponent.transform.Rotate (0, 0, 1);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("alien")) {

			opponent = other.gameObject;
			other.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (Random.Range(-400, 400), 800));

			ExplosionResidue ();
			Destroy (gameObject, 0.1f);
		}
	}


	void ExplosionResidue(){
		// starting vector values
		float xValue = -50;
		float yValue = 0;

		for (int i = 0; i < smokeParticles.Length; i++) {
			smokeParticles [i] = Instantiate (smokeExplosion, transform.position, Quaternion.identity);
			smokeParticles [i].GetComponent<Rigidbody2D> ().AddForce (new Vector3 (xValue, yValue));
			Destroy (smokeParticles [i], 1.5f);
			xValue += 25;
			if (i < 2) {
				yValue += 17;
			} else {
				yValue -= 17;
			}
		}


	}

}
