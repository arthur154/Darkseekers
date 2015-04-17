using UnityEngine;
using System.Collections;

public class GotoCutScene : MonoBehaviour {

	void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == "Player")
        {
			// Set data slot to load from
			int slotNumber = PlayerPrefs.GetInt("CurrentSave");
			
			// Level progress
			PlayerPrefs.SetInt("Save"+slotNumber+"Level",0);
			// Current health
			// *** Get Health Here ***
			PlayerPrefs.SetInt("Save"+slotNumber+"Health",100);
			
			
			Application.LoadLevel("CutScene");
        }
    }
}
