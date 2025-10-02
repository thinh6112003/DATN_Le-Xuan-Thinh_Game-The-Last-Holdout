using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage.Enumerations;

public enum GameState
{
    Loading,
    Story,
    Home,
    LevelSelect,
    Gameplay,
    Pause,
    Win,
    Lose,
    Victory,
    Quit
}

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState CurrentState { get; private set; }

    public void ChangeStateAfter(float delay, GameState newState)
    {
        StartCoroutine(ChangeStateAfterCoroutine(delay, newState));
    }
    public IEnumerator ChangeStateAfterCoroutine(float delay, GameState newState)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(newState);
    }
    public void ChangeState(GameState newState)
    {
        if (((CurrentState == GameState.Lose || CurrentState == GameState.Win) && newState == GameState.LevelSelect) || newState == GameState.Victory)
        {
            GameManager.Instance.RemoveCurrentLevel();
        }
        switch (newState)
        {
            case GameState.Home:
                if (CurrentState == GameState.Pause)
                    GameManager.Instance.RemoveCurrentLevel();
                break;
            case GameState.Lose:
                AudioManager.Instance.PlaySFX(SFXType.Lose);
                Time.timeScale = 0f;
                break;
            case GameState.Win:
                Time.timeScale = 0f;
                AudioManager.Instance.PlaySFX(SFXType.Win);
                DataManager.Instance.SetStar();
                if (DataManager.Instance.gamePlayData.level == 4 )
                {
                    ChangeState(GameState.Victory);
                    return;
                }
                DataManager.Instance.CompleteLevel();
                Time.timeScale = 0f;
                break;

        }
        CurrentState = newState;
        UIManager.Instance.OnGameStateChanged(newState);
    }

    private void Start()
    {
        if (DataManager.Instance.userData.firstTimePlay)
            ChangeState(GameState.Story);
        else
            ChangeState(GameState.Home);
    }
}
