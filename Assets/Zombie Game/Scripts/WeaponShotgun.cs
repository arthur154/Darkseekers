using UnityEngine;
using System.Collections;

public class WeaponShotgun : MonoBehaviour {
public GameObject bullet, muzzleFlashObj, bulletSpawnPoint1, bulletSpawnPoint2, bulletSpawnPoint3, bulletSpawnPoint4, bulletShell, shellSpawnPoint;
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
			// Initialize the bullet
			Vector3 direction1 = bulletSpawnPoint1.transform.forward;
			Vector3 direction2 = bulletSpawnPoint2.transform.forward;
			Vector3 direction3 = bulletSpawnPoint3.transform.forward;
			Vector3 direction4 = bulletSpawnPoint4.transform.forward;
			direction1.x += Random.value/30*Random.Range(-1.0f,1.0f);
			direction1.y += Random.value/30*Random.Range(-1.0f,1.0f);
			direction2.x += Random.value/30*Random.Range(-1.0f,1.0f);
			direction2.y += Random.value/30*Random.Range(-1.0f,1.0f);
			direction3.x += Random.value/30*Random.Range(-1.0f,1.0f);
			direction3.y += Random.value/30*Random.Range(-1.0f,1.0f);
			direction4.x += Random.value/30*Random.Range(-1.0f,1.0f);
			direction4.y += Random.value/30*Random.Range(-1.0f,1.0f);
			if (lastAccurateShot > 0.0f) {
				float randX = Random.value/40*Random.Range(-1.0f,1.0f);
				float randY = Random.value/40*Random.Range(-1.0f,1.0f);
				direction1.x += randX;
				direction1.y += randY;
				direction2.x += randX;
				direction2.y += randY;
				direction3.x += randX;
				direction3.y += randY;
				direction4.x += randX;
				direction4.y += randY;
			}
			GameObject tempBullet1 = (GameObject) Instantiate(bullet,bulletSpawnPoint1.transform.position,bulletSpawnPoint1.transform.rotation);
			GameObject tempBullet2 = (GameObject) Instantiate(bullet,bulletSpawnPoint2.transform.position,bulletSpawnPoint2.transform.rotation);
			GameObject tempBullet3 = (GameObject) Instantiate(bullet,bulletSpawnPoint3.transform.position,bulletSpawnPoint3.transform.rotation);
			GameObject tempBullet4 = (GameObject) Instantiate(bullet,bulletSpawnPoint4.transform.position,bulletSpawnPoint4.transform.rotation);
			
			tempBullet1.rigidbody.AddForce(direction1 * 100 * bulletSpeed);
			tempBullet2.rigidbody.AddForce(direction2 * 100 * bulletSpeed);
			tempBullet3.rigidbody.AddForce(direction3 * 100 * bulletSpeed);
			tempBullet4.rigidbody.AddForce(direction4 * 100 * bulletSpeed);
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