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

    public GameObject[] mono;
    public GameObject[] duo;
    public GameObject trio;
    private int numberOfEnemys;
    public int monoRemaining;
    public int duoRemaining;

    public bool lastManStanding;

    public float healthDropAmount = 5f;

    private bool isAttacking;

    private WeaponHolder weaponHolder;
    private int playerSelectedWeaapon;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        isAttacking = playerStats.GetComponent<PlayerCombatController>().isAttacking;
        weaponHolder = playerStats.GetComponentInChildren<WeaponHolder>();
        

        //ResetCooldown();

        foreach(GameObject enemy in duo)
        {
            enemy.SetActive(false);
        }
        trio.SetActive(false);

        monoRemaining = mono.Length;
        duoRemaining = duo.Length;
        numberOfEnemys = monoRemaining + duoRemaining + 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (monoRemaining == 0 && duoRemaining > 1)
        {
            
            foreach(GameObject enemy in duo)
            {
                enemy.SetActive(true);
            }
        }
        else if(duoRemaining == 0)
        {
            
            healthDropAmount = 7f;
            trio.SetActive(true);
        }

        playerSelectedWeaapon = weaponHolder.selectedWeapon;
    }
  //public void ResetCooldown()
  //{
  //    cooldownTimer.ResetBar();
  //}
    public void StartPlayerCooldownUI()
    {
        cooldownTimer.UpdateCooldownTime(cooldown);
    }
    public void PlayerUpdateActiveDamageDice()
    {
        //int previousSelectedWeapon = playerSelectedWeaapon;
        bool axe = d12Roll;
        bool fist = d4Roll;

        if (playerSelectedWeaapon == 0)
        {
            cooldown = 0.7f;
            d4.SetActive(false);
            d12.SetActive(true);
            damagePlayerPoof = d12Roll;
            fist = false;
        }
        else if(playerSelectedWeaapon == 1)
        {
            cooldown = 0.5f;
            d12.SetActive(false);
            d4.SetActive(true);
            damagePlayerPoof = d4Roll;
            axe = false;
        }
        
        //else if(playerSelectedWeaapon = 2)
        //{
        //
        //}
      // if (axe)
      // {
      //     d4.SetActive(false);
      // }
      // else if (fist)
      // {
      //     d12.SetActive(false);
      // }
    }
    public void PlayerRollToHit()
    {
       playerPoof.SetActive(true);
       Debug.Log("poof");
    }

    public void OnDeathXP()
    {
       if (monoRemaining == 0 && duoRemaining > 1)
       {
           flatRateXP *= 1.5f;
            backgroundParra.SetActive(true);
       }
       else if (duoRemaining == 0)
       {
           flatRateXP *= 2f;
       }
            levelSystem.GainExperienceFlatRate(flatRateXP);
    }

    public void PLayerDamageRoll()
    {
        damagePlayerPoof.SetActive(true);
        Debug.Log("damagepoof");
    }

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

    public void IncreasePlayerHP()
    {
        playerStats.IncreaseHealth(healthDropAmount);
    }
}
