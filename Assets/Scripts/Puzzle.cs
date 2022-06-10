using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _playerDistance = 2f;


    [SerializeField] private Animator _completePuzzleAnimation;

    [SerializeField] private PuzzleGenerateWord _puzzle;

    private void Start()
    {

    }

    private void Update()
    {


        if (Vector3.Distance(_playerTransform.position, transform.position) < _playerDistance && GameManager.instance.GetGameState()==GameState.WalkMode)
        {
            GameManager.instance.InviteToPuzzle(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                GameManager.instance.ShowPuzzle();
                _puzzle.gameObject.SetActive(true);
            }
        }

        else
        {
            GameManager.instance.InviteToPuzzle(false);
        }
    }

    public void CompletePuzzle()

    {
        _completePuzzleAnimation.SetTrigger("solve");
        _puzzle.gameObject.SetActive(false);
        GameManager.instance.EnablePuzzleExitUI(false);
    }

    public void AddLetterToPuzzleGenerator(char letter)
    {
        _puzzle.AddLetter(letter);
    }
}





