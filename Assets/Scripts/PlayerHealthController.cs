using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth;
    public float invincibleLength = 1f;
    private float invincCounter;


    private void Awake()
    {
        instance = this;
    }

  
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void SetInitialHealth()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    
    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int damageAmount)
    {

        if (invincCounter <= 0)

        {
            currentHealth -= damageAmount;

            UIController.instance.ShowDamage();

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;

                GameManager.instance.PlayerDied();
            }

            invincCounter = invincibleLength;

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;

        }
    }

    public void HealPlayer (int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)

        {
            currentHealth = maxHealth;
        }

    UIController.instance.healthSlider.value = currentHealth;
    UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

}