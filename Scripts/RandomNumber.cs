using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//random number generator for to hit dice, attached to player and enemies
public class RandomNumber : MonoBehaviour
{
    //declare and set start value
    public int RadNum = 0;

    //roll dice function public as called alot
    public void GenRadNum()
    {
        RadNum = Random.Range(1, 21);
        Debug.Log("generated" + RadNum);
    }
}
