using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject enemy;
    public GameObject hpBar;
    public Image healthBar;

    public GameObject segments;
    public Material segmentMaterial;
    private float segmentAmount;
    public float newSegmentAmount;

    private float maxHP;

    public Data_Entity data;
   // public Entity entity;
    public MonoFly_Combat monoFly;


    public void Awake()
    {
       hpBar = this.gameObject;
       segmentMaterial = segments.GetComponent<SpriteRenderer>().material;

        if (!monoFly)
        {
            data = enemy.GetComponent<Entity>().entityData;
        }
        else
        {
            return;
        }
        
    }
    public void Start()
    {
        if (!monoFly)
        {
            maxHP = data.maxHealth;
        }
        else if(monoFly)
        {
            maxHP = monoFly.monoflyData.maxHealth;
        }

        segmentAmount = maxHP / 5;

        newSegmentAmount = (Mathf.Round(segmentAmount / 1) * 1) - 0.05f;

        SetSegments();
    }

    public void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        //SetSegments();
    }
 public void SetSegments()
 {
     segmentMaterial.SetFloat("_Frequency", newSegmentAmount);
     segmentMaterial = new Material(segmentMaterial);
 }

    public void UpdateHealth(float fraction)
    {
        healthBar.fillAmount = fraction;
    }
}
