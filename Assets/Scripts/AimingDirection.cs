using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingDirection : MonoBehaviour
{
    Animator anim;
    public Transform thirdPersonCamera;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorValues();
    }

    void SetAnimatorValues() {
        float angle = Vector3.SignedAngle(thirdPersonCamera.forward, transform.forward, transform.right);

        anim.SetFloat("AimY", angle / 90f );
    }
}
