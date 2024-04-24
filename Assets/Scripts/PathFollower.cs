using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>Follows any given path, moving the object to do so.</summary>
public class PathFollower : MonoBehaviour
{
    [Tooltip("The speed at which the path will be followed, in units per second.")]
    public float speed = 5f;

    private Vector3[] path;
    private int pathIndex;


    /// <summary>Starts following the specified path.</summary>
    /// <param name="path">
    /// The path to follow.
    /// A path is a series of points in space that will be visited in order.
    /// </param>
    public void StartFollowingPath(IEnumerable<Vector3> path)
    {
        this.path = path.ToArray();
        pathIndex = 0;
    }

    /// <summary>Stops following the current path.</summary>
    public void StopFollowingPath()
    {
        path = null;
    }

    /// <summary>Determines if a path is currently being followed.</summary>
    /// <returns>
    /// <c>true</c> if a path is currently being followed;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool IsFollowingPath()
    {
        return path != null;
    }


    private void Update()
    {
        // Do nothing if there is no path to follow.
        if (path == null) return;
        Vector3 position = transform.position;
        Vector3 target = path[pathIndex];
        Vector3 toTarget = target - position;
        // Determine how many units should be moved this frame.
        float distanceToMove = speed * Time.deltaTime;
        float distanceToTarget = toTarget.magnitude;
        // While the target will be reached this frame...
        while (distanceToTarget <= distanceToMove)
        {
            // Move to the target.
            position = target;
            distanceToMove -= distanceToTarget;
            pathIndex++;
            // If the end of the path has been reached,
            // stop following the path.
            if (pathIndex >= path.Length)
            {
                path = null;
                break;
            }
            // Change target to the next point in the path.
            target = path[pathIndex];
            toTarget = target - position;
            distanceToTarget = toTarget.magnitude;
        }
        // If the end of the path has not been reached,
        // move towards the next target.
        if (path != null)
        {
            Vector3 directionToTarget = toTarget.normalized;
            position += distanceToMove * directionToTarget;
        }
        // Apply the changes to position.
        transform.position = position;
    }
}
