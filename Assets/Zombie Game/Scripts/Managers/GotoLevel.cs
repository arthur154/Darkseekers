using UnityEngine;
using System.Collections;

public class GotoLevel : MonoBehaviour {
	public GameObject loadingObj, survivalObj;

	public static bool LoadFromMenu=false;
	
	void Start () {
		// Get data slot to load from
		int slotNumber = PlayerPrefs.GetInt("CurrentSave");

		// Survival Mode
		if (slotNumber == -1) { Application.LoadLevel("SurvivalMode"); loadingObj.SetActive(false); }
		else {
			survivalObj.SetActive(false);

			// Get the Level
			int level;
			if (LoadFromMenu) level = PlayerPrefs.GetInt("Save"+slotNumber+"Level");
			else level = PlayerPrefs.GetInt("Save"+slotNumber+"Level") + 1;

			// Increment Level Progress
			PlayerPrefs.SetInt("Save"+slotNumber+"Level",level);
			// Load next level
			if (level == 1) { Application.LoadLevel("CutScene"); }
			else { 
				Application.LoadLevel("Level"+level); 
			}
		}
	}
}
