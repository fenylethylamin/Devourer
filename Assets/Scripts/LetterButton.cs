using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterButton : MonoBehaviour
{

    [SerializeField] private char _letter;

    public void AssignLetter(char letter)
    {
        _letter = letter;

    }

    public void ButtonPress()
    {
        if (!PlayerController.instance.HasLetter(_letter))
            return;

        PlayerController.instance.UseLetter(_letter);

        GameManager.instance.AddLetterToCurrentPuzzle(_letter);

    }

}
