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

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    [SerializeField] private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }
    private float _gravityScale;
    public float GravityScale { get { return _gravityScale; } set { _gravityScale = value; }}
    
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
        _collider = GetComponent<BoxCollider2D>();
        _playerTransform = GetComponent<Transform>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerJump = GetComponent<PlayerJump>();
        _playerGrab = GetComponent<PlayerGrab>();
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall, ref _isLeftWall, ref _isRightWall);

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
        _rb.constraints = RigidbodyConstraints2D.None;
        _isJump = false;
        _isLanding = false;
        _isDash = false;
        Manager.Input.IsJumpCut = false;
        SetGravityScale(0f);
    }
}