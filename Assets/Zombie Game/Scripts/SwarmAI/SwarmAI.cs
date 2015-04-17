using UnityEngine;
using System.Collections;

public class SwarmAI : MonoBehaviour {
	// Set in Unity editor
	public GameObject playerObj;
	public int zombieNumber, strengthDelimeter;
	public float searchDist;
	public bool typeMale1, typeMale2, typeFemale1, typeFemale2, typeCop;
	// Constants
	public GameObject zombieMale1, zombieMale2, zombieFemale1, zombieFemale2, zombieCop;

	private int numbOfZombies;
	private ZombieAI[] zombieSwarm;

	void Start () {
		// Create the new GameObjects so we can assign them
		zombieSwarm = new ZombieAI[zombieNumber];
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
		InitZombies();
	}

	public void InitZombies () {
		for (int i=0; i<zombieNumber; i++) {
			// Decide what zombie to create
			GameObject g;
			int m = Random.Range(0,5);
			switch(m){
			case 0:
				if(zombieMale1) {
					g = (GameObject)Instantiate(zombieMale1);
					break;
				}
				goto case 1;
			case 1:
				if(zombieMale2) {
					g = (GameObject)Instantiate(zombieMale2);
					break;
				}
				goto case 2;
			case 2:
				if(zombieFemale1) {
					g = (GameObject)Instantiate(zombieFemale1);
					break;
				}
				goto case 3;
			case 3:
				if(zombieFemale2) {
					g = (GameObject)Instantiate(zombieFemale2);
					break;
				}
				goto case 4;
			case 4: 
				if(zombieCop) {
					g = (GameObject)Instantiate(zombieCop);
					break;
				}
				goto default;
			default:
				g = (GameObject)Instantiate(zombieCop);
				break;
			}
			
			// Place the zombie - placement = (1,0) (0,1) (-1,0) (0,-1) (2,0) (0,2) (0,-2)...
			if (i%4==0 || (i+1)%4==0) m = (i/4)+1;
			else m = -1*(i/4)-1;
			float x = ((i+1)%2)*m;
			float z = (i%2)*m;
			g.transform.position = transform.position;
			g.transform.Translate(new Vector3(x,0.0f,z));
			g.tag="zombie";
			
			// Get the ZombieLocalAI class and initialize it's values
			zombieSwarm[i] = g.gameObject.GetComponent<ZombieAI>();
			zombieSwarm[i].target = playerObj.transform;
			zombieSwarm[i].swarmContoller = gameObject.GetComponent<SwarmAI>();
			zombieSwarm[i].maxStrength = strengthDelimeter;
			zombieSwarm[i].proximity = searchDist;
			//zombieSwarm[i].searchLength = Random.Range(5.0f, 10.0f);
		}
	}

	// Recieved from a zombie who was hit 
	public void AlertTheSwarm () {
		// Send out an alert to all zombies (search for x amound of time?)
		foreach (ZombieAI z in zombieSwarm) {
			//z.AlterFromSwarm(true);
		}
	}

	// Recieved from a zomibe who has found the player
	public void PlayerFoundAlert () {
		foreach (ZombieAI z in zombieSwarm) {
			//z.AlterFromSwarm(false);
		}
	}
	
	public void GameLevelRestart () {
		InitZombies();
	}
	
	public void GameLevelUnload () {
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
