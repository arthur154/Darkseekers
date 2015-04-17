using UnityEngine;
using System.Collections;

public class ZombieLocalAI : MonoBehaviour {
	public float speed, rotationSpeed;
	public Player p;
	public Transform target;
	public Vector3 startPos;
	public SwarmAI swarmController;
	public float searchLength;
	public int strength;
	public GameObject idleSoundObj, attackSoundObj, damageSoundObj;
	public AudioClip zombieDamageSound1, zombieDamageSound2;
	public AudioClip zombieAlertSound1;
	public AudioClip zombieAttackSound1;

	private int health = 100;
	private CharacterController controller;
	private Animator anim;	
	private bool searching=false, playerFound=false;
	private float searchingTime=0.0f, attackTime=0.5f;
	
	void Start () {
		strength = Random.Range(3, strength);
		p = target.transform.GetComponent<Player>();
		controller = transform.GetComponent<CharacterController>();
		anim = transform.GetComponent<Animator>();
		anim.speed = 1.0f;
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;	
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}
	
	void Update () {
		anim.SetInteger("Health", health);
		if (health < 0) {
			gameObject.GetComponent<BoxCollider>().enabled = false;
			gameObject.GetComponent<CharacterController>().enabled = false;
		}
		if (health >= 0) {
			float distToTarget = Vector3.Distance(transform.position, target.position);
			anim.SetFloat("Dist", distToTarget);
			
			// Maybe have a vision? -> bool playerFound
			// if in vision then playerFound = true, this will alert the swarm too
			if (distToTarget < 20.0f || playerFound) {
				if (!playerFound) {
					audio.clip = zombieAlertSound1;
					audio.Play();
					swarmController.PlayerFoundAlert();
				}
				LookAtTarget();
				if (distToTarget > 2.0f) {
					MoveToTarget();	
				}
				else {
					if (attackTime < 0.0f) {
						attackTime = 1.5f;
						if (!attackSoundObj.audio.isPlaying) {
							attackSoundObj.audio.clip = zombieAttackSound1;
							attackSoundObj.audio.Play();
						}
						p.addHealth(-1*strength);
					}
					else {
						attackTime -= Time.deltaTime;
					}
				}
			}
			else if (searching) {
				if (searchingTime > 0.0f) {
					// Randomly search
					searchingTime -= Time.deltaTime;
				}
				else {
					searchingTime = 0.0f;
					searching = false;
				}
			}
		}
	}
	
	private void LookAtTarget() {
		Vector3 direction = target.position - transform.position;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
	
	private void MoveToTarget() {
		Vector3 direction = (target.position - transform.position).normalized;
		// Can possibly use collision flags for object avoidance 
		controller.Move(direction * speed * Time.deltaTime);
	}
	
	public void AlterFromSwarm (bool search) {
		if (search) {
			searching = true;
			searchingTime = searchLength;
		}
		else playerFound = true;
	}

	public void TakeDamage (int d) {
		if (!damageSoundObj.audio.isPlaying) {
			int r = Random.Range(0,4);
			if (r==0) damageSoundObj.audio.clip = zombieDamageSound1;
			else if (r==1) damageSoundObj.audio.clip = zombieDamageSound2;
			else if (r==2) damageSoundObj.audio.clip = zombieDamageSound1;
			else damageSoundObj.audio.clip = zombieDamageSound2;
			
			damageSoundObj.audio.Play();
		}

		swarmController.AlertTheSwarm();
		health -= d;
	}
	
	public void GamePause() {
		gameObject.SetActive(false);
	}
	
	public void GameUnpause() {
		gameObject.SetActive(true);
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
