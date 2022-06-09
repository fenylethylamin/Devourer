using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public bool canAutoFire;
    public float fireRate;
    [HideInInspector]
    public float fireCounter = 0f;

    //public int currentAmmo, pickupAmount;

    public Transform firepoint;

    public string gunName;

    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    //public void GetAmmo()
    //{
        //currentAmmo += pickupAmount;

        //UIController.instance.ammoText.text = "AMMO: " + currentAmmo;
    //}
}
