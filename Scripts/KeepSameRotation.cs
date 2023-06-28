using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSameRotation : MonoBehaviour
{
    public void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
