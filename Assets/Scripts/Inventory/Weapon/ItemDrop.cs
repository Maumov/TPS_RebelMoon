using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour, ICollectable
{
   
    public GameObject itemToReceive;

    private void OnTriggerEnter(Collider other) {
        Gear g = other.GetComponent<Gear>();
        if(g != null) {
            OnCollect(g);
        } 
    }

    public virtual void OnCollect(Gear g) {
        g.AddItem(itemToReceive.GetComponent<Item>());
    }

    public enum ItemType{
        Weapon, Key
    }
}

public interface ICollectable
{
    void OnCollect(Gear g);

}