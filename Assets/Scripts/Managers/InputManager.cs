using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager
{
    PlayerController _playerController;
    Rigidbody2D _rb;

    #region 입력값
    private Vector2 _moveDir;
    private bool _isHoldJump;
    private bool _isPressLand;
    private bool _isPressDash;

    public Vector2 MoveDir { get { return _moveDir; } }
    public bool IsPressMove { get { return _moveDir != Vector2.zero; } }
    public bool IsHoldJump { get { return _isHoldJump; } }
    public bool IsPressLand { get { return _isPressLand; } }
    public bool IsPressDash { get { return _isPressDash; } }
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
    public Action<Rigidbody2D> jumpAction;
    //public Action<Rigidbody2D> longJumpAction;
    public Action airStopAction;
    public Action<Rigidbody2D> landAction;
    public Action<Rigidbody2D, Vector2> dashAction;
    #endregion

    public void Init()
    {
        _playerController = Manager.Game.PlayerController;
        _rb = _playerController.RB;

        _playerInputSystem = new PlayerInputSystem();

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
        _jumpInputAction.performed += OnJump;
        _jumpInputAction.canceled += OnJump;

        _downInputAction.started += OnAirStopStarted;
        _downInputAction.canceled += OnAirStopCanceled;

        _leftDashInputAction.performed += OnLeftDash;
        _rightDashInputAction.performed += OnRightDash;
        #endregion

        _isHoldJump = false;
        _isPressLand = false;
        _isPressDash = false;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveDir = context.ReadValue<Vector2>();
            _playerController.IsMove = true;
            _playerController.IsGrab = true;
            //Debug.Log("이동: " + _moveDir);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveDir = Vector2.zero;
            _playerController.IsMove = false;
            _playerController.IsGrab = false;
            //Debug.Log("정지: " + _moveDir);
        }
    }


    #region 숏점프, 롱점프 스펙트럼버전
    void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!_playerController.IsGround)
            {
                return;
            }
            Debug.Log("JumpStarted");
            
            //_playerController.IsJump = true;
            jumpAction?.Invoke(_rb);
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            _isHoldJump = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isHoldJump = false;

            // 땐다고 점프중이 아닌게 아니지만, 레이에서 체크하려니 점프시작할때 이미 체크해버려서 일단 여기서 false처리
            //_playerController.IsJump = false;
        }
    }
    #endregion

    /* 숏점프, 롱점프 두개 높이만
    #region 숏점프, 롱점프
    void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            if (!_playerController.IsGround)
            {
                return;
            }

            Debug.Log("JumpStarted");
            __isHoldJump = true;
            _playerController.IsJump = true;
            shoutJumpAction?.Invoke(_rb);
        }
        else if(context.phase == InputActionPhase.Performed)
        {
            if (!_playerController.IsJump)
            {
                return;
            }
            Debug.Log("JumpHold");
            longJumpAction?.Invoke(_rb);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            __isHoldJump = false;
            //_playerController.IsChargeJump = false;
            _playerController.IsJump = false;
        }
    }
    #endregion
    */

    /* [Legacy - charge jump]
    #region 차지 점프
    void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (!_playerController.IsGround)
        {
            return;
        }

        Debug.Log("JumpStarted");
        __isHoldJump = true;
        _playerController.ReadyJump();
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        __isHoldJump = false;
        _playerController.IsChargeJump = false;
        _playerController.IsJump = false; ;
        jumpAction?.Invoke(_rb);
    }
    #endregion
    */

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
            Debug.Log("슈히랜!");
            _isPressLand = false;
            _playerController.IsChargeLanding = false;
            _playerController.IsLanding = true;
            landAction?.Invoke(Manager.Game.PlayerController.RB);
        }
    }
    #endregion

    #region 대시
    void OnLeftDash(InputAction.CallbackContext context)
    {
        if (_playerController.IsChargeJump)
        {
            return;
        }
        if (_playerController.IsChargeLanding)
        {
            return;
        }
        if(_playerController.IsLanding)
        {
            return;
        }

        if (_playerController.IsDash)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("왼쪽 대시~");
            _playerController.IsDash = true;
            dashAction?.Invoke(_rb, Vector2.left);
        }
    }

    void OnRightDash(InputAction.CallbackContext context)
    {
        if (_playerController.IsChargeJump)
        {
            return;
        }
        if (_playerController.IsChargeLanding)
        {
            return;
        }
        if(_playerController.IsLanding)
        {
            return;
        }

        if (_playerController.IsDash)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            _playerController.IsDash = true;
            dashAction?.Invoke(_rb, Vector2.right);
        }
    }
    #endregion

    public void Clear()
    {
        /* 액션 해제 */
        jumpAction = null;     // 점프
        dashAction = null;      // 대시
        airStopAction = null;   // Air Stop
        landAction = null;      // 다운
    }
}