using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior_Attack : EnemyBehavior, IMessageReceiver
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
        EquipWeapon();
    }

    void EquipWeapon() {
        weaponEquipped = Instantiate(weaponToEquip.gameObject, weaponPosition);
        weaponEquipped.transform.localPosition = Vector3.zero;
        weaponEquipped.transform.localRotation = Quaternion.identity;
        weapon = weaponEquipped.GetComponent<Weapon>();
        weapon.onUseMessageReceivers.Add(this);
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
        EnemyAttackMessage data = new EnemyAttackMessage();
        data.isAttacking = false;
        SendMessage(MessageType.ATTACKING, data);
    }

    public override void Resume() {
        _target = enemyController.currentTarget;
        SetNextAttackTime(Time.time + timeBetweenAttacks);
        EnemyAttackMessage data = new EnemyAttackMessage();
        data.isAttacking = true;
        SendMessage(MessageType.ATTACKING, data);
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
        EnemyAttackMessage data = new EnemyAttackMessage();
        data.aimAngle = aimAngle;
        
        SendMessage(MessageType.AIM, data);
        
        aimTransform.LookAt(_target.position + new Vector3(0f, 1.3f, 0f));
        Debug.DrawRay(aimTransform.position, aimTransform.forward * 100f, Color.yellow, 0.3f);
    }

    void Attack() {
        //Debug.Log("Attacked");
        
        SetNextAttackTime(Time.time + timeBetweenAttacks);
        weapon.Fire(aimTransform);

    }

    void SetNextAttackTime(float nextAttackTime) {
        nextAttack = nextAttackTime;
    }

    public void AddDelayToNextAttack(float delay) {
        nextAttack += delay;
    }




    private void OnEnable() {
        enemyController = GetComponent<EnemyController>();
    }
    private void OnDisable() {


    }

    void SendMessage(MessageType messageType ,EnemyAttackMessage data) {
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public void OnReceiveMessage(MessageType type, object sender, object msg) {
        EnemyAttackMessage data = new EnemyAttackMessage();
        switch(type) {
            case MessageType.DAMAGED:
            break;
            case MessageType.DEAD:
            break;
            case MessageType.RESPAWN:
            break;
            case MessageType.FIRE:
            data.aimAngle = 0f;
            SendMessage(MessageType.FIRE, data);

            break;
            case MessageType.RELOAD:
            SetNextAttackTime(Time.time + weapon.reloadTime);
            data.aimAngle = 0f;
            SendMessage(MessageType.RELOAD, data);

            break;
            case MessageType.EQUIP:
            break;
            case MessageType.SIGHTED:
            break;
            case MessageType.INTERACT:
            break;
            case MessageType.WALK:
            break;
            case MessageType.IDLE:
            break;
            case MessageType.AIM:
            break;
            case MessageType.ATTACKING:
            break;
        }
    }

    public struct EnemyAttackMessage
    {
        public float aimAngle;
        public bool isAttacking;
    }

}