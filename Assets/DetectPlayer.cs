using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter() {
		Debug.Log(gameObject.name);
	}
}
