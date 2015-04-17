// file: Drone.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drone : MonoBehaviour
{
    public void SetParameters(float speed, float maxSpeed, float maxDirectionChange)
    {
        _speed = speed;
        _maxSpeed = maxSpeed;
        _maxDirectionChange = maxDirectionChange;
    }

    private float _speed = 5f;
    private float _maxSpeed = 10f;
    private float _maxDirectionChange = .05f;

    public void UpdateVelocity(Vector3 swarmCenterAverage, Vector3 swarmMovementAverage)
    {
        // calc velocity adjustment for swarm
        Vector3 moveTowardsSwarmCenter = VectorTowards(swarmCenterAverage);
        Vector3 adjustment = moveTowardsSwarmCenter + (2 * swarmMovementAverage);
        Vector3 swarmVelocityAdjustment = UsefulFunctions.ClampMagnitude(adjustment, _maxDirectionChange);

        // apply velocity adjustment to this drone
        rigidbody.velocity += (swarmVelocityAdjustment * _speed);
        rigidbody.velocity = UsefulFunctions.ClampMagnitude(rigidbody.velocity, _maxSpeed);
    }

    private Vector3 VectorTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        targetDirection.Normalize();
        targetDirection *= _maxSpeed;
        return (targetDirection - rigidbody.velocity);
    }
}