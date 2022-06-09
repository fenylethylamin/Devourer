using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamageOnTouch : MonoBehaviour
{
	[SerializeField] private int _damageStrength = 1;
	private void OnTriggerStay(Collider other)
	{
		if( other.CompareTag( "Player" ) )
		{
			PlayerHealthController.instance.DamagePlayer( _damageStrength );
			
		}
	}

}
