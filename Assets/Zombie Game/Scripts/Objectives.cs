using UnityEngine;
using System.Collections;

public class Objectives : MonoBehaviour {
	public string[] objectivesList;
	
	private bool showObjectives = true;
	private static int objectiveCounter = 0;

	public static int GetObjectiveCount () {
		return (objectiveCounter);
	}

	public static void ResetObjectiveCount () {
		objectiveCounter=0;
	}

	public static void IncObjectiveCount () {
		objectiveCounter++;
	}

	void Start () {
		objectiveCounter=0;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)) showObjectives = !showObjectives;
	}
	
	void OnGUI () {
		string objective = objectivesList[objectiveCounter];
		string[] objectiveWords = objective.Split(' ');
		int counter = 0;
		string wrappedobjective = "";
		int wrapLength = 31;
		for(int i = 0; i < objectiveWords.Length; i++){
			if((wrappedobjective.Length + objectiveWords[i].Length) > wrapLength){
				wrappedobjective = wrappedobjective + "\n";
				wrapLength+=31;
			}
			wrappedobjective = wrappedobjective + " " + objectiveWords[i];
		}
		int length = objectivesList[objectiveCounter].Length;
		if (showObjectives) {
			float h;
			if(objectivesList[objectiveCounter].Length>200){
				h = 125;
			}
			else if(objectivesList[objectiveCounter].Length>100){
				h = 100;
			}
			else if(objectivesList[objectiveCounter].Length>80){
				h = 75;
			}
			else if (objectivesList[objectiveCounter].Length>35){
				h = 50; 
			}
			else {
				h = 25;
			}
			GUI.enabled = false;
			GUI.TextField(new Rect(Screen.width-210,10,210,h),wrappedobjective);
			GUI.enabled = true;
		}
	}
	
	public void IncObjective() {
		objectiveCounter+=1;
	}
}
