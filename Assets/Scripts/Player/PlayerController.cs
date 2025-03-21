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
    public bool IsGrab { get { return _isGrab; } set { _isGrab = value; } }

    public bool IsGround { get { return _isGround; } }
    public bool IsWall { get { return _isWall; } }
    public bool IsLeftWall { get { return _isLeftWall; } }
    public bool IsRightWall { get { return _isRightWall; } }

    Rigidbody2D _rb;
    private float _gravityScale;
    BoxCollider2D _collider;

    [SerializeField] bool _isMove;
    [SerializeField] bool _isJump;
    [SerializeField] bool _isChargeJump;
    [SerializeField] bool _isLanding;
    [SerializeField] bool _isChargeDown;
    [SerializeField] bool _isDash;
    [SerializeField] bool _isGrab;
    
    [SerializeField] bool _isGround;
    [SerializeField] bool _isWall;
    [SerializeField] bool _isLeftWall;
    [SerializeField] bool _isRightWall;

    PlayerMove _playerMove;
    PlayerCheckObstacle _playerCheckObstacle;
    PlayerGrab _playerGrab;
    PlayerJump _playerJump;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gravityScale = _rb.gravityScale;
        _collider = GetComponent<BoxCollider2D>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerJump = GetComponent<PlayerJump>();
        _playerGrab = GetComponent<PlayerGrab>();
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall, ref _isLeftWall, ref _isRightWall);
    }

    private void FixedUpdate()
    {
        _playerMove.Move(_rb);
        _playerGrab.Grab(_rb);
        if (Manager.Game.PlayerController.IsJump && !_isGrab)
        {
            _playerJump.controlJumpGravity(_rb);
        }
    }

    /* [Legacy - charge jump]
    public void ReadyJump()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _isChargeJump = true;
        _isJump = true;
    }
    */

    public void LandOnGround()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _isJump = false;
        _isLanding = false;
        _isDash = false;
        _rb.gravityScale = _gravityScale;
    }
}