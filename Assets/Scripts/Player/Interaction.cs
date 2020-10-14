using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public List<MonoBehaviour> onUseMessageReceivers;

    public LayerMask interactionLayerMask;

    Ray ray;
    RaycastHit hit;
    public void Execute() {
        Debug.Log("Executed");
        ray.origin = transform.position + new Vector3(0f,1f,0f);
        ray.direction = transform.forward;
        if(Physics.Raycast(ray, out hit,1f,interactionLayerMask)) {
            hit.collider.GetComponent<IInteractable>().OnInteract(GetComponent<Gear>());
            InteractionMessage data;
            data.someValue = 0f;
            var messageType = MessageType.INTERACT;
            for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
                var receiver = onUseMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }

        }
    }
    public struct InteractionMessage
    {
        public float someValue;
    }
}
