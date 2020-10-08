using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public virtual void Update() {
        ExecuteBehavior();
    }


    public virtual void Pause() {
    
    }

    public virtual void Resume() {

    }
    public virtual void ExecuteBehavior() {
        Debug.Log("Executing behavior");
    }
}
