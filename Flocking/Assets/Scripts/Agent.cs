using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    protected Vector3 velocity, acceleration;
    [SerializeField]
    protected float moveSpeed, visionRange, radius;
    
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    /// <summary>
    /// Returns to direction to apply force in order to move towards a point
    /// </summary>
    protected Vector3 Seek(Vector3 target)
    {
        Vector3 targetVelocity = (target - transform.position).normalized * moveSpeed;

        return (targetVelocity - velocity).normalized;
    }

    /// <summary>
    /// Returns the direction to apply force in order to flee a point weighted by the distance from the point
    /// </summary>
    protected Vector3 Flee(Vector3 target)
    {
        Vector3 targetVelocity = (transform.position - target).normalized;
        return targetVelocity;
    }

    protected virtual void Update()
    {
        velocity += acceleration;

        velocity += AvoidObstacle(new Vector2(Flock.SeekPoint.x - transform.position.x, Flock.SeekPoint.z - transform.position.z).normalized);

        transform.rotation = Quaternion.Euler(Mathf.Asin(velocity.y / velocity.magnitude) * Mathf.Rad2Deg * -1, Mathf.Atan2(velocity.z * -1, velocity.x) * Mathf.Rad2Deg + 90, 0);
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// Returns the amount to alter velocity in order to avoid running into any obstacles
    /// </summary>
    protected Vector3 AvoidObstacle(Vector2 targetDirection)
    {
        Vector2 position2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 forward2D = targetDirection.normalized;
        Vector2 right2D = new Vector2(targetDirection.y, -1 * targetDirection.x).normalized;
        Vector2 direction = Vector2.zero;
        List<Obstacle> nearbyObstacles = ObstacleManager.GetNearbyObstacles(position2D, visionRange);
        Obstacle nearestObstacle = null;

        foreach (Obstacle obstacle in nearbyObstacles)
        {
            direction = obstacle.Position - position2D;
            float forwardDistance = Vector2.Dot(forward2D, direction) / Vector2.Dot(forward2D, forward2D);
            
            if(forwardDistance > 0)
            {
                float sideDistance = Vector2.Dot(right2D, direction) / Vector2.Dot(right2D, right2D);

                if(Mathf.Abs(sideDistance) < obstacle.Radius + radius)
                {
                    if(nearestObstacle == null)
                    {
                        nearestObstacle = obstacle;
                    }
                    else
                    {
                        if(direction.sqrMagnitude < (nearestObstacle.Position - position2D).sqrMagnitude)
                        {
                            nearestObstacle = obstacle;
                        }
                    }
                }
            }
        }

        if(nearestObstacle == null)
        {
            return Vector3.zero;
        }

        direction = nearestObstacle.Position - position2D;

        Vector2 seekPosition = Vector2.zero;
        Vector2 rightProjection = right2D * (Vector2.Dot(right2D, direction) / Vector2.Dot(right2D, right2D));
        Vector2 forwardProjection = forward2D * (Vector2.Dot(forward2D, direction) / Vector2.Dot(forward2D, forward2D));

        seekPosition = forwardProjection + (rightProjection - rightProjection.normalized * (nearestObstacle.Radius + radius));
        
        return Seek(new Vector3(seekPosition.x, transform.position.y, seekPosition.y));
    }
}
