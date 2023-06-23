using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    public GameController gameManager;
    public Image bar;
    public float maxTime = 10f;
    public float timeLeft;
    public TextMeshProUGUI text;
    public float numberOfUses;
    public bool invulnerable;


    // Start is called before the first frame update
    void Start()
    {
        invulnerable = false;
        bar = GetComponent<Image>();
        bar.fillAmount = 0f;
        UpdateAmount();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && bar.fillAmount == 0f && numberOfUses > 0)
        {
            bar.fillAmount = 1f;
            UpdateCooldownTime(maxTime);
            numberOfUses -= 1f;
            UpdateAmount();
            //activate invincibility
            invulnerable = true;
            MakeInvulnerable();
        }



         if (timeLeft > 0)
         {
             timeLeft -= Time.deltaTime;
             bar.fillAmount = (timeLeft / maxTime);
         }
         else if (bar.fillAmount <= 0)
         {
             bar.fillAmount = 0f;
            //deactivate invincibility
            invulnerable = false;
            MakeInvulnerable();
         }

         //if(numberOfUses >= 0)
        //freeze time!!!
        //Time.timeScale = 0;


    }
    public void UpdateAmount()
    {
        if(numberOfUses < 0)
        {
            numberOfUses = 0f;
        }

        text.SetText(numberOfUses.ToString());
    }
    public void MakeInvulnerable()
    {
        if (invulnerable)
        {
            gameManager.playerStats.invulnerable = true;
        }
        else
        {
            gameManager.playerStats.invulnerable = false;
        }
        
    }
    public void UpdateCooldownTime(float cooldown)
    {
        timeLeft = cooldown;
    }
    //reset bar
}
