using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Move : MonoBehaviour
{
    public float jumpHeight; // s
    public float timeToJumpApex; // t
    float currentTimeToJumpApex;

    public float jumpWallHeight; // s
    public float timeToJumpWallApex; // t
    float currentTimeToJumpWallApex;

    public float movementSpeed = 5f;
    

    public Transform myCamera;
    CharacterController characterController;
    Animator anim;
    //Inputs
    float vertical, horizontal;
    bool jump , sprint;
    public static bool targeting;
    float mouseX;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeToJumpApex = timeToJumpApex;
        currentTimeToJumpWallApex = timeToJumpWallApex;

        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Rotation();
        Movement();
        Animate();
    }

    void GetInputs() {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        //jump = Input.GetButtonDown("Jump");
        //targeting = Input.GetButton("Targeting");
        mouseX = Input.GetAxis("Mouse X");
        sprint = Input.GetButton("Sprint");
    }

    Vector3 inputDirectionRotation;
    Quaternion fixedCameraRotation;
    Vector3 lookDirection;
    Quaternion lookRotation;
    bool isSlidingOnWall;
    void Rotation() {

        if(!targeting) {
            if((vertical != 0f || horizontal != 0f)) {
                inputDirectionRotation = new Vector3(horizontal, 0f, vertical);
                fixedCameraRotation = Quaternion.Euler(0f, myCamera.eulerAngles.y, 0f);
                lookDirection = fixedCameraRotation * inputDirectionRotation;
                if(isOnTheWall()) {
                    if(velocityY <= 0f) {
                        //sliding down on the wall
                        lookRotation = Quaternion.LookRotation(TouchingWallNormal());
                        isSlidingOnWall = true;
                    } else {
                        //jumping up touching the wall
                        lookRotation = Quaternion.LookRotation(lookDirection);
                        isSlidingOnWall = false;
                    }
                } else {
                    isSlidingOnWall = false;
                    //normal rotation
                    lookRotation = Quaternion.LookRotation(lookDirection);
                }
                transform.rotation = lookRotation;
            }
        } else {
            inputDirectionRotation = new Vector3(horizontal, 0f, vertical);
            fixedCameraRotation = Quaternion.Euler(0f, myCamera.eulerAngles.y, 0f);
            lookDirection = fixedCameraRotation * inputDirectionRotation;
            transform.Rotate(0f, mouseX, 0f);
        }
    }

    Ray ray;
    RaycastHit hit;
    Vector3 TouchingWallNormal() {
        ray.origin = transform.position + characterController.center;
        ray.direction = lookDirection;
        Debug.DrawRay(ray.origin,ray.direction * 5f, Color.red);
        if(Physics.Raycast(ray, out hit, 10f)) {
            return hit.normal;
        }
        return transform.forward;
    }

    float inputDirectionSpeed;
    float gravity;
    float gravityWall;
    float jumpVelocity;
    public float velocityY;
    public float velocityNormal;
    Vector3 finalDirection;

    float jumpFromWallVelocity;
    Vector3 wallDirection;

    void CalculateJumpVariables() {
        gravity = -(2 * jumpHeight) / Mathf.Pow(currentTimeToJumpApex, 2f);
        jumpVelocity = Mathf.Abs(gravity) * currentTimeToJumpApex;
    }

    void CalculateJumpFromWallVariables() {
        gravityWall = -(2 * jumpWallHeight) / Mathf.Pow(currentTimeToJumpWallApex, 2f);
        jumpFromWallVelocity = Mathf.Abs(gravityWall) * currentTimeToJumpWallApex;
    }

    void Movement() {

        CalculateJumpVariables();
        CalculateJumpFromWallVariables();

        if(isOnTheFloor()) {
            velocityY = 0f;
        }
        if(isSlidingOnWall) {
            velocityY = -1f;
        }
        //simple jump
        if(jump && characterController.isGrounded) {
            //Debug.Log("Jump");
            velocityY = jumpVelocity;
        }

        //climb jump
        if(jump && isSlidingOnWall) {
            //Debug.Log("Climb");
            velocityY = jumpVelocity;
            velocityNormal = jumpFromWallVelocity;
            wallDirection = hit.normal;
        }

        //when sliding cant change direction of the slide
        
        
        inputDirectionSpeed = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        inputDirectionSpeed = Mathf.Clamp(inputDirectionSpeed, 0f, 1f);

        if(isSlidingOnWall) {
            inputDirectionSpeed *= 0.1f;
        }

        velocityY += gravity * Time.deltaTime;

        velocityNormal = velocityNormal >= 0f ? velocityNormal += gravityWall * Time.deltaTime : 0f;
        
        finalDirection = (lookDirection * inputDirectionSpeed * movementSpeed) + (transform.up * velocityY) + (velocityNormal >= 0f ? wallDirection * velocityNormal : Vector3.zero);
        
        characterController.Move(finalDirection  * Time.deltaTime);
        //characterController.Move(transform.up * velocityY * Time.deltaTime);
    }

    bool isOnTheFloor() {
        return (characterController.collisionFlags & CollisionFlags.Below) != 0;
    }

    bool isOnTheWall() {
        return (characterController.collisionFlags & CollisionFlags.Sides) != 0 && !isOnTheFloor();
    }

    float animMovementSpeed;
    void Animate() {
        animMovementSpeed = finalDirection.z + (sprint? 1f: 0f);
        anim.SetFloat("movementSpeed", animMovementSpeed);
    }
}

