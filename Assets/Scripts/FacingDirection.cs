using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FacingDirection : MonoBehaviour
{
    public Transform thirdPersonCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, thirdPersonCamera.rotation.eulerAngles.y, 0f);
    }
}
