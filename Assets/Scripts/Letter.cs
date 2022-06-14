using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
	[SerializeField] private char _letter;
	[SerializeField] private float floatSpeed = 0.4f;
	[SerializeField] private float timeInOneDirection = 1f;

	private float timeRemaining;
	private bool isGoingUp = true;

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
		//transform.Rotate(Vector3.forward);

		if(isGoingUp)
        {
			transform.position += transform.up * Time.deltaTime * floatSpeed;
		}
		else
        {
			transform.position += -transform.up * Time.deltaTime * floatSpeed;
		}

		timeRemaining -= Time.deltaTime;

		if(timeRemaining <= 0f)
        {
			timeRemaining = timeInOneDirection;
			isGoingUp = !isGoingUp;
        }
    }
}
