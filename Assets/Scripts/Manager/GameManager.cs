using Behaviors;
using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public float secondsBeforeRestart;
    public PauseMenu pauseMenu;
    public VictoryMenu victoryMenu;
    public DialogueManager dialogueManager;
    public KeyPannel keyPannel;

    public static Action OnRestart;
    public static Action OnVictory;
    public static Action OnExit;
    public static Action OnHealthAtZero;
    public static GameManager instance
    {
        get; private set; 
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HealthSystem.OnEnemyGrab -= GameOver;
        HealthSystem.OnEnemyGrab += GameOver;

        AudioManager.Initialize();
        AudioManager.PlaySound(AudioManager.Sound.Background, true);
    }

    public void GameOver() 
    {
        Debug.Log("Game over!");
        StartCoroutine(Restart());
    }

    public void RestartFromMenu()
    {
        StartCoroutine(Restart(0f));
    }

    private IEnumerator Restart(float time = -1)
    {
        if (time == -1)
            time = secondsBeforeRestart;
        
        yield return new WaitForSecondsRealtime(time);
        
        OnHealthAtZero?.Invoke();
        OnRestart?.Invoke();
    }

    public void Victory() 
    {
        OnVictory?.Invoke();
        Debug.Log("Game won!");
    }

    public void ExitGame()
    {
        OnExit?.Invoke();
        Debug.Log("Game won!");
    }

    public bool uiOnScreen()
    {
        return dialogueManager.isOnScreen || pauseMenu.isPaused || victoryMenu.victoryMenuUI.activeSelf || keyPannel.isOnScreen;
    }
}
