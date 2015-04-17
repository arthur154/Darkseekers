using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == "Player")
        {	
			WeaponManager.SaveCurrentWeapons();
			Player.SaveCurrentHeatlh();
			Inventory.SaveCurrentInventory();
			gameManager.TriggerLevelUnload();
			Application.LoadLevel("LoadingScreen");
        }
    }
}
