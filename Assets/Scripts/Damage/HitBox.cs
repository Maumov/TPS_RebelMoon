using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public void ApplyDamage(Damageable.DamageMessage data) {
        GetComponentInParent<Damageable>().ApplyDamage(data);
    }
}
