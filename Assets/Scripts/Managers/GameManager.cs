using UnityEngine;

public class GameManager
{
    public bool IsGameStart => _isGameStart;
    public bool IsPause => _isPause;

    public PlayerController PlayerController =>_playerController;
    public CameraController CameraController => _cameraController;

    #region 게임 흐름 관련
    private bool _isGameStart;
    private bool _isPause;
    #endregion

    private PlayerController _playerController;
    private CameraController _cameraController;

    public void Init()
    {
        _cameraController = GameObject.FindAnyObjectByType<CameraController>();
        
        //TODO
        /*
        플레이어 소환 및  초기화 등
         */
        GameStart();
        Manager.Input.gameExitAction += GameExit;
    }

    public void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer()");
        GameObject playerPrefab = Manager.Resource.Instantiate("MCPlayerPrefab");
        playerPrefab.transform.position = new Vector3(15, 6, 0);

        _playerController = playerPrefab.GetComponentInChildren<PlayerController>();
        Manager.Input.FindPlayer();

        Debug.Log("플레이어 등록 완료");
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");

        _isGameStart = true;
        SpawnPlayer();
        _cameraController.Init(_playerController.transform);
    }

    public void SetPause()
    {
        _isPause = true;
        Time.timeScale = 0f;
    }

    public void ReleasePause()
    {
        _isPause = false;
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        Debug.LogWarning("게임 종료");
        //Manager.Instance.Clear();  // InputManager 초기화
        Application.Quit();
    }
}