using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateEnemies : MonoBehaviour {

	public float range;
	public GameObject enemy; //can become a Red, Blue, Green... Purple monster randomly
	public GameObject prizeCan;

	public Text killsText;
	public int numKills;

	private int alienStartingSpeed; // ?? to be increased with difficulty ??



	void Start ()
	{
		numKills = 0;
		SetKillsText ();
		alienStartingSpeed = 66;

		InvokeRepeating ("CreateMon", 2f, 1.3f); //starts in 2 second(s)... method is invoked every 1 second(s)
	}

	void CreateMon(){
		Vector2 monLocale = new Vector2 (8.0f - (range * Random.value), 5.55f); //random (horizontal) X placement


		int x = Random.Range (0, 6); //Randomly select an integer between 0-5 inclusive

		//Use this int to select between the 6 colors of monsters (each is equally likely)
		if (x == 0) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien red", typeof(GameObject));
		} else if (x == 1) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien blue", typeof(GameObject));
		} else if (x == 2) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien green", typeof(GameObject));
		} else if (x == 3) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien yellow", typeof(GameObject));
		} else if (x == 4) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien orange", typeof(GameObject));
		} else if (x == 5) {
			enemy = (GameObject)Resources.Load ("Prefabs/Alien purple", typeof(GameObject));
		}

		GameObject newMon = Instantiate(enemy, monLocale, Quaternion.identity);
		newMon.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,-1 * alienStartingSpeed)); // Give the Aliens some random starting speed


		StartCoroutine (checkDestroyed (newMon)); //IMPORTANT for score*
	}


	private IEnumerator checkDestroyed(GameObject newMon){
		bool hit = false; //This is used in the while loop
		while (newMon != null && !hit) {
			if (newMon.GetComponent<MonsterBehavior> ().isSpinning) {
				hit = true; //So numKills is only incremented once (and not continuously while the Alien is spinning)
				numKills++;
				SetKillsText ();

				// 7.3% chance of a mystery prize!
				if (Random.Range (0, 13) == 0) {
					GameObject cannister = Instantiate (prizeCan, newMon.transform.position, Quaternion.identity);
					cannister.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-10, 10), 0));
				}
			}
			yield return null;
		}
		yield return new WaitForSeconds (0.1f);
	}


	void SetKillsText(){
		killsText.text = "Aliens Defeated: " + numKills.ToString ();
	}

}