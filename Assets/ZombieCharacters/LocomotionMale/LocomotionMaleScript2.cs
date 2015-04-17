using UnityEngine;
using System.Collections;

public class LocomotionMaleScript2 : MonoBehaviour {
	
	public float speed = 1.0f;
	public float runSpeed = 2.0f;
	public float proximity = 20.0f;
	public float attackDistance = 2.0f;
	public Transform target;
		
	private int health =100;
	private bool dead = false;
	private bool canSearch = false;
	private bool canMove = false;
	private int old_normalizedTime=0;
	private bool once = true;
	
	private Animator anim;	
	private CharacterController controller;
	public float animSpeed = 1.0f;				// a public setting for overall animator animation speed
	
//	static int idleState = Animator.StringToHash("Base Layer.idle");	
//	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int attackState = Animator.StringToHash("Base Layer.attack");				// and are used to check state for various actions to occur
//	static int deathDownState = Animator.StringToHash("Base Layer.death");		// within our FixedUpdate() function below
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	
	// Use this for initialization
	void Start () {
		anim = this.transform.GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		gameObject.tag = "zombie";
		gameObject.layer=LayerMask.NameToLayer("Zombies_Layer");
		
		gameManager.GamePause += GamePause;
		gameManager.GameUnpause += GameUnpause;		
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
			if (distToTarget>proximity)
			{
				if (canSearch)
				{
					canSearch=false;
					SendMessage("setCanSearch", false);
					SendMessage("setCanMove",false);
				}				
			}		
			else 
			{	
				RotateTowards(target.transform.position - transform.position);
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
						
					if(currentBaseState.nameHash == attackState) // attack the player
					{
						if ( once && (currentBaseState.normalizedTime-(int)currentBaseState.normalizedTime)>0.5f)
						{
							target.SendMessage("CaughtByZombie",10);
							once =false;							
						}
						if ((int)currentBaseState.normalizedTime>old_normalizedTime)	
						{
							old_normalizedTime=(int)currentBaseState.normalizedTime;
							once = true;
						}
					}
					else
						old_normalizedTime=0; // reset the number						
							
					
		// a bug here is when target changes its position, but still within the attackDistance, npcPlayerAI doze not change its oritation.
				//RotateTowards(target.transform.position - transform.position);
			
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
				} 			
			}
		}		
					
		if (health <= 0 )
		{
			dead = true;
			canMove = false;   SendMessage("setCanMove",false);
			canSearch = false; SendMessage("setCanSearch", false);
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

	/*
	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag=="zombie")
		{
			Debug.Log(gameObject.name + "colide enter "+ collision.transform.name);
			collision.transform.SendMessage("stopOnTargetReached");
		}
			
    }
	void OnCollisionExit(Collision collision)
	{
		if (collision.transform.tag=="zombie")
		{
			Debug.Log(gameObject.name + "colide exit "+ collision.transform.name);
			collision.transform.SendMessage("setCanMove",true);
			collision.transform.SendMessage("setCanSearch", true);
		}
	}
	void OnTriggerEnter(Collider c)
	{
		if(c.transform.tag=="zombie")
		{
			Debug.Log(gameObject.name + "trigger enter "+ c.transform.name);
			//c.transform.SendMessage("stopOnTargetReached");
			c.transform.SendMessage("setCanMove",false);
			c.transform.SendMessage("setCanSearch", false);
		}
	}
	void OnTriggerStay(Collider c)
	{
		if(c.transform.tag=="zombie")
		{
			Debug.Log(gameObject.name + "trigger stay "+ c.transform.name);
			//c.transform.SendMessage("stopOnTargetReached");
			c.transform.SendMessage("setCanMove",false);
			c.transform.SendMessage("setCanSearch", false);
		}
	}
	void OnTriggerExit(Collider c)
	{
		if (c.transform.tag=="zombie")
		{
			Debug.Log(gameObject.name + "trigger exit "+ c.transform.name);
			c.transform.SendMessage("setCanMove",true);
			c.transform.SendMessage("setCanSearch", true);
		}
	}
	*/
	
	// called by AIpath.cs 
	public void  stopOnTargetReached()
	{
		canMove = false;
	}
	
	// called by weapon.cs 
	public void HitByWeapon(int hurt)
	{
		health -= hurt;		
		//Debug.Log("zombie health: "+health);
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
}
