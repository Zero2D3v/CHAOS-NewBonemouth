using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal;
using UnityEngine;

public class MonoFly_Charge : StateMachineBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    EnemyAI AI;

    [SerializeField]
    float speed;

    [SerializeField]
    float attackRange;

    public MonoFly_Combat monoflyCombat;
    //public MonoFly_Stun monoflyStun;

   public bool isStunned;
    bool isDead;
    //bool isStunTimeOver;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        monoflyCombat = animator.GetComponentInParent<MonoFly_Combat>();
        //monoflyStun = animator.GetComponentInParent<MonoFly_Stun>();
        rb = animator.GetComponentInParent<Rigidbody2D>();
        AI = animator.GetComponentInParent<EnemyAI>();
        speed = AI.speed;

        isStunned = monoflyCombat.isStunned;
        isDead = monoflyCombat.isDissolving;
        //isStunTimeOver = monoflyStun.isStunTimeOver;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI.speed = speed + 200f;

        if (isStunned)
        {
            animator.SetBool("isStunned", true);
        }
        //if (isStunTimeOver)
        //{
        else if (!isStunned && Vector2.Distance(player.position, rb.position) <= attackRange)
            {
                animator.SetTrigger("Attack");
            }
       // }
       
        if (isDead)
        {
            animator.SetBool("Dead", true);
        }  
}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.SetBool("isStunned", false);
    }
}
