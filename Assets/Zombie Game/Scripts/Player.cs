using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {
	public Texture2D healthIcon;
	public GUIStyle s;
	
	private Vector3 startPos;
	private static int health, saveSlot;
	
	// Use this for initialization
	void Start () {
		GotoLevel.LoadFromMenu=false;
		startPos = transform.position;
		
		// Code to allow pausing
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
		
		// Load health 
		saveSlot = PlayerPrefs.GetInt("CurrentSave");
		LoadHealth();
	}
	
	void LoadHealth () {
		health = PlayerPrefs.GetInt("Save"+saveSlot+"Health",100);
	}

	public static void SaveCurrentHeatlh () {
		PlayerPrefs.SetInt("Save"+saveSlot+"Health",health);
	}
	
	// Update is called once per frame
	void Update () {
		// I don't know why the player can fly when zombies attack it and it tries to avoid
		// this conflicts with player's jump motion.
		//if(transform.position.y>2.0f) 
		//	transform.position=new Vector3(transform.position.x,2.0f,transform.position.z);
		
	}
	
	void OnGUI () {
		// Draw Health
		GUI.enabled = false;
		GUI.DrawTexture(new Rect(10,10,50,50),healthIcon);
		GUI.TextArea(new Rect(60,10,60,50),""+health,s);
		GUI.enabled = true;
	}
	
	public void GamePause() {
		//enabled = false;
		transform.gameObject.SetActive(false);
	}
	
	public void GameUnpause() {
		//enabled = true;
		transform.gameObject.SetActive(true);
	}
	
	public void GameLevelRestart() {
		transform.position = startPos;
		LoadHealth();
		gameManager.TriggerGameUnpause();
	}
	
	public void GameLevelUnload() {
		gameManager.GamePause -= GamePause;
		gameManager.GameUnpause -= GameUnpause;
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
	
	// Increase the player health by 20
	// This method is called from ObjectInteract script
	public void addHealth(int h) {
		if (health + h > 100) {
			health = 100;
		} else {
			health += h;
		}
		if (health <= 0) gameManager.TriggerLevelRestart(); 
	}

	public int getHealth()
	{return health;}
}
