using Message;
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
    Transform aimTransform;
    Weapon weapon;
    public override void Start() {
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        aimTransform = go.transform;
        aimTransform.localPosition = new Vector3(0f, 1.3f, 0f);

        base.Start();

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
        weapon.Fire(aimTransform);

        Weapon.WeaponUseMessage data;
        data.someValue = 0f;
        var messageType = MessageType.FIRE;
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    private void OnEnable() {
        enemyController = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnDisable() {


    }
}