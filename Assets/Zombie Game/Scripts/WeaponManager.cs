using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {
	public GameObject pistol, M16, AK47, sniper, shotgun;
	public MouseLook m1, m2;
	public AudioClip pistolSound, pistolReloadSound, M16Sound, M16ReloadSound, AK47Sound, AK47ReloadSound, shotgunSound, shotgunReloadSound, sniperSound, sniperReloadSound, emptyClipSound;
	public Texture2D pistolImg, M16Img, AK47Img, shotgunImg, sniperImg, sniperAimImage, aimer;
	public WeaponPistol pistolObject;
	public WeaponM16 M16Object;
	public WeaponAK47 AK47Object;
	public WeaponSniper sniperObject;
	public WeaponShotgun shotgunObject;
	public Camera MainCamera;
	public float ratioZoom = 60,ratioZoomV,ratioZoomSpeed = .2f,defaultCameraAngle = 60,currentTargetCameraAngle = 60;

	private static bool enabledPistol, enabledM16, enabledAK47, enabledSniper, enabledShotgun, noAction=false;
	private static int pistolAmmo, M16Ammo, AK47Ammo, shotgunAmmo, sniperAmmo, pistolClip, M16Clip, AK47Clip, shotgunClip, sniperClip, equipedWeapon=0, saveSlot;
	private float pistolAmmoEmpty=0.0f, M16AmmoEmpty=0.0f, AK47AmmoEmpty=0.0f, shotgunAmmoEmpty, sniperAmmoEmpty=0.0f, noWeaponAmmoEmpty=0.0f;
	
	/*
		1 - Pistol (clip size 6)
		2 - M16 (clip size 20)
		3 - AK47 (clip size 30)
		4 - Sniper Rifle (clip size 4) 
	*/
	
	void Start () {
		// Load current save data
		saveSlot = PlayerPrefs.GetInt("CurrentSave");
		LoadWeapons();
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
		if (pistol.activeInHierarchy){
			currentTargetCameraAngle = 50;
		}else if (M16.activeInHierarchy){
			currentTargetCameraAngle = 40;
		}else if (AK47.activeInHierarchy){
			currentTargetCameraAngle = 40;
		}else if (sniper.activeInHierarchy){
			currentTargetCameraAngle = 20;
		}else if (shotgun.activeInHierarchy){
			currentTargetCameraAngle = 50;
		}
	}

	public static void SaveCurrentWeapons () {
		// Set enabled/disabled
		if (enabledPistol) PlayerPrefs.SetInt("Save"+saveSlot+"Weapon1",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Weapon1",0);
		if (enabledM16) PlayerPrefs.SetInt("Save"+saveSlot+"Weapon2",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Weapon2",0);
		if (enabledAK47) PlayerPrefs.SetInt("Save"+saveSlot+"Weapon3",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Weapon3",0);
		if (enabledSniper) PlayerPrefs.SetInt("Save"+saveSlot+"Weapon4",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Weapon4",0);
		if (enabledShotgun) PlayerPrefs.SetInt("Save"+saveSlot+"Weapon5",1);
		else PlayerPrefs.SetInt("Save"+saveSlot+"Weapon5",0);
		// Set the number of clips
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponClips1",pistolAmmo);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponClips2",M16Ammo);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponClips3",AK47Ammo);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponClips4",sniperAmmo);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponClips5",shotgunAmmo);
		// Set the bullets
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponBullets1",pistolClip);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponBullets2",M16Clip);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponBullets3",AK47Clip);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponBullets4",sniperClip);
		PlayerPrefs.SetInt("Save"+saveSlot+"WeaponBullets5",shotgunClip);
	}

	void LoadWeapons () {
		// Survival Mode
		if (saveSlot == -1) {
			enabledPistol = true;
			pistolAmmo = 5;
			enabledM16 = true;
			M16Ammo = 7;
			enabledAK47 = true;
			AK47Ammo = 4;
			enabledSniper = false;
			sniperAmmo = 0;
			enabledShotgun = false;
			shotgunAmmo = 0;
			
			equipedWeapon = 1;
			pistol.SetActive(true);
		}
		// Story Mode
		else {
			// Turn off all weapons
			equipedWeapon = 0;
			pistol.SetActive(false);
			M16.SetActive(false);
			AK47.SetActive(false);
			sniper.SetActive(false);
			shotgun.SetActive(false);
			// Get enabled/disabled
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Weapon1",0)==0) { enabledPistol=false; }
			else { enabledPistol=true; }
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Weapon2",0)==0) { enabledM16=false; }
			else { enabledM16=true; }
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Weapon3",0)==0) { enabledAK47=false; }
			else { enabledAK47=true; }
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Weapon4",0)==0) { enabledSniper=false; }
			else { enabledSniper=true; }
			if (PlayerPrefs.GetInt("Save"+saveSlot+"Weapon5",0)==0) { enabledShotgun=false; }
			else { enabledShotgun=true; }
			// Get the ammo clips
			pistolAmmo = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponClips1",0);
			M16Ammo = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponClips2",0);
			AK47Ammo = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponClips3",0);
			sniperAmmo = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponClips4",0);
			shotgunAmmo = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponClips5",0);
			// Get the current bullets
			pistolClip = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponBullets1",0);
			M16Clip = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponBullets2",0);
			AK47Clip = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponBullets3",0);
			sniperClip = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponBullets4",0);
			shotgunClip = PlayerPrefs.GetInt("Save"+saveSlot+"WeaponBullets5",0);
		}
		
	}
	
	void OnGUI () {
		GUI.enabled = false;
		bool drawAimer = true;
		// Display current weapon information
		GUI.Box(new Rect(Screen.width-260,Screen.height-55,250,45),""); // Background box
		if (equipedWeapon == 1) {
			GUI.DrawTexture(new Rect(Screen.width-255,Screen.height-45,50,25),pistolImg);
			GUI.TextArea(new Rect(Screen.width-200,Screen.height-45,185,25),"Bullets: "+pistolClip+" Clips:"+pistolAmmo);
		}
		else if (equipedWeapon == 2) {
			GUI.DrawTexture(new Rect(Screen.width-255,Screen.height-55,50,45),M16Img);
			GUI.TextArea(new Rect(Screen.width-200,Screen.height-45,185,25),"Bullets: "+M16Clip+" Clips:"+M16Ammo);
		}
		else if (equipedWeapon == 3) {
			GUI.DrawTexture(new Rect(Screen.width-257,Screen.height-55,67,55),AK47Img);
			GUI.TextArea(new Rect(Screen.width-200,Screen.height-45,185,25),"Bullets: "+AK47Clip+" Clips:"+AK47Ammo);
		}
		else if (equipedWeapon == 4) {
			GUI.DrawTexture(new Rect(Screen.width-255,Screen.height-48,55,35),sniperImg);
			GUI.TextArea(new Rect(Screen.width-200,Screen.height-45,185,25),"Bullets: "+sniperClip+" Clips:"+sniperAmmo);
			if(Input.GetMouseButton(1)){
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),sniperAimImage);
				drawAimer=false;
			}
		}
		else if (equipedWeapon == 5) {
			GUI.DrawTexture(new Rect(Screen.width-259,Screen.height-50,55,30),shotgunImg);
			GUI.TextArea(new Rect(Screen.width-200,Screen.height-45,185,25),"Bullets: "+shotgunClip+" Clips:"+shotgunAmmo);
		}
		else { 
			GUI.TextArea(new Rect(Screen.width-198,Screen.height-45,126,25),"No weapon equipped");
		}
		// Display 'No ammo clips left' message
		if (pistolAmmoEmpty > 0.0f) { 
			GUI.TextArea(new Rect(Screen.width/2-100,Screen.height-35,200,25),"No pistol clips left!");
		}
		else if (M16AmmoEmpty > 0.0f) { 
			GUI.TextArea(new Rect(Screen.width/2-100,Screen.height-35,200,25),"No M16 clips left!");
		}
		else if (AK47AmmoEmpty > 0.0f) { 
			GUI.TextArea(new Rect(Screen.width/2-100,Screen.height-35,200,25),"No AK47 clips left!");
		}
		else if (sniperAmmoEmpty > 0.0f) { 
			GUI.TextArea(new Rect(Screen.width/2-100,Screen.height-35,200,25),"No Sniper Rifle clips left!");
		}
		else if (noWeaponAmmoEmpty > 0.0f) { 
			GUI.TextArea(new Rect(Screen.width/2-93,Screen.height-35,185,25),"No weapon equipped to reload!");
		}
		// Decide weather to draw the aimer or not and set the look sensitivity
		if (drawAimer) {
			GUI.DrawTexture(new Rect(Screen.width/2-20,Screen.height/2-20,40,40),aimer);
			m1.sensitivityX = 10.0f;
			m1.sensitivityY = 10.0f;
			m2.sensitivityX = 10.0f;
			m2.sensitivityY = 10.0f;
		}
		else {
			m1.sensitivityX = 0.7f;
			m1.sensitivityY = 0.7f;
			m2.sensitivityX = 0.7f;
			m2.sensitivityY = 0.7f;
		}
		GUI.enabled = true;
	}
	
	void Update () {
		// Decrement all the error message times
		if (pistolAmmoEmpty <= 0.0f) { pistolAmmoEmpty = 0.0f; }
		else { pistolAmmoEmpty -= Time.deltaTime; }
		if (M16AmmoEmpty <= 0.0f) { M16AmmoEmpty = 0.0f; }
		else { M16AmmoEmpty -= Time.deltaTime; }
		if (AK47AmmoEmpty <= 0.0f) { AK47AmmoEmpty = 0.0f; }
		else { AK47AmmoEmpty -= Time.deltaTime; }
		if (sniperAmmoEmpty <= 0.0f) { sniperAmmoEmpty = 0.0f; }
		else { sniperAmmoEmpty -= Time.deltaTime; }
		if (shotgunAmmoEmpty <= 0.0f) { shotgunAmmoEmpty = 0.0f; }
		else { shotgunAmmoEmpty -= Time.deltaTime; }
		if (noWeaponAmmoEmpty <= 0.0f) { noWeaponAmmoEmpty = 0.0f; }
		else { noWeaponAmmoEmpty -= Time.deltaTime; }

		//Zoom
		if(Input.GetMouseButton(1)){
			ratioZoom = Mathf.SmoothDamp(ratioZoom, 1, ref ratioZoomV, ratioZoomSpeed);
		}else{
			ratioZoom = Mathf.SmoothDamp(ratioZoom, 0, ref ratioZoomV, ratioZoomSpeed);
		}

		MainCamera.fieldOfView = Mathf.Lerp(defaultCameraAngle, currentTargetCameraAngle, ratioZoom);

		if (!noAction) {
			// Reload button pressed
			if (Input.GetKeyDown(KeyCode.R)) {
				StartCoroutine("tryReload");
			}
			// Shoot button pressed
			else if (Input.GetMouseButton(0)) {
				if (enabledPistol && equipedWeapon==1) {
					// If enough bullets try to shoot
					if (pistolClip > 0) { 
						// If the gun can shoot (time between shots is small enough) then
						// play a shot sound and decrement the bullets in the clip
						if(pistolObject.Shoot()) { audio.clip = pistolSound; audio.Play(); pistolClip--; }
					}
					// If 0 bullets then clip is empty
					else if (Input.GetMouseButtonDown(0)) { audio.clip = emptyClipSound; audio.Play(); }
				}
				else if (enabledM16 && equipedWeapon==2) {
					if (M16Clip > 0) { 
						if(M16Object.Shoot()) { audio.clip = M16Sound; audio.Play(); M16Clip--; }
					}
					else if (Input.GetMouseButtonDown(0))  { audio.clip = emptyClipSound; audio.Play(); }
				}
				else if (enabledAK47 && equipedWeapon==3) {
					if (AK47Clip > 0) { 
						if(AK47Object.Shoot()) { audio.clip = AK47Sound; audio.Play(); AK47Clip--; }
					}
					else if (Input.GetMouseButtonDown(0))  { audio.clip = emptyClipSound; audio.Play(); }
				}
				else if (enabledSniper && equipedWeapon==4) {
					if (sniperClip > 0) { 
						if(sniperObject.Shoot()) { audio.clip = sniperSound; audio.Play(); sniperClip--; }
					}
					else if (Input.GetMouseButtonDown(0))  { audio.clip = emptyClipSound; audio.Play(); }
				}
				else if (enabledShotgun && equipedWeapon==5) {
					if (shotgunClip > 0) { 
						if(shotgunObject.Shoot()) { audio.clip = shotgunSound; audio.Play(); shotgunClip--; }
					}
					else if (Input.GetMouseButtonDown(0))  { audio.clip = emptyClipSound; audio.Play(); }
				}
			}
			// Switch current weapon buttons
			else if (Input.GetKeyDown(KeyCode.Alpha1) && enabledPistol) {
				switchWeapon(1);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2) && enabledM16) {
				switchWeapon(2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3) && enabledAK47) {
				switchWeapon(3);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4) && enabledSniper) {
				switchWeapon(4);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5) && enabledShotgun) {
				switchWeapon(5);
			}
		}
	}
	
	void switchWeapon (int w) {
		// Weapon enabled has already been checked
		if (w==1) { 
			equipedWeapon=1;
			pistol.SetActive(true);
			M16.SetActive(false);
			AK47.SetActive(false);
			sniper.SetActive(false);
			shotgun.SetActive(false);
			currentTargetCameraAngle = 50;
		}
		else if (w==2) { 
			equipedWeapon=2; 
			pistol.SetActive(false);
			M16.SetActive(true);
			AK47.SetActive(false);
			sniper.SetActive(false);
			shotgun.SetActive(false);
			currentTargetCameraAngle = 40;
		}
		else if (w==3) { 
			equipedWeapon=3; 
			pistol.SetActive(false);
			M16.SetActive(false);
			AK47.SetActive(true);
			sniper.SetActive(false);
			shotgun.SetActive(false);
			currentTargetCameraAngle = 40;
		}
		else if (w==4) { 
			equipedWeapon=4; 
			pistol.SetActive(false);
			M16.SetActive(false);
			AK47.SetActive(false);
			sniper.SetActive(true);
			shotgun.SetActive(false);
			currentTargetCameraAngle = 5;
		}
		else if (w==5) { 
			equipedWeapon=5; 
			pistol.SetActive(false);
			M16.SetActive(false);
			AK47.SetActive(false);
			sniper.SetActive(false);
			shotgun.SetActive(true);
			currentTargetCameraAngle = 50;
		}
	}
	
	IEnumerator tryReload () {
		// Weapon enabled has been checked already
		if (equipedWeapon==1) {
			if (pistolAmmo > 0 && pistolClip != 6) {
				audio.clip = pistolReloadSound; 
				audio.Play();
				noAction = true;
				yield return new WaitForSeconds(0.7f);
				noAction = false;
				pistolAmmo -= 1;
				pistolClip = 6;
			}
			else if (pistolClip == 6) {  }
			else { pistolAmmoEmpty = 1.5f; }
		}
		else if (equipedWeapon==2) {
			if (M16Ammo > 0 && M16Clip != 20) {
				audio.clip = M16ReloadSound; 
				audio.Play();
				noAction = true;
				yield return new WaitForSeconds(1.0f);
				noAction = false;
				M16Ammo -= 1;
				M16Clip = 20;
			}
			else if (M16Clip == 20) {  }
			else { M16AmmoEmpty = 1.5f; }
		}
		else if (equipedWeapon==3) {
			if (AK47Ammo > 0 && AK47Clip != 30) {
				audio.clip = AK47ReloadSound; 
				audio.Play();
				noAction = true;
				yield return new WaitForSeconds(2.7f);
				noAction = false;
				AK47Ammo -= 1;
				AK47Clip = 30;
			}
			else if (AK47Clip == 30) {  }
			else { AK47AmmoEmpty = 1.5f; }
		}
		else if (equipedWeapon==4) {
			if (sniperAmmo > 0 && sniperClip != 4) {
				audio.clip = sniperReloadSound; 
				audio.Play();
				noAction = true;
				yield return new WaitForSeconds(1.2f);
				noAction = false;
				sniperAmmo -= 1;
				sniperClip = 4;
			}
			else if (sniperClip == 4) {  }
			else { sniperAmmoEmpty = 1.5f; }
		}
		else if (equipedWeapon==5) {
			if (shotgunAmmo > 0 && shotgunClip != 10) {
				audio.clip = shotgunReloadSound; 
				audio.Play();
				noAction = true;
				yield return new WaitForSeconds(2.7f);
				noAction = false;
				shotgunAmmo -= 1;
				shotgunClip = 10;
			}
			else if (shotgunClip == 10) {  }
			else { shotgunAmmoEmpty = 1.5f; }
		}
		else { noWeaponAmmoEmpty = 1.5f; }
	}

	public void enableWeapon (int w) {
		if (w==1) { enabledPistol = true; pistolAmmo += 8; pistolClip = 6; switchWeapon(1); }
		else if (w==2) { enabledM16 = true; M16Ammo += 7; M16Clip = 20; switchWeapon(2); }
		else if (w==3) { enabledAK47 = true; AK47Ammo += 5; AK47Clip = 30; switchWeapon(3); }
		else if (w==4) { enabledSniper = true; sniperAmmo += 4; sniperClip = 4; switchWeapon(4); }
		else if (w==5) { enabledShotgun = true; shotgunAmmo += 3; shotgunClip = 10; switchWeapon(5); }
	}
	
	public void addAmmoClips (int w, int clips) {
		if (w==1) { pistolAmmo += clips; }
		else if (w==2) { M16Ammo += clips; }
		else if (w==3) { AK47Ammo += clips; }
		else if (w==4) { sniperAmmo += clips; }
		else if (w==5) { shotgunAmmo += clips; }
	}

	public void GameLevelRestart () {
		LoadWeapons();
	}

	public void GameLevelUnload () {
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
