using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
	[SerializeField] private char _letter;

	public char GetLetter()
	{
		return _letter;
	}

	private void OnTriggerEnter(Collider other)
	{
		if( other.CompareTag( "Player" ) )
		{
			PlayerController.instance.CollectLetter(_letter);
			Destroy( gameObject );
		}
	}

    private void Update()
    {
		transform.Rotate(Vector3.forward);

    }
}
