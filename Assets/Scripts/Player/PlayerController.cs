using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 물리 관련
    public Rigidbody2D RB => _rb;
    public BoxCollider2D Collider => _collider;
    public float GravityScale { get { return _gravityScale; } set { _gravityScale = value; } }
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private float _gravityScale;
    #endregion

    #region 플레이어 UI 관련
    public TextMeshPro MumbleText => _mumbleText;   // 혼잣말
    TextMeshPro _mumbleText;
    #endregion

    #region 상태 관련
    public bool IsMove { get { return _isMove; } set { _isMove = value; } }
    public bool IsJump { get { return _isJump; } set { _isJump = value; } }
    public bool IsLanding { get { return _isLanding; } set { _isLanding = value; } }
    public bool IsAirStop { get { return _isAirStop; } set { _isAirStop = value; } }
    public bool IsDash { get { return _isDash; } set { _isDash = value; } }
    public bool IsGround => _isGround;

    [SerializeField] bool _isSeeRight = true;
    [SerializeField] bool _isMove;
    [SerializeField] bool _isJump;
    [SerializeField] bool _isLanding;
    [SerializeField] bool _isAirStop;
    [SerializeField] bool _isDash;    
    [SerializeField] bool _isGround;

    Coroutine _dashCoolTimeCoroutine;   // 대시 쿨타임 코루틴 (땅 끊기는 문제 방지용)
    #endregion

    #region 벽 관련
    public bool IsWall => _isWall;
    public bool IsGrabJump { get { return _isGrabJump; } set { _isGrabJump = value; } }
    public bool IsHoldWall { get { return _isHoldWall; } set { _isHoldWall = value; } }
    [SerializeField] bool _isWall;
    [SerializeField] bool _isGrabJump;
    [SerializeField] bool _isHoldWall = false;
    #endregion

    #region 플레이어 외형
    public bool IsSeeRight => _isSeeRight;
    public Transform Visual => _visual;
    Transform _visual;  // 플레이어 외형
    #endregion

    #region 플레이어 기능 관련
    public PlayerAirStop PlayerAirStop => _playerAirStop;
    public PlayerLanding PlayerLanding => _playerLanding;
    PlayerMove _playerMove;
    PlayerCheckObstacle _playerCheckObstacle;
    PlayerGrab _playerGrab;
    PlayerJump _playerJump;
    PlayerAirStop _playerAirStop;
    PlayerLanding _playerLanding;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerJump = GetComponent<PlayerJump>();
        _playerGrab = GetComponent<PlayerGrab>();
        _playerAirStop = GetComponent<PlayerAirStop>();

        _visual = transform.GetChild(0);
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall);

        #region 중력 깍기 : 점프, 벽, 땅 등등 다 여기서 처리하도록 이식해주세요.
        if(_isGround)
        {
            SetGravityScale(0f);
        }
        else if (!_isDash)
        {
            if (_isGrabJump && _rb.linearVelocityY > 0)
            {
                SetGravityScale(_gravityScale);
                Debug.Log("벽 점프");
                return;
            }
            if (_isGrabJump && _rb.linearVelocityY < 0)
            {
                SetGravityScale(_gravityScale * _playerJump.FallGravityMultiplier);
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));
                Debug.Log("벽 점프이후 추락 중력");
                return;
            }
            if (Manager.Input.IsJumpCut)
            {
                SetGravityScale(_gravityScale * _playerJump.JumpCutGravityMultiplier);
                //여기서 벨로시티 건드려서 머지 후 테스트할때 여기 체크 한번해야해요. 내용은 최대 떨어지는 속도 체크입니다.
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));

                Debug.Log("중력1");
            }
            // 호버링
            else if (_isJump && Mathf.Abs(_rb.linearVelocity.y) < _playerJump.JumpHangTime)
            {
                SetGravityScale(_gravityScale * _playerJump.JumpHangGravityMultiplier);

                Debug.Log("중력2");
            }
            else if (_rb.linearVelocityY < 0)
            {
                //if (_playerJump == null)
                //    Debug.Log("플레이어 점프가 널");

                SetGravityScale(_gravityScale * _playerJump.FallGravityMultiplier);
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));

                Debug.Log("중력3");
            }
            else // 평소 중력
            {
                SetGravityScale(_gravityScale);

                Debug.Log("중력4");
            }
        }
        else
        {
            if (_rb.linearVelocityY < 0)
            {
                //if (_playerJump == null)
                //    Debug.Log("플레이어 점프가 널");

                SetGravityScale(_gravityScale * _playerJump.FallGravityMultiplier);
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_playerJump.MaxFallSpeed));

                Debug.Log("중력3-1");
            }
            else // 평소 중력
            {
                SetGravityScale(_gravityScale);

                Debug.Log("중력4-1");
            }

            //SetGravityScale(_gravityScale);
            //Debug.Log("중력5");
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (_isWall && !_isGround && (_isMove || _isDash)) // 벽 감지, 공중, 이동 및 대시 중
        {
            HoldWallState();
            _playerGrab.Grab(_rb);
        }
        _playerMove.Move(_rb);
    }

    public void SetGravityScale(float gravityScale)
    {
        _rb.gravityScale = gravityScale;
    }

    public void LandOnGroundState()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = 1f;  // 중력 복구

        _isHoldWall = false;    // 벽 붙잡기 X
        //_isDash = false;        // 대시 X

        if(_dashCoolTimeCoroutine == null)
        {
            _dashCoolTimeCoroutine = StartCoroutine(WaitDashCoroutine());
        }

        _isLanding = false;     // 랜딩 X
        _isJump = false;        // 점프 X
        _isGrabJump = false;    // 벽 점프 X
        Manager.Input.IsJumpCut = false;
    }

    private IEnumerator WaitDashCoroutine()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        _isDash = false;
        _dashCoolTimeCoroutine = null;
    }

    public void HoldWallState()
    {
        _isDash = false;        // 다시 대시 가능하게 하기 위함
        _isHoldWall = true;
        _isJump = false;
        _isGrabJump = false;
    }

    public void DetachWallState()
    {
        _isDash = false;        // 다시 대시 가능하게 하기 위함
        _isHoldWall = false;
        _isJump = true;
        _isGrabJump = true;
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