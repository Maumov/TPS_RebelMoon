using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRange : MonoBehaviour
{
    EnemyCheck_PlayerInSight playerInSight;
    private void OnEnable() {
        playerInSight = GetComponentInParent<EnemyCheck_PlayerInSight>();
    }


    public virtual void OnTriggerEnter(Collider other) {
        
        PlayerController pl = other.GetComponent<PlayerController>();
        if(pl != null) {
            playerInSight.SetTarget(pl);
        }
    }

    public virtual void OnTriggerExit(Collider other) {
        PlayerController pl = other.GetComponent<PlayerController>();
        if(pl != null) {
            playerInSight.RemoveTarget();
        }
    }
}
