using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFollow : MonoBehaviour {

	private GameObject[] aliensAlive;
	private GameObject target;
	public GameObject smokeTrail;
	private bool hasCollided;


	void Start () {
		hasCollided = false;
		aliensAlive = GameObject.FindGameObjectsWithTag ("alien");
		SetTarget ();
	}
	
	void Update () {
		if (target == null) { //fire straight up if there are no active enemies
			transform.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (0, 100f));
		} else { //otherwise, target rocket's physics vector towards nearest enemy
			Vector3 vectorToTarget = transform.position - target.transform.position;
			float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90f;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * 20f);

			transform.GetComponent<Rigidbody2D> ().AddForce (10 * (target.transform.position - transform.position));
			if (!hasCollided) {
				TrailResidue ();
			}
		}
	}

	void TrailResidue(){
		GameObject smoker = Instantiate (smokeTrail, transform.position, Quaternion.identity);
		Destroy (smoker, 0.25f);
	}

	void SetTarget(){
		for (int i = 0; i < aliensAlive.Length; i++) {
			if (!aliensAlive [i].GetComponent<MonsterBehavior> ().isSpinning) {
				target = aliensAlive [i];
				break;
			}
		}
	}




	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.CompareTag ("alien")) {
			hasCollided = true;
			Destroy (gameObject, 0.2f);
		}
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}

}