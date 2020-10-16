using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    public LayerMask layerMask;
    Ray ray;
    RaycastHit hit;
    public List<MonoBehaviour> onUseMessageReceivers;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    
    //(m_cam.transform, m_Input.AttackDown, m_Input.AttackUp);
    public void Attack(Transform viewCamera, bool buttonUp) {
        ray = new Ray(viewCamera.position, viewCamera.forward);
        hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, 100f, layerMask)) {
            Debug.Log(hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if(rb) {
                Debug.Log(rb.name);
                //Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = ray.direction * 100f;
                rb.AddForce(force, ForceMode.Force);

                ForceMessage data = new ForceMessage();
                data.usingForce = !buttonUp;
                SendMessage(MessageType.FORCE, data);
            }
        }
    }

    void SendMessage(MessageType messageType, ForceMessage data) {
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public struct ForceMessage{
        public float someValue;
        public bool usingForce;
    }
}
