using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
public class Data_IdleState : ScriptableObject
{
    public float minIdleTime = .5f;
    public float maxIdleTime = 1f;
}

