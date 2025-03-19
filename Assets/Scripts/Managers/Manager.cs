using UnityEngine;

public class Manager : MonoBehaviour
{
    // Singleton pattern by 복무창
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    public Transform _playerTransform;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    
}
