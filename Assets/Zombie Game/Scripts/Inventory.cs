using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public Player user;
	public Flashlight userFlashlight;
	
	private static int healthPacks, batteries, saveSlot;
	private float displayHealthPacks = 0.0f, displayBatteries = 0.0f;
	private bool showInventory = false;
	private static bool flashlightEnabled;

	// Use this for initialization
	void Start () {
		saveSlot = PlayerPrefs.GetInt("CurrentSave");
		LoadInventory();
		
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}

	public static void SaveCurrentInventory () {
		PlayerPrefs.SetInt("Save"+saveSlot+"HealthPacks",healthPacks);
		PlayerPrefs.SetInt("Save"+saveSlot+"Batteries",batteries);
		if (flashlightEnabled) PlayerPrefs.SetInt("Save"+saveSlot+"Flashlight",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Flashlight",0);
	}
	
	void LoadInventory () {
		if (saveSlot == -1) {
			healthPacks = 1;
			batteries = 4;
			userFlashlight.enableFlashlight(true);
		}
		else {
			healthPacks = PlayerPrefs.GetInt("Save"+saveSlot+"HealthPacks",0);
			batteries = PlayerPrefs.GetInt("Save"+saveSlot+"Batteries",0);
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Flashlight",0) == 0) { flashlightEnabled = false; }
			else { flashlightEnabled = true; }
			userFlashlight.enableFlashlight(flashlightEnabled); 
		}
	}
	
	void OnGUI () {
		GUI.enabled = false;
		if (showInventory) {
			GUI.TextField(new Rect(10,90,125,55),"Inventory\nHealth Packs: "+healthPacks+"\nBatteries: "+batteries);
		}
		if (displayHealthPacks > 0.0f) {
			GUI.TextField(new Rect(10,155,150,25),"No Health Packs to use!");
		}
		if (displayBatteries > 0.0f) {
			GUI.TextField(new Rect(10,190,130,25),"No Batteries to use!");
		}
		GUI.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Health Packs
		if (displayHealthPacks <= 0.0f) { displayHealthPacks = 0.0f; }
		else { displayHealthPacks -= Time.deltaTime; }
		if (Input.GetKeyDown(KeyCode.H)) {
			if (healthPacks > 0) {
				user.addHealth(25);
				healthPacks--;
			}
			else { displayHealthPacks = 3.0f; }
		}
		// Batteries
		if (displayBatteries <= 0.0f) { displayBatteries = 0.0f; }
		else { displayBatteries -= Time.deltaTime; }
		if (Input.GetKeyDown(KeyCode.B)) {
			if (batteries > 0) {
				userFlashlight.addPower(1f);
				batteries--;
			}
			else { displayBatteries = 3.0f; }
		}
		// Inventory GUI
		if (Input.GetKeyDown(KeyCode.I)) { showInventory = !showInventory; }
	}

	public void AddBatteries (int b) {
		batteries += b;
	}
	
	public void AddHealthPacks (int h) {
		healthPacks += h;
	}
	
	public void EnableFlashlight() {
		flashlightEnabled = true;
		userFlashlight.enableFlashlight(true);
	}
	
	public void GameLevelRestart () {
		LoadInventory();
	}
	
	public void GameLevelUnload () {
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
