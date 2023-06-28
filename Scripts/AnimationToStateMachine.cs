using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script speaks between animation events in different states
public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;

    public DeadState deadState;

    public MonoFly_Combat monoflyCombat;

    //I have two base entity classes that the state machines inherit from so depending on which one enemy has, will trigger that attack
    private void TriggerAttack()
    {
        if(!monoflyCombat)
        {
            attackState.TriggerAttack();
        }
        else
        {
            monoflyCombat.TriggerAttack();
        }
    }
    //removed the need for finish attack in the monoFly base class
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
    //call death trigger function on deadstate
    private void TriggerDeath()
    {
        if (!monoflyCombat)
        {
           deadState.TriggerDeath();
        }
        else
        {
           monoflyCombat.TriggerDeath();
        }
       
    }

    private void FinishDeath()
    {
        deadState.FinishDeath();
    }
}
