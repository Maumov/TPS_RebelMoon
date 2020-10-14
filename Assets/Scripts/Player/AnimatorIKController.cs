using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorIKController : MonoBehaviour
{
    //Animator anim;
    public List<AnimatorLimb> limbs;

    void FootR() {
    
    }
    void FootL() {
    
    }

    void Land() {
    
    }

    void WeaponSwitch() {

    }

    void Activate() {
    
    }
}

[System.Serializable]
public class AnimatorLimb{
    public Transform hint;
    public AvatarIKHint ikHint;
    public float ikHintWeight;

    public Transform goal;
    public AvatarIKGoal ikGoal;
    public float ikGoalWeight;

}
