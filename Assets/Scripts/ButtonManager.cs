using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // allows us to switch between _Scenes

public class ButtonManager : MonoBehaviour {

	// used for: Play It! & Instructions & Go Back
	public void switchScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}
}