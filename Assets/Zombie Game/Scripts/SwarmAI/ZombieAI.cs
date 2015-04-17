using UnityEngine;
using System.Collections;
using Pathfinding;

public class ZombieAI : MonoBehaviour {
	// Constants
	public float repathRate = 0.5f, attackDuration;
	public GameObject idleSoundObj, attackSoundObj, damageSoundObj;
	public AudioClip zombieDamageSound1, zombieDamageSound2; // Add 2 damage
	public AudioClip zombieAlertSound1; // Add 3 alert
	public AudioClip zombieAttackSound1; // Add 3 attack
	// Set by SpawnPoint
	public Transform target;
	public SwarmAI swarmContoller;
	public int health =100, maxStrength;
	public float proximity;
	
	private Animator anim;	
	private AnimatorStateInfo currentBaseState;	
	private CharacterController controller;
	private Path path;
	private Player player;
	private Seeker seeker;
	private float nextWaypointDistance=3, attackDistance=2.5f, lastRepath=-9999f, attackTime=0.0f;
	private int currentWaypoint = 0, old_normalizedTime=0, strength;
	private bool canSearch = false, canMove = false, collided = false;

	static int attackState = Animator.StringToHash("Base Layer.attack");				

	public void SetPlayer (Transform p) {
		player = p.GetComponent<Player>();
	}

	void Start () {
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		player = target.gameObject.GetComponent<Player>();
		seeker = GetComponent<Seeker>();
		
		gameObject.layer = LayerMask.NameToLayer("Zombies_Layer");
		anim.speed = 1.0f;
		audio.clip = zombieAlertSound1;
		strength = Random.Range (4,maxStrength);

		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;		
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}
	
	void Update () {
		var distToTarget = Vector3.Distance(transform.position, target.position);
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		anim.SetInteger("Health", health);

		if (health >= 0) {
			if (distToTarget>proximity && !canSearch)
			{
				// Set Idle animation
				anim.SetFloat("Dist", 25);
			}		
			else 
			{	
				if(distToTarget<attackDistance)
				{			
					// Set Attack animation
					anim.SetFloat("Dist", 0);
					canSearch=false;
					TryAttack();		
				}
				else
				{
					if (distToTarget>10.0f) RotateTowards(); 

					canSearch=true;
					// Set Run animation
					anim.SetFloat("Dist", 10);
					Vector3 dir = searchPath();
						dir *= 2.0f;
						if (collided)
							dir+=2*Vector3.Cross(dir, Vector3.up);
						controller.SimpleMove (dir);
				}
				RotateTowards(); 			
			}
		}

		if(!canSearch) currentWaypoint = 0;

		if (attackTime < 0.0f) attackTime=0.0f;
		else attackTime-=Time.deltaTime;
	}

	void RotateTowards () 
	{
		Vector3 direction = target.position - transform.position;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5 * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}

	
	public void OnPathComplete (Path p) {
		p.Claim (this);
		if (!p.error) {
			if (path != null) path.Release (this);
			path = p;
			currentWaypoint = 0;
		} else {
			p.Release (this);
		}
	}
	
	Vector3 searchPath()
	{
		Vector3 dir=new Vector3(0,0,0);
		if (!canSearch) return dir;
		
		if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
			lastRepath = Time.time+ Random.value*repathRate*0.5f;
			seeker.StartPath (transform.position,target.position, OnPathComplete);
		}
		
		if (path == null) {
			//We have no path to move after yet
			return dir;
		}
		
		if (currentWaypoint > path.vectorPath.Count) return dir; 
		if (currentWaypoint == path.vectorPath.Count) {
			currentWaypoint++;
			return dir;
		}
		
		dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		
		//if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
		if ( (transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			//return;
		}
		
		return dir;
	}

	public void TryAttack() {
		if (!attackSoundObj.audio.isPlaying) {
			int r = Random.Range(0,4);
			if (r==0) attackSoundObj.audio.clip = zombieAttackSound1;
			else if (r==1) attackSoundObj.audio.clip = zombieAttackSound1;
			else if (r==2) attackSoundObj.audio.clip = zombieAttackSound1;
			else attackSoundObj.audio.clip = zombieAttackSound1;
			
			attackSoundObj.audio.Play();
		}
		if (attackTime<=0.0f) {
			attackTime = attackDuration;
			player.addHealth(-strength);
		} 
	}

	public void TakeDamage (int d) {
		if (health >= 0) {
			if (!damageSoundObj.audio.isPlaying) {
				int r = Random.Range(0,4);
				if (r==0) damageSoundObj.audio.clip = zombieDamageSound1;
				else if (r==1) damageSoundObj.audio.clip = zombieDamageSound2;
				else if (r==2) damageSoundObj.audio.clip = zombieDamageSound1;
				else damageSoundObj.audio.clip = zombieDamageSound2;
				
				damageSoundObj.audio.Play();
			}
			
			//swarmController.AlertTheSwarm();
			health -= d;
		}
		if (health < 0) {
			audio.Stop();
			gameObject.layer = LayerMask.NameToLayer("Default");
			gameObject.GetComponent<CharacterController>().enabled = false;
			ZCount.ZombieKill();
		}
	}
	
	public void GamePause() {
		enabled = false;
		anim.enabled = false;
	}
	
	public void GameUnpause() {
		enabled = true;
		anim.enabled = true;
	}
	
	public void GameLevelRestart () {
		gameManager.GamePause -= GamePause;
		gameManager.GameUnpause -= GameUnpause;	
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
		Destroy(gameObject);
	}
	
	public void GameLevelUnload () {
		gameManager.GamePause -= GamePause;
		gameManager.GameUnpause -= GameUnpause;	
		gameManager.GameLevelRestart -= GameLevelRestart;
		gameManager.GameLevelUnload -= GameLevelUnload;
	}
}
