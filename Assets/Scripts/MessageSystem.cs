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
        FIRE,
        RELOAD,
        EQUIP,
        //Add your user defined message type after
    }

    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type, object sender, object msg);
        
    }
}
