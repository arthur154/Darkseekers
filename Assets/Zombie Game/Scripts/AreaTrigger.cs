using UnityEngine;
using System.Collections;

public class AreaTrigger : MonoBehaviour {
	public GameObject[] enable, disable;
	public bool incObjective=false;

	void Start () {
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}

	void OnTriggerEnter () {
		if (incObjective) Objectives.IncObjectiveCount();
		foreach (GameObject g in enable) {
			g.SetActive(true);
		}
		foreach (GameObject g in disable) {
			g.SetActive(false);
		}
		gameObject.SetActive(false);
	}
	
	public void GameLevelRestart() {
		foreach (GameObject g in enable) {
			g.SetActive(false);
		}
		foreach (GameObject g in disable) {
			g.SetActive(true);
		}
		gameObject.SetActive(true);
	}
	
	public void GameLevelUnload() {
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
