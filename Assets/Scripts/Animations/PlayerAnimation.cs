using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public int num_type;
    AttackMGR attackMGR;
    StaminaMGR staminaMGR;

    public void Event_AttackMotion_Ended() {
        animator.SetInteger("motionType", 0);
    }

    public void AttackMotion_Started() {
        animator.SetInteger("motionType", attackMGR.Return_Num_MotionType());
        if (staminaMGR.is_in_groggy_state) {
            animator.SetFloat("attackMotion_speed", 0.7f);
        } else {
            animator.SetFloat("attackMotion_speed", 1f);
        }

    }
    
    void Start()
    {
		GameObject.Find("AttackMGR").TryGetComponent<AttackMGR>(out attackMGR);
		GameObject.Find("StaminaBar").TryGetComponent<StaminaMGR>(out staminaMGR);
    }

    void Update()
    {
        
    }
}
