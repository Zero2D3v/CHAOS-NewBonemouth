using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    public int RadNum = 0;

    // Update is called once per frame
    //void Update()
    //{
    //    RadNum = Random.Range(1, 21);
    //    Debug.Log(RadNum);
    //}

    public void GenRadNum()
    {
        RadNum = Random.Range(1, 21);
        Debug.Log("generated" + RadNum);
    }
}
