using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

//class to handle enemy healthbars, different stats applied depending on enemy specific type and if using varient state machine
public class EnemyHealthBar : MonoBehaviour
{
    //declare fields
    public GameObject enemy;
    public GameObject hpBar;
    public Image healthBar;
    //shader relevent fields
    public GameObject segments;
    public Material segmentMaterial;
    private float segmentAmount;
    public float newSegmentAmount;

    private float maxHP;

    //data source for different state machines enemy dependent
    public Data_Entity data;

    public MonoFly_Combat monoFly;


    public void Awake()
    {
        //set to this instance on object
       hpBar = this.gameObject;
       //set references
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
        //determine which enemy and fetch correct stat data
        if (!monoFly)
        {
            maxHP = data.maxHealth;
        }
        else if(monoFly)
        {
            maxHP = monoFly.monoflyData.maxHealth;
        }
        //calculate correct amount of segements for shader
        segmentAmount = maxHP / 5;
        //round to nearest int
        newSegmentAmount = (Mathf.Round(segmentAmount / 1) * 1) - 0.05f;
        //apply correct amount of segments and update shader at run time
        SetSegments();
    }

    public void LateUpdate()
    {
        //keep healthbar rotation as would look wierd if healthbar flips with player
        transform.rotation = Quaternion.identity;
    }
 public void SetSegments()
 {
        //set frequency amount in shader which determines amount of segments
     segmentMaterial.SetFloat("_Frequency", newSegmentAmount);
        //create instance of shader on this game object which uses the new frequency values so enemies not all have the same amounts
     segmentMaterial = new Material(segmentMaterial);
 }
    //function for updating enemy health bar UI
    public void UpdateHealth(float fraction)
    {
        healthBar.fillAmount = fraction;
    }
}
