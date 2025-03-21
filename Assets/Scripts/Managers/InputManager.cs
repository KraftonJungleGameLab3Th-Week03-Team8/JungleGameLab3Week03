using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager
{
    PlayerController _playerController;
    Rigidbody2D _rb;

    #region 입력값
    [SerializeField] private Vector2 _moveDir;
    [SerializeField] private bool _isPressJump;
    [SerializeField] private bool _isPressLand;
    [SerializeField] private bool _isPressChargeDown;
    [SerializeField] private bool _isPressDash;

    public Vector2 MoveDir { get { return _moveDir; } }
    public bool IsPressMove { get { return _moveDir != Vector2.zero; } }
    public bool IsPressJump { get { return _isPressJump; } }
    public bool IsPressLand { get { return _isPressLand; } }
    public bool IsPressDash { get { return _isPressDash; } set { _isPressDash = value; } }
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
    public Action<Rigidbody2D> jumpAction;
    public Action<Rigidbody2D> jumpChargeAction;
    public Action airStopAction;
    public Action airRotateAction;
    public Action<Rigidbody2D> downAction;
    public Action<Rigidbody2D, Vector2> dashAction;
    #endregion

    public void Init()
    {
        _playerController = Manager.Game.PlayerController;
        _rb = _playerController.RB;

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
        
        _downInputAction.started += OnAirStopStarted;
        _downInputAction.canceled += OnAirStopCanceled;

        _leftDashInputAction.performed += OnLeftDash;
        _rightDashInputAction.performed += OnRightDash;
        #endregion

        _isPressJump = false;
        _isPressLand = false;
        _isPressChargeDown = false;
        _isPressDash = false;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveDir = context.ReadValue<Vector2>();
            _playerController.IsMove = true;
            //Debug.Log("이동: " + _moveDir);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveDir = Vector2.zero;
            _playerController.IsMove = false;
            //Debug.Log("정지: " + _moveDir);
        }
    }

    #region 차지 점프
    void OnJumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log("JumpStarted");
        _isPressJump = true;
        _playerController.ReadyJump();
        //jumpChargeAction?.Invoke(Manager.Game.PlayerController.RB);
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _isPressJump = false;
        _playerController.IsChargeJump = false;
        _playerController.IsJump = false; ;
        jumpAction?.Invoke(_rb);
    }
    #endregion

    #region 에어 스탑, 슈퍼 히어로 랜딩
    void OnAirStopStarted(InputAction.CallbackContext context)
    {
        if (Manager.Game.PlayerController.IsGround)
        {
            return;
        }

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
        if (!_playerController.IsChargeLanding)
        {
            return;
        }

        if (context.canceled)
        {
            _isPressLand = false;
            Debug.Log("슈히랜!");
            _playerController.IsChargeLanding = false;
            _playerController.IsLanding = true;
            downAction?.Invoke(Manager.Game.PlayerController.RB);
        }
    }
    #endregion

    #region 대시
    void OnLeftDash(InputAction.CallbackContext context)
    {
        _isPressDash = true;
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("왼쪽 대시~");

            dashAction?.Invoke(_rb, Vector2.left);
            //액션에 코루틴이 있어서 isDash 세터 추가했습니다..
            //_isDash = false;
        }
    }

    void OnRightDash(InputAction.CallbackContext context)
    {
        _isPressDash = true;
        if (context.phase == InputActionPhase.Performed)
        {
            dashAction?.Invoke(_rb, Vector2.right);
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
}