// file: LookAt.cs
using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform playerTransform;

    private void LateUpdate()
    {
        transform.LookAt(playerTransform);
        UsefulFunctions.DebugRay(transform.position, transform.forward * 100, Color.green);
    }
}