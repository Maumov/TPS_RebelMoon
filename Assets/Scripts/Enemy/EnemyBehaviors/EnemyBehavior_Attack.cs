using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior_Attack : EnemyBehavior
{
    public float timeBetweenAttacks;
    float nextAttack;
    EnemyController enemyController;
    Transform _target;
    NavMeshAgent agent;
    public override void Start() {
        base.Start();
        enemyController = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
    }

    public Transform Target {
        get {
            return _target;
        }
        set {
            _target = value;
        }
    }

    public override void Pause() {
        base.Pause();
    }

    public override void Resume() {
        _target = enemyController.currentTarget;
        base.Resume();        
    }

    public override void ExecuteBehavior() {
        if(m_isPaused) {
            return;
        }
        base.ExecuteBehavior();
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
        if(Time.time > nextAttack) {
            Attack();
        }    
    }

    void Attack() {
        Debug.Log("Attacked");
        nextAttack = Time.time + timeBetweenAttacks;
    }
}
