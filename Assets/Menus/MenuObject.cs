using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {
	public GameObject mainMenuObj;
	public GameObject selectSaveMenuObj;
	public GameObject loadMenuObj;
	/*
	 0 = New Game
	 1 = Load Game
	 2 = Survival
	 3 = Credits
	 4 = Quit
	 5 = Save Slot 1
	 6 = Save Slot 2
	 7 = Save Slot 3
	 8 = Back Button
	 9 = Load Save Slot 1
	 10 = Load Save Slot 2
	 11 = Load Save Slot 3
	 12 = Back Button
	 */
	public int buttonID;
	
	private bool buttonActive = false, loadMenu = false, selectSaveMenu = false, canLoad = true;
	
	void Start () {
		Screen.showCursor = true;
		renderer.material.color = Color.red;
		
		int levelProg = -1;
		// Set load game - slot #1's text
		if(buttonID==9) { 
			// Get the level progress and add it to the text
			levelProg = PlayerPrefs.GetInt("Save1Level",-1);
			// Change the text
			if(levelProg>-1) {
				GetComponent<TextMesh>().text = "Save 1 - Level " + levelProg;
			}
			else {
				GetComponent<TextMesh>().text = "(Empty - No Save)";
				canLoad = false;
			}
		}
		// Set load game - slot #2's text
		else if(buttonID==10) {
			// Get the level progress and add it to the text
			levelProg = PlayerPrefs.GetInt("Save2Level",-1);
			// Change the text
			if(levelProg>-1) {
				GetComponent<TextMesh>().text = "Save 2 - Level " + levelProg;
			}
			else {
				GetComponent<TextMesh>().text = "(Empty - No Save)";
				canLoad = false;
			}
		}
		// Set load game - slot #3's text
		else if(buttonID==11) { 
			// Get the level progress and add it to the text
			levelProg = PlayerPrefs.GetInt("Save3Level",-1);
			// Change the text
			if(levelProg>-1) {
				GetComponent<TextMesh>().text = "Save 3 - Level " + levelProg;
			}
			else {
				GetComponent<TextMesh>().text = "(Empty - No Save)";
				canLoad = false;
			}
		}
	}
	
	void OnMouseEnter()
	{
		buttonActive = true;
		if(canLoad) renderer.material.color = Color.white;
	}
	
	void OnMouseExit()
	{
		buttonActive = false;
		renderer.material.color = Color.red;
	}

	void OnMouseDown() {
		if(buttonActive){
			// New Game
			if(buttonID==0) { 
				disableEnableChildren(false, mainMenuObj);
				disableEnableChildren(false, selectSaveMenuObj);
				selectSaveMenu = true; 
			}
			// Load Game
			else if(buttonID==1) { 
				disableEnableChildren(false, mainMenuObj);
				disableEnableChildren(false, loadMenuObj);
				loadMenu = true; 
			}
			// Survival Mode
			else if(buttonID==2) { 
				//Application.LoadLevel("SurvivalMode"); 
				PlayerPrefs.SetInt("CurrentSave",-1);
				Application.LoadLevel("LoadingScreen");
			}
			// Credits
			else if(buttonID==3) { Application.LoadLevel("Credits"); }
			// Quit
			else if(buttonID==4) { Application.Quit(); }
			// Start New Game (Slot #1)
			else if(buttonID==5) { StartNewGame(1); }
			// Start New Game (Slot #2)
			else if(buttonID==6) { StartNewGame(2); }
			// Start New Game (Slot #3)
			else if(buttonID==7) { StartNewGame(3); }
			// Back Button 
			else if(buttonID==8) { 
				disableEnableChildren(false, mainMenuObj);
				disableEnableChildren(false, selectSaveMenuObj);
				selectSaveMenu = false; 
			}
			// Load Game (Slot #1)
			else if(buttonID==9 && canLoad) { LoadGame(1); }
			// Load Game (Slot #2)
			else if(buttonID==10 && canLoad) { LoadGame(2); }
			// Load Game (Slot #3)
			else if(buttonID==11 && canLoad) { LoadGame(3); }
			// Back Button 
			else if(buttonID==12) { 
				disableEnableChildren(false, mainMenuObj);
				disableEnableChildren(false, loadMenuObj);
				loadMenu = false; 
			}
			// Play audio sound
			//transform.audio.Play();
		}
	}
	
	void Update() {
		// New Game button pressed
		if (buttonID==0 && selectSaveMenu) {
			Vector3 pos = new Vector3(0,0,0);
			mainMenuObj.transform.Translate(-1,0,0);
			if (mainMenuObj.transform.position.x < -90) {
				pos = mainMenuObj.transform.position;
				pos.x = -90;
				mainMenuObj.transform.position = pos;
			}
		 	selectSaveMenuObj.transform.Translate(-1,0,0);
			if (selectSaveMenuObj.transform.position.x < 21) {
				pos = selectSaveMenuObj.transform.position;
				pos.x = 21;
				selectSaveMenuObj.transform.position = pos;
			}
			if (mainMenuObj.transform.position.x==-90 && selectSaveMenuObj.transform.position.x==21) { 
				disableEnableChildren(true, mainMenuObj);
				disableEnableChildren(true, selectSaveMenuObj);
				selectSaveMenu=false;
			}
		}
		// Back button pressed from 'select a save slot'
		if (buttonID==8 && !selectSaveMenu) {
			Vector3 pos = new Vector3(0,0,0);
			mainMenuObj.transform.Translate(1,0,0);
			if (mainMenuObj.transform.position.x > 21) {
				pos = mainMenuObj.transform.position;
				pos.x = 21;
				mainMenuObj.transform.position = pos;
			}
		 	selectSaveMenuObj.transform.Translate(1,0,0);
			if (selectSaveMenuObj.transform.position.x > 135) {
				pos = selectSaveMenuObj.transform.position;
				pos.x = 135;
				selectSaveMenuObj.transform.position = pos;
			}
			if (mainMenuObj.transform.position.x==21 && selectSaveMenuObj.transform.position.x==135) { 
				disableEnableChildren(true, mainMenuObj);
				disableEnableChildren(true, selectSaveMenuObj);
				selectSaveMenu=true;
			}
		}
		// Continue Game button pressed
		if (buttonID==1 && loadMenu) {
			Vector3 pos = new Vector3(0,0,0);
			mainMenuObj.transform.Translate(-1,0,0);
			if (mainMenuObj.transform.position.x < -90) {
				pos = mainMenuObj.transform.position;
				pos.x = -90;
				mainMenuObj.transform.position = pos;
			}
		 	loadMenuObj.transform.Translate(-1,0,0);
			if (loadMenuObj.transform.position.x < 21) {
				pos = loadMenuObj.transform.position;
				pos.x = 21;
				loadMenuObj.transform.position = pos;
			}
			if (mainMenuObj.transform.position.x==-90 && loadMenuObj.transform.position.x==21) {
				disableEnableChildren(true, mainMenuObj);
				disableEnableChildren(true, loadMenuObj);
				loadMenu=false;
			}
		}
		// Back button pressed from 'load a save slot'
		if (buttonID==12 && !loadMenu) {
			Vector3 pos = new Vector3(0,0,0);
			mainMenuObj.transform.Translate(1,0,0);
			if (mainMenuObj.transform.position.x > 21) {
				pos = mainMenuObj.transform.position;
				pos.x = 21;
				mainMenuObj.transform.position = pos;
			}
		 	loadMenuObj.transform.Translate(1,0,0);
			if (loadMenuObj.transform.position.x > 135) {
				pos = loadMenuObj.transform.position;
				pos.x = 135;
				loadMenuObj.transform.position = pos;
			}
			if (mainMenuObj.transform.position.x==21 && loadMenuObj.transform.position.x==135) {
				disableEnableChildren(true, mainMenuObj);
				disableEnableChildren(true, loadMenuObj);
				loadMenu=true;
			}
		}
	}
	
	void StartNewGame(int slotNumber) {
		// Handle removal and reinitation of save slot
		// Level progress
		PlayerPrefs.SetInt("Save"+slotNumber+"Level",0);
		// Current health
		PlayerPrefs.SetInt("Save"+slotNumber+"Health",100);
		// Set Inventory
		PlayerPrefs.SetInt("Save"+slotNumber+"HealthPacks",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"Batteries",0);
		// Disable Flashlight
		PlayerPrefs.SetInt("Save"+slotNumber+"Flashlight",0);
		// Disable all weapons
		PlayerPrefs.SetInt("Save"+slotNumber+"Weapon1",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"Weapon2",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"Weapon3",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"Weapon4",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"Weapon5",0);
		// Set all ammo to 0
		PlayerPrefs.SetInt("Save"+slotNumber+"WeaponClips1",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"WeaponClips2",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"WeaponClips3",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"WeaponClips4",0);
		PlayerPrefs.SetInt("Save"+slotNumber+"WeaponClips5",0);
		
		// Set the data slot to save to
		PlayerPrefs.SetInt("CurrentSave",slotNumber);
		
		//Application.LoadLevel("LoadingScreen");
		Application.LoadLevel("StartLevel");
	}
	
	void LoadGame(int slotNumber) {
		// Set data slot to load from
		PlayerPrefs.SetInt("CurrentSave",slotNumber);
		// Get the level number and load it
		int levelNum = PlayerPrefs.GetInt("Save"+slotNumber+"Level",0);
		
		if (levelNum == 0) { Application.LoadLevel("StartLevel"); }
		else {
			GotoLevel.LoadFromMenu=true;
			Application.LoadLevel("LoadingScreen");
			//Application.LoadLevel("Level"+levelNum); 
		}
	}
	
	void disableEnableChildren(bool enabled, GameObject parent) {
		BoxCollider[] colliders = parent.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider bc in colliders) {
            bc.enabled = enabled;
        }
	}
}
