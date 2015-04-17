using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	public delegate void GameEvent();
	public Texture2D GamePaused, Controls, ConfirmQuit;
	public static event GameEvent GamePause, GameUnpause, GameLevelRestart, GameLevelUnload;
	public bool survivalMode;
	
	private static bool paused;
	private bool showControls, confirmQuit;
	private int currentSave;

	public static void TriggerGamePause(){
		Screen.showCursor = true;
		if(GamePause != null){
			GamePause();
		}
	}

	public static void TriggerGameUnpause(){
		Screen.showCursor = false;
		if(GameUnpause != null){
			GameUnpause();
		}
	}
	
	public static void TriggerLevelRestart(){
		ZCount.ResetKills();
		Objectives.ResetObjectiveCount();
		// Spawn points and pickup objects should reset
		paused = false;
		GameLevelRestart();
	}
	
	public static void TriggerLevelUnload(){
		ZCount.ResetKills();
		Objectives.ResetObjectiveCount();
		GameLevelUnload();
	}
	
	// Use this for initialization
	void Start () {
		paused = false;
		showControls = false;
		confirmQuit = false;
		Screen.showCursor = false;
		// Get current save
		currentSave = PlayerPrefs.GetInt("CurrentSave");
	}
	
	// Update is called once per frame
	void Update () {
		// If ESC is press pause/unpause
		if(Input.GetKeyDown(KeyCode.Escape)){
			paused = !paused;
			PauseGame(paused);
			showControls = false;
			confirmQuit = false;
		}
	}
	
	// GUI Code
	void OnGUI() {
		if (paused) MainMenu();
	}
	
	void PauseGame (bool pause) {
		if(pause) {
			// Disable all game objects (that move)
			TriggerGamePause();
		}
		else {
			// Enable all game objects (that move)
			TriggerGameUnpause();
		}
	}
	
	void MainMenu () {
		// In-game pause background
		GUI.Box(new Rect(5,5,Screen.width-10,Screen.height-10), "");
		
		// Show controls
		if (showControls) {
			ControlsMenu();
			return;
		}
		else if (confirmQuit) {
			ConfQuit();
			return;
		}
		
		// Main game paused text
		GUI.DrawTexture(new Rect(5*Screen.width/16,3*Screen.height/8,3*Screen.width/8,2*Screen.height/10),GamePaused);
		
		// Start main button area
		GUILayout.BeginArea(new Rect(Screen.width/2-75,3*Screen.height/4,150,300));
			if(GUILayout.Button("Resume")){
				paused = false;
				PauseGame(paused);
			}
			if(GUILayout.Button("Controls")){
				showControls=true;
			}
			if(!survivalMode) {
				if(GUILayout.Button("Restart Level")){
					TriggerLevelRestart();
				}
				if(GUILayout.Button("Save and Quit")){
					confirmQuit=true;
				}
			}
			else{
				if(GUILayout.Button("Quit")){
					confirmQuit=true;
				}
			}
		GUILayout.EndArea();
	}
	
	void ControlsMenu () {
		// Controls picture
		GUI.DrawTexture(new Rect(50,Screen.height/10,Screen.width-100,7*Screen.height/10),Controls);
		// Controls Menu
		if(GUI.Button(new Rect(Screen.width/2-75,7*Screen.height/8,150,20),"Back")) {
			showControls=false;
		}
	}
	
	void ConfQuit () {
		// Are you sure? text
		GUI.DrawTexture(new Rect(Screen.width/10,3*Screen.height/8,4*Screen.width/5,2*Screen.height/10),ConfirmQuit);
		// Controls Menu
		GUILayout.BeginArea(new Rect(Screen.width/2-75,3*Screen.height/4,150,300));
			if(GUILayout.Button("Yes")){
				// Save game
				// ADD A SAVE FUNCTION TO SCRIPTS?
				//Application.LoadLevel(1);
				TriggerLevelUnload();
				Application.LoadLevel("MainMenu");
			}
			if(GUILayout.Button("No")){
				confirmQuit = false;
			}
		GUILayout.EndArea();
	}
}
