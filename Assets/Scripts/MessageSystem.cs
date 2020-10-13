using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Message
{
    public enum MessageType
    {
        DAMAGED,
        DEAD,
        RESPAWN,
        //Add your user defined message type after
        FIRE,
        RELOAD,
        EQUIP,
        SIGHTED,
        INTERACT,
        WALK,
        IDLE,
    }

    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type, object sender, object msg);
        
    }
}
