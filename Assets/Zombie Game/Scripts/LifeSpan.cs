using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {
	public float totalLife;
	
	private float lifeTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (lifeTime > totalLife) {
			Destroy(gameObject);
		}
		lifeTime += Time.deltaTime;
	}
}
