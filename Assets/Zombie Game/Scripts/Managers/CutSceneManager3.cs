using UnityEngine;
using System.Collections;

public class CutSceneManager3 : MonoBehaviour {
	public GameObject disableBelow0;
	public SplineMovement m;
	public float waitTime;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0.25f;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < -38.5f) Time.timeScale = 1.0f;
		if (transform.position.z <= -70.0f) {
			m.enabled = false;

			if (transform.position.y <= -300) {
				Application.LoadLevel("MainMenu");
			}

			transform.Translate(0,-0.05f,0);
		}
		if (transform.position.y <= -0.5f) disableBelow0.SetActive(false);
	}
}
