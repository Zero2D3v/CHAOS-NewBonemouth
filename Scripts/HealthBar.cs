using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    public GameObject segments;
    public Material segmentMaterial;
    private float segmentAmount;
    private float newSegmentAmount;

    private float maxHP;
    public PlayerStats PS;

    public TextMeshProUGUI healthText;

    public void Awake()
    {
        PS = GameObject.Find("Player").GetComponent<PlayerStats>();
    }
    public void Start()
    {
        SetHealthUI();

        SetSegments();
    }

   //public void LateUpdate()
   //{
   //    transform.rotation = Quaternion.identity;
   //}
    public void SetSegments()
    {
        maxHP = PS.maxHealth;
        segmentAmount = maxHP / 5;

        newSegmentAmount = (Mathf.Round(segmentAmount / 1) * 1) - 0.05f;

        segmentMaterial.SetFloat("_Frequency", newSegmentAmount);
    }

    public void UpdateHealth(float fraction)
    {
        healthBar.fillAmount = fraction;
    }

    public void SetHealthUI()
    {
        //PS.IncreaseHealth(PS.maxHealth - PS.previousMaxHealth);
        healthText.text = "Health:" + PS.currentHealth + "/" + PS.maxHealth;
        
    }
}
