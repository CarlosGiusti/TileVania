using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : StateMachineBehaviour
{
    [SerializeField] float minPLayerYPosition;

    // Cached References
    Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>();
        animator.GetComponent<AudioSource>().Play();

        // Face the player
        if (player.transform.position.x < animator.transform.position.x)
        {
            animator.transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            animator.transform.localScale = new Vector2(1f, 1f);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si el player se encuentra en la parte alta del mapa, comienza a saltar
        if (player.transform.position.y > minPLayerYPosition)
        {
            Boss boss = FindObjectOfType<Boss>();
            if (boss.GetBossLife() >= 3)
            {
                animator.SetTrigger("Iddle");
            }
            else
            {
                animator.SetTrigger("Hurt");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<AudioSource>().Stop();
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
