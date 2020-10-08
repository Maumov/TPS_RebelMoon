using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyCheck_PlayerInSight : EnemyStatusCheck
{
    SphereCollider rangeTrigger;
    public float angleOfLineOfSight;
    public float RangeOfSight;

    PlayerController player;

    public bool IsInSight;

    public override void Start() {
        base.Start();
        rangeTrigger = GetComponent<SphereCollider>();
        rangeTrigger.radius = RangeOfSight;
    }


    public override void Check() {
        if(player != null) {
            IsInSight = Vector3.Angle(transform.forward, player.transform.position - transform.position) < angleOfLineOfSight;
            SendMessageToAllMessageReceivers(IsInSight, player.transform);
        }
    }

    public virtual void OnTriggerEnter(Collider other) {
        PlayerController pl = other.GetComponent<PlayerController>();
        if(pl != null) {
            player = pl;
        }
    }
    public virtual void OnTriggerExit(Collider other) {
        PlayerController pl = other.GetComponent<PlayerController>();
        if(pl != null) {
            player = null;
            SendMessageToAllMessageReceivers(false, null);
        }
    }

    void SendMessageToAllMessageReceivers(bool isInLineOfSight, Transform target) {
        StatusCheckMessage data = new StatusCheckMessage();
        var messageType = MessageType.SIGHTED;
        data.message = "Player In Sight";
        //needs to check with raycast for a clear view to attack.
        data.isInLineOfSight = isInLineOfSight;
        data.target = target;
        for(var i = 0; i < onEnemyCheckMessageReceivers.Count; ++i) {
            var receiver = onEnemyCheckMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }
}
