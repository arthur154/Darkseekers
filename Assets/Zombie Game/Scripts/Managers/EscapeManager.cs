using UnityEngine;
using System.Collections;

public class EscapeManager : MonoBehaviour {
	public float destructionTime;  

	private float timeUntilDestruction;
	// Use this for initialization
	void Start () {
		timeUntilDestruction = destructionTime;
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;	
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}
	
	void OnGUI () {
		GUI.enabled = false;
		GUI.TextArea(new Rect(Screen.width/2-100, 10, 200, 20), "Time Until Self Destruct: "+(int)timeUntilDestruction);
		GUI.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilDestruction < 0.0f) {
			gameManager.TriggerLevelUnload();
			Application.LoadLevel("Level8");
		}
		timeUntilDestruction -= Time.deltaTime;
	}
	
	public void GamePause() {
		gameObject.SetActive(false);
	}
	
	public void GameUnpause() {
		gameObject.SetActive(true);
	}
	
	public void GameLevelRestart () {
		timeUntilDestruction = destructionTime;
	}
	
	public void GameLevelUnload () {
		gameManager.GamePause -= GamePause;
		gameManager.GameUnpause -= GameUnpause;	
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
