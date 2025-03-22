using UnityEngine;

public class GameManager
{
    public bool IsGameStart => _isGameStart;
    public PlayerController PlayerController =>_playerController;
    public CameraController CameraController => _cameraController;
    private bool _isGameStart;
    private PlayerController _playerController;
    private CameraController _cameraController;


    public void Init()
    {
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();

        //TODO
        /*
        플레이어 소환 및  초기화 등
         */
        GameStart();
        Manager.Input.gameExitAction += GameExit;
    }

    public void SpawnPlayer()
    {
        GameObject playerPrefab = Manager.Resource.Instantiate("MCPlayerPrefab");
        _playerController = playerPrefab.GetComponentInChildren<PlayerController>();
        Manager.Input.FindPlayer();

        Debug.Log("플레이어 등록 완료");
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");

        _isGameStart = true;
        SpawnPlayer();
        _cameraController.SetTarget(_playerController.transform);
    }

    public void GameExit()
    {
        Debug.LogWarning("게임 종료");
        //Manager.Instance.Clear();  // InputManager 초기화
        Application.Quit();
    }
}