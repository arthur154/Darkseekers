using UnityEngine;
using System.Collections;

public class SplashFade : MonoBehaviour {
	public float timer;
	
	// Use this for initialization
	void Start () {
		StartCoroutine("DisplayScene");
	}
	
	// Update is called once per frame
	void Update () {
		// If any key is pressed
		if(Input.anyKey){
			//Application.LoadLevel(1);
			Application.LoadLevel("MainMenu");
		}
	}
	
	IEnumerator DisplayScene() {
		yield return new WaitForSeconds(timer);
		//Application.LoadLevel(1);
		Application.LoadLevel("MainMenu");
	}
}
