using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun Data")]

public class Data_StunState : ScriptableObject
{
    public float stunTime = 0.6f;

    public float stunKnockbackTime = 0.2f;
    public float stunKnockbackSpeed = 0f;
    public float stunCritKnocbackSpeed = 20f;

    public Vector2 stunKnockbackAngle;

}
