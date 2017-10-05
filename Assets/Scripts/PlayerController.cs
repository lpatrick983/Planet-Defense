using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject tank;
	public GameObject bar;
	public GameObject projectile;
	public GameObject rocket;
	public GameObject cannonball;
	private GameObject blackbomb;
	public GameObject mine;


	public Text gameOverText;
	public Text lifeCountText; // "Lives: _"
	private int numLives;
	private int numHealth;

	private GameObject[] yourhealth = new GameObject[5];
	private bool gotPrize;

	public Text upgradeText; // "Acquired __!"
	private int shotgunPermit;
	private int missileCount;
	private int landmineCount;
	private bool cannonActivated;



	void Start()
	{
		numLives = 3;
		numHealth = 5;
		gotPrize = false;
		cannonActivated = false;
		shotgunPermit = 0;
		missileCount = 0;
		landmineCount = 0;

		GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f); //set initial color to: Solid Gray

		CreateHealthBars();
		SetLifeCountText ();

		gameOverText.text = null;
		upgradeText.text = null;

		Physics2D.IgnoreLayerCollision (9, 10, true); //just in case.
	}

	void Update ()
	{
		// Move left
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Translate (Vector3.left/5.0f);
		}
		// Move right
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Translate (Vector3.right/5.0f);
		}

		// Regular firing
		if (Input.GetKeyDown (KeyCode.Space)) {
			fireBullet ();
		}

		// Shoot Special Shotgun
		if (Input.GetKeyDown (KeyCode.C) && shotgunPermit > 0) {
			fireShotgun ();
		}

		// Create the cannonball
		if (Input.GetKeyDown (KeyCode.C) && cannonActivated) {
			blackbomb = Instantiate (cannonball, transform.position + new Vector3(0,-0.5f,0), Quaternion.identity);
		}

		// Grow the cannonball
		if (Input.GetKey (KeyCode.C) && cannonActivated) {
			growCannonball ();
		}

		// Launch the cannonball
		if (Input.GetKeyUp (KeyCode.C) && cannonActivated) {
			launchCannonball ();
		}

		// Fire a missile!
		if (Input.GetKeyDown (KeyCode.C) && missileCount > 0) {
			missilesAway ();
		}

		// Plant a mine!
		if (Input.GetKeyDown (KeyCode.C) && landmineCount > 0) {
			plantMine ();
		}

	}


	private void fireBullet()
	{
		GameObject bullet = Instantiate (projectile, transform.position + new Vector3 (0, 0.5f, 0), Quaternion.identity); //creates the bullet object
		bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 925));
		Destroy (bullet, 2.5f);
	}

	private void fireShotgun()
	{
		shotgunPermit--;
		if (shotgunPermit == 0) {
			gotPrize = false;
		}

		GameObject bullet1 = Instantiate (projectile, transform.position + new Vector3 (0, 0.5f, 0), Quaternion.identity); //creates the bullet object
		GameObject bullet2 = Instantiate (projectile, transform.position + new Vector3(0,0.5f,0), Quaternion.identity); //creates the bullet object
		GameObject bullet3 = Instantiate (projectile, transform.position + new Vector3(0,0.5f,0), Quaternion.identity); //creates the bullet object

		bullet1.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (500, 500));
		bullet2.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 1000));
		bullet3.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-500, 500));

		Destroy (bullet1, 2.5f);
		Destroy (bullet2, 2.5f);
		Destroy (bullet3, 2.5f);
	}

	private void growCannonball(){
		if (blackbomb.GetComponent<Transform> ().localScale.x < 4.5f && blackbomb.GetComponent<Transform> ().localScale.y < 4.5f) {
			blackbomb.GetComponent<Transform> ().localScale += new Vector3 (0.2f, 0.2f, 0);
		}
	}

	private void launchCannonball(){
		gotPrize = false;
		blackbomb.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 2500));
		Destroy (blackbomb, 5f);
		cannonActivated = false;
	}

	private void missilesAway(){
		missileCount--;
		if (missileCount == 0) {
			gotPrize = false;
		}
		Instantiate (rocket, transform.position + new Vector3 (0, 0.5f, 0), Quaternion.identity);
	}

	private void plantMine(){
		landmineCount--;
		if (landmineCount == 0) {
			gotPrize = false;
		}
		Instantiate (mine, transform.position - new Vector3 (0, 0.3f, 0), Quaternion.identity);
	}






	/*
	 * All Trigger entities: blaster fire, health orb, or cannister upgrade
	 * 
	 * */
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("blaster trigger")) {
			numHealth--;
			other.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 1, 1);
			Destroy (yourhealth [numHealth]);
			yourhealth [numHealth] = null; //Redundant?

			StartCoroutine (hitColor ());

			if (numHealth == 0) {
				gameObject.SetActive (false);
				if(gameObject.activeSelf == false && cannonActivated == true){
					if (blackbomb != null) {
						launchCannonball ();
					}
				}
				numLives--;
				if (numLives != 0) {
					CreateHealthBars ();
					numHealth = 5;
				}
				Invoke ("resetPos", 1);
			}
			SetLifeCountText ();
			if (numLives == 0) {
				Destroy (gameObject, 0.05f);
				SetGameOverText ();
			}
		} else if (other.CompareTag ("blue orb")) {
			DestroyObject (other.gameObject);

			if (numHealth < 5) {
				numHealth++;
				yourhealth [numHealth - 1] = Instantiate (bar, yourhealth [numHealth - 2].transform.position + new Vector3 (0.7f, 0, 0), Quaternion.identity);
				StartCoroutine (itemColor (1, -1));
			}
		} else if (other.CompareTag ("cannister")) {
			if (gotPrize == false) {

				gotPrize = true;
				int x = other.gameObject.GetComponent<MysteryCan> ().upgradeInt;

				StartCoroutine (itemColor (2, x)); //change to Magenta
				StartCoroutine (upgradeDisplayed(x)); //flash upgrade text briefly

				if (x == 0) { // invincibility ... 5% chance
					StartCoroutine (gracePeriod (2));

				} else if (x == 1) { // extra life ... 5%
					numLives++;
					SetLifeCountText ();
					gotPrize = false; //**

				} else if (x == 2 || x == 3 || x == 4 || x == 5 || x == 6) { // one giant cannonball ... 25%
					cannonActivated = true;

				} else if (x == 7 || x == 8 || x == 9 || x == 10 || x == 11) { // ten shotgun sprays ... 25%
					shotgunPermit = 10;

				} else if (x == 12 || x == 13 || x == 14 || x == 15) { // five rocket missiles ... 20%
					missileCount += 5;

				} else { // three land mines ... 20%
					landmineCount += 3;

				}
			}

			Destroy (other.gameObject);
		}
	}


	/*
	 * Called when you lose 1 life.
	 * 
	 * */
	void resetPos()
	{
		transform.position = new Vector3 (0, -4.4f, 0);
		gameObject.SetActive (true);
		StartCoroutine (gracePeriod (1));
	}


	/*
	 * Fade the player's original color (grace period OR invincibility)
	 * 
	 * */
	private IEnumerator gracePeriod(int i)
	{
		Physics2D.IgnoreLayerCollision (9, 11, true);
		GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 0.5f); //Faded gray

		if (i == 1) { //Grace period
			yield return new WaitForSeconds (2.5f);
		} else if (i == 2) { //Invincibility period
			yield return new WaitForSeconds (10f);
		}


		Physics2D.IgnoreLayerCollision (9, 11, false);
		GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f); //Solid gray
		if (i == 2) {
			gotPrize = false; //the prize is over.
		}
	}


	/*
	 * Color change if tank is hit by blaster
	 * 
	 * */
	private IEnumerator hitColor()
	{
		//change to yellow
		GetComponent<SpriteRenderer> ().color = new Color (1, 0.92f, 0.016f, 1);
		yield return new WaitForSeconds (0.2f);


		GetComponent<SpriteRenderer> ().color = new Color (0.2f, 0.2f, 0.2f); //always return to SOLID GRAY.
	}


	/*
	 * Color change if tank picks up item
	 * 
	 * */
	private IEnumerator itemColor(int i, int x)
	{
		//Save the original color
		Color oldcolor = GetComponent<SpriteRenderer> ().color;

		//change to:
		if (i == 1) {
			GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0, 1); // green
		} else if (i == 2) {
			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1); // magenta
		}

		yield return new WaitForSeconds (0.2f);

		//change back to solid gray ONLY if it's not during invincibility bonus
		if (x == 0) {
			//do nothing
		} else {
			if (oldcolor == new Color (0.2f, 0.2f, 0.2f, 0.5f)) {
				GetComponent<SpriteRenderer> ().color = oldcolor; //go back to Faded gray
			} else {
				GetComponent<SpriteRenderer> ().color = new Color (0.2f, 0.2f, 0.2f); //else, go back to Solid gray
			}
		}
	}

	/*
	 * Briefly flash Upgrade Prize info at top of the screen
	 * 
	 * */
	private IEnumerator upgradeDisplayed(int x){
		if (x == 0) {
			upgradeText.text = "BONUS: invincibility!";

		} else if (x == 1) {
			upgradeText.text = "BONUS: extra life!";

		} else if (x == 2 || x == 3 || x == 4 || x == 5 || x == 6) {
			upgradeText.text = "BONUS: fire cannonball!";

		} else if (x == 7 || x == 8 || x == 9 || x == 10 || x == 11) {
			upgradeText.text = "BONUS: 10 shotgun ammo!";

		} else if (x == 12 || x == 13 || x == 14 || x == 15) {
			upgradeText.text = "BONUS: launch 5 rockets!";

		} else {
			upgradeText.text = "BONUS: plant 3 mines!";

		}
		yield return new WaitForSeconds (2.2f);

		upgradeText.text = null;
	}




	/*
	 * UI Text
	 * 
	 * */
	void SetLifeCountText()
	{
		lifeCountText.text = "Life Count: " + numLives.ToString ();
	}
	void SetGameOverText()
	{
		gameOverText.text = "GAME OVER!";
	}

	/*
	 * Updates the Player's health bars displayed.
	 * 
	 * */
	void CreateHealthBars()
	{
		float ypos = 4.05f;
		float xpos = -8.3f; //-11.11f;
		for (int i = 0; i < yourhealth.Length; i++) {
			Vector2 barlocation = new Vector2 (xpos, ypos);
			yourhealth [i] = Instantiate (bar, barlocation, Quaternion.identity);
			xpos += 0.70f;
		}
	}

}