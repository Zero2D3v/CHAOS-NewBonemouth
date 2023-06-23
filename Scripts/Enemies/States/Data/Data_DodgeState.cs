using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State")]
public class Data_DodgeState : ScriptableObject
{
    public float dodgeSpeed = 10f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2f;
    public Vector2 dodgeAngle;
    
}
