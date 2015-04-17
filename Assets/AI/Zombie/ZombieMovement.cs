using UnityEngine;
using System.Collections;

public class ZombieMovement : MonoBehaviour {

    public float zombieSpeed = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.forward * zombieSpeed * Time.deltaTime);
       
    }
}
