using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    #region 입력값
    [SerializeField] private Vector2 _moveDir;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isChargeJump;
    [SerializeField] private bool _isDown;
    [SerializeField] private bool _isDash;

    public Vector2 MoveDir { get { return _moveDir; } }
    public bool IsJump { get { return _isJump; } }
    public bool IsChargeJump { get { return _isChargeJump; } }
    public bool IsDown { get { return _isDown; } }
    public bool IsDash { get { return _isDash; } }
    #endregion

    #region InputSystem
    private PlayerInputSystem _playerInputSystem;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _downAction;
    private InputAction _dashAction;
    #endregion

    #region 플레이어 액션 등록 = 실제 동작하는 로직, inputSystem에서 호출
    public Action<Vector2> moveAction;
    public Action jumpAction;
    public Action jumpChargeAction;
    public Action downAction;
    public Action dashAction;
    #endregion

    public void Init()
    {
        _playerInputSystem = new PlayerInputSystem();

        _moveAction = _playerInputSystem.Player.Move;
        _jumpAction = _playerInputSystem.Player.Jump;
        //_downAction = _playerInputSystem.Player.Down;
        //_dashAction = _playerInputSystem.Player.Dash;

        // 활성화 = 다른 오브젝트의 컴포넌트여도 자동 호출되게 세팅
        _moveAction.Enable();
        _jumpAction.Enable();
        //_downAction.Enable();
        //_dashAction.Enable();

        #region 키 입력 이벤트 등록
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        _jumpAction.started += OnJumpStarted;
        _jumpAction.canceled += OnJumpCanceled;

        //_downAction.started += OnDown;
        //_downAction.canceled += OnDown;

        //_dashAction.performed += OnDash;
        #endregion

        _isJump = false;
        _isChargeJump = false;
        _isDown = false;
        _isDash = false;
    }

    //private void OnEnable()
    //{
    //    _jumpAction.Enable();
    //    _landAction.Enable();
    //}

    //private void OnDisable()
    //{
    //    _jumpAction.Disable();
    //    _landAction.Disable();
    //}

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
        _isChargeJump = true;
        jumpChargeAction?.Invoke();
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
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

    void OnDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Debug.Log("Down started");
            _isDown = true;
            downAction?.Invoke();
        }
        else if (context.canceled)
        {
            //Debug.Log("Down cancled");
            _isDown = false;
        }
    }

    void OnDash(InputAction.CallbackContext context)
    {
        _isDash = true;
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Dash started");
            dashAction?.Invoke();
            _isDash = false;
        }
    }
}