using UnityEngine;
using System.Collections;
using Pathfinding;

public class zombieAnimationController : MonoBehaviour {
	private Seeker seeker;

	//The calculated path
	public Path path;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	public float repathRate = 0.5f;
	private float lastRepath = -9999;
	
	public float speed = 1.0f;
	public float runSpeed = 2.5f;
	public float proximity = 20.0f;
	public float attackDistance = 2.4f;
	public Transform target;
		
	private int health =100;
	private bool dead = false;
	private bool canSearch = false;
	private bool canMove = false;
	private int old_normalizedTime=0;
	private bool once = true;
	private bool collided = false;
	
	private Animator anim;	
	private CharacterController controller;
	public float animSpeed = 1.0f;				// a public setting for overall animator animation speed
	
//	static int idleState = Animator.StringToHash("Base Layer.idle");	
//	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int attackState = Animator.StringToHash("Base Layer.attack");				// and are used to check state for various actions to occur
//	static int deathDownState = Animator.StringToHash("Base Layer.death");		// within our FixedUpdate() function below
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer

	public AudioClip zombieDamageSound1;
	public AudioClip zombieDamageSound2;
	public AudioClip zombieAlertSound1;
	public AudioClip zombieAttackSound1;
	private AudioSource audioAttack;
	private AudioSource audioDamage1;
	private AudioSource audioAlert;
	private AudioSource audioDamage2;
	
	// Use this for initialization
	void Start () {
		anim = this.transform.GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		seeker = GetComponent<Seeker>();

		gameObject.tag = "zombie";
		gameObject.layer=LayerMask.NameToLayer("Zombies_Layer");

		// add the necessary AudioSources:
		audioAttack = AddAudio(zombieAttackSound1, true, true, 0.2f);
		audioDamage1 = AddAudio(zombieDamageSound1, true, true, 0.4f);
		audioAlert = AddAudio(zombieAlertSound1, false, false, 0.8f);
		audioDamage2 = AddAudio(zombieDamageSound2, false, false, 0.8f);

		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;		
		gameManager.GameLevelRestart += GameLevelRestart;
		gameManager.GameLevelUnload += GameLevelUnload;
	}
	
	// Update is called once per frame
	void Update () {
		var distToTarget = Vector3.Distance(transform.position, target.position);
		//RotateTowards(target.transform.position - transform.position);
		//Debug.Log("distToTarget: "+distToTarget);
		
		anim.SetFloat("Dist", distToTarget);
		anim.SetInteger("Health", health);
		//anim.SetBool("canMove",canMove);
		anim.speed = animSpeed;		
		
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
						
		if(!dead)
		{						
			if (distToTarget>proximity)// too far from target, idle 
			{
				canSearch=false;	
				canMove = true;
				//audio.clip=idle Sound;

				RaycastHit hit;
				if(Physics.Raycast(transform.position,transform.forward, out hit, 400))
				{
					Debug.DrawLine(transform.position, hit.point, Color.red);
					Vector3 direction= transform.forward;
					direction = new Vector3(direction.x+Random.Range(-0.1f,0.1f), direction.y, direction.z);
					controller.SimpleMove(speed*direction); //*Time.deltaTime

					if(hit.transform!=transform && hit.distance<10.0f)
						if(transform.InverseTransformDirection (hit.point+hit.normal*10).x>0)
							transform.Rotate(0,3,0);
						else 
							transform.Rotate(0,-3,0);
				}
			}		
			else 
			{	
				RotateTowards(target.transform.position - transform.position);
				if(distToTarget<attackDistance ) // start attack 
				{					
					canSearch=false;
					canMove=false;
					//audio.clip = zombieAttackSound1;
					if(!audioAttack.isPlaying)
						audioAttack.Play();
					//Debug.Log("audio "+ audio.clip.name);

						
					if(currentBaseState.nameHash == attackState) // attack the player
					{
						if ( once && (currentBaseState.normalizedTime-(int)currentBaseState.normalizedTime)>0.5f)
						{
							target.SendMessage("addHealth",-10);
							once =false;							
						}
						if ((int)currentBaseState.normalizedTime>old_normalizedTime)	
						{
							old_normalizedTime=(int)currentBaseState.normalizedTime;
							once = true;
						}
					}
					else // after quit the attack state, reset the attack animation loop number
						old_normalizedTime=0; 									
					
		// a bug here is when target changes its position, but still within the attackDistance, npcPlayerAI doze not change its oritation.
				//RotateTowards(target.transform.position - transform.position);			
				}
				else// walk and run towards the target
				{
						canSearch=true;
						canMove=true;	
						if(!audioAlert.isPlaying)
							audioAlert.Play();						
					    //Debug.Log("audio "+ audio.clip.name);

				} 			
			}
		}		
		if(canSearch)
		{
			Vector3 dir = searchPath();
			if(canMove)
			{
				dir *= runSpeed;// * Time.deltaTime;
				if (collided)
					dir+=2*Vector3.Cross(dir, Vector3.up);

				//transform.Translate (dir);
				controller.SimpleMove (dir);
			}
		}
		else
		{
			currentWaypoint = 0;
		}
							
		if (health <= 0 )
		{
			dead = true;
			canMove = false;   
			canSearch = false;
			gameObject.tag = "Untagged";
		}	
	
		
	}
	public virtual Vector3 GetFeetPosition () {
		if (controller != null) {
			return transform.position - Vector3.up*controller.height*0.5F;
		}

		return transform.position;
	}
	
	void RotateTowards (Vector3 dir) 
	{
		Quaternion rot= transform.rotation;
		Quaternion toTarget = Quaternion.LookRotation (dir);
		
		float rotateSpeedStanding = 16.0f;
		rot = Quaternion.Slerp (rot,toTarget,rotateSpeedStanding*Time.fixedDeltaTime);
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		transform.rotation = rot;
	}

	public void OnPathComplete (Path p) {
		p.Claim (this);
		if (!p.error) {
			if (path != null) path.Release (this);
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		} else {
			p.Release (this);
			Debug.Log ("Oh noes, the target was not reachable: "+p.errorLog);
		}
		
		//seeker.StartPath (transform.position,targetPosition, OnPathComplete);
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
			Debug.Log ("End Of Path Reached");
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

	void OnControllerColliderHit(ControllerColliderHit c)
	{
		if (c.transform.tag=="zombie")
			collided = true;
	}

	public void TakeDamage (int d) {

	}
	AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)  {
		AudioSource newAudio = gameObject.AddComponent("AudioSource") as AudioSource;
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	// called by weapon.cs 
	public void HitByWeapon(int hurt)
	{
		health -= hurt;		
		//Debug.Log("zombie health: "+health);

		if(!audioDamage1.isPlaying)
			audioDamage1.Play();				
	}
	public void setTarget(Transform t)
	{
		target =t;
	}
	//public void ShowBlood(Vector3 p)
	//{
		
	//}
	
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
