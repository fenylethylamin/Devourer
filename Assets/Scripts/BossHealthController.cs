using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{

	public int currentHealth;
	public EnemyController theEC;
	public bool activatePhone = false;
	public GameObject phone;

	public void DamageEnemy(int damageAmount)
	{
		currentHealth -= damageAmount;

		if (theEC != null)
		{
			theEC.GetShot();
		}

		if (currentHealth <= 0)
		{
			if(activatePhone)
            {
				phone.SetActive(true);
            }
			Die();
		}
	}

	public void Die()
	{
		var s = GetComponent<SpawnObjectOnDeath>();
		if (s != null) s.SpawnObject();
		
		Destroy(gameObject);
	}

}