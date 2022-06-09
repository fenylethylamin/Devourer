using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetParentOnG : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if( other.CompareTag( "Player" ) )
		{
			PlayerController.instance.PinToObject( transform );
			// EditorApplication.isPaused = true;
		}
	}

	// private void OnTriggerExit(Collider other)
	// {
	// 	if( other.CompareTag( "Player" ) )
	// 	{
	// 		PlayerController.instance.transform.SetParent( null );
	// 	}
	// }
}
