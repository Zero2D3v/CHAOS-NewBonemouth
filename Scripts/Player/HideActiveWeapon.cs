using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideActiveWeapon : MonoBehaviour
{
    public GameObject sprite;

    private void OnEnable()
    {
        sprite.SetActive(true);
        Invoke("DisableActiveWeapon", 0.2f);
    }

    private void DisableActiveWeapon()
    {
        sprite.SetActive(false);
    }
}
