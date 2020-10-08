﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior_Patrol : EnemyBehavior
{
    public List<Waypoint> waypoints;

    public Waypoint currentWaypointToGo;
    int currentWaypoint = 0;
    NavMeshAgent agent;

    public override void Start() {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointToGo = waypoints[currentWaypoint];
        SetDestinationToWaypoint();
    }

    public override void Pause() {
        base.Pause();
        agent.isStopped = true;
    }

    public override void Resume() {
        base.Resume();
        agent.isStopped = false;
    }

    public override void ExecuteBehavior() {
        base.ExecuteBehavior();
        if(agent.remainingDistance <= 0.5f) {
            currentWaypoint++;
            
            currentWaypointToGo = waypoints[currentWaypoint % waypoints.Count];
            SetDestinationToWaypoint();
        }
    }

    void SetDestinationToWaypoint() {
        agent.SetDestination(currentWaypointToGo.position);
    }
}
