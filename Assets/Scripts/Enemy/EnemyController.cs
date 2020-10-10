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
    public virtual void Start() {
        OnEnemyStart.Invoke();
    }
    public virtual void Update() {

    }
    public void OnReceiveMessage(MessageType type, object sender, object msg) {

        switch(type) {
            case MessageType.DAMAGED:
            break;
            case MessageType.DEAD:
            break;
            case MessageType.RESPAWN:
            break;
            case MessageType.FIRE:
            break;
            case MessageType.RELOAD:
            break;
            case MessageType.EQUIP:
            break;
            case MessageType.SIGHTED:
            StatusCheckMessage data = (StatusCheckMessage)msg;
            Debug.Log(data.isInLineOfSight);
            if(data.isInLineOfSight) {
                Debug.Log(data.target.name);
                currentTarget = data.target;
                OnEnemySighted.Invoke();
            } else {
                currentTarget = null;
                OnEnemyLostSight.Invoke();
            }
            break;
            default:
            Debug.Log("Whatever");
            break;
        }
    }

    private void OnEnable() {
        EnemyStatusCheck[] enemyChecks = GetComponents<EnemyStatusCheck>();
        foreach(EnemyStatusCheck e in enemyChecks) {
            e.onEnemyCheckMessageReceivers.Add(this);
        }
    }
    private void OnDisable() {
        EnemyStatusCheck[] enemyChecks = GetComponents<EnemyStatusCheck>();
        foreach(EnemyStatusCheck e in enemyChecks) {
            e.onEnemyCheckMessageReceivers.Remove(this);
        }
    }
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