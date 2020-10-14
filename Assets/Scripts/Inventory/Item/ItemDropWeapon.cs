using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropWeapon : ItemDrop
{
    public Transform Visuals;

    public GameObject modelToShow;

    private void Start() {
        Instantiate(modelToShow, Visuals);
    }

    public override void OnCollect(Gear g) {
        g.AddWeapon(itemToReceive.GetComponent<Weapon>());
    }
}
