using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropKey : ItemDrop
{
    public override void OnCollect(Gear g) {
        g.AddItem(itemToReceive.GetComponent<Item>());
    }
}
