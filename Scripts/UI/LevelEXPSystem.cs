using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEXPSystem : MonoBehaviour
{
    //declare fields
    public GameController gameManager;
    public HealthBar healthbarHUD;
    

    public int level;
    public float currentXp;
    public float requiredXp;

    private float lerpTimer;
    private float delayTimer;
    //better organise editor interface for tweaking
    [Header("UI")]
    public Image expBar;
    public Image expBarBack;
    public TextMeshProUGUI levelText;
    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionmultiplier = 300f;
    [Range(2f, 4f)]
    public float powerMultiplier = 2f;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7f;

    // Start is called before the first frame update
    void Start()
    {
        //set references
        expBar.fillAmount = currentXp / requiredXp;
        expBarBack.fillAmount = currentXp / requiredXp;
        levelText.SetText(level.ToString());
        requiredXp = CalculateRequiredXP();
        
    }

    // Update is called once per frame
    void Update()
    {
        //always upate UI XP bar
        UpdateXpUI();
        //for testing
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExperienceFlatRate(5);
        }
        //level up if have enough XP
        if(currentXp >= requiredXp)
        {
            LevelUP();
        }
    }
    public void UpdateXpUI()
    {
        //calculate new XP percentage
        float xpFraction = currentXp / requiredXp;
        //get initial fill amount of XP bar
        float FXP = expBar.fillAmount;
        //check if intial fill amount is less than new xp amount 
        if(FXP < xpFraction)
        {
            //delay timer going
            delayTimer += Time.deltaTime;
            //update UI XP bar to new amount using 1st bar
            expBarBack.fillAmount = xpFraction;
            //when enemy dissolve animation finished solidify xp gain using a lerp to animate bar to catch u in different colour
            if(delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                //tweaked time 
                float percentComplete = lerpTimer / 4;
                //lerp from initial xp amount to new xp amount over already updated xp amount bar
                expBar.fillAmount = Mathf.Lerp(FXP, expBarBack.fillAmount, percentComplete);
            }
        }
    }
    //function for gaining XP using preset amount on enemy death
    public void GainExperienceFlatRate(float xpGained)
    {
        //add XP 
        currentXp += xpGained;
        //reset start lerp timer
        lerpTimer = 0f;
        //reset start delay timer
        delayTimer = 0f;
    }
    //function for gaining XP using levels as modifiers if wanted to
    public void GainExperienceScaled(float xpGained, int passedLevel)
    {
        if(passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp += xpGained * multiplier;
        }
        else
        {
            currentXp += xpGained;
        }
        lerpTimer = 0f;
        delayTimer = 0f;
    }
    public void LevelUP()
    {
        //increase level by 1
        level++;
        //Update Level count UI
        levelText.SetText(level.ToString());
        //reset front XP bar UI (lerp bar)
        expBar.fillAmount = 0f;
        //reset back XP bar UI
        expBarBack.fillAmount = 0f;
        //round xp to int
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        //increase player max health
        gameManager.playerStats.IncreaseMaxHealth(level);
        //calculate next level XP required
        requiredXp = CalculateRequiredXP();
        //Update number of segments needed for healthbar if needed
        healthbarHUD.SetSegments();
        //show max health gain in health bar UI
        healthbarHUD.SetHealthUI();
        //UI healthbar show increase health equal to change in max health so can't just use level up to heal back to full
        healthbarHUD.PS.IncreaseHealth(healthbarHUD.PS.maxHealth - healthbarHUD.PS.previousMaxHealth);

    }
    private int CalculateRequiredXP()
    {
        //container for solution
        int solveForRequiredXp = 0;
        //workout for current level
        for(int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            //calculate required XPfor next level using runescape formula, values in formula tweakable in inspector
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionmultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        //complete formula by returning value divided by 4
        return solveForRequiredXp / 4;
    }
}
