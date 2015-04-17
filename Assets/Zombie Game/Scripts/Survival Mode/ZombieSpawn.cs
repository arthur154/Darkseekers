using UnityEngine;
using System.Collections;

public class ZombieSpawn : MonoBehaviour {

	private SwarmAI script;
	private float time;

	// Use this for initialization
	void Start () {
		script = this.transform.GetComponent<SwarmAI>();
		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Time: " + Time.time+ ", time: " + time);
		if ((Time.time - time) > 10) {
			Debug.Log("WAHH!" + "Time: " + Time.time+ ", time: " + time);
			time = Time.time;
			Debug.Log("WOO!" + "Time: " + Time.time+ ", time: " + time);
			if (script.zombieNumber < 10) {
				Debug.Log(script.zombieNumber);
				script.zombieNumber++;
			}
			//script.InitZombies();

		}
		//Debug.Log(Time.time);
	}
}
