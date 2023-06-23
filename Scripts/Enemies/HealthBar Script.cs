using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public GameObject hpBar;
    public GameObject Segments;

    public Material segmentMaterial;

    public float maxHP;

    private float segmentAmount;

    public float newSegmentAmount;

    public Image healthbar;


    public void Awake()
    {
        hpBar = this.gameObject;
        segmentMaterial = Segments.GetComponent<SpriteRenderer>().material;
    }

    public void Start()
    {
        segmentAmount = maxHP / 5;

        newSegmentAmount = (Mathf.Round(segmentAmount / 1) * 1) - 0.05f;

        SetSegments();
    }

    public void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetSegments()
    {
        segmentMaterial.SetFloat("_Frequency", newSegmentAmount);
        segmentMaterial = new Material(segmentMaterial);
    }

    public void UpdateHealthBar(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}
