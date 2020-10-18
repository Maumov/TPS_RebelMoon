using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;

    ParticleSystem part;
    public ParticleSystem wallHitFX;
    public List<ParticleCollisionEvent> collisionEvents;


    private void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        HitBox dmg = other.GetComponent<HitBox>();
        

        while(i < numCollisionEvents) {
            Instantiate(wallHitFX, collisionEvents[i].intersection, Quaternion.LookRotation(collisionEvents[i].normal));
            if(dmg) {
                Debug.Log("Hit box:" + dmg.name);
                Damageable.DamageMessage data = new Damageable.DamageMessage();
                data.damager = this;
                data.amount = damage;
                data.direction = collisionEvents[i].velocity.normalized;
                data.damageSource = collisionEvents[i].intersection;
                data.throwing = false;
                data.stopCamera = false;
                dmg.ApplyDamage(data);
            } else {
                if(rb) {
                    Debug.Log("Rigidbody: " + rb.name);
                    Vector3 pos = collisionEvents[i].intersection;
                    Vector3 force = collisionEvents[i].velocity * 10;
                    rb.AddForce(force);
                }
            }
            
            
            i++;
        }
    }
}
