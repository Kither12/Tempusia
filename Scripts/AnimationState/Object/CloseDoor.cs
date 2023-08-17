using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : StateMachineBehaviour
{
    private AudioSource audioSource;
    private bool check;
    private Door door;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.GetComponent<AudioSource>();
        audioSource.Play();
        door = animator.GetComponent<Door>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (door.LightAllGreen())
        {
            animator.SetBool("isActive", true);
        }
        else
        {
            animator.SetBool("isActive", false);
        }
        if (animator.GetBool("isActive")&& 1 - stateInfo.normalizedTime >0 && 1 - stateInfo.normalizedTime <1)
        {
            animator.Play("Door2",-1, 1 - stateInfo.normalizedTime);
        }else if (animator.GetBool("isActive") && 1 - stateInfo.normalizedTime < 0)
        {
            animator.Play("Door2", -1, 0);

        }else if (animator.GetBool("isActive") && 1 - stateInfo.normalizedTime >1)
        {
            animator.Play("Door2", -1, 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
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
