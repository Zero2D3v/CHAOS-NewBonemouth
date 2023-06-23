using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public GameObject hitMarker;
    
    // Start is called before the first frame update
    void Start()
    {
        hitMarker.SetActive(true);
        transform.parent = GameObject.Find("hitMarker").transform;
        Invoke("DisableHitMarker", 0.2f);
    }

    void DisableHitMarker()
    {
        hitMarker.SetActive(false);
    }
}
