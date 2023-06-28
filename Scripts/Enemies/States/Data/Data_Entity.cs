using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make able to be created from menu
[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class Data_Entity : ScriptableObject
{
    //store stats to be shared accross enemy types, can be changed in inspector to better fit
    public float maxHealth = 30f;

    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;
    public int AC;

    //store important values can be tweaked in editor
    public float damageHopSpeed = 3f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    public float closeRangeActionDistance = 2f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    public float stunResistance = 1f;
    public float stunRecoveryTime = 0.6f;

    //prefab references to health drop and hitmarker
    public GameObject hitParticle;
    public GameObject healthDrop;

    //references to important layers, ground, player
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
