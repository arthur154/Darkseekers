using UnityEngine;
using System.Collections;

public class MessageOnCol : MonoBehaviour {
	public string Message;
	public int TerminateOn;

	private float timeDisplay, timeDisplayLen=3.0f;

	void OnGUI () {
		if (timeDisplay>0.0f) GUI.TextArea(new Rect(Screen.width/2-100,Screen.height/2-30,200,20),Message);
	}

	void OnTriggerEnter() {
		timeDisplay = timeDisplayLen;
	}

	void Update () {
		if (Objectives.GetObjectiveCount()<TerminateOn) {
			gameObject.SetActive(true);
			if (timeDisplay<=0.0f) timeDisplay=0.0f;
			else timeDisplay-=Time.deltaTime;
		}
		else gameObject.SetActive(false);
	}
}
