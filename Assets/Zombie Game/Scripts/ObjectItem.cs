using UnityEngine;
using System.Collections;

public class ObjectItem : MonoBehaviour {
	public string itemName;
	public bool isObjective, oneTimeItem, startActive;
	public int itemID, addNumber;
	/*
	One Time Items (ID)	| (Item)
	 	1				| Pistol
	 	2				| M16
	 	3				| AK-47
	 	4				| Sniper
	 	5				| Shotgun
	 	6				| Flashlight
	 Other Items (ID)	| (Item)
	 	1				| Pistol Ammo
	 	2				| M16 Ammo
	 	3				| AK-47 Ammo
	 	4				| Sniper Ammo
	 	5				| Shotgun Ammo
	 	6				| Battery
	 	7				| Heath Pack
	*/
	
	private bool dependents = false;
	private EnableDisable e;
	
	// Use this for initialization
	void Start () {
		if (!isObjective) {
			ParticleSystem p = gameObject.GetComponent<ParticleSystem>();
			p.enableEmission = false;
		}
		gameObject.SetActive(startActive);
		e = gameObject.GetComponent<EnableDisable>();
		if (e != null) { dependents = true; }

		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}
	
	public string GetName() {
		return (itemName);
	}
	
	public void Interact(out int ID, out int addNum, out bool oneTime) {
		ID = itemID;
		addNum = addNumber;
		oneTime = oneTimeItem;
		if (dependents) { e.TriggerEnableDisable(); }
		gameObject.SetActive(false);
	}
	
	public bool IsObjective() {
		return(isObjective);
	}
	
	public void GameLevelRestart () {
		gameObject.SetActive(startActive);
	}
	
	public void GameLevelUnload () {
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
