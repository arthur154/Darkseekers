// file: SeekTarget.cs
using UnityEngine;
using System.Collections;

public class SeekTarget : MonoBehaviour
{
    public GameObject playerGO;
    public const float MAX_MOVE_DISTANCE = 50.0f;

    private void FixedUpdate()
    {
        float moveDistance = MAX_MOVE_DISTANCE * Time.deltaTime;
        Vector3 source = transform.position;
        Vector3 target = playerGO.transform.position;

        Vector3 seekVelocity = Seek(source, target, moveDistance);
        seekVelocity = UsefulFunctions.ClampMagnitude(seekVelocity, moveDistance);
        rigidbody.AddForce(seekVelocity, ForceMode.VelocityChange);
        UsefulFunctions.DebugRay(transform.position, seekVelocity, Color.blue);
        UsefulFunctions.DebugRay(transform.position, rigidbody.velocity, Color.yellow);
    }

    private Vector3 Seek(Vector3 source, Vector3 target, float moveDistance)
    {
        Vector3 directionToTarget = Vector3.Normalize(target - source);
        Vector3 velocityToTarget = moveDistance * directionToTarget;
        transform.LookAt(playerGO.transform);

        return velocityToTarget - rigidbody.velocity;
    }
}