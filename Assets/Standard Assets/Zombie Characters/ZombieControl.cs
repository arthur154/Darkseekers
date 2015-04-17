using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour {
	private Animator anim;
	private Vector3 pos;
	public GameObject target; // The Player
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag("Player"); // Find the player
		anim = GetComponent<Animator>();
		pos = transform.parent.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		float speed = 2;
		anim.SetFloat("Speed", speed);
		
		transform.LookAt(target.transform.position); // Rotate towards the player
		pos += transform.forward*0.05f;
		transform.position = pos;					// Move towards the player
		
	}
}