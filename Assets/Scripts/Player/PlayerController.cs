using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB { get { return _rb; } }
    public BoxCollider2D Collider { get { return _collider; } }

    public bool IsMove { get { return _isMove; } set { _isMove = value; } }
    public bool IsJump { get { return _isJump; } set { _isJump = value; } }
    public bool IsChargeJump { get { return _isChargeJump; } set { _isChargeJump = value; } }
    public bool IsLanding { get { return _isLanding; } set { _isLanding = value; } }
    public bool IsChargeLanding { get { return _isChargeDown; } set { _isChargeDown = value; } }
    public bool IsDash { get { return _isDash; } set { _isDash = value; } }

    public bool IsGround { get { return _isGround; } }
    public bool IsWall { get { return _isWall; } }

    Rigidbody2D _rb;
    BoxCollider2D _collider;

    [SerializeField] bool _isMove;
    [SerializeField] bool _isJump;
    [SerializeField] bool _isChargeJump;
    [SerializeField] bool _isLanding;
    [SerializeField] bool _isChargeDown;
    [SerializeField] bool _isDash;
    
    [SerializeField] bool _isGround;
    [SerializeField] bool _isWall;

    PlayerMove _playerMove;
    PlayerCheckObstacle _playerCheckObstacle;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall);
    }

    private void FixedUpdate()
    {
        _playerMove.Move(_rb);
    }

    public void ReadyJump()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _isChargeJump = true;
        _isJump = true;
    }
}