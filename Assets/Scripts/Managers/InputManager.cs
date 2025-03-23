using System;
using System.Collections;
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

    public Vector2 MoveDir => _moveDir;
    public bool IsPressMove => _moveDir != Vector2.zero;
    public bool IsJumpCut { get { return _isJumpCut; } set { _isJumpCut = value; } }
    public bool IsPressLand => _isPressLand;
    public bool IsPressDash => _isPressDash;
    #endregion

    #region InputSystem
    private PlayerInputSystem _playerInputSystem;
    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;
    private InputAction _downInputAction;
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
    public Action<Rigidbody2D> wallJumpAction;   // 벽점프
    #endregion

    #region UI 액션 등록
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
        _downInputAction = _playerInputSystem.Player.Down;
        _leftDashInputAction = _playerInputSystem.Player.LeftDash;
        _rightDashInputAction = _playerInputSystem.Player.RightDash;
        #endregion

        #region 활성화
        _moveInputAction.Enable();
        _jumpInputAction.Enable();
        _downInputAction.Enable();
        _leftDashInputAction.Enable();
        _rightDashInputAction.Enable();
        #endregion

        #region 키 입력 이벤트 등록
        _moveInputAction.performed += OnMove;
        _moveInputAction.canceled += OnMove;

        _jumpInputAction.started += OnJump;
        //_jumpInputAction.performed += OnJump;
        _jumpInputAction.canceled += OnJump;

        _downInputAction.started += OnAirStopStarted;
        _downInputAction.canceled += OnAirStopCanceled;

        _leftDashInputAction.performed += OnLeftDash;
        _rightDashInputAction.performed += OnRightDash;
        #endregion

        //_isHoldJump = false;
        _isJumpCut = false;
        _isPressLand = false;
        _isPressDash = false;
    }

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
            Debug.Log("이동: " + _moveDir);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Manager.Game.PlayerController.IsMove = false;
            _moveDir = Vector2.zero;
            Debug.Log("정지: " + _moveDir);
        }
    }

    #region 숏점프, 롱점프 스펙트럼버전
    void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_playerController.IsHoldWall)    // 벽점프
            {
                Debug.Log("벽점프");
                wallJumpAction?.Invoke(_rb);
            }
            else if (_playerController.IsGround)
            {
                // 점프
                jumpAction?.Invoke(_rb);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
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

        // 다운키 누르고 있으면 에어스탑, 때면 다운
        if (!_playerController.IsLanding && context.started)
        {
            _isPressLand = true;
            _playerController.IsChargeLanding = true;
            airStopAction?.Invoke();
        }
    }
    void OnAirStopCanceled(InputAction.CallbackContext context)
    {
        bool isLockSuperHeroLanding = !_playerController.IsChargeLanding || _playerController.IsHoldWall;
        if(isLockSuperHeroLanding)
            return;

        if (context.canceled)
        {
            Debug.Log("슈히랜!");
            _isPressLand = false;
            _playerController.IsChargeLanding = false;
            _playerController.IsLanding = true;
            landingAction?.Invoke(Manager.Game.PlayerController.RB);
        }
    }
    #endregion

    #region 대시
    void OnLeftDash(InputAction.CallbackContext context)
    {
        bool isLockDash = _playerController.IsChargeLanding || _playerController.IsLanding || _playerController.IsDash;
        if (isLockDash)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("왼쪽 대시~");
            _playerController.IsDash = true;
            dashAction?.Invoke(_rb, Vector2.left);
        }
    }

    void OnRightDash(InputAction.CallbackContext context)
    {
        bool isLockDash = _playerController.IsChargeLanding || _playerController.IsLanding || _playerController.IsDash ;
        if (isLockDash)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            _playerController.IsDash = true;
            dashAction?.Invoke(_rb, Vector2.right);
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
        /* 액션 해제 */
        jumpAction = null;     // 점프
        dashAction = null;      // 대시
        airStopAction = null;   // Air Stop
        landingAction = null;      // 다운
    }
}