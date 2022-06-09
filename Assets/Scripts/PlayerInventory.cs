using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] private List<char> _lettersInventory;
	[SerializeField] private LettersInUI _lettersInUI;

	public void AddLetter(char let)
	{
		if( !_lettersInventory.Contains( let ) )
		{
			_lettersInventory.Add( let );
			_lettersInUI.ShowLetters( _lettersInventory );
		}
	}

	public bool HaveLetter(char letter)
    {
		return _lettersInventory.Contains(letter);
    }

	public void UseLetter(char letter)
	{
		 if (HaveLetter (letter))
        {
			_lettersInventory.Remove(letter);
			_lettersInUI.ShowLetters(_lettersInventory);
		}
	}
}
