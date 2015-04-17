using UnityEngine;
using System.Collections;

public class codeCracking : MonoBehaviour {

	public string[] Questions;
	public string[] Answers;
	public GameObject exitDoor;
	//public GameObject zombieSpawnPoint;
	public GameObject player;
	public WeaponManager playerWeapManage;
	public SwarmAI spawnPoint;

	private string password="password";
	private bool showGUI = false;
	private bool bwelcome = false;
	private bool bpassword= false;
	private bool bforget = false;
	private bool bquestion=false;
	private bool bcracked=false;
	private bool bdestroy = false;
	private int    numOfCorrect=0;
	private int questionID=0;

	//private int countdown=0;
	//private float startTime=0.0f;
	// Use this for initialization
	void Start () {
		//zombieSpawnPoint.SetActive(false);
		player=GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E) && (showGUI || bwelcome))
		{
			playerWeapManage.enabled=false;
			bpassword=true;
			bwelcome =false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		showGUI = true;
		bwelcome=true;
		Screen.showCursor = true;

		enableDisableMouseLook(false);
	}
	void OnTriggerStay(Collider other)
	{

	}
	void OnTriggerExit(Collider other)
	{
		playerWeapManage.enabled=true;
		showGUI=false;
		bwelcome = false;
		bpassword= false;
		bforget = false;
		bquestion=false;
		bcracked=false;
		questionID=0;
		//bdestroy = false;
		Screen.showCursor = false;

		enableDisableMouseLook(true);
	}
	void enableDisableMouseLook(bool b)
	{
		Component[] cs = player.GetComponentsInChildren<MouseLook>(true);
		foreach (MouseLook c in cs) 
			c.enabled = b;
	}
	void OnGUI()
	{

		if (showGUI) {
			if(bwelcome)
			{
				string s= "Welcome to Last Man Base! Press 'E' to enter control system. ";
				int len = s.Length;
				GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-50,6*len+90,25),s);
			}

			if (bpassword)
			{
				password = GUI.PasswordField(new Rect(Screen.width/2-100,Screen.height/2-25, 100, 20), password, "*"[0], 25);
				if (password.Equals("cobra") || password.Equals("Cobra") || password.Equals("COBRA"))
				//if (word.Equals(password) )
				{
					bcracked=true;
					bpassword=false;
				}

				if(GUI.Button(new Rect(Screen.width/2,Screen.height/2-25, 150, 20), "Forgot your password?"))
				{bquestion=true; bpassword=false;}
			}

			if(bquestion)
			{
				if(questionID==0)
				{
					string s = "Please answer the security quesion to get your password back.\n Who is the most famous professor?";
					int len = s.Length;
					GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-80,6*len+90,50),s);
					
					string[] selStrings = new string[] {"Jame Davis", "Eric Fosler-Lussier", "Roger Crawfis", "Alan Price"};
					int selGridInt = -1;
					selGridInt = GUI.SelectionGrid(new Rect(Screen.width/2-100,Screen.height/2-20, 280, 50), selGridInt, selStrings, 2);
					if(selGridInt==2)
					{
						numOfCorrect++;
						questionID=1;					
					}
					else if(selGridInt==0||selGridInt==1||selGridInt==3){
						questionID=1;
					}
				}
				if(questionID==1)
				{
					string s = "Please answer the security quesion to get your password back.\n Who is the most humorous professor?";
					int len = s.Length;
					GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-80,6*len+90,50),s);
					
					string[] selStrings = new string[] {"Jame Davis", "Eric Fosler-Lussier", "Roger Crawfis", "Alan Price"};
					int selGridInt = -1;
					selGridInt = GUI.SelectionGrid(new Rect(Screen.width/2-100,Screen.height/2-20, 280, 50), selGridInt, selStrings, 2);
					if(selGridInt==2)
					{
						numOfCorrect++;
						questionID=2;					
					}
					else if(selGridInt==0||selGridInt==1||selGridInt==3){
						questionID=2;
					}
				}
				if(questionID==2)
				{
					string s = "Please answer the security quesion to get your password back.\n Who is the most critical professor?";
					int len = s.Length;
					GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-80,6*len+90,50),s);
					
					string[] selStrings = new string[] {"Jame Davis", "Eric Fosler-Lussier", "Roger Crawfis", "Alan Price"};
					int selGridInt = -1;
					selGridInt = GUI.SelectionGrid(new Rect(Screen.width/2-100,Screen.height/2-20, 280, 50), selGridInt, selStrings, 2);
					if(selGridInt==2)
					{
						numOfCorrect++;
						questionID=3;					
					}
					else if(selGridInt==0||selGridInt==1||selGridInt==3){
						questionID=3;
					}
				}
			}
			if(questionID==3)
			{
				if(numOfCorrect==3)
				{
					playerWeapManage.enabled=true;
					string s = "You know too much!"; //\n Zombies come! 
					int len = s.Length;
					GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-80,6*len+90,50),s);
					//spawnPoint.InitZombies();
					Screen.showCursor = false;
					numOfCorrect = 0;
					// active zombies spawn point
					//zombieSpawnPoint.SetActive(true);
				}
				else if(numOfCorrect==2)
				{
					bcracked=true; 
				}	
				else
				{
					playerWeapManage.enabled=true;
					string s = "You are idiot!"; //\n Zombies come! 
					int len = s.Length;
					GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-80,6*len+90,50),s);
					//spawnPoint.InitZombies();
					Screen.showCursor = false;
					numOfCorrect = 0;
					// active zombies spawn point
					// zombieSpawnPoint.SetActive(true);
				}
				bquestion=false;
				enableDisableMouseLook(true);
			}
			if(bcracked)
			{
				string s = "Welcome commander. What do you want to do?";
				int len = s.Length;
				GUI.TextField(new Rect(Screen.width/2-100,Screen.height/2-50,6*len+90,25),s);

				string[] selStrings = new string[] {"create zombies", "train zombies","release zombies", "destroy zombies"};
				int selGridInt = 0;
				selGridInt = GUI.SelectionGrid(new Rect(Screen.width/2-100,Screen.height/2-20, 240, 80), selGridInt, selStrings, 2);
				if(selGridInt==3)
				{
					bdestroy=true; bcracked = false;
					gameManager.TriggerLevelUnload();
					Application.LoadLevel("LoadingScreen");
				}					
				else{
				}
			}

		}
	}
}
