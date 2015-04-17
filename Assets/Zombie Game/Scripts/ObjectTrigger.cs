using UnityEngine;
using System.Collections;

public class ObjectTrigger : MonoBehaviour {
	public bool destroyAfterPlay, advanceObjectives;
	public GameObject[] enableList, disableList;
	
	private bool colliderTriggered;
	
	void OnTriggerEnter() {
		if (advanceObjectives) Objectives.IncObjectiveCount();

		colliderTriggered = true;
		
		foreach (GameObject g in enableList) g.SetActive(true);
		foreach (GameObject g in disableList) g.SetActive(false);
	}
	
	void Update() {
		if (colliderTriggered && destroyAfterPlay) {
			gameObject.SetActive(false);
		}
	}
}
