using UnityEngine;
using System.Collections;

public class SplineMovement : MonoBehaviour {
	public int numOfPoints;
	public GameObject[] points;
	public float[] timeBetween;
	public GameObject focus;
	
	private float previousTime, deltaT, accumulatedTime;
	private int currentTimePeriod;
	private Vector3 P0, P1, P2;
	private bool interp;
	
	// Use this for initialization
	void Start () {
		interp = true;
		//P0 = transform.position;
		//P1 = points[0].transform.position;
		//P2 = points[1].transform.position;
		P0 = points[0].transform.position;
		P1 = points[1].transform.position;
		P2 = points[2].transform.position;
		currentTimePeriod = 0;
		accumulatedTime = 0;
		previousTime = Time.time;
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (interp) {
			// Calculate the accumulated time for this time period
			accumulatedTime += Time.time - previousTime;
			// Get the t value for the Bezier curve
			float t = (accumulatedTime / timeBetween[currentTimePeriod]); // 1 - timePeriod/accumulatedTime
			
			// If t > 1 then set t=0 and go to next point
			if (t > 1) {
				t = 0;
				accumulatedTime = 0;
				currentTimePeriod++;
				currentTimePeriod++;
				if (currentTimePeriod+2 > numOfPoints) { interp = false; }
				else {
					P0 = points[currentTimePeriod].transform.position;
					P1 = points[currentTimePeriod+1].transform.position;
					P2 = points[currentTimePeriod+2].transform.position;
				}
			}
			else {
				// Bezier curve function
				transform.position = (1-t)*(1-t)*P0+2*(1-t)*t*P1+t*t*P2;
			}
			
			// Look at a focal point
			transform.LookAt(focus.transform.position);
			
			// Update the previous time
			previousTime = Time.time;
		}
	}
}
