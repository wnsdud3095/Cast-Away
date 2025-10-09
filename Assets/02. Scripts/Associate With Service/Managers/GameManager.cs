using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameEventType GameType { get; private set; }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.LOGIN, Login);
        GameEventBus.Subscribe(GameEventType.LOADING, Loading);
    } 

    private void OnDisable()
    {
        GameEventBus.Subscribe(GameEventType.LOGIN, Login);
        GameEventBus.Subscribe(GameEventType.LOADING, Loading);
    }

    private void Login()
    {
        GameType = GameEventType.LOGIN;
    }

    private void Loading()
    {
        GameType = GameEventType.LOADING;
    }

    public void InPlay()
    {
        GameType = GameEventType.INPLAY;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Interacting()
    {
        GameType = GameEventType.INTERACTING;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        GameType = GameEventType.PAUSE;
    }

    public void GameClear()
    {
        GameType = GameEventType.GAMECLEAR;
    }

    public void GameOver()
    {
        GameType = GameEventType.GAMEOVER;
    }
}
