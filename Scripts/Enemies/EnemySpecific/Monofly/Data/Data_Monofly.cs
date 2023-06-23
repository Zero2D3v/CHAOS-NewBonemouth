using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMonoflyData", menuName = "Data/Enemy Data/Monofly Data")]
public class Data_Monofly : ScriptableObject
{
//
    public float attackRadius = 0.5f;
    public int attackDamage;
    public int maxHealth;

    public GameObject hitParticle;

    public LayerMask whatisPlayer;

    public void GetDamage()
    {
        attackDamage = Random.Range(1, 5);
    }
    //

    public float stunResistance = 1f;
    public float stunRecoveryTime = 0.6f;

    public float stunTime = 0.6f;

    public float stunKnockbackTime = 0.2f;
    public float stunKnockbackSpeed = 0f;
    public float stunCritKnocbackSpeed = 20f;

    public Vector2 stunKnockbackAngle;
}
