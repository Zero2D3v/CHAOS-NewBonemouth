using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    Rigidbody2D rb;
    bool hasHit;
    GameObject hitPoint;
    public GameObject hitMarker;
    

    // Start is called before the first frame update
    void Start()
    {
        //set referneces
        rb = GetComponent<Rigidbody2D>();
        hitMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit == false)
        {
            //calculate angle
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            //apply roation
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //record hit
        hasHit = true;
        //stop velocity
        rb.velocity = Vector2.zero;
        //make kinematic to stop bouncing
        rb.isKinematic = true;
        //make arrow stick in whatever it hit
        transform.parent = collision.transform;
        //disable box collider and sprite of original arrow and replace with arrow cut off at top so it looks like it embedded in target
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

        if (collision.gameObject.tag == "Enemy")
        {
            //give player feedback if hit using himarker
            Debug.Log("enemy hit");
            EnableHitMarker();
            Invoke("DisableHitMarker", 0.2f);
        }
    }

    private void EnableHitMarker()
    {
        hitMarker.SetActive(true);
    }

    private void DisableHitMarker()
    {
        hitMarker.SetActive(false);
    }
}
