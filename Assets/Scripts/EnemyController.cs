using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public GuardingMode guardMode;
    public CurrentBehavior currentBehavior;

    public List<Waypoint> waypoints;

    public UnityEvent OnWaypointReached;
    public UnityEvent OnPlayerSeen;
    public UnityEvent OnPlayerLostSeen;
    public UnityEvent OnLowHealth;



    public void GoToNextRandomWaypoint() {
        
    }

    public void GoToNextWaypoint() {

    }

    public void Attack() {

    }

    public void Cover() {
    
    }

}

public enum GuardingMode{
    Idle, Patrol, Wander
}

public enum CurrentBehavior
{
    Guarding, Attacking, ChasingPlayer, Covering
}


[System.Serializable]
public class Waypoint
{
    public Vector3 position;
    public float areaRadius = 1f;
    public List<int> connections;

    public void AddConnection(int connection) {
        connections.Add(connection);
    }
}