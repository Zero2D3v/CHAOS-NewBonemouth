using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    public float maxHealth;
    [SerializeField]
    private GameObject deathChunkParticle, deathBloodParticle;

    public HealthBar healthbar;

    public GameObject canvas;

    public HealthBar healthbarHUD;

    public GameObject canvasHUD;

    //public AbilityCooldown abilityCooldown;

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
        currentHealth = maxHealth;
        canvas = GameObject.Find("PlayerCanvasHP");
        canvasHUD = GameObject.Find("PlayerHP");
        DisableHealthBar();
        //invulnerable = canvasHUD.GetComponentInChildren<AbilityCooldown>().invulnerable; 
    }

    public void DisableHealthBar()
    {
        canvas.SetActive(false);
    }
    public void EnableHealthBar()
    {
        canvas.SetActive(true);
    }

    public void DecreaseHealth(float amount)
    {
            currentHealth -= amount;

            healthbar.UpdateHealth(currentHealth / maxHealth);

            //update UI HUD
            healthbarHUD.UpdateHealth(currentHealth / maxHealth);
            healthbarHUD.SetHealthUI();
        
        

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }
    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthbar.UpdateHealth(currentHealth / maxHealth);

        //update UI HUD
        healthbarHUD.UpdateHealth(currentHealth / maxHealth);
        healthbarHUD.SetHealthUI();
    }
    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }
    public void IncreaseMaxHealth(int level)
    {
        previousMaxHealth = maxHealth;

        maxHealth += (currentHealth * 0.01f) * ((100 - level) * 0.1f);
        
        maxHealth = (Mathf.Round(maxHealth / 1) * 1);
        //healthbarHUD.SetSegments();
        Debug.Log(maxHealth);
    } 
}
