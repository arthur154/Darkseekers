using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {
	public Texture2D player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// GUI Code
	void OnGUI() {
		// Draw player icon
		GUI.DrawTexture(new Rect(Screen.width/8,33*Screen.height/40,12,12),player);
		}
	
}
