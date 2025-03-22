using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB => _rb;
    public BoxCollider2D Collider => _collider;

    public TextMeshPro MumbleText => _mumbleText;

    public bool IsMove { get { return _isMove; } set { _isMove = value; } }
    public bool IsJump { get { return _isJump; } set { _isJump = value; } }
    public bool IsChargeJump { get { return _isChargeJump; } set { _isChargeJump = value; } }
    public bool IsLanding { get { return _isLanding; } set { _isLanding = value; } }
    public bool IsChargeLanding { get { return _isChargeDown; } set { _isChargeDown = value; } }
    public bool IsDash { get { return _isDash; } set { _isDash = value; } }
    public bool IsGrab { get { return _isGrab; } set { _isGrab = value; } }

    public bool IsGround => _isGround;
    public bool IsWall => _isWall;
    public bool IsLeftWall => _isLeftWall;
    public bool IsRightWall => _isRightWall;

    Rigidbody2D _rb;
    BoxCollider2D _collider;
    TextMeshPro _mumbleText;

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

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _mumbleText = GetComponentInChildren<TextMeshPro>();

        _playerMove = GetComponent<PlayerMove>();
        _playerCheckObstacle = GetComponent<PlayerCheckObstacle>();
        _playerGrab = GetComponent<PlayerGrab>();
    }

    private void Update()
    {
        _playerCheckObstacle.CheckGround(ref _isGround);
        _playerCheckObstacle.CheckWall(ref _isWall, ref _isLeftWall, ref _isRightWall);
    }

    private void FixedUpdate()
    {
        _playerGrab.Grab(_rb);
        if (!_isGrab)
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
        _rb.constraints = RigidbodyConstraints2D.None;
    }
}