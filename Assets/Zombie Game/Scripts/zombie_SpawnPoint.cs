using UnityEngine;
using System.Collections;

public class zombie_SpawnPoint : MonoBehaviour {
	
	public GameObject zombiePrefab, target;
	public int numberOfZombies;
	public float sweep = 1.0f;   // distribution of horde
	public float spreadRadius  = 10.0f;   // distribution of horde
	
	// Use this for initialization
	void Start () {	
		//GameObject[] gos=GameObject.FindGameObjectsWithTag("Player");
		
		for (int i=0; i<numberOfZombies; i++) {
			float amp = ( Mathf.Sin(i/6.14f) * spreadRadius ) ;
			Vector3 pos  = new Vector3( transform.position.x + ( Mathf.Sin(i*sweep) * amp ), transform.position.y, transform.position.z + ( Mathf.Cos(i*sweep) * amp ) );
			GameObject obj = (GameObject)Instantiate(zombiePrefab, pos , Quaternion.Euler( 0, Random.Range(0,360), 0) );
			//obj.tag="zombie"; // this is done in LocomotionMaleScripts2.cs
			
			//AIPath script = (AIPath)obj.GetComponent("AIPath");			
			//script.target = gos[0].transform;
			//if (target)
			//	script.target = target.transform;
			
			//zombieController ctrl=(zombieController)obj.GetComponent("zombieController");
			//ctrl.target = gos[0].transform;
			//ctrl.target = target.transform;
			
			LocomotionMaleScript3 ctrl1=(LocomotionMaleScript3)obj.GetComponent("LocomotionMaleScript3");
			//ctrl.target = gos[0].transform;
			if (target && ctrl1)
				ctrl1.target = target.transform;

			//dont delete the following part because we have two controllers . Even if some zombeis do have this, it dozent hurt.
			//zombieAnimationController ctrl2=(zombieAnimationController)obj.GetComponent("zombieAnimationController");
			//ctrl2.target = gos[0].transform;
			//if (target && ctrl2)
				//ctrl2.target = target.transform;
		}
	}
}
