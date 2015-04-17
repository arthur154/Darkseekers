using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectiveList : MonoBehaviour {

    bool showGUI = false;
    int batteryCount = 0;
    //ObjectInteract itemRef = new ObjectInteract();

    List<string> objectiveInstructions = new List<string>()
    {
       "Zombies are roaming throughout the city,\nlocate an AK-47!",
       "You're flashlight battery life doesn't last forever,\nfind three batteries to recharge your flashlight"
    };
    int objectiveIndex = 0;
      
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "AK-47")
        {
            objectiveIndex++;
            //objectiveInstruction = "";
            //showGUI = true;
        }
        else if (collider.name == "Battery")
        {
            batteryCount++;
            if (batteryCount >= 3)
            {
                objectiveIndex++;
                //objectiveInstruction = "";
                //showGUI = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.name == "AK-47")
        {
            //objectiveInstruction = "Excellent! Move on to your next objective!";
            showGUI = false;
        }
        else if (collider.name == "Battery")
        {
            batteryCount++;
            if (batteryCount >= 3)
            {
                showGUI = false;                
            }
        }
    }

    void OnGUI()
    {
        if (!showGUI)
        {
            GUI.Box(new Rect(Screen.width / 2, 0, Screen.width/2, Screen.height/4), objectiveInstructions[objectiveIndex]);
        }
        else
        {
            GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), objectiveInstructions[objectiveIndex]);
        }
    }
}
