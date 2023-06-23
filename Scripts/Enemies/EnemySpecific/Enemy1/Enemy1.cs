using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }

    public E1_MoveState moveState { get; private set; }

    public E1_PlayerDetectedState playerDetectedState { get; private set; }

    public E1_ChargeState chargeState { get; private set; }

    public E1_LookForPlayerState lookForPlayerState { get; private set; }

    public E1_MeleeAtackState meleeAtackState { get; private set; }

    public E1_StunState stunState { get; private set; }

    public E1_DeadState deadState { get; private set; }


    [SerializeField]
    private Data_IdleState idleStateData;
    [SerializeField]
    private Data_MoveState moveStateData;
    [SerializeField]
    private Data_PlayerDetected playerDetectedData;
    [SerializeField]
    private Data_ChargeState chargeStateData;
    [SerializeField]
    private Data_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private Data_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private Data_StunState stunStateData;
    [SerializeField]
    private Data_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new E1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAtackState = new E1_MeleeAtackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E1_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E1_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }

    }
}
