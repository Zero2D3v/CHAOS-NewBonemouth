using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CooldownTimer : MonoBehaviour
{
    public Image bar;
    public float maxTime = 0.7f;
    public float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Image>();
        //timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            bar.fillAmount = (timeLeft / maxTime);
        }
      else if(bar.fillAmount <= 0)
      {
          bar.fillAmount = 1f;
      }
        
        
            //freeze time!!!
            //Time.timeScale = 0;

        
    }

  public void UpdateCooldownTime(float cooldown)
    {
        timeLeft = cooldown;
    }
    //reset bar
}
