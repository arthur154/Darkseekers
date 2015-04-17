using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
	
	private Light pointlight;
	private Light spotlight;
	
	private float flicker = 0.05f;
	
    // Use this for initialization
    void Start () {
        pointlight = (Light) transform.GetChild(4).GetComponent("Light");
		spotlight = (Light) transform.GetChild(5).GetComponent("Light");
		pointlight.enabled = false;
		spotlight.enabled = false;
		StartCoroutine(Flicker());
    }
    
    // Update is called once per frame
    void Update () {
    }
	
	IEnumerator Flicker (){
		float rand = Random.Range(10f, 20f);
		pointlight.enabled = true;
		spotlight.enabled = true;
		yield return new WaitForSeconds(flicker);
		pointlight.enabled = false;
		spotlight.enabled = false;
		yield return new WaitForSeconds(2*flicker);
		pointlight.enabled = true;
		spotlight.enabled = true;
		yield return new WaitForSeconds(flicker);
		pointlight.enabled = false;
		spotlight.enabled = false;
		yield return new WaitForSeconds(2*flicker);
		pointlight.enabled = true;
		spotlight.enabled = true;
		yield return new WaitForSeconds(40*flicker);
		pointlight.enabled = false;
		spotlight.enabled = false;
		yield return new WaitForSeconds(rand);
		StartCoroutine(Flicker());
	}
	
}
