using UnityEngine;
using System.Collections;

public class TeleportZombies : MonoBehaviour {
	private int ZombiesToTele = 0;
	public GameObject ZombieSpawn;
	SwarmAI spawn;

	// Use this for initialization
	void Start () {
		spawn = ZombieSpawn.transform.GetComponent<SwarmAI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider Zombie){
		Debug.Log(Zombie.name);
		ZombieAI temp = Zombie.transform.GetComponent<ZombieAI>();
		if(temp != null){
			Debug.Log("hit zombie");
			Destroy(Zombie.gameObject);
			ZombiesToTele++;
			spawn.InitZombies();
		}
	}
}
