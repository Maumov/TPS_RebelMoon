using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    protected bool m_isPaused;

    public virtual void Awake() {
        m_isPaused = true;
    }

    public virtual void Start() {
    
    }

    public virtual void Update() {
        ExecuteBehavior();
    }


    public virtual void Pause() {
        m_isPaused = true;
    }

    public virtual void Resume() {
        m_isPaused = false;
    }
    public virtual void ExecuteBehavior() {
        Debug.Log("Executing behavior");
    }
}
