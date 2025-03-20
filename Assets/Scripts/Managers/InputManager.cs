using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager
{
    #region 입력값
    [SerializeField] private Vector2 _moveDir;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isChargeJump;
    [SerializeField] private bool _isDown;
    [SerializeField] private bool _isChargeDown;
    [SerializeField] private bool _isDash;

    public Vector2 MoveDir { get { return _moveDir; } }
    public bool IsJump { get { return _isJump; } }
    public bool IsChargeJump { get { return _isChargeJump; } }
    public bool IsDown { get { return _isDown; } set { _isDown = value; } }
    public bool IsChargeDown { get { return _isChargeDown; } }
    public bool IsDash { get { return _isDash; } set { _isDash = value; } }
    #endregion

    #region InputSystem
    private PlayerInputSystem _playerInputSystem;
    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;
    private InputAction _downInputAction;
    private InputAction _leftDashInputAction;
    private InputAction _rightDashInputAction;
    #endregion

    #region 플레이어 액션 등록 = 실제 동작하는 로직, inputSystem에서 호출
    public Action<Vector2> moveAction;
    public Action jumpAction;
    public Action jumpChargeAction;
    public Action airStopAction;
    public Action airRotateAction;
    public Action downAction;
    public Action<Vector2> dashAction;
    #endregion

    public void Init()
    {
        _playerInputSystem = new PlayerInputSystem();

        _moveInputAction = _playerInputSystem.Player.Move;
        _jumpInputAction = _playerInputSystem.Player.Jump;
        _downInputAction = _playerInputSystem.Player.Down;
        _leftDashInputAction = _playerInputSystem.Player.LeftDash;
        _rightDashInputAction = _playerInputSystem.Player.RightDash;

        // 활성화 = 다른 오브젝트의 컴포넌트여도 자동 호출되게 세팅
        _moveInputAction.Enable();
        _jumpInputAction.Enable();
        _downInputAction.Enable();
        _leftDashInputAction.Enable();
        _rightDashInputAction.Enable();

        #region 키 입력 이벤트 등록
        _moveInputAction.performed += OnMove;
        _moveInputAction.canceled += OnMove;

        _jumpInputAction.started += OnJumpStarted;
        _jumpInputAction.canceled += OnJumpCanceled;
        
        _downInputAction.started += OnDownStarted;
        _downInputAction.canceled += OnDownCanceled;

        _leftDashInputAction.performed += OnLeftDash;
        _rightDashInputAction.performed += OnRightDash;
        #endregion

        _isJump = false;
        _isChargeJump = false;
        _isDown = false;
        _isChargeDown = false;
        _isDash = false;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveDir = context.ReadValue<Vector2>();
            //Debug.Log("이동: " + _moveDir);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveDir = Vector2.zero;
            //Debug.Log("정지: " + _moveDir);
        }
    }

    #region 차지 점프
    void OnJumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log("JumpStarted");

        if (!Manager.Game.PlayerController.IsGround)
        {
            return;
        }
        _isChargeJump = true;
        jumpChargeAction?.Invoke();
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        if (!Manager.Game.PlayerController.IsGround)
        {
            return;
        }
        _isChargeJump = false;
        _isJump = true;
        if (_isJump)
        {
            jumpAction?.Invoke();
            //Debug.Log("Jump");
            _isJump = false;
        }
    }
    #endregion

    #region 다운
    void OnDownStarted(InputAction.CallbackContext context)
    {
        if (Manager.Game.PlayerController.IsGround)
        {
            return;
        }

        if (_isDown)
        {
            return;
        }
        // 다운키 누르고 있으면 에어스탑, 때면 다운
        if (context.started)
        {
            _isChargeDown = true;
            airStopAction?.Invoke();
        }
    }
    void OnDownCanceled(InputAction.CallbackContext context)
    {
        if (!_isChargeDown)
        {
            return;
        }
        if (context.canceled)
        {
            _isChargeDown = false;
            _isDown = true;
            
            downAction?.Invoke();
        }
    }
    #endregion

    #region 대시
    void OnLeftDash(InputAction.CallbackContext context)
    {
        _isDash = true;
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("왼쪽 대시~");

            dashAction?.Invoke(Vector2.left);
            //액션에 코루틴이 있어서 isDash 세터 추가했습니다..
            //_isDash = false;
        }
    }

    void OnRightDash(InputAction.CallbackContext context)
    {
        _isDash = true;
        if (context.phase == InputActionPhase.Performed)
        {
            dashAction?.Invoke(Vector2.right);
            //_isDash = false;
        }
    }
    #endregion

    public void Clear()
    {
        /* 액션 해제 */
        // 점프
        jumpChargeAction = null;
        jumpAction = null;

        // 대시
        dashAction = null;

        // Air Stop
        airStopAction = null;

        // e다운
        downAction = null;
    }

    void OnDisable()
    {
        //_moveAction.Disable();
        //_jumpAction.Disable();
        //_downAction.Disable();
        //_leftDashAction.Disable();
        //_rightDashAction.Disable();
    }
}