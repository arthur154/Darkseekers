using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	
	// Initial values for the spotlight
	public float range;
	public float spotAngle;
	public float intensity;
	public float batteryDrain;
	public float storePower;
	public bool lightOn, flashEnabled;
	
	// Use this for initialization
	void Start () {
		light.range = range;
		light.spotAngle = spotAngle;
	}
	
	// Update is called once per frame
	void Update () {
		if (flashEnabled) {
			if (Input.GetKeyDown(KeyCode.F)) lightOn = !lightOn;
			
			if (lightOn) {
				// Slowly drain the battery
				storePower -= batteryDrain;
				light.intensity = storePower;
			}
			else {
				light.intensity = 0f;
			}
		}
		else {
			light.intensity = 0f;
		}
	}
	
	// Increase the power
	public void addPower(float p) {
		storePower += p;
		if (storePower > 1) { storePower = 1; }
	}
	
	public void enableFlashlight(bool f) {
		flashEnabled = f;
	}
	
	public bool isObjective() {
		return true;
	}
}
