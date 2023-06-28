using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple parrallax script attached to each background element with different weights for each layer to give sense of depth
public class Parallax : MonoBehaviour
{
    //declare fields with reference to camera
    public GameObject cam;

    private float length, startPos;
    
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        startPos = transform.position.x;
        //width of texture
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //temporary value based on camera x position and parrallax effect weight
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        //calculate distance between
        float distance = (cam.transform.position.x * parallaxEffect);
        //update position of object with parrallax scrolling
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        //if temp position greater than object length plus starting position then bring object across to new starting position to repeat
        if (temp > startPos + length) startPos += length;
        //otherwise keep parrallax scrolling
        else if (temp < startPos - length) startPos -= length;
    }
}
