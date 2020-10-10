using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Message;
public class PlayerController : MonoBehaviour, IMessageReceiver
{
    public GameObject cameraRig;
    
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float gravity = 20f;
    public float jumpSpeed = 10f;

    public int currentWeapon = 0;
    public Transform hips;

    Transform m_cam;
    CinemachineFreeLook m_freelook;
    Animator m_Animator;
    CharacterController m_CharacterController;
    PlayerInput m_Input;
    Damageable m_Damageable;
    Gear m_Gear;

    bool m_IsGrounded = true;
    bool m_ReadyToJump;
    float m_HorizotalSpeed;
    float m_VerticalSpeed;
    float m_AimY;
    float m_AimX;
    // These constants are used to ensure Ellen moves and behaves properly.
    // It is advised you don't change them without fully understanding what they do in code.
    const float k_JumpAbortSpeed = 10f;
    const float k_StickingGravityProportion = 0.3f;
    const float k_AnimationAimXCorrectionOffset = 36.65f;
    // Parameters
    // Animator Values
    readonly int m_HashVertical = Animator.StringToHash("Vertical");
    readonly int m_HashHorizontal = Animator.StringToHash("Horizontal");
    readonly int m_HashmoveInputMagnitude = Animator.StringToHash("Move Input Magnitude");
    readonly int m_HashAimY = Animator.StringToHash("Aim Y");
    readonly int m_HashAimX = Animator.StringToHash("Aim X");
    readonly int m_HashCurrentWeapon = Animator.StringToHash("Current Weapon");
    readonly int m_HashFireWeapon = Animator.StringToHash("Fire");
    readonly int m_HashReload = Animator.StringToHash("Reload");
    readonly int m_HashHurt = Animator.StringToHash("Get Hit");
    readonly int m_HashDeath = Animator.StringToHash("Dead");
    readonly int m_HashJump = Animator.StringToHash("Jump");
    readonly int m_HashLanded = Animator.StringToHash("Landed");
    readonly int m_HashFire = Animator.StringToHash("Weapon_Aim_Fire");
    

    private void Start() {
        GameObject go = Instantiate(cameraRig, transform.position, Quaternion.identity);
        m_cam = go.GetComponentInChildren<Camera>().transform;
        m_freelook = go.GetComponentInChildren<CinemachineFreeLook>();

        m_freelook.m_Follow = transform;
        m_freelook.m_LookAt = transform;

        m_Animator = GetComponent<Animator>();
        m_Input = GetComponent<PlayerInput>();
        m_CharacterController = GetComponent<CharacterController>();
        m_Gear = GetComponent<Gear>();
    }

    void Update() {

        DirectionalMovement();
        RotationMovement();
        VerticalMovement();
        AimingY();
        AimingX();
        Shooting();
        SetAnimatorValues();
    }

    void VerticalMovement() {
        if(!m_Input.JumpInput && m_IsGrounded) {
            m_ReadyToJump = true;
        }
        if(m_IsGrounded) {
            m_VerticalSpeed = -gravity * k_StickingGravityProportion;
            if(m_Input.JumpInput && m_ReadyToJump ) {
                m_Animator.SetTrigger(m_HashJump);
                m_VerticalSpeed = jumpSpeed;
                m_IsGrounded = false;
                m_ReadyToJump = false;
            }
        } else {
            if(!m_Input.JumpInput && m_VerticalSpeed > 0.0f) {
                // This is what causes holding jump to jump higher that tapping jump.
                m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
            }

            // If a jump is approximately peaking, make it absolute.
            if(Mathf.Approximately(m_VerticalSpeed, 0f)) {
                m_VerticalSpeed = 0f;
            }
            m_VerticalSpeed -= gravity * Time.deltaTime;
        }
    }

    Vector3 moveDirection;
    float currentSpeed;
    void DirectionalMovement() {
        m_HorizotalSpeed = Mathf.Abs(m_Input.MoveInput.x) >= Mathf.Abs(m_Input.MoveInput.y)? Mathf.Abs(m_Input.MoveInput.x) : Mathf.Abs(m_Input.MoveInput.y);

        //currentSpeed = m_HorizotalSpeed <= 0.5f ? Mathf.Lerp(0f, walkSpeed, m_HorizotalSpeed * 2f) : Mathf.Lerp(walkSpeed, runSpeed, (m_HorizotalSpeed * 2) - 1f);
        currentSpeed = Mathf.Lerp(walkSpeed, runSpeed, (m_HorizotalSpeed * 2) - 1f);


        moveDirection = new Vector3(m_Input.MoveInput.x, 0f, m_Input.MoveInput.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, m_HorizotalSpeed);
        moveDirection = transform.rotation * moveDirection;

    }

    void RotationMovement() {
        m_CharacterController.transform.Rotate(0f, m_Input.CameraInput.x, 0f);
    }

    void AimingY() {

        m_freelook.m_YAxis.m_InputAxisValue = m_Input.CameraInput.y;
        float angle = Vector3.SignedAngle(transform.forward, m_cam.forward, transform.right );
        m_AimY = -angle / 90f;
    }

    void AimingX() {
        Vector3 hipsForward = new Vector3(hips.forward.x, 0f, hips.forward.z);
        Vector3 baseForward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        float angle = Vector3.SignedAngle(baseForward, hipsForward, Vector3.up);
        
        angle -= k_AnimationAimXCorrectionOffset;
        //Debug.Log(angle);
        m_AimX = -angle / 45f;
    }

    void Shooting() {
        if(m_Input.Attack || m_Input.AttackDown || m_Input.AttackUp) {
            m_Gear.Attack(m_cam.transform, m_Input.AttackDown, m_Input.AttackUp);
        }

        if(m_Input.Attack2) {
            m_Gear.Attack2();
        }

        if(m_Input.Reload) {
            m_Gear.Reload();
        }
    }

    void SetAnimatorValues() {
        moveDirection *= currentSpeed;
        moveDirection.y = m_VerticalSpeed;
        m_CharacterController.Move(moveDirection * Time.deltaTime);
        m_IsGrounded = m_CharacterController.isGrounded;

        m_Animator.SetFloat(m_HashVertical, m_Input.MoveInput.y);
        m_Animator.SetFloat(m_HashHorizontal, m_Input.MoveInput.x);
        m_Animator.SetFloat(m_HashmoveInputMagnitude, m_HorizotalSpeed);
        m_Animator.SetFloat(m_HashAimY, m_AimY);
        m_Animator.SetFloat(m_HashAimX, m_AimX);
        m_Animator.SetInteger(m_HashCurrentWeapon, currentWeapon);
        m_Animator.SetBool(m_HashLanded, m_IsGrounded);
    }

    

    private void OnEnable() {
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);
        m_Gear = GetComponent<Gear>();
        m_Gear.OnGearUse.Add(this);
    }
    private void OnDisable() {
        m_Damageable.onDamageMessageReceivers.Remove(this);
        m_Gear.OnGearUse.Remove(this);
    }

    // Called by Ellen's Damageable when she is hurt.
    public void OnReceiveMessage(MessageType type, object sender, object data) {
        switch(type) {
            case MessageType.DAMAGED: {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Damaged(damageData);
                }
                break;
            case MessageType.DEAD: {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Die(damageData);
                }
                break;
            case MessageType.FIRE: {
                    Weapon.WeaponUseMessage itemData = (Weapon.WeaponUseMessage)data;
                    BulletFired();
                }
                break;
            case MessageType.RELOAD: {
                    Weapon.WeaponUseMessage itemData = (Weapon.WeaponUseMessage)data;
                    Reloading();
                }
                break;
            case MessageType.EQUIP: {
                    Gear.ItemMessage itemData = (Gear.ItemMessage)data;
                    itemData.item.onUseMessageReceivers.Add(this);
                    currentWeapon = 1;
                }
                break;
        }
    }

    // Called by OnReceiveMessage.
    void Damaged(Damageable.DamageMessage damageMessage) {
        // Set the Hurt parameter of the animator.
        m_Animator.SetTrigger(m_HashHurt);

    }

    // Called by OnReceiveMessage and by DeathVolumes in the scene.
    public void Die(Damageable.DamageMessage damageMessage) {
        m_Animator.SetTrigger(m_HashDeath);
        
    }

    public void BulletFired() {
        m_Animator.Play(m_HashFire, 1);
        //m_Animator.SetTrigger(m_HashFireWeapon);
    }

    public void Reloading() {
        m_Animator.SetTrigger(m_HashReload);
    }
}
