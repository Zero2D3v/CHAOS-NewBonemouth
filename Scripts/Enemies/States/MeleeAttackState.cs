using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected Data_MeleeAttackState stateData;

    protected AttackDetails attackDetails;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, Data_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();


        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGO.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        
        entity.RN.GenRadNum();
        Debug.Log("entity" + entity.RN.RadNum);
        if (entity.PCC.isAttacking)
        {
            if (entity.RN.RadNum >= entity.PRN.RadNum)
            {
                stateData.GetDamage();
                //Debug.Log("enemy damage" + stateData.attackDamage);

                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);

                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);

                    Debug.Log("player HIT");
                }
            }
            else
            {
                entity.PCC.ShowMiss("Miss");
            }
        }
        else if (entity.playerInCombat.inCombat || !entity.PCC.isAttacking)
        {
            if (entity.RN.RadNum >= entity.PS.AC)
            {
                stateData.GetDamage();

                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);

                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);

                    Debug.Log("player HIT AC");
                }
            }
            else
            {
                entity.PCC.ShowMiss("Miss");
            }
        }

       //if (entity.playerInCombat.inCombat)
       //{
       //    if(entity.RN.RadNum >= entity.PS.AC)
       //    {
       //        stateData.GetDamage();
       //
       //        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);
       //
       //        foreach (Collider2D collider in detectedObjects)
       //        {
       //            collider.transform.SendMessage("Damage", attackDetails);
       //            //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);
       //
       //            Debug.Log("player HIT AC");
       //        }
       //    }
       //}
       //else if(entity.RN.RadNum >= entity.PRN.RadNum)
       //{
       //    stateData.GetDamage();
       //    //Debug.Log("enemy damage" + stateData.attackDamage);
       //
       //    Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);
       //
       //    foreach (Collider2D collider in detectedObjects)
       //    {
       //        collider.transform.SendMessage("Damage", attackDetails);
       //        //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);
       //
       //        Debug.Log("player HIT");
       //    }
       //}
       //else
       //{
       //    Debug.Log("player missed");
       //}

    }
}
