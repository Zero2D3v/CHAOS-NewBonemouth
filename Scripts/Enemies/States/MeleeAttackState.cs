using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base melee attack state inherits from base attack state
public class MeleeAttackState : AttackState
{
    //reference relavent data
    protected Data_MeleeAttackState stateData;
    //reference struct to be container of relevant values
    protected AttackDetails attackDetails;
    //constructor for state with necessary components
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, Data_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        //match data in constructor to declared data above
        this.stateData = stateData;
    }
    //base attack state checks
    public override void DoChecks()
    {
        base.DoChecks();
    }
    //set struct values
    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGO.transform.position;
    }
    //base functions so for if need to add functionality using overrides
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
    //enemy attack function
    public override void TriggerAttack()
    {
        base.TriggerAttack();

        //generates enemy to hit roll
        entity.RN.GenRadNum();
        Debug.Log("entity" + entity.RN.RadNum);
        //if in battle with player then vs to hit rolls
        if (entity.PCC.isAttacking)
        {
            if (entity.RN.RadNum >= entity.PRN.RadNum)
            {
                //damage dice roll
                stateData.GetDamage();
                //check attack hit box
                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);
                //apply damage to player using SendMessage()
                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    Debug.Log("player HIT");
                }
            }
            //if lose roll, then show miss text
            else
            {
                entity.PCC.ShowMiss("Miss");
            }
        }
        //otherwise enemy to hit vs player defense stat
        else if (entity.playerInCombat.inCombat || !entity.PCC.isAttacking)
        {
            if (entity.RN.RadNum >= entity.PS.AC)
            {
                //damage dice roll
                stateData.GetDamage();
                //check attack hit box
                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatisPlayer);
                //apply damage to player using SendMessage()
                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    Debug.Log("player HIT AC");
                }
            }
            //if lose roll, then show miss text
            else
            {
                entity.PCC.ShowMiss("Miss");
            }
        }
    }
}
