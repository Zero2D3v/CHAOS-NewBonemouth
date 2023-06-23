using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Ionic.Zlib;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D.IK;

public class CharacterController : MonoBehaviour
{
    public GameObject spider;
    private Rigidbody2D rb;

    public IKManager2D ikManager;
    public Transform[] limbTargets;

    public Transform limbTarget1;
    public Transform limbTarget2;
    public Transform limbTarget3;
    public Transform limbTarget4;
    //public IKChain2D[] solvers;

    public float speed = 1f;

    private void Start()
     {
        rb = spider.GetComponent<Rigidbody2D>();
     }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        if (movement.magnitude > 0)
        {
            //movement.Normalize();

            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            

           foreach(Transform target in limbTargets)
           {
                target.rotation = Quaternion.Euler(0f, angle, 0f);
           }


            // Set the IK Targets for the four limbs
          limbTarget1.position = limbTarget1.position + new Vector3(movement.x * 0.1f, movement.y * 0.1f, 0);
          limbTarget2.position = limbTarget2.position + new Vector3(movement.x * -0.1f, movement.y * -0.1f, 0);
          limbTarget3.position = limbTarget3.position + new Vector3(movement.x * 0.1f, movement.y * -0.1f, 0);
          limbTarget4.position = limbTarget4.position + new Vector3(movement.x * -0.1f, movement.y * 0.1f, 0);

            //ikManager.up(Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) < speed)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector2.right * 50f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(-Vector2.right * 50f);
            }
        }
    }
}