using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryCan : MonoBehaviour {

	public int upgradeInt;


	// Use this for initialization
	void Start () {
		upgradeInt = Random.Range (0, 20); //randomly chooses 1 of 5 upgrade prizes
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
