using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public float flatRateXP = 5;

    public LevelEXPSystem levelSystem;

    public PlayerStats playerStats;
    
    public GameObject playerPoof;
    public GameObject damagePlayerPoof;

    public GameObject d12Roll;
    public GameObject d12;
    public GameObject d4Roll;
    public GameObject d4;

    public CooldownTimer cooldownTimer;
    public float cooldown;
    
    public GameObject backgroundParra;
    //array for to keep track of how many enemies of each type
    public GameObject[] mono;
    public GameObject[] duo;
    public GameObject trio;
    private int numberOfEnemys;
    public int monoRemaining;
    public int duoRemaining;
    //bool to see if last enemy of its type
    public bool lastManStanding;
    //set base health drop amount
    public float healthDropAmount = 5f;

    private bool isAttacking;

    private WeaponHolder weaponHolder;
    public int playerSelectedWeaapon;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        isAttacking = playerStats.GetComponent<PlayerCombatController>().isAttacking;
        weaponHolder = playerStats.GetComponentInChildren<WeaponHolder>();
        //turn off enemy type 2 and 3 so they can be spawned in after enemy type 1 and 2 is killed
        foreach(GameObject enemy in duo)
        {
            enemy.SetActive(false);
        }
        trio.SetActive(false);
        //count starting number of each enemy type in array
        monoRemaining = mono.Length;
        duoRemaining = duo.Length;
        numberOfEnemys = monoRemaining + duoRemaining + 1;

    }

    // Update is called once per frame
    void Update()
    {
        //spawn in enemy type 2 when no enemy type 1 remaining
        if (monoRemaining == 0 && duoRemaining > 1)
        {
            
            foreach(GameObject enemy in duo)
            {
                enemy.SetActive(true);
            }
        }
        //spawn in enemy type 3 when no more enemy type 2 remaining
        else if(duoRemaining == 0)
        {
            //change health drop amount of health if dropped from enemy type 2
            healthDropAmount = 7f;
            trio.SetActive(true);
        }
        //always update active weapon for correct UI and damage
        playerSelectedWeaapon = weaponHolder.selectedWeapon;
    }
    //cooldown dependent on active weapon, UI bar faster or slower
    public void StartPlayerCooldownUI()
    {
        cooldownTimer.UpdateCooldownTime(cooldown);
    }
    public void PlayerUpdateActiveDamageDice()
    {
        //internal bools for me to see more clearly
        bool axe = d12Roll;
        bool fist = d4Roll;
        //axe
        if (playerSelectedWeaapon == 0)
        {
            //longer coooldown
            cooldown = 0.7f;
            //enable and disable correct damage dice UI (d12 active, d4 disabled)
            d4.SetActive(false);
            d12.SetActive(true);
            //sets animated object to correct object and true
            damagePlayerPoof = d12Roll;
            //disable unselected weapon (fist)
            fist = false;
            
        }
        else if(playerSelectedWeaapon == 1)
        {
            //shorter coooldown
            cooldown = 0.5f;
            //enable and disable correct damage dice UI (d12 disabled, d4 active)
            d12.SetActive(false);
            d4.SetActive(true);
            //sets animated object to correct object and true
            damagePlayerPoof = d4Roll;
            //disable unselected weapon (axe)
            axe = false;
            
        }
    }
    //animation and lightning particle system on child of dice UI object plays on enable
    public void PlayerRollToHit()
    {
       playerPoof.SetActive(true);
       Debug.Log("poof");
    }
    //player gain xp when kill enemy function
    public void OnDeathXP()
    {
        //xp modifier if killed all enemy 1 already
       if (monoRemaining == 0 && duoRemaining > 1)
       {
           flatRateXP *= 1.5f;
            //on kill activate floating islands and mist parrallax script
            backgroundParra.SetActive(true);
       }
       //xp modifier if kill boss
       else if (duoRemaining == 0)
       {
           flatRateXP *= 2f;
       }
       //gain experience function with xp value passed through dependent on which enemy killed
            levelSystem.GainExperienceFlatRate(flatRateXP);
    }
    //damage dice UI animation
    public void PLayerDamageRoll()
    {
        damagePlayerPoof.SetActive(true);
        Debug.Log("damagepoof");
    }
    //check wether or not to drop health if last enemy of type
    public void CheckLastManStanding()
    {
        if (monoRemaining == 1 || duoRemaining == 1)
        {
            lastManStanding = true;
        }
        else
        {
            lastManStanding = false;
        }
        
    }
    //increases player health based on health drop amount
    public void IncreasePlayerHP()
    {
        playerStats.IncreaseHealth(healthDropAmount);
    }
}
