using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Message;
public class Weapon : Item
{
    
    public int currentBullets;
    public int bulletsPerMagazine;
    public float damage;
    public float fireRate = 1f;
    public bool isAutomatic;

    public ParticleSystem muzzleFlash;
    public Transform bulletSpawnPosition;
    public ParticleSystem bullet;
    public List<MonoBehaviour> onUseMessageReceivers;

    public AudioSource reloadAudio;
    public float reloadTime;

    private void Start() {    
        GetComponentInChildren<Collider>().enabled = false;
        currentBullets = bulletsPerMagazine;
    }

    Ray ray;
    RaycastHit hit;
    public LayerMask layerMask;
    float nextShot;
    public override void Attack(Transform viewCamera , bool buttonDown, bool buttonUp) {
        if(isAutomatic) {
            if(nextShot < Time.time) {
                Fire(viewCamera);
            }
        } else {
            if(buttonDown) {
                Fire(viewCamera);
            }
        }
    }

    public void Fire(Transform viewCamera) {
        if(HasAmmo()) {
            ray = new Ray(viewCamera.position, viewCamera.forward);
            hit = new RaycastHit();

            if(Physics.Raycast(ray, out hit, 100f, layerMask)) {
                Debug.Log(hit.collider.name);
                
                Vector3 bulletSpawnForward = hit.point - bulletSpawnPosition.transform.position;
                //Debug.Log("Forward: " + bulletSpawnForward);
                bulletSpawnPosition.transform.forward = bulletSpawnForward;

            } else {
                bulletSpawnPosition.transform.forward = ray.origin + (ray.direction * 100f) - bulletSpawnPosition.transform.position;
            }
            
            muzzleFlash.Play();
            Instantiate(bullet,bulletSpawnPosition.position, bulletSpawnPosition.rotation);
            currentBullets--;
            nextShot = Time.time + 1f / (fireRate <= 0f ? 1f : fireRate);
            WeaponUseMessage data;
            data.someValue = 0f;
            var messageType = MessageType.FIRE;
            for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
                var receiver = onUseMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }
            Debug.DrawRay(bulletSpawnPosition.position, bulletSpawnPosition.forward * 100f, Color.red, 0.3f);
        } else {
            Reload();
        }
    }

    bool HasAmmo() {
        return currentBullets > 0;
    }

    public void Reload() {
        if(reloadAudio.clip != null) {
            reloadAudio.Play();
        }
        nextShot = Time.time + reloadTime;
        currentBullets = bulletsPerMagazine;
        WeaponUseMessage data;
        data.someValue = 0f;
        var messageType = MessageType.RELOAD;
        for(var i = 0; i < onUseMessageReceivers.Count; ++i) {
            var receiver = onUseMessageReceivers[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }


    public override void Attack2() {

    }

    public struct WeaponUseMessage
    {
        public float someValue;
    }

}
