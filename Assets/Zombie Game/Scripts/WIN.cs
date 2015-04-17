using UnityEngine;
using System.Collections;

public class WIN : MonoBehaviour {

	void OnTriggerEnter () {
		gameManager.TriggerLevelUnload();
		Application.LoadLevel("CutScene3");
	}
}
