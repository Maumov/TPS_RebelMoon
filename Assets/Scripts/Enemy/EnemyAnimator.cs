﻿using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IMessageReceiver
{
    Animator m_Animator;
    EnemyBehavior[] m_EnemyBehaviors; 

    readonly int m_HashVertical = Animator.StringToHash("Vertical");
    readonly int m_HashHorizontal = Animator.StringToHash("Horizontal");
    readonly int m_HashmoveInputMagnitude = Animator.StringToHash("Move Input Magnitude");
    readonly int m_HashAimY = Animator.StringToHash("Aim Y");
    readonly int m_HashAimX = Animator.StringToHash("Aim X");
    readonly int m_HashCurrentWeapon = Animator.StringToHash("Current Weapon");
    readonly int m_HashFireWeapon = Animator.StringToHash("Fire");
    readonly int m_HashReload = Animator.StringToHash("Reload");
    readonly int m_HashHurt = Animator.StringToHash("Get Hit");
    readonly int m_HashDeath = Animator.StringToHash("Dead");
    readonly int m_HashJump = Animator.StringToHash("Jump");
    readonly int m_HashLanded = Animator.StringToHash("Landed");
    readonly int m_HashFire = Animator.StringToHash("Weapon_Aim_Fire");
    private void Start() {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        m_Animator = GetComponent<Animator>();

        m_EnemyBehaviors = GetComponents<EnemyBehavior>();
        foreach(EnemyBehavior eb in m_EnemyBehaviors) {
            eb.onUseMessageReceivers.Add(this);
        }
    }
    private void OnDisable() {
        foreach(EnemyBehavior eb in m_EnemyBehaviors) {
            eb.onUseMessageReceivers.Remove(this);
        }
    }



    // Start is called before the first frame update
    public void OnReceiveMessage(MessageType type, object sender, object data) {
        //Debug.Log(type.ToString());
        switch(type) {
            case MessageType.DAMAGED: {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Damaged(damageData);
                }
            
            break;
            case MessageType.DEAD: {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Die(damageData);
                }
            break;
            case MessageType.RESPAWN: {
                
                }

            break;
            case MessageType.FIRE: {
                    Weapon.WeaponUseMessage itemData = (Weapon.WeaponUseMessage)data;
                    BulletFired();
                }

            break;
            case MessageType.RELOAD: {
                    Weapon.WeaponUseMessage itemData = (Weapon.WeaponUseMessage)data;
                    Reloading();
                }
            break;
            case MessageType.EQUIP: {
                    Gear.ItemMessage itemData = (Gear.ItemMessage)data;
                    itemData.item.onUseMessageReceivers.Add(this);
                }
            break;
            case MessageType.SIGHTED: {

                }
            break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage) {
        // Set the Hurt parameter of the animator.
        m_Animator.SetTrigger(m_HashHurt);

    }

    // Called by OnReceiveMessage and by DeathVolumes in the scene.
    public void Die(Damageable.DamageMessage damageMessage) {
        m_Animator.SetTrigger(m_HashDeath);

    }

    public void BulletFired() {
        m_Animator.Play(m_HashFire, 1);
        //m_Animator.SetTrigger(m_HashFireWeapon);
    }

    public void Reloading() {
        m_Animator.SetTrigger(m_HashReload);
    }
}