using UnityEngine;
using System.Collections;

public class CuteSceneManager2 : MonoBehaviour {
	
	private float timeTracker = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (timeTracker > 15.0f){
			Application.LoadLevel("LoadingScreen");
		}
		timeTracker += Time.deltaTime;
	}
}
