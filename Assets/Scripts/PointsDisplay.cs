using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour {

	public GameObject generator; //Generator object: GET "Monsters Killed"
	public GameObject player;

	public Text ricochets; //Total number of "Ricochet" bonuses
	public Text knockouts; //Total number of "Double Knockouts"

	public Text pointtotal; //Total number of points
	public Text invaderText; //Total number of invaders

	private int points;
	private int prevKills;
	private int prevRico;
	private int prevDual;
	private int prevInvaders;



	void Start () {
		points = 0;
		prevKills = 0;
		prevRico = 0;
		prevDual = 0;
		prevInvaders = 0;

		ricochets.text = null;
		knockouts.text = null;
		SetPointText ();
		SetInvadeText ();
	}

	//FIX
	void Update () {
		if (prevKills != generator.GetComponent<GenerateEnemies> ().numKills) {
			prevKills = generator.GetComponent<GenerateEnemies> ().numKills;
			points += 50;
		}
		if (prevRico != Ricochet.ricochetCount) {
			prevRico = Ricochet.ricochetCount;
			points += 20;
			StartCoroutine (displayRicoText ());
		}
		if (prevDual != MonsterBehavior.doubleKO) {
			prevDual = MonsterBehavior.doubleKO;
			points += 10;
			StartCoroutine (displayDualKOText ());
		}
		if (prevInvaders != MonsterBehavior.invaders && player != null) {
			prevInvaders = MonsterBehavior.invaders;
			points -= 5;
		}

		SetInvadeText ();
		SetPointText ();
	}


	/*
	 * UI Text methods
	 * 
	 * */
	private IEnumerator displayRicoText(){
		ricochets.text = "Ricochet Bonus! +20";
		yield return new WaitForSeconds (.8f);
		ricochets.text = null;
	}

	private IEnumerator displayDualKOText(){
		knockouts.text = "Double Knockout! +10";
		yield return new WaitForSeconds (.8f);
		knockouts.text = null;
	}

	private void SetPointText(){
		pointtotal.text = "SCORE: " + points.ToString ();
	}
	private void SetInvadeText(){
		invaderText.text = "Invaders: " + prevInvaders.ToString ();
	}
}