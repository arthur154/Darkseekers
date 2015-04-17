// file: MoveTowardsWaypoint.cs
using UnityEngine;
using System.Collections;

public class MoveTowardsWaypoint : MonoBehaviour
{
    public const float ARRIVE_DISTANCE = 3f;
    public float speed = 5.0F;
    private GameObject targetGO;
    private WaypointManager waypointManager;

    private void Awake()
    {
        waypointManager = GetComponent<WaypointManager>();
        targetGO = waypointManager.NextWaypoint(null);
    }

    private void Update()
    {
        transform.LookAt(targetGO.transform);
        float distance = speed * Time.deltaTime;
        Vector3 source = transform.position;
        Vector3 target = targetGO.transform.position;
        transform.position = Vector3.MoveTowards(source, target, distance);

        if (Vector3.Distance(source, target) < ARRIVE_DISTANCE)
            targetGO = waypointManager.NextWaypoint(targetGO);
    }
}