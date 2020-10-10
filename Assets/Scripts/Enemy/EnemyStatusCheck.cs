using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusCheck : MonoBehaviour
{
    protected EnemyController enemyController;

    public List<MonoBehaviour> onEnemyCheckMessageReceivers;
    public virtual void Start() {
        enemyController = GetComponent<EnemyController>();
    }

    public virtual void Update() {
        Check();
    }

    public virtual void Check() {
        Debug.Log("Checking");
    }

    public struct StatusCheckMessage
    {
        public string message;
        public bool isInLineOfSight;
        public Transform target;
    }
}
