using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : StateMachineBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minPLayerYPosition;
    [SerializeField] float attackRange = 2.75f;

    // Cached References
    Player player;
    Rigidbody2D rigidbody2D;
        
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>();
        rigidbody2D = FindObjectOfType<Boss>().GetComponent<Rigidbody2D>();

        // Face Player
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

        // Perseguir al player
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, new Vector2(player.transform.position.x, 0f), moveSpeed * Time.deltaTime);

        // atacar player si esta lo suficientemente cerca
        if (Vector2.Distance(animator.transform.position, player.transform.position) < attackRange)
        {
            animator.GetComponent<Boss>().PlayAttackClip();
            animator.SetTrigger("Attack");
        }

        // Si el player se encuentra en la parte baja del mapa, comienza a saltar
        if(player.transform.position.y < minPLayerYPosition)
        {
            animator.SetTrigger("Jump");
        }            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
