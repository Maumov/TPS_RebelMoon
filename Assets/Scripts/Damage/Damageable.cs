using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Message;

public class Damageable : MonoBehaviour
{
    public float maxHitPoints = 100f;

    public float currentHitPoints;

    public UnityEvent OnDeath, OnReceiveDamage;

    public List<MonoBehaviour> onDamageMessageReceivers;

    private void Start() {
        ResetDamage();
    }

    public void ResetDamage() {
        currentHitPoints = maxHitPoints;
    }

    public void ApplyDamage(DamageMessage data) {
        if(currentHitPoints <= 0f) {
            return;
        }

        currentHitPoints -= data.amount;

        if(currentHitPoints <= 0f) {
            OnDeath.Invoke();
        } else {
            OnReceiveDamage.Invoke();
        }
        var messageType = currentHitPoints <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

        for(var i = 0; i < onDamageMessageReceivers.Count; ++i) {
            var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public struct DamageMessage
    {
        public MonoBehaviour damager;
        public float amount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;

        public bool stopCamera;
    }
}
