using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MonologueData
{
    public string MonologueText;

    public AudioClip MonologueVO;
}

[System.Serializable]
public struct MonologuePart
{
    public MonologueData[] Monologues;
}

public class Monologue : MonoBehaviour
{
   [SerializeField] private MonologuePart[] _monologues;

   public TextMeshProUGUI _monologueWindow;
   public AudioSource _monologueAudio;

    private IEnumerator _timerCoroutine;

    public void PlayMonologue(int indexMonologue)
    {
        //@ChochosanDev handle out of bounds errors
        if (indexMonologue >= _monologues.Length)
            return;

        if(_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
        _timerCoroutine = TimerCoroutine(indexMonologue);
        StartCoroutine(_timerCoroutine);
    }

    private IEnumerator TimerCoroutine(int index)
    {
        for(int i = 0; i < _monologues[index].Monologues.Length;i++)
        {
            _monologueWindow.text = _monologues[index].Monologues[i].MonologueText;
            _monologueAudio.PlayOneShot(_monologues[index].Monologues[i].MonologueVO);
            yield return new WaitForSeconds(_monologues[index].Monologues[i].MonologueVO.length);

        }
        _monologueWindow.text = "";
        _timerCoroutine = null;
    }

}


