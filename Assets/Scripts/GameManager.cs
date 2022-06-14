using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    WalkMode,
    PuzzleMode,
    DialogueMode,
}

public class GameManager : MonoBehaviour
{

    public GameObject _gunCrosshair;

    public GameObject _puzzleUIScreen;

    public GameObject _deathScreen;

    public static GameManager instance

    {
        get;
        private set;
    }

    [SerializeField] private GameObject _crosshairObject;
    [SerializeField] private GameState _gameState;
    [SerializeField] private GameObject _invitingText;
    [SerializeField] private Monologue _monologue;
    [SerializeField] private GameObject exitPuzzleUI;
    [SerializeField] private GameObject pressTabToSwitchGunsUI;

    public int _currentPuzzleIndex = 0;

    public float waitAfterDying = 2f;

    public float waitAfterDeathScreen = 2f;

    [SerializeField] private Puzzle[] _puzzles;


    public Puzzle[] Puzzles { get => _puzzles; set => _puzzles = value; }

    public Monologue Monologue { get => _monologue; }

    private int escNo = 0;
    public bool locker = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (locker == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

         //   Debug.Log("INVISIBLE");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
         //   Debug.Log("VISIBLE");
        }
    }

    private void Awake()
    {

        Time.timeScale = 1f;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else   
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {


        SetCursor(false);
        SetGameState(GameState.WalkMode);
        InviteToPuzzle(false);
        _currentPuzzleIndex = 0;
        ActivateCurrentPuzzle();
        _gunCrosshair.SetActive(false);
        EnablePuzzleExitUI(false);
        SetActiveSwitchGunsTip(false);
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void SetGameState(GameState state)
    {
        _gameState = state;

    }
   
    public void SetCursor(bool show)
    {
        if (show)
        {
            Cursor.lockState = CursorLockMode.None;
            _crosshairObject.SetActive(false);
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _crosshairObject.SetActive(true);
        }

    }

    public void StartConversation()
    {
        SetCursor(true);
        locker = true;
        SetGameState(GameState.DialogueMode);
    }

    public void EndConversation()
    {
        SetCursor(false);
        locker = false;
        SetGameState(GameState.WalkMode);
    }

    public void ShowPuzzle()
    {
        SetCursor(true);
        locker = true;
        SetGameState(GameState.PuzzleMode);
        EnablePuzzleExitUI(true);
    }

    public void EscapePuzzle()
    {
        SetCursor(false);
        SetGameState(GameState.WalkMode);
        locker = false;
        EnablePuzzleExitUI(false);
        DisableAllPuzzleUIScreens();
    }

    public void InviteToPuzzle(bool active)

    {
        _invitingText.gameObject.SetActive(active);
    }

    public void AddLetterToCurrentPuzzle(char letter)

    {
        _puzzles[_currentPuzzleIndex].AddLetterToPuzzleGenerator(letter);
    }

    public void CompleteCurrentPuzzle()
    {
        _puzzles[_currentPuzzleIndex].CompletePuzzle();
        SetCursor(false);
        SetGameState(GameState.WalkMode);
        InviteToPuzzle(false);
        _currentPuzzleIndex++;
        ActivateCurrentPuzzle();
        locker = false;
    }

    public void SetActiveSwitchGunsTip(bool value)
    {
        pressTabToSwitchGunsUI.SetActive(value);
    }

    private void DisableAllPuzzleUIScreens()
    {
        foreach(Transform child in _puzzleUIScreen.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnablePuzzleExitUI(bool value)
    {
        exitPuzzleUI.SetActive(value);
    }

    private GameObject activatedPuzzle;

    private void ActivateCurrentPuzzle()
    {
        for (int i = 0; i < _puzzles.Length; i++)
        {
            if (_currentPuzzleIndex == i)
            {
                _puzzles[i].gameObject.SetActive(true);
                activatedPuzzle = _puzzles[i].gameObject;
                Debug.Log($"Activated {_puzzles[i].gameObject.name}");
            }
            else
            {
                if (_puzzles[i].gameObject != activatedPuzzle)
                    _puzzles[i].gameObject.SetActive(false);
                Debug.Log($"Deactivated {_puzzles[i].gameObject.name}");
            }
        }
    }

    public void PlayerDied()

    {
        StartCoroutine(PlayerDiedCo());
    }

    public IEnumerator PlayerDiedCo()

    {
        yield return new WaitForSeconds(waitAfterDying);

        _deathScreen.SetActive(true);


        yield return new WaitForSeconds(waitAfterDeathScreen);



        _deathScreen.SetActive(false);

        CheckpointManager.Instance.RespawnAtLastCheckpoint();
    }

    public void PauseUnpause()
    {
        
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;

            locker = false;

        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f;

            locker = true;

        }
    }

    public void ShowGunCrosshair()

    {
        _gunCrosshair.SetActive(true);
        locker = false;
    }

    //private void OnGUI()
    //{
      // GUI.Label(new Rect(50, 50, 200, 50), locker.ToString());
    //}
}




