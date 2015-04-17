using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            Application.LoadLevel("LoadingScreen");
        }
    }
}
