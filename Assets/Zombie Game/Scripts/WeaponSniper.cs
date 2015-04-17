using UnityEngine;
using System.Collections;

public class WeaponSniper : MonoBehaviour {
	public GameObject bullet, muzzleFlashObj, bulletSpawnPoint, bulletShell, shellSpawnPoin;
	public float bulletSpeed, timeBetweenShots;
	public int bulletDamage;
	
	private float lastShot=0.0f;
	
	// Update is called once per frame
	void Update () {
		if (lastShot > 0.0f) { lastShot-=Time.deltaTime; }
		else { lastShot = 0.0f; }
	}
	
	public bool Shoot () {
		if (lastShot <= 0.0f) {
			lastShot = timeBetweenShots;
			StartCoroutine(muzzleFlash());
			// Initialize the bullet
			GameObject tempBullet = (GameObject) Instantiate(bullet,bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.rotation);
			tempBullet.rigidbody.AddForce(bulletSpawnPoint.transform.forward * 100 * bulletSpeed);
			Bullet b = tempBullet.GetComponent<Bullet>();
			b.SetDamage(bulletDamage);
			
			// Initialize the bullet shell
			//GameObject tempBulletShell = (GameObject) Instantiate(bulletShell,shellSpawnPoint.transform.position,shellSpawnPoint.transform.rotation);
			//tempBulletShell.rigidbody.AddForce(shellSpawnPoint.transform.forward * 300);
			
			return (true);
		}
		return (false);
	}
	
	IEnumerator muzzleFlash() {
		muzzleFlashObj.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		muzzleFlashObj.SetActive(false);
	}
}
