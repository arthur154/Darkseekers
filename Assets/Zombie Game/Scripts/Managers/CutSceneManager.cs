using UnityEngine;
using System.Collections;

public class CutSceneManager : MonoBehaviour {
	public GameObject[] disableList;
	
	private float timeTracker = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("Level1");
		}
		if (timeTracker > 58.5f){
			Application.LoadLevel("Level1");
		}
		timeTracker += Time.deltaTime;
	}
	
	void OnGUI () {
		if (timeTracker < 5.0f) {
			GUI.TextArea(new Rect(10,Screen.height-25,125,35),"Press 'Esc' to skip");
		}
		else {
			foreach (GameObject g in disableList) g.SetActive(false);
		}
	}
}
