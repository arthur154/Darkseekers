using UnityEngine;
using System.Collections;

public class DelayStart : MonoBehaviour {
	public GameObject[] enableObjects, disableObjects;
	public float enableDelayTime, disableDelayTime;

	private float timmer;
	void Start () {
		timmer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (timmer >= enableDelayTime) {
			foreach (GameObject g in enableObjects) g.SetActive(true);
		}
		if (timmer >= disableDelayTime) {
			foreach (GameObject g in disableObjects) g.SetActive(false);
		}
		if (timmer >= disableDelayTime && timmer >= enableDelayTime) enabled = false;
		timmer += Time.deltaTime;
	}
}
