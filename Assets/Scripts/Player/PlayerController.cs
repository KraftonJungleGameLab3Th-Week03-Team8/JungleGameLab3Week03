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
    public bool IsGrabJump { get { return _isGrabJump; } set { _isGrabJump = value; } }

    public bool IsGround { get { return _isGround; } }
    public bool IsWall { get { return _isWall; } }
    public bool IsLeftWall { get { return _isLeftWall; } }
    public bool IsRightWall { get { return _isRightWall; } }
    public bool IsWallJumping { get { return _isWallJumping; } set { _isWallJumping = value; } }

    Rigidbody2D _rb;
    BoxCollider2D _collider;

    [SerializeField] bool _isMove;
    [SerializeField] bool _isJump;
    [SerializeField] bool _isChargeJump;
    [SerializeField] bool _isLanding;
    [SerializeField] bool _isChargeDown;
    [SerializeField] bool _isDash;
    [SerializeField] bool _isGrab;
    [SerializeField] bool _isGrabJump;
    
    [SerializeField] bool _isGround;
    [SerializeField] bool _isWall;
    [SerializeField] bool _isLeftWall;
    [SerializeField] bool _isRightWall;

    private bool _isWallJumping = false;

    [SerializeField] bool _isSeeRight = true;
    public bool IsSeeRight => _isSeeRight;


    Transform _visual;  // 플레이어 외형
    public Transform Visual => _visual;

    PlayerMove _playerMove;
    PlayerCheckObstacle _playerCheckObstacle;
    PlayerGrab _playerGrab;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerGrab = GetComponent<PlayerGrab>();

        _visual = transform.GetChild(0);
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall);
    }

    private void FixedUpdate()
    {
        // 1. 벽점프 중이 아닐 경우에만 Grab 가능
        if (!IsGrabJump && !IsWallJumping)
        {
            //_playerGrab.Grab(_rb);
        }



        if (_isWall)
        {
            _playerGrab.Grab(_rb);
            Debug.Log("벽 붙잡기");
        }





        // 2. Grab 상태이거나 땅에 서 있으면 이동 가능
        if (IsGrab || IsGround)
        {
            _playerMove.Move(_rb);
        }
    }

    public void ReadyJump()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _isChargeJump = true;
        _isJump = true;
    }

    public void LandOnGround()
    {
        _rb.gravityScale = 0f;
        IsLanding = false;
        IsDash = false;
        IsGrabJump = false;
        IsWallJumping = false;
        _rb.constraints = RigidbodyConstraints2D.None;
    }

    // 해당 방향으로 바라보게 플립
    public void Flip(float x)
    {
        if (x > 0)
        {
            _visual.rotation = Quaternion.identity;
            _isSeeRight = true;
        }
        else if (x < 0)
        {
            _visual.rotation = Quaternion.Euler(0, 180, 0);
            _isSeeRight = false;
        }
    }
}