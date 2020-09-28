using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    protected Vector2 m_Movement;

    public Vector2 MoveInput {
        get {
            return m_Movement;
        }
    }
    protected Vector2 m_Camera;
    public Vector2 CameraInput {
        get {
            return m_Camera;
        }
    }

    protected bool m_Jump;
    public bool JumpInput {
        get {
            return m_Jump;
        }
    }

    protected bool m_Attack;
    public bool Attack {
        get {
            return m_Attack;
        }
    }

    protected bool m_AttackDown;
    public bool AttackDown {
        get {
            return m_AttackDown;
        }
    }

    protected bool m_AttackUp;
    public bool AttackUp {
        get {
            return m_AttackUp;
        }
    }

    protected bool m_Attack2;
    public bool Attack2 {
        get {
            return m_Attack2;
        }
    }
    protected bool m_Reload;
    public bool Reload {
        get {
            return m_Reload;
        }
    }
    

    protected bool m_Pause;
    public bool Pause {
        get {
            return m_Pause;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        m_Jump = Input.GetButton("Jump");
        m_Attack = Input.GetButton("Fire1");
        m_AttackDown = Input.GetButtonDown("Fire1");
        m_AttackUp = Input.GetButtonUp("Fire1");
        m_Attack2 = Input.GetButton("Fire2");
        m_Reload = Input.GetButton("Reload");
    }



}
