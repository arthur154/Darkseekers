using UnityEngine;
using System.Collections;

public class SniperLevelManager : MonoBehaviour {
	public SwarmAI[] spawnPoints;
	public Transform roofSpawnPoint1, roofSpawnPoint2, playerTrans, ammoSpawnPoint;
	public float timeBetweenSpawns, timeBetweenAmmoSpawns;
	public GameObject sniperAmmo;

	private bool spawnPointsEnabled=false;
	private float lastSpawn=0.0f, lastAmmoSpawn=28.0f;

	void Start () {
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;	
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}

	void OnGUI () {
		GUI.enabled = false;
		if (spawnPointsEnabled) {
			GUI.TextArea(new Rect(Screen.width/2-50, 10, 100, 20), "Kills: "+ZCount.GetKills());
		}
		GUI.enabled = true;
	}

	void OnTriggerEnter (Collider c) {
		GameObject temp = c.transform.gameObject;
		ZombieAI z = temp.GetComponent<ZombieAI>();
		if (z != null) {
			if ((int)Random.Range(0,1)==0) c.transform.position = roofSpawnPoint1.transform.position;
			else c.transform.position = roofSpawnPoint2.transform.position;
			c.transform.rotation = transform.rotation;
			z.target = playerTrans;
			z.SetPlayer(playerTrans);
		}
	}

	void Update () {
		if(Objectives.GetObjectiveCount()>0) {
			// Enable the spawn points
			if (!spawnPointsEnabled) {
				for(int i=0; i<5; i++) spawnPoints[i].gameObject.SetActive(true);
				spawnPointsEnabled=true;
			}
			// Spawn Zombies (based on time)
			if (lastSpawn > timeBetweenSpawns || (ZCount.GetKills()>20 && lastSpawn > 1.0f)) {
				spawnPoints[Random.Range (0,4)].InitZombies();
				lastSpawn = 0.0f;
			}
			else lastSpawn+=Time.deltaTime;
			// Spawn Ammo (based on time)
			if (lastAmmoSpawn > timeBetweenAmmoSpawns) {
				GameObject a = (GameObject)Instantiate(sniperAmmo);
				a.transform.position = ammoSpawnPoint.transform.position;
				lastAmmoSpawn = 0.0f;
			}
			else lastAmmoSpawn+=Time.deltaTime;

		
			if (ZCount.GetKills()>=40) {
				WeaponManager.SaveCurrentWeapons();
				Player.SaveCurrentHeatlh();
				Inventory.SaveCurrentInventory();
				gameManager.TriggerLevelUnload();
				Application.LoadLevel("CutScene2");
			}
		}
	}

	public void GamePause() {
		gameObject.SetActive(false);
	}
	
	public void GameUnpause() {
		gameObject.SetActive(true);
	}
	
	public void GameLevelRestart () {
		for(int i=0; i<5; i++) spawnPoints[i].gameObject.SetActive(false);
		spawnPointsEnabled=false;
	}
	
	public void GameLevelUnload () {
		gameManager.GamePause -= GamePause;
		gameManager.GameUnpause -= GameUnpause;	
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
