using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;

    public DeadState deadState;

    public MonoFly_Combat monoflyCombat;

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

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

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
