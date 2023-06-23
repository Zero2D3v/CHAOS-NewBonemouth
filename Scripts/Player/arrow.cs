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
        rb = GetComponent<Rigidbody2D>();
        hitMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        transform.parent = collision.transform;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

        if (collision.gameObject.tag == "Enemy")
        {
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
