using Message;
using System.Collections;
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
    public override void Awake() {
        agent = GetComponent<NavMeshAgent>();
        base.Awake();
    }

    public override void Start() {
        
    }
    
    public override void Pause() {
        base.Pause();
        agent.isStopped = true;
    }

    public override void Resume() {
        base.Resume();
        currentWaypointToGo = waypoints[currentWaypoint];
        StartCoroutine(DelayBetweenWaypoints());
        agent.isStopped = false;
    }

    public override void ExecuteBehavior() {
        if(m_isPaused) {
            return;
        }
        base.ExecuteBehavior();
        if(agent.remainingDistance <= 0.5f) {
            currentWaypoint++;
            currentWaypoint = currentWaypoint % waypoints.Count;
            currentWaypointToGo = waypoints[currentWaypoint];
            StartCoroutine(DelayBetweenWaypoints());
            //SetDestinationToWaypoint();
        }
        EnemyMoveMessage data;
        data.velocity = agent.velocity.magnitude > 0.25f? 1f : 0f;
        var messageType = MessageType.WALK;
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    void SetDestinationToWaypoint() {
        agent.SetDestination(currentWaypointToGo.position);
    }

    public float timeInWaypoint = 2f;
    IEnumerator DelayBetweenWaypoints() {
        yield return new WaitForSeconds(timeInWaypoint);
        SetDestinationToWaypoint();
    }

    public struct EnemyMoveMessage
    {
        public float velocity;
    }
}
