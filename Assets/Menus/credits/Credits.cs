using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	public Texture2D Background;
	public Texture2D[] groupMembers;
	public float timer;
	private int currentMember;
	
	// GUI Code
	public void OnGUI () {
		// Draw the background
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), Background);
		
		timer -= Time.deltaTime;
		if( timer <= 0 ) {
      		currentMember = (currentMember+1)%groupMembers.Length;
      		timer = 4; //4 seconds
   		}
		
		// Draw the credit picture
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), groupMembers[currentMember], ScaleMode.ScaleToFit, true, 0);
			
		// If a button is pressed return to the main menu
		if(Input.anyKey) {
			//Application.LoadLevel(1);
			Application.LoadLevel("MainMenu");
		}
	}
}