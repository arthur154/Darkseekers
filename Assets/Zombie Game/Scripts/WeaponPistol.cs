using UnityEngine;
using System.Collections;

public class WeaponPistol : MonoBehaviour {
	public GameObject bullet, muzzleFlashObj, bulletSpawnPoint, bulletShell, shellSpawnPoin;
	public float bulletSpeed, timeBetweenShots, accuracyTime;
	public int bulletDamage;
	
	private float lastShot=0.0f, lastAccurateShot=0.0f;
	
	// Update is called once per frame
	void Update () {
		if (lastShot > 0.0f) { lastShot-=Time.deltaTime; }
		else { lastShot = 0.0f; }
		if (lastAccurateShot > 0.0f) { lastAccurateShot-=Time.deltaTime; }
		else { lastAccurateShot = 0.0f; }
	}
	
	public bool Shoot () {
		if (lastShot <= 0.0f) {
			lastShot = timeBetweenShots;
			StartCoroutine(muzzleFlash());
			// Init bullet and shoot it
			GameObject tempBullet = (GameObject) Instantiate(bullet,bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.rotation);
			Vector3 tempDirection = bulletSpawnPoint.transform.forward;
			if (lastAccurateShot > 0.0f) {
				tempDirection.x += Random.value/65*Random.Range(-1.0f,1.0f);
				tempDirection.y += Random.value/65*Random.Range(-1.0f,1.0f);
			}
			tempBullet.rigidbody.AddForce(tempDirection * 100 * bulletSpeed);
			Bullet b = tempBullet.GetComponent<Bullet>();
			b.SetDamage(bulletDamage);
			lastAccurateShot = accuracyTime;
			
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
