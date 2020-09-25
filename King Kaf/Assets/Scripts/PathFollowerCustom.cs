using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

/// <summary>
/// Player movement system. Handles the autoMove and Rotate through pathCreators.
/// </summary>
public class PathFollowerCustom : Manager
{

    public PathCreator currentPathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    public bool followRotation;
    public bool followPath;
    public Vector3 positionOffet;

    public float moveSpeed = 1;

    [Range(0, 1)]
    public float progress;

    [Space(1)]
    public UnityEvent OnPathEnd;

    /// <summary>
    /// Distance in which the Player will snap to the path, otherwise, it will look at the first point and try to approach it.
    /// </summary>
    public float snapPathThreshhold = 1;


    private void Awake()
    {
        //when path end, search for new path
        OnPathEnd.AddListener(UpdatePath);
    }

    void Start()
    {
        //Search first path
        if (currentPathCreator == null) SetPathCreator(GetClosestPath());
        else SetPathCreator(currentPathCreator);
    }

    void Init()
    {

    }

    void FixedUpdate()
    {

        if (followPath && currentPathCreator != null)
        {
            FollowPath();

            if (followRotation) transform.rotation = currentPathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            


            if (progress >= 1)
            {
                OnPathEnd.Invoke();
            }
        }

    }

    private void FollowPath()
    {
        distanceTravelled += speed * Time.deltaTime;
        progress = distanceTravelled / currentPathCreator.path.length;

        transform.position = currentPathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) + positionOffet;
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path


    public void UpdatePath()
    {
        Debug.Log("Searching New Path");
        PathCreator closestPath = GetClosestPath();

        SetPathCreator(closestPath);


    }

    #region internal methods

    void OnPathChanged()
    {
        distanceTravelled = currentPathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    PathCreator GetClosestPath()
    {
        PathCreator[] paths = FindObjectsOfType<PathCreator>();
        PathCreator bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (PathCreator potentialTarget in paths)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    void SetPathCreator(PathCreator newPath)
    {
        currentPathCreator = newPath;
        //unsub former path
        currentPathCreator.pathUpdated -= OnPathChanged;
       
        distanceTravelled = 0;
        // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
        currentPathCreator.pathUpdated += OnPathChanged;
    }

    #endregion
}
