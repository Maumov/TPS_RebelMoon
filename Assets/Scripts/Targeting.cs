using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public Vector3 center;
    public Vector3 halfExtends;
    public LayerMask layerMaskForTargets;
    // Update is called once per frame

    bool strafing;
    void Update()
    {
        //strafing = Input.GetButtonDown("Targeting");
        if(strafing) {
            FindTarget();
        }
    }

    Collider[] colliders;
    public void FindTarget() {
        if(colliders != null) {
            foreach(Collider c in colliders) {
                c.GetComponent<Target>().UnTargeted();
            }
        }
        colliders = Physics.OverlapBox(transform.position + (transform.rotation * center), halfExtends, transform.rotation, layerMaskForTargets);
        
        //Debug.Log(colliders.Length);
        foreach(Collider c in colliders) {
            c.GetComponent<Target>().Targeted();
        }
    }


}
