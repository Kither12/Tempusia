using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFClose : StateMachineBehaviour
{
    private RewindRecorder rewindRecorder;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rewindRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<RewindRecorder>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rewindRecorder.isRecorded)
        {
            animator.SetBool("isRewind", true);   
        }
        else
        {
            animator.SetBool("isRewind", false);
        }
        if (animator.GetBool("isRewind") && 1 - stateInfo.normalizedTime > 0 && 1 - stateInfo.normalizedTime < 1)
        {
            animator.Play("ObjectFRewind1", -1, 1 - stateInfo.normalizedTime);
        }
        else if (animator.GetBool("isRewind") && 1 - stateInfo.normalizedTime < 0)
        {
            animator.Play("ObjectFRewind1", -1, 0);
        }
        else if (animator.GetBool("isRewind") && 1 - stateInfo.normalizedTime > 1)
        {
            animator.Play("ObjectFRewind1", -1, 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
