using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager
{
    PlayerController _playerController;
    Rigidbody2D _rb;

    #region 입력값
    private Vector2 _moveDir;
    private bool _isJumpCut;
    private bool _isPressLand;
    private bool _isPressDash;
    private bool _isPressJump;

    public Vector2 MoveDir => _moveDir;
    public bool IsJumpCut { get { return _isJumpCut; } set { _isJumpCut = value; } }
    public bool IsPressLand => _isPressLand;
    public bool IsPressDash => _isPressDash;
    public bool IsPressJump => _isPressJump;
    #endregion

    #region InputSystem
    private PlayerInputSystem _playerInputSystem;

    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;
    private InputAction _landingInputAction;
    private InputAction _leftDashInputAction;
    private InputAction _rightDashInputAction;

    private InputAction _gameStartInputAction;
    private InputAction _gameExitInputAction;
    #endregion

    #region 플레이어 액션 등록 = 실제 동작하는 로직, inputSystem에서 호출
    public Action<Rigidbody2D> jumpAction;
    public Action airStopAction;
    public Action<Rigidbody2D> landingAction;
    public Action<Rigidbody2D, Vector2> dashAction;
    public Action<Rigidbody2D> wallJumpAction;          // 벽점프
    #endregion

    #region UI 액션
    public Action gameStartAction;
    public Action gameExitAction;
    #endregion

    public void Init()
    {
        _playerInputSystem = new PlayerInputSystem();

        InitUI();
        InitPlayer();
    }

    void InitUI()
    {
        #region InputAction 할당
        _gameStartInputAction = _playerInputSystem.UI.GameStart;
        _gameExitInputAction = _playerInputSystem.UI.GameExit;
        #endregion

        #region 활성화
        _gameStartInputAction.Enable();
        _gameExitInputAction.Enable();
        #endregion

        #region 키 입력 이벤트 등록
        _gameStartInputAction.performed += OnGameStart;
        _gameExitInputAction.performed += OnGameExit;
        #endregion
    }

    void InitPlayer()
    {
        #region InputAction 할당
        _moveInputAction = _playerInputSystem.Player.Move;
        _jumpInputAction = _playerInputSystem.Player.Jump;
        _landingInputAction = _playerInputSystem.Player.Down;
        _leftDashInputAction = _playerInputSystem.Player.LeftDash;
        _rightDashInputAction = _playerInputSystem.Player.RightDash;
        #endregion

        #region 활성화
        _moveInputAction.Enable();
        _jumpInputAction.Enable();
        _landingInputAction.Enable();
        _leftDashInputAction.Enable();
        _rightDashInputAction.Enable();
        #endregion

        #region 키 입력 이벤트 등록
        _moveInputAction.performed += OnMove;
        _moveInputAction.canceled += OnMove;

        _jumpInputAction.started += OnJump;
        _jumpInputAction.canceled += OnJump;

        _landingInputAction.started += OnAirStopStarted;
        _landingInputAction.canceled += OnAirStopCanceled;

        _leftDashInputAction.performed += OnLeftDash;
        _leftDashInputAction.canceled += OnLeftDash;
        _rightDashInputAction.performed += OnRightDash;
        _rightDashInputAction.canceled += OnRightDash;
        #endregion

        _isJumpCut = false;
        _isPressLand = false;
        _isPressDash = false;
    }

    // Input, UI 초기화 후, 게임 시작 시에 찾기 용도
    public void FindPlayer()
    {
        _playerController = Manager.Game.PlayerController;
        _rb = _playerController.RB;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveDir = context.ReadValue<Vector2>();
            Manager.Game.PlayerController.IsMove = true;
            //Debug.Log("이동: " + _moveDir);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Manager.Game.PlayerController.IsMove = false;
            _moveDir = Vector2.zero;
            //Debug.Log("정지: " + _moveDir);
        }
    }

    #region 숏점프, 롱점프 스펙트럼버전
    void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _isPressJump = true;

            if (_playerController.IsHoldWall)    // 벽점프
            {
                Debug.Log("벽점프");
                wallJumpAction?.Invoke(_rb);
            }
            else if (_playerController.IsGround)
            {
                // 점프
                Debug.LogWarning("일반 점프");

                jumpAction?.Invoke(_rb);
            }
            else if(!_playerController.IsGround && _playerController.CoyoteTimeTimer > 0f)
            {
                // 코요테 점프
                if (_playerController.CoyoteTimeTimer > 0f)
                    Debug.LogWarning("코요테 타임 점프");

                jumpAction?.Invoke(_rb);
            }
            else if(!_playerController.IsGround) // 공중에 떠있을 때, 키 입력 인식
            {
                _isPressJump = true;
                if(_isPressJump)
                {
                    _playerController.IsReadyJumpBuffer = true;
                }
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressJump = false;

            if (_playerController.IsJump && _rb.linearVelocity.y > 0)
            {
                _isJumpCut = true;
            }
        }
    }
    #endregion

    #region 에어 스탑, 슈퍼 히어로 랜딩
    void OnAirStopStarted(InputAction.CallbackContext context)
    {
        bool isLockAirStopStart = _playerController.IsGround || _playerController.IsWall || _playerController.IsHoldWall;
        if (isLockAirStopStart)
            return;

        // 최소 높이 이상이고 랜딩 아니며, 코요테 타임 지난 경우에 해당 키 누르면 에어 스탑
        if (_playerController.IsCanAirStop && !_playerController.IsLanding && _playerController.CoyoteTimeTimer < 0f && context.started)
        {
            _isPressLand = true;
            _playerController.IsAirStop = true;
            airStopAction?.Invoke();
        }
    }

    void OnAirStopCanceled(InputAction.CallbackContext context) // == 슈퍼 히어로 랜딩
    {
        bool isLockSuperHeroLanding = !_playerController.IsAirStop || _playerController.IsHoldWall || !_playerController.IsCanAirStop;
        if (isLockSuperHeroLanding)
        {
            _playerController.IsAirStop = false;
            _isPressLand = false;
            return;
        }

        if (context.canceled)
        {
            Debug.Log("슈히랜!");
            _isPressLand = false;
            _playerController.IsAirStop = false;
            _playerController.IsLanding = true;
            landingAction?.Invoke(Manager.Game.PlayerController.RB);
        }
    }
    #endregion

    #region 대시
    void OnLeftDash(InputAction.CallbackContext context)
    {
        bool isLockDash = _playerController.IsAirStop || _playerController.IsLanding || _playerController.IsDash;
        if (isLockDash)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("왼쪽 대시~");
            _playerController.IsDash = true;
            _isPressDash = true;
            dashAction?.Invoke(_rb, Vector2.left);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressDash = false;
        }
    }

    void OnRightDash(InputAction.CallbackContext context)
    {
        bool isLockDash = _playerController.IsAirStop || _playerController.IsLanding || _playerController.IsDash;
        if (isLockDash)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            _playerController.IsDash = true;
            _isPressDash = true;
            dashAction?.Invoke(_rb, Vector2.right);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isPressDash = false;
        }
    }
    #endregion

    #region UI
    void OnGameStart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !Manager.Game.IsGameStart)
        {
            //InitPlayerAction(); // 플레이어 InputSystem 초기화
            gameStartAction?.Invoke();
        }
        //_gameStartInputAction = null; // 최초에 한 번만 누르면 되므로 바로 해제
    }

    void OnGameExit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && Manager.Game.IsGameStart)
        {
            gameExitAction?.Invoke();
        }
    }
    #endregion

    public void Clear()
    {
        /* UI 관련 해제 */
        _gameStartInputAction.Disable();
        _gameExitInputAction.Disable();

        _gameStartInputAction = null;
        _gameExitInputAction = null;

        /* 플레이어 관련 해제 */
        // InputAction 해제
        _moveInputAction.Disable();
        _jumpInputAction.Disable();
        _landingInputAction.Disable();
        _leftDashInputAction.Disable();
        _rightDashInputAction.Disable();

        // 액션 해제
        jumpAction = null;      // 점프
        airStopAction = null;   // Air Stop
        landingAction = null;   // 다운
        dashAction = null;      // 대시
        wallJumpAction = null;  // 벽점프
    }
}