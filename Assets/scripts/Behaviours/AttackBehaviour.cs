﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<Character> ().Attack = true;

        if (animator.tag != "Player") {
            animator.SetFloat ("speed", 0);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     Hero.Instance.Attack = true;
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<Character> ().Attack = false;
        animator.GetComponent<Character> ().AttackCollider.enabled = false;
        animator.ResetTrigger ("attack");

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}