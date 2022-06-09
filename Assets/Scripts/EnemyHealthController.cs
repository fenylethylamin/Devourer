using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

	public int currentHealth;
	public EmojiAIController theEC;

	public void DamageEnemy(int damageAmount)
	{
		Debug.Log("herw");
		currentHealth -= damageAmount;

		if (theEC != null)
		{
			theEC.GetShot();
		}

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		var s = GetComponent<SpawnObjectOnDeath>();
		if (s != null) s.SpawnObject();
		Destroy(gameObject);


	}

}