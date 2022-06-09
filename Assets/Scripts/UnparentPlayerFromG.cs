using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentPlayerFromG : MonoBehaviour
{
	public Collider _gCollider;

	public void StartRotate()
	{
		// print( "start" );
		_gCollider.enabled = true;

	}
	public void StopRotate()
	{
		// print( "stop" );
		_gCollider.enabled = false;

		PlayerController.instance.PinToObject( null );
	}
}
