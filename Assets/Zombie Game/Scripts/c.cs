using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	public AnimationClip idleAnimation;
	public AnimationClip walkAnimatioin;
	public AnimationClip runAnimation;
	public AnimationClip attackAnimation;
	public AnimationClip deathAnimation;
	public AudioClip idleAudio;
	public AudioClip walkAudio;
	public AudioClip runAudio;
	public AudioClip attackAudio;
	public AudioClip deathAudio;
	
	public float speed = 1.0f;
	public float runSpeed = 2.0f;
	public float proximity = 30.0f;
	public float attackDistance = 1.0f;
	public Transform target;
		
	private int health =100;
	private bool dead = false;
	//private Vector3 moveDirection = Vector3.zero;
	private bool canSearch = false;
	private bool canMove = false;
	
	private Animation _animation;
	//private CharacterController _controller;
	
	
	void Awake()
	{
		// set values to these variables so we don't have to use GetComponent over and over
		_animation = (Animation)GetComponent(typeof(Animation));
		//_controller = (CharacterController)GetComponent(typeof(CharacterController));
		// make sure of these settings
		gameObject.tag = "zombie";
	}
	
	// Use this for initialization
	void Start () {		
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;	
	}
	
	// Update is called once per frame
	void Update () {
		
		var distToTarget = Vector3.Distance(transform.position, target.position);
		
		if(!dead)
		{
			if (distToTarget>proximity)
			{
				if (canSearch)
				{
					canSearch=false;
					SendMessage("setCanSearch", false);
				}
				// canMove == true;
				if(canMove)
				{
					if (!animation.IsPlaying(runAnimation.name))
					{				
						_animation.CrossFade(runAnimation.name, 0.2f);	
					}	
				}
				else
				{
					if (!animation.IsPlaying(idleAnimation.name))
						_animation.CrossFade(idleAnimation.name, 0.2f);
				}
				
			}		
			else 
			{
				if(distToTarget<attackDistance ) // start attack 
				{
					if (canSearch)
					{
						canSearch=false;
						SendMessage("setCanSearch", false);
					}
					if(canMove)
					{
						canMove=false;
						SendMessage("setCanMove",false);
					}
						
					if(!animation.IsPlaying(attackAnimation.name))
					{
						_animation.CrossFade(attackAnimation.name, 0.2f);	
						target.SendMessage("CaughtByZombie",10);
						
					}
		// a bug here is when target changes its position, but still within the attackDistance, npcPlayerAI doze not change its oritation.
		//			RotateTowards(target.transform.position - transform.position);
			
				}
				else// walk and run towards the target
				{
					if (!canSearch)
					{
						canSearch=true;
						SendMessage("setCanSearch", true);
					}
					if (!canMove)
					{
						canMove=true;
						SendMessage("setCanMove",true);
					}				
						
					if (!animation.IsPlaying(runAnimation.name))
					{				
						_animation.CrossFade(runAnimation.name, 0.2f);	
					}				
				} 			
			}
		}		
		
		if (health <= 0 )
		{
			dead = true;
			canMove = false;  SendMessage("setCanMove",false);
			canSearch = false; SendMessage("setCanSearch", false);
			bool playingDeathAnim  = false;
			gameObject.tag = "Untagged";
			if (deathAnimation) playingDeathAnim = true;
			if (playingDeathAnim) {
				_animation.CrossFade(deathAnimation.name, 0.2f);
				if (animation[deathAnimation.name].time > 7.0f) playingDeathAnim = false;			
			}		
		}	
	
	}
	
	// when zombies hit players
	//void OnControllerColliderHit(ControllerColliderHit hit)
	//{
		// print("I hit " + hit.transform)
	//	if(hit.transform.tag == "Player")
	//	{
	//		hit.transform.SendMessage("");
	//		
	//	}
		
	
	//}
	
	void RotateTowards (Vector3 dir) 
	{
		Quaternion rot= transform.rotation;
		Quaternion toTarget = Quaternion.LookRotation (dir);
		
		float rotateSpeedStanding = 2.0f;
		rot = Quaternion.Slerp (rot,toTarget,rotateSpeedStanding*Time.fixedDeltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		transform.rotation = rot;
	}
	
	// called by AIpath.cs 
	void  stopOnTargetReached()
	{
		canMove = false;
	}
	
	// called by weapon.cs 
	void HitByWeapon(int hurt)
	{
		health -= hurt;		
		Debug.Log("zombie health: "+health);
	}
	void ShowBlood(Vector3 p)
	{
		
	}
	
	public void GamePause() {
		gameObject.SetActive(false);
	}
	
	public void GameUnpause() {
		gameObject.SetActive(true);
	}
	
}
