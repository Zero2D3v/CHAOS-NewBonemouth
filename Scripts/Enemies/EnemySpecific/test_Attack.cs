using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class test_Attack : MonoBehaviour
{
    //public int damageAmount = 20;


    public Vector3 attackOffset;
    public float attackRange =1f;
    public LayerMask attackMask;

    public AttackDetails attackDetails;

    public Data_Monofly moflyData;


    public void Start()
    {
        attackDetails.damageAmount = moflyData.attackDamage;
        attackDetails.position = transform.position;
    }
    public void TriggerAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if(colInfo != null)
        {
            colInfo.GetComponent<PlayerCombatController>().Damage(attackDetails);
        }
    }
}
