using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB { get { return _rb; } }
    public BoxCollider2D Collider { get { return _collider; } }
    public bool IsGround { get { return _isGround; } }
    public bool IsWall { get { return _isWall; } }
    Rigidbody2D _rb;
    BoxCollider2D _collider;
    [SerializeField] bool _isGround;
    [SerializeField] bool _isWall;

    PlayerCheckObstacle _playerCheckObstacle;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall);
    }
}