using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ranged attack state inherits from state
public class RangedAttackState : AttackState
{
    //declare fields
    protected Data_RangedAttackState stateData;

    protected GameObject projectile;
    protected Projectile projectileScript;
    //inherited constructor with added components
    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, Data_RangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        //set instance to declared data
        this.stateData = stateData;
    }
    //base functionality
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
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
        
        //executes ranged attack logic instead of melee attack logic
        //spawn and set spear prefab reference
        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        //reference script attached to instantiated prefab
        projectileScript = projectile.GetComponent<Projectile>();
        //apply stat values from data to spear script to be executed
        projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);
    }
}
