using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	void OnGUI () {
		GUI.enabled = false;
		GUI.TextArea(new Rect(Screen.width/2-50, 10, 100, 20), "Score: "+ZCount.GetKills());
		GUI.enabled = true;
	}
}
