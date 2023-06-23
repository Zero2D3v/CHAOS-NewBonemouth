using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttckStateData", menuName = "Data/State Data/Melee Attack State")]
public class Data_MeleeAttackState : ScriptableObject
{
    public float attackRadius = 0.5f;
    public int attackDamage;

    public LayerMask whatisPlayer;

    //private void OnEnable()
    //{
    //    attackDamage = Random.Range(1, 5);
    //}

    public void GetDamage()
    {
            attackDamage = Random.Range(1, 5);
    }
}
