using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck_PlayerInSight : EnemyStatusCheck
{
    EnemyDetectionRange rangeTrigger;
    public float angleOfLineOfSight;
    public float RangeOfSight;

    PlayerController player;

    public bool IsInSight;

    public override void Start() {
        base.Start();
        rangeTrigger = GetComponentInChildren<EnemyDetectionRange>();
        rangeTrigger.GetComponent<SphereCollider>().radius = RangeOfSight;
    }


    public override void Check() {
        if(player != null) {
            
            if(IsInSight != Vector3.Angle(transform.forward, player.transform.position - transform.position) < angleOfLineOfSight) {
                IsInSight = !IsInSight;
                SendMessageToAllMessageReceivers(IsInSight, player.transform);
            }
        }
    }

    public void RemoveTarget() {
        player = null;
        if(IsInSight) {
            SendMessageToAllMessageReceivers(false, null);
            IsInSight = false;
        }
    }

    public void SetTarget(PlayerController p) {
        player = p;
    }
    

    void SendMessageToAllMessageReceivers(bool isInLineOfSight, Transform target) {
        StatusCheckMessage data = new StatusCheckMessage();
        var messageType = MessageType.SIGHTED;
        data.message = "Player In Sight";
        //needs to check with raycast for a clear view.
        data.isInLineOfSight = isInLineOfSight;
        data.target = target;
        for(var i = 0; i < onEnemyCheckMessageReceivers.Count; ++i) {
            var receiver = onEnemyCheckMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }
}
