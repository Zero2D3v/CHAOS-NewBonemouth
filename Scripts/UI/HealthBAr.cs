using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBAr : MonoBehaviour
{
    public Image healthBar;

    public GameObject segments;
    public Material segmentMaterial;
    private float segmentAmount;
    public float newSegmentAmount;

    public float maxHP;
    

   
    public void Start()
    {
        //etHealthUI();

        SetSegments();
    }

    //public void LateUpdate()
    //{
    //    transform.rotation = Quaternion.identity;
    //}
    public void SetSegments()
    {
        //maxHP = PS.maxHealth;
        segmentAmount = maxHP / 5;

        newSegmentAmount = (Mathf.Round(segmentAmount / 1) * 1) - 0.05f;

        segmentMaterial.SetFloat("_Frequency", newSegmentAmount);
    }

    public void UpdateHealth(float fraction)
    {
        healthBar.fillAmount = fraction;
    }
}
