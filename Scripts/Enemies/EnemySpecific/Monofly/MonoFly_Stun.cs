using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MonoFly_Stun : StateMachineBehaviour
{
    private float startTime;

    public bool isStunTimeOver;
    public bool isMovementStopped;

    private int PRN;

    public MonoFly_Combat monoflyCombat;

    public Data_Monofly stateData;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateData = animator.GetComponentInParent<Data_Monofly>();
        monoflyCombat = animator.GetComponentInParent<MonoFly_Combat>();
        PRN = monoflyCombat.PRN.RadNum;


        isStunTimeOver = false;


        if (PRN != 20)
        {
            monoflyCombat.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, monoflyCombat.lastDamageDirection);
        }
        else
        {
            monoflyCombat.SetVelocity(stateData.stunCritKnocbackSpeed, stateData.stunKnockbackAngle, monoflyCombat.lastDamageDirection);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        if (Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            monoflyCombat.SetVelocity(0f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoflyCombat.ResetStunResistance();
    }
}
