using UnityEngine;
using System.Collections;

public class SurvivorModeManager : MonoBehaviour {
	public SwarmAI[] spawnPoints;
	public Transform ammoSpawnPoint;
	public float timeBetweenSpawns, timeBetweenAmmoSpawns;
	public GameObject m16Ammo;
	
	private bool spawnPointsEnabled=false;
	private float lastSpawn=0.0f, lastAmmoSpawn=28.0f;
	
	void Update () {
		// Enable the spawn points
		if (!spawnPointsEnabled) {
			for(int i=0; i < spawnPoints.Length; i++) {
				spawnPoints[i].gameObject.SetActive(true);
			}
			spawnPointsEnabled=true;
		}
		// Spawn Zombies (based on time)
		if (lastSpawn > timeBetweenSpawns || (ZCount.GetKills()>20 && lastSpawn > 1.0f)) {
			for(int i=0; i < spawnPoints.Length; i++) {
				spawnPoints[i].InitZombies();
			}
			lastSpawn = 0.0f;
		}
		else lastSpawn+=Time.deltaTime;
		// Spawn Ammo (based on time)
		if (lastAmmoSpawn > timeBetweenAmmoSpawns) {
			GameObject a = (GameObject)Instantiate(m16Ammo);
			a.transform.position = ammoSpawnPoint.transform.position;
			lastAmmoSpawn = 0.0f;
		}
		else lastAmmoSpawn+=Time.deltaTime;
	}
}
