using UnityEngine;
using System.Collections;

public class EnableDisable : MonoBehaviour {
	public GameObject[] enableList;
	public GameObject[] disableList; 
	
	/*
	void OnTriggerEnter() {
		print ("EnableDisable");
		foreach (GameObject g in enableList) { g.SetActive(true); }
		foreach (GameObject g in disableList) { g.SetActive(false); }
	}
	*/
	
	
	public void TriggerEnableDisable () {
		for (int i = 0; i < enableList.Length; i += 1) { enableList[i].SetActive(true); }
		for (int i = 0; i < disableList.Length; i += 1) { disableList[i].SetActive(false); }
	}
}
