using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionDirection : MonoBehaviour
{
    Animator anim;

    float vertical, horizontal;
    bool crouch;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        SetAnimatorValues();
    }

    void GetInputs() {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        crouch = Input.GetButton("Crouch");
    }

    void SetAnimatorValues() {
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool("Crouch", crouch);
    }
}
