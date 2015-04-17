using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {
	public bool destroyAfterPlay, advanceObjectives;
	public GameObject[] enableList, disableList;
	
	private bool audioTriggered;
	
	void OnTriggerEnter() {
		if (advanceObjectives) Objectives.IncObjectiveCount();

		gameObject.audio.Play();
		audioTriggered = true;

		foreach (GameObject g in enableList) g.SetActive(true);
		foreach (GameObject g in disableList) g.SetActive(false);
	}

	void Update() {
		if (audioTriggered && !gameObject.audio.isPlaying && destroyAfterPlay) {
			gameObject.SetActive(false);
		}
	}
}
