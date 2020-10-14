using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public List<MonoBehaviour> onObjectiveMessageReceivers;

    public void ObjectiveDestroyed() {
        ObjectiveMessage d = new ObjectiveMessage();
        d.someValue = 0f;
        SendMessage(MessageType.DEAD, d);
    }
    void SendMessage(MessageType messageType, ObjectiveMessage data) {
        for(var i = 0; i < onObjectiveMessageReceivers.Count; ++i) {
            var receiver = onObjectiveMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public struct ObjectiveMessage
    {
        public float someValue;
    }
}
