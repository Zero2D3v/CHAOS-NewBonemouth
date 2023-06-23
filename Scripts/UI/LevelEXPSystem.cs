using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEXPSystem : MonoBehaviour
{
    public GameController gameManager;
    public HealthBar healthbarHUD;
    

    public int level;
    public float currentXp;
    public float requiredXp;

    private float lerpTimer;
    private float delayTimer;
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
        
        expBar.fillAmount = currentXp / requiredXp;
        expBarBack.fillAmount = currentXp / requiredXp;
        levelText.SetText(level.ToString());
        requiredXp = CalculateRequiredXP();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateXpUI();
        if (Input.GetKeyDown(KeyCode.E))
        {
            GainExperienceFlatRate(5);
        }

        if(currentXp >= requiredXp)
        {
            LevelUP();
        }
    }
    public void UpdateXpUI()
    {
        float xpFraction = currentXp / requiredXp;
        float FXP = expBar.fillAmount;
        if(FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            expBarBack.fillAmount = xpFraction;
            if(delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                expBar.fillAmount = Mathf.Lerp(FXP, expBarBack.fillAmount, percentComplete);
            }
        }
    }
    public void GainExperienceFlatRate(float xpGained)
    {
        currentXp += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }
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
        level++;
        levelText.SetText(level.ToString());
        expBar.fillAmount = 0f;
        expBarBack.fillAmount = 0f;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        gameManager.playerStats.IncreaseMaxHealth(level);
        requiredXp = CalculateRequiredXP();
        healthbarHUD.SetSegments();
        healthbarHUD.SetHealthUI();
        healthbarHUD.PS.IncreaseHealth(healthbarHUD.PS.maxHealth - healthbarHUD.PS.previousMaxHealth);

    }
    private int CalculateRequiredXP()
    {
        int solveForRequiredXp = 0;
        for(int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionmultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
