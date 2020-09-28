using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public GameObject WeaponPrefab;

    public Transform Visuals;

    public GameObject modelToShow;
    private void Start() {
        Instantiate(modelToShow, Visuals);
    }

    private void OnTriggerEnter(Collider other) {
        Gear g = other.GetComponent<Gear>();
        if(g != null) {
            g.AddWeapon(WeaponPrefab.GetComponent<Weapon>());
        } 
    }
}
