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

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private float _gravityScale;
    public float GravityScale { get { return _gravityScale; } set { _gravityScale = value; }}
    
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
    PlayerJump _playerJump;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerJump = GetComponent<PlayerJump>();
        _playerGrab = GetComponent<PlayerGrab>();

        _visual = transform.GetChild(0);
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall);
        //_playerCheckObstacle.CheckWall(ref _isWall, ref _isLeftWall, ref _isRightWall);

        #region 중력 깍기 : 점프, 벽, 땅 등등 다 여기서 처리하도록 이식해주세요.
        if (!_isDash)
        {
            //벽잡은 상태
            if (_isGrab)
            {
                //동영님 코드에서 이식 필요
                SetGravityScale(0f);
            }
            else if (Manager.Input.IsJumpCut)
            {
                SetGravityScale(_gravityScale * _playerJump.JumpCutGravityMultiplier);
                //여기서 벨로시티 건드려서 머지 후 테스트할때 여기 체크 한번해야해요. 내용은 최대 떨어지는 속도 체크입니다.
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));
            }
            // 호버링
            else if (_isJump && Mathf.Abs(_rb.linearVelocity.y) < _playerJump.JumpHangTime)
            {
                SetGravityScale(_gravityScale * _playerJump.JumpHangGravityMultiplier);
            }
            else if (_rb.linearVelocityY < 0)
            {
                SetGravityScale(_gravityScale * _playerJump.FallGravityMultiplier);
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));
            }
            else
            {
                SetGravityScale(_gravityScale);
            }
        }
        //대시 적용 때 중력
        else
        {
            //동영님 코드에서 이식 필요
            SetGravityScale(_gravityScale);
        }
        #endregion
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
        _playerMove.Move(_rb);
        _playerGrab.Grab(_rb);
    }

    public void SetGravityScale(float gravityScale)
    {
        _rb.gravityScale = gravityScale;
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
        _rb.gravityScale = 0f;
        IsLanding = false;
        IsDash = false;
        IsGrabJump = false;
        IsWallJumping = false;
        _rb.constraints = RigidbodyConstraints2D.None;
        _isJump = false;
        _isLanding = false;
        _isDash = false;
        Manager.Input.IsJumpCut = false;
        SetGravityScale(0f);
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