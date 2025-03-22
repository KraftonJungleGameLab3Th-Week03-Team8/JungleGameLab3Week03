using UnityEngine;

public class GameManager
{
    public PlayerController PlayerController { get { return _playerController; } }
    public CameraController CameraController { get { return _cameraController; } }
    PlayerController _playerController;
    CameraController _cameraController;

    public void Init()
    {
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();

        //TODO
        /*
        플레이어 소환 및  초기화 등
         */

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject playerPrefab = Manager.Resource.Instantiate("MCPlayerPrefab");
        _playerController = playerPrefab.GetComponentInChildren<PlayerController>();
    }
}