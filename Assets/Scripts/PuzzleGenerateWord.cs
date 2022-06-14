using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGenerateWord : MonoBehaviour
{
    [SerializeField] private Sprite[] _letterIconsUnbroken;
    [SerializeField] private Sprite _emptyletter;
    [SerializeField] private GameObject _puzzleButtonPrefab;

    private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField] private string _correctWord;
    [SerializeField] private string _unfinishedWord;
    [SerializeField] private float _delay = 1f;

    private string _currentword = "";

    void Start()
    {
        _currentword = _unfinishedWord;

        for (int i = 0; i < _unfinishedWord.Length; i++)
        {
            var obj = Instantiate(_puzzleButtonPrefab, transform, true);

            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            if (_unfinishedWord[i] == '*')
            {
                obj.GetComponent<Image>().sprite = _emptyletter;
            }

            else
            {
                obj.GetComponent<Image>().sprite = _letterIconsUnbroken[GetLetterIndex(_correctWord[i])];
            }

            obj.GetComponent<PuzzleUIButton>().AssignLetter(_letters[i]);


        }
    }

    private int GetLetterIndex(char letter)
    {
        for (int i = 0; i < _letters.Length; i++)
        {
            if (letter == _letters[i])
                return i;
        }

        return 0;

    }

    public void AddLetter(char letter)
    {
        string s = "";
        print(_currentword);
        bool changed = false;

        for (int i = 0; i < _currentword.Length; i++)
        {
            if (_currentword[i] == '*')
            {
                if (changed)
                    s += _currentword[i];
                else
                {
                    s += letter;
                    changed = true;
                }

            }
            else
            {
                s += _currentword[i];
            }
        }
        _currentword = s;
        print(_currentword);
        RedrawWord();

        if (FilledPuzzle())
        {
            StartCoroutine(CompleteAfterDelay());
        }

    }

    private IEnumerator CompleteAfterDelay()
    {
        yield return new WaitForSeconds(_delay);
        CompletePuzzle();
    }

    private void RedrawWord()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            if (_currentword[i] == '*')
            {

                transform.GetChild(i).GetComponent<Image>().sprite = _emptyletter;

            }

            else
            {
                transform.GetChild(i).GetComponent<Image>().sprite = _letterIconsUnbroken[GetLetterIndex(_currentword[i])];
            }


        }
    }

    public bool FilledPuzzle()

    {
        for (int i = 0; i < _currentword.Length; i++)
        {

            if (_currentword[i] == '*')
                return false;

        }

        return true;

    }


    public void CompletePuzzle()

    {
        print(_currentword == _correctWord);
        if (_currentword == _correctWord)

        {
            GameManager.instance.CompleteCurrentPuzzle();
        }

        else
        {
            string s = "";
            for (int i = 0; i < _correctWord.Length; i++)

            {
                if (_correctWord[i] != _currentword[i])
                {
                    PlayerController.instance.CollectLetter(_currentword[i]);
                    s += '*';
                }
                else
                {
                    s += _correctWord[i];
                }
            }
            _currentword = s;
            RedrawWord();

        }
    }
}
    

