using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour, ICollectable
{
    public GameObject itemToReceive;

    public Transform Visuals;

    public GameObject modelToShow;
    private void Start() {
        Instantiate(modelToShow, Visuals);
    }

    private void OnTriggerEnter(Collider other) {
        Gear g = other.GetComponent<Gear>();
        if(g != null) {
            OnCollect(g);
        } 
    }

    public void OnCollect(Gear g) {
        g.AddWeapon(itemToReceive.GetComponent<Weapon>());
    }
}

public interface ICollectable
{
    void OnCollect(Gear g);

}