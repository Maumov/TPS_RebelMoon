using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook cam;
    public Transform cameraTransform;
    public AnimationCurve cameraCenteringSpeed;
    float timeInTargetingMode;
    float cameraMoveValue;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //cam = GetComponent<Cinemachine.CinemachineFreeLook>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        MoveCamera();
        //SetAnimatorValues();
    }

    void GetInputs() {
        cameraMoveValue = 0f;
        //if(Input.GetKeyDown(MoveLeft90)) {
        //    cameraMoveValue -= 90f;
        //}
        //if(Input.GetKeyDown(MoveRight90)) {
        //    cameraMoveValue += 90f;
        //}
        if(Move.targeting) {
            cam.m_XAxis.m_InputAxisName = "";
            timeInTargetingMode += Time.deltaTime;
            //Debug.Log(cameraTransform.name + " , "+ cameraTransform.forward);
            //Debug.Log(cam.m_Follow.name + " , "+ cam.m_Follow.forward);
            cameraMoveValue = Vector3.SignedAngle(new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z), new Vector3(cam.m_Follow.forward.x, 0f, cam.m_Follow.forward.z), Vector3.up);
            cameraMoveValue *= cameraCenteringSpeed.Evaluate(timeInTargetingMode) * Time.deltaTime;
            //Debug.Log(cameraMoveValue);
        } else {
            cam.m_XAxis.m_InputAxisName = "Mouse X";
            timeInTargetingMode = 0f;
            //cam.
        }
    }

    void MoveCamera() {
        
        cam.m_XAxis.m_InputAxisValue = cameraMoveValue ;
    }
    void SetAnimatorValues() {
        //if(Move.strafing) {
        //    cam.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.LockToTarget;
        //} else {
        //    cam.m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        //}
        
        //anim.SetBool("Targeting",Move.strafing);
    }

}
