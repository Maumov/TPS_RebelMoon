using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior_Attack : EnemyBehavior
{
    public float timeBetweenAttacks;
    public Weapon weaponToEquip;
    public GameObject weaponEquipped;
    Weapon weapon;
    public Transform weaponPosition;
    float nextAttack;
    EnemyController enemyController;
    Transform _target;
    //NavMeshAgent agent;
    Transform aimTransform;
    
    public override void Start() {
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        aimTransform = go.transform;
        aimTransform.localPosition = new Vector3(0f, 1.3f, 0f);

        base.Start();
        weaponEquipped = Instantiate(weaponToEquip.gameObject, weaponPosition);
        weaponEquipped.transform.localPosition = Vector3.zero;
        weaponEquipped.transform.localRotation = Quaternion.identity;
        weapon = weaponEquipped.GetComponent<Weapon>();
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
        nextAttack = Time.time + timeBetweenAttacks;
        base.Resume();
    }

    public override void ExecuteBehavior() {
        if(m_isPaused) {
            return;
        }
        base.ExecuteBehavior();
        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
        Aim();
        
        

        if(Time.time > nextAttack) {
            Attack();
        }
    }

    float aimAngle;
    void Aim() {
        aimAngle = Vector3.SignedAngle(_target.position - transform.position, transform.forward, transform.right);
        aimAngle = aimAngle / 90f;
        EnemyAttackMessage data;
        data.someValue = aimAngle;
        var messageType = MessageType.AIM;
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
        aimTransform.LookAt(_target.position + new Vector3(0f, 1.3f, 0f));
        Debug.DrawRay(aimTransform.position, aimTransform.forward * 100f, Color.yellow, 0.3f);
    }

    void Attack() {
        //Debug.Log("Attacked");
        nextAttack = Time.time + timeBetweenAttacks;
        weapon.Fire(aimTransform);

        EnemyAttackMessage data;
        data.someValue = 0f;
        var messageType = MessageType.FIRE;
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    private void OnEnable() {
        enemyController = GetComponent<EnemyController>();
        //agent = GetComponent<NavMeshAgent>();
    }
    private void OnDisable() {


    }
    public struct EnemyAttackMessage
    {
        public float someValue;
    }

}