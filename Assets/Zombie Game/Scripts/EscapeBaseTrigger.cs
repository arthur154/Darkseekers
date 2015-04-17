using UnityEngine;
using System.Collections;

public class EscapeBaseTrigger : MonoBehaviour {

	private bool close=true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter()
	{
		if(!close)
		{
			// leave the level go to main menu
			Debug.Log("Successfully escape the base!");
			Application.LoadLevel("Level8");

		}
	}
	public void getClose(bool c)
	{
		close = c;
		//Debug.Log("Door close "+ close);
	}
}
