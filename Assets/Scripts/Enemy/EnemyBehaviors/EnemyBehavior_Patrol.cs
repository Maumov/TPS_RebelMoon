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

    public float patrolSpeed;
    public float patrolAngularSpeed;

    int currentWaypoint = 0;
    NavMeshAgent agent;
    Rigidbody rb;
    public override void Awake() {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //agent.updatePosition = false;
        //agent.updateRotation = false;
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

    bool arrived;
    public override void ExecuteBehavior() {
        if(m_isPaused) {
            return;
        }
        base.ExecuteBehavior();
        if(agent.remainingDistance <= 0.5f && !arrived) {
            Debug.Log("arrived");
            arrived = true;
            StartCoroutine(DelayBetweenWaypoints());
        } else {
            if(agent.remainingDistance > 0.5f) {
                Debug.Log("moving");
                rb.MovePosition(transform.position + (transform.forward * patrolSpeed * Time.deltaTime));
                rb.position = transform.position;
                //Quaternion lookTowards = Quaternion.LookRotation(agent.nextPosition - transform.position, Vector3.up);
                //Quaternion rot = Quaternion.RotateTowards(transform.rotation, lookTowards, patrolAngularSpeed * Time.deltaTime);
                //rb.MoveRotation(rot);
                rb.rotation = transform.rotation;
            } else {
                Debug.Log("Nothing");
            }
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
        arrived = false;
    }

    //public float timeInWaypoint = 2f;
    IEnumerator DelayBetweenWaypoints() {
        yield return new WaitForSeconds(waypoints[currentWaypoint].timeInWaypoint);
        currentWaypoint++;
        currentWaypoint = currentWaypoint % waypoints.Count;
        currentWaypointToGo = waypoints[currentWaypoint];
        SetDestinationToWaypoint();
    }

    public struct EnemyMoveMessage
    {
        public float velocity;
    }
}
