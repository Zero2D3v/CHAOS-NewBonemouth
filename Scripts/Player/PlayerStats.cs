using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //declare fields for stats to be set in editor, and scripts and objects to call and interact with
    public float maxHealth;
    [SerializeField]
    private GameObject deathChunkParticle, deathBloodParticle;

    public HealthBar healthbar;

    public GameObject canvas;

    public HealthBar healthbarHUD;

    public GameObject canvasHUD;

    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;
    public int AC;

    public bool invulnerable;

    public float currentHealth { get; private set; }
    public float previousMaxHealth { get; private set; }




    private void Start()
    {
        //set references
        currentHealth = maxHealth;
        canvas = GameObject.Find("PlayerCanvasHP");
        canvasHUD = GameObject.Find("PlayerHP");
        //previously had healthbar over player head to become visable when take damage but since moved to health on UI player HUD
        //DisableHealthBar(); 
    }

   //public void DisableHealthBar()
   //{
   //    canvas.SetActive(false);
   //}
   //public void EnableHealthBar()
   //{
   //    canvas.SetActive(true);
   //}

    public void DecreaseHealth(float amount)
    {
        //apply health loss
            currentHealth -= amount;
            //update UI HUD health bar and text
            healthbarHUD.UpdateHealth(currentHealth / maxHealth);
            healthbarHUD.SetHealthUI();
        //call die function if no health
        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }
    public void IncreaseHealth(float amount)
    {
        //apply health gain
        currentHealth += amount;
        //cap health at max health so no over heal
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        //update UI HUD health bar and text
        healthbarHUD.UpdateHealth(currentHealth / maxHealth);
        healthbarHUD.SetHealthUI();
    }
    private void Die()
    {
        //spawn blood and chunk particles on death and destroy player game object
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }
    //function called by level increase function in LevelEXPSystem script
    public void IncreaseMaxHealth(int level)
    {
        //record previous max health
        previousMaxHealth = maxHealth;
        //add calculated difference between prvious and new max health
        maxHealth += (currentHealth * 0.01f) * ((100 - level) * 0.1f);
        //round and set to nearest int
        maxHealth = (Mathf.Round(maxHealth / 1) * 1);
        Debug.Log(maxHealth);
    } 
}
