using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that handles the floating island elements to go with the parrallax script, designed as the floating islands behave diferently to the constant swirling mist
public class ParallaxIslands : MonoBehaviour
{
    //declare camera and fields
    public GameObject cam;

    private float length, startPos;
    
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //same as in mist controll parrallax script but does not repeat, will either procedurally generate islands and destroy when out of view or set active object pool bank of various different designs and scales to flow past player
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
