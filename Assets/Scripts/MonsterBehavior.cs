using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBehavior : MonoBehaviour {

	public static int doubleKO = 0;
	public static int invaders = 0;

	private bool didInvade;

	public GameObject projectile; //blaster we will instantiate for firing (IsTrigger)
	private Vector3 oldAngles; //angles of the Alien monster
	public bool isSpinning; //whether or not the Alien is spinning or tilted after collision

	//Make this more compact? (use arrays)
	private Sprite currentSprite;

	public Sprite redNorm;
	public Sprite blueNorm;
	public Sprite greenNorm;
	public Sprite yellowNorm;
	public Sprite orangeNorm;
	public Sprite purpleNorm;

	public Sprite redSwirl;
	public Sprite blueSwirl;
	public Sprite greenSwirl;
	public Sprite yellowSwirl;
	public Sprite orangeSwirl;
	public Sprite purpleSwirl;



	void Start () {
		oldAngles = gameObject.transform.rotation.eulerAngles;
		isSpinning = false;
		currentSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
		didInvade = false;

		InvokeRepeating ("fireBall", 0.0f, 1.5f); //Start the method b/t 0.0 seconds in & repeat every 1.5 sec

	}
	
	void Update () {
		if (oldAngles != gameObject.transform.rotation.eulerAngles) { //Once the Monster has been struck by bullet...
			CancelInvoke (); //...stop shooting!
			isSpinning = true; //set the Boolean

			//Maybe use ARRAYS Here??
			//Change the gameObject's sprite from "Normal" to "Swirly Eyes"
			if (currentSprite == redNorm) {
				gameObject.GetComponent<SpriteRenderer>().sprite = redSwirl;
			} else if (currentSprite == blueNorm) {
				gameObject.GetComponent<SpriteRenderer>().sprite = blueSwirl;
			} else if (currentSprite == greenNorm) {
				gameObject.GetComponent<SpriteRenderer>().sprite = greenSwirl;
			} else if (currentSprite == yellowNorm) {
				gameObject.GetComponent<SpriteRenderer>().sprite = yellowSwirl;
			} else if (currentSprite == orangeNorm) {
				gameObject.GetComponent<SpriteRenderer>().sprite = orangeSwirl;
			} else {
				gameObject.GetComponent<SpriteRenderer>().sprite = purpleSwirl;
			}
		}

		if (isSpinning) {
			GetComponent<Rigidbody2D> ().gravityScale = 0.25f; // initiate Gravity once it starts tilting
		}

		if (gameObject.transform.position.y < -5.50f && !isSpinning && didInvade == false) {
			invaders++;
			didInvade = true;
		}
	}

	/*
	 * These 2 methods handle the Randomized firing pattern.
	 * 
	 * */
	void fireBall(){
		Invoke ("actualFire", Random.Range (0.95f, 1.95f));
	}
	void actualFire(){
		GameObject blaster = Instantiate (projectile, gameObject.transform.position + new Vector3(0,-0.75f,0), Quaternion.identity);

		blaster.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, -1 * Random.Range(150, 450)));
		Destroy(blaster, 5f);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (!isSpinning && coll.collider.CompareTag("alien") && coll.gameObject.GetComponent<MonsterBehavior> ().isSpinning) {
			doubleKO++;
		}
	}

	/*
	 * Destroy the Alien if it leaves the screen bottom.
	 * */
	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}
}
