using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

//script for handling player attack cooldown bar used by game controller script (composition)
public class CooldownTimer : MonoBehaviour
{
    //declare fields
    public Image bar;
    public float maxTime = 0.7f;
    public float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        //set reference
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //if time on timer then slowly fill bar again
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            bar.fillAmount = (timeLeft / maxTime);
        }
      else if(bar.fillAmount <= 0)
      {
          bar.fillAmount = 1f;
      }
    }

  public void UpdateCooldownTime(float cooldown)
    {
        timeLeft = cooldown;
    }
}
