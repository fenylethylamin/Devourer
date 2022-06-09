using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LettersInUI : MonoBehaviour
{
	[SerializeField] private GameObject _letterInUIPrefab;

	[SerializeField] private Sprite[] _letterIconsUnbroken;
	[SerializeField] private Sprite[] _letterIconsBroken;

	[SerializeField] private Color _ownedColor;
	[SerializeField] private Color _unownedColor;

	private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	void Start()
	{
		for( int i = 0; i < _letters.Length; i++ )
		{
			var obj = Instantiate( _letterInUIPrefab, transform, true );

			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;

			obj.GetComponent<Image>().sprite = _letterIconsBroken[ i ];

			obj.GetComponent<LetterButton>().AssignLetter(_letters[i]);


		}
	}

	public void ShowLetters(List<char> letters)
	{
		for( int i = 0; i < _letters.Length; i++ )
		{
			if( letters.Contains( _letters[ i ] ) )
			{

				transform.GetChild(i).GetComponent<Image>().sprite = _letterIconsUnbroken[i];
			}
			else
			{
				transform.GetChild(i).GetComponent<Image>().sprite = _letterIconsBroken[i];
			}
		}
	}
}
