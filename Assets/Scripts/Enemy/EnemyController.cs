using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Message;
using static EnemyStatusCheck;

[RequireComponent(typeof(Damageable))]
public class EnemyController : MonoBehaviour, IMessageReceiver
{
    
    public Transform currentTarget;

    public UnityEvent OnEnemyStart;
    public UnityEvent OnEnemySighted;
    public UnityEvent OnEnemyLostSight;

    public UnityEvent OnEnemyHit;
    public UnityEvent OnEnemyDead;

    public List<MonoBehaviour> onMessageReceivers;

    EnemyBehavior[] m_EnemyBehaviors;
    Damageable m_Damageable;

    public virtual void Start() {
        OnEnemyStart.Invoke();
    }
    public virtual void Update() {

    }
    public void OnReceiveMessage(MessageType type, object sender, object msg) {

        switch(type) {
            case MessageType.DAMAGED:
                OnEnemyHit.Invoke();
                SendMessage(type, msg);
            break;
            case MessageType.DEAD:
                OnEnemyDead.Invoke();
                SendMessage(type, msg);
            break;
            case MessageType.RESPAWN:
                SendMessage(type, msg);
            break;
            case MessageType.FIRE:
                SendMessage(type, msg);
            break;
            case MessageType.RELOAD:
                SendMessage(type, msg);
            break;
            case MessageType.EQUIP:
                SendMessage(type, msg);
            break;
            case MessageType.SIGHTED:
                StatusCheckMessage data = (StatusCheckMessage)msg;
                if(data.isInLineOfSight) {
           
                    currentTarget = data.target;
                    OnEnemySighted.Invoke();
                } else {
                    currentTarget = null;
                    OnEnemyLostSight.Invoke();
                }
            break;

            case MessageType.INTERACT:
                SendMessage(type, msg);
            break;

            case MessageType.WALK:
                SendMessage(type, msg);
            break;

            case MessageType.IDLE:
                SendMessage(type, msg);
            break;

            case MessageType.AIM:
                SendMessage(type, msg);
            break;

            case MessageType.ATTACKING:
                SendMessage(type, msg);
            break;

            default:
            Debug.Log("Whatever");
            break;
        }
    }

    void SendMessage(MessageType messageType, object data) {
        for(var i = 0; i < onMessageReceivers.Count; ++i) {
            var receiver = onMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    private void OnEnable() {
        EnemyStatusCheck[] enemyChecks = GetComponents<EnemyStatusCheck>();
        foreach(EnemyStatusCheck e in enemyChecks) {
            e.onEnemyCheckMessageReceivers.Add(this);
        }
        GetComponent<Damageable>().onDamageMessageReceivers.Add(this);

        m_EnemyBehaviors = GetComponents<EnemyBehavior>();
        foreach(EnemyBehavior eb in m_EnemyBehaviors) {
            eb.onUseMessageReceivers.Add(this);
        }
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);

    }
    private void OnDisable() {
        EnemyStatusCheck[] enemyChecks = GetComponents<EnemyStatusCheck>();
        foreach(EnemyStatusCheck e in enemyChecks) {
            e.onEnemyCheckMessageReceivers.Remove(this);
        }
        GetComponent<Damageable>().onDamageMessageReceivers.Remove(this);

        foreach(EnemyBehavior eb in m_EnemyBehaviors) {
            eb.onUseMessageReceivers.Remove(this);
        }
        m_Damageable.onDamageMessageReceivers.Remove(this);

    }
}

[System.Serializable]
public class Waypoint
{
    public Vector3 position;
    public float areaRadius = 1f;
    public List<int> connections;
    public float timeInWaypoint = 2f;
    public void AddConnection(int connection) {
        connections.Add(connection);
    }
}