using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject blood, bulletMiss;
	public float totalLife;
	
	private int damage=10;
	private float lifeTime = 0.0f;
	private Vector3 lastPosition;
	
	public float floatInFrontOfWall = 0.00001f;
	public GameObject decalHitWall;
	
	void Start () { lastPosition = transform.position; }
	
	void Update () { 
		Vector3 direction = transform.position - lastPosition;
	    Ray ray = new Ray(lastPosition, direction);
	    RaycastHit hit = new RaycastHit();
	    if (Physics.Raycast(ray, out hit, direction.magnitude))
	    {	
			//Instantiate(decalHitWall, hit.point + (hit.normal * floatInFrontOfWall), Quaternion.LookRotation(hit.normal));
	        // We hit something (see if it is a zombie)
				DamageControl temp = hit.collider.transform.GetComponent<DamageControl>();
				// If zombie then give damage and create blood
				if (temp != null) {
					temp.TakeDamage(damage);
					// Initialize hit prefab (blood)
					Instantiate(blood,hit.point,hit.transform.rotation);
				}
				// Else create a spark for a miss
				else {
					Instantiate(bulletMiss,hit.point,hit.transform.rotation);
				}

				// Destroy the bullet after hit
				Destroy(gameObject);
	    }
		// Update position
    	lastPosition = transform.position;
		// If the bullet has been in flight too long destroy it
		if (lifeTime > totalLife) {
			Destroy(gameObject);
		}
		// Update the total life
		lifeTime += Time.deltaTime;
	} 
	
	public void SetDamage (int d) {
		damage = d;
	}
}
