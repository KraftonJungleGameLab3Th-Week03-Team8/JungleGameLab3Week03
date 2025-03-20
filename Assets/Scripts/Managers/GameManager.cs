using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    Transform _playerTransform;
    CameraController _cameraController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();

        /*
         TODO
        플레이어 소환 및  초기화 등
         */
    }
}