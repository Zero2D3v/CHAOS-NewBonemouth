using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//ability script
public class AbilityCooldown : MonoBehaviour
{
    //declare references and values
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
        //set references and starting values, number of uses set in inspector for tweaking purposes
        invulnerable = false;
        bar = GetComponent<Image>();
        bar.fillAmount = 0f;
        //Update UI to match number of uses
        UpdateAmount();
    }

    // Update is called once per frame
    void Update()
    {
        //ability activated by pressing "1" key, but only if ability not cooling down or all uses used
        if (Input.GetKeyDown(KeyCode.Alpha1) && bar.fillAmount == 0f && numberOfUses > 0)
        {
            //shaded bar over icon set to full to give player feedback that ability has been used
            bar.fillAmount = 1f;
            //cooldown timer started
            UpdateCooldownTime(maxTime);
            //minus one use 
            numberOfUses -= 1f;
            //update UI to reflect that one use been used
            UpdateAmount();
            //record internal bool
            invulnerable = true;
            //use internal bool to affect damage on player through game manager script
            MakeInvulnerable();
        }

         if (timeLeft > 0)
         {
            //record timer
             timeLeft -= Time.deltaTime;
            //reduce icon UI fill amount to match ability timer
             bar.fillAmount = (timeLeft / maxTime);
         }
         else if (bar.fillAmount <= 0)
         {
            //if ability over then reset
             bar.fillAmount = 0f;
            //record bool
            invulnerable = false;
            //reset player damage through game manager using bool value
            MakeInvulnerable();
         }
    }
    //function to handle UI amount of uses
    public void UpdateAmount()
    {
        if(numberOfUses < 0)
        {
            numberOfUses = 0f;
        }

        text.SetText(numberOfUses.ToString());
    }
    //function for handling player invulnerability
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
    //function for handling cooldown time, scalable for different abilities
    public void UpdateCooldownTime(float cooldown)
    {
        timeLeft = cooldown;
    }
}
