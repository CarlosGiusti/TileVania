using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : StateMachineBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] float attackRange;
    [SerializeField] float minPLayerYPosition;
    private float iddleTime = 2;
    private float iddleTimeAtStart;
    private float attackTime = 1;
    private float attackTimeAtStart;

    Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<CapsuleCollider2D>().enabled = false;
        iddleTimeAtStart = iddleTime;
        attackTimeAtStart = attackTime;
        player = FindObjectOfType<Player>();
        animator.GetComponent<AudioSource>().Stop();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        iddleTimeAtStart -= Time.deltaTime;

        if (iddleTimeAtStart <= Mathf.Epsilon)
        {
            animator.SetTrigger("Run");
        }

        attackTimeAtStart -= Time.deltaTime;

        if (attackTimeAtStart <= Mathf.Epsilon && Vector2.Distance(animator.transform.position, player.transform.position) < attackRange)
        {
            animator.GetComponent<Boss>().PlayAttackClip();
            animator.SetTrigger("Attack");
        }

        // Si el player se encuentra en la parte baja del mapa, comienza a saltar
        if (player.transform.position.y < minPLayerYPosition)
        {
            animator.SetTrigger("Jump");
        }
        else
        {
            animator.transform.localScale = new Vector2(1f, 1f);
        }

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
