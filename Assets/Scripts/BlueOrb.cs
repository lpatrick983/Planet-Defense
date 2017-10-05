using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueOrb : MonoBehaviour {

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
