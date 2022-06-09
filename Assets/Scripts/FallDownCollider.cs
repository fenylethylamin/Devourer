using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownCollider : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if( other.CompareTag( "Player" ) )
			PlayerHealthController.instance.DamagePlayer( Int32.MaxValue );

	}
}
