using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectInteract : MonoBehaviour {
	public Objectives objectivesObj;
	public Inventory userInventory;
	public WeaponManager userWeapons;
	
    private RaycastHit hit;
    private bool showGUI = false;
	private string itemName;

    void OnGUI()
    {
        if (showGUI) {
			int len = itemName.Length;
			GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-50,6*len+90,25),"Press 'E' to "+itemName);
		}
    }
	
	void Start() {
		// add to game manager?
	}
	
	void Update () 
    {
		Ray ray = Camera.main.ScreenPointToRay (new Vector3(Screen.width/2, Screen.height/2, 0f));
		if(Physics.Raycast(ray, out hit, 4.0f))
        {
			GameObject temp = hit.collider.gameObject;
			ObjectItem o = temp.GetComponent<ObjectItem>();
			if (o != null) {
			//if (!o.Equals(null)) {
				itemName = o.GetName();
				if (Input.GetKey(KeyCode.E)) {
					bool isObjective = o.IsObjective();
					if (isObjective) {
						objectivesObj.IncObjective();
					}
					int id, addNum;
					bool oneTime;
					o.Interact(out id, out addNum, out oneTime);
					InteractObj(id, addNum, oneTime);
					showGUI = false;
				}
				else {
					showGUI = true;
				}
			}
			else { showGUI = false; }
		}
		else { showGUI = false; }
	}
	
	void InteractObj (int itemID, int addNumber, bool oneTimeItem) {
		if (oneTimeItem) {
			switch (itemID) {
				case 1: enablePistol(); break;
				case 2: enableM16(); break;
				case 3: enableAK47(); break;
				case 4: enableSniper(); break;
				case 5: enableShotgun(); break;
				case 6: enableFlashlight(); break;
			}
		}
		else {
			switch (itemID) {
				case 1: addPistolAmmo(addNumber); break;
				case 2: addM16Ammo(addNumber); break;
				case 3: addAK47Ammo(addNumber); break;
				case 4: addSniperAmmo(addNumber); break;
				case 5: addShotgunAmmo(addNumber); break;
				case 6: addBattery(addNumber); break;
				case 7: addHealthPack(addNumber); break;
			}
		}
	}
	
	void enablePistol() { userWeapons.enableWeapon(1); }
	
	void enableM16() {  userWeapons.enableWeapon(2); }
	
	void enableAK47() { userWeapons.enableWeapon(3); }
	
	void enableSniper() { userWeapons.enableWeapon(4); }
	
	void enableShotgun() { userWeapons.enableWeapon(5); }
	
	void enableFlashlight() { userInventory.EnableFlashlight(); }
	
	void addPistolAmmo(int addNumber) { userWeapons.addAmmoClips(1, addNumber); }
	
	void addM16Ammo(int addNumber) { userWeapons.addAmmoClips(2, addNumber); }
	
	void addAK47Ammo(int addNumber) { userWeapons.addAmmoClips(3, addNumber); }
	
	void addSniperAmmo(int addNumber) { userWeapons.addAmmoClips(4, addNumber); }
	
	void addShotgunAmmo(int addNumber) { userWeapons.addAmmoClips(5, addNumber); }
	
	void addBattery(int addNumber) { userInventory.AddBatteries(addNumber); }
	
	void addHealthPack(int addNumber) { userInventory.AddHealthPacks(addNumber); }
}
