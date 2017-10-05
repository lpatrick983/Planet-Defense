using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour {

	public float timer;
	public float minutes;
	public float seconds;
	public Text clock;

	public GameObject player;



	void Start () {
		timer = 0;
		minutes = 0;
		seconds = 0;

		clock = GetComponent<Text> ();
		SetTimerText ();
	}
	
	void Update () {
		if (player != null) {
			timer += Time.deltaTime;

			minutes = Mathf.Floor (timer / 60);
			seconds = Mathf.Floor (timer % 60);
			SetTimerText ();
		}
	}

	private void SetTimerText(){
		clock.text = minutes.ToString () + "m " + Mathf.RoundToInt (seconds).ToString () + "s";
	}

}
