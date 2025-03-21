using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //일단 rigidbody 다 가져올게요. 나중에 리팩토링 ㄱㄱ
    private Rigidbody2D _rb;
    private InputManager _inputManager;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpForceLimit;
    private float _jumpForceChargeValue;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_inputManager = InputManager.Instance;
        _jumpForce = 180f;
        _jumpForceChargeValue = 15f;
        _jumpForceLimit = 800f;
    }

    private void FixedUpdate()
    {
        if(_inputManager.IsChargeJump && _jumpForce < _jumpForceLimit)
        {
            _jumpForce += _jumpForceChargeValue;
        }
    }

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        _inputManager.jumpChargeAction += ConstraintPosition;
        _inputManager.jumpAction += ReleaseConstraint;
        _inputManager.jumpAction += Jump;
    }

    private void OnDisable()
    {
        _inputManager.jumpChargeAction -= ConstraintPosition;
        _inputManager.jumpAction -= ReleaseConstraint;
        _inputManager.jumpAction -= Jump;
    }

    private void ConstraintPosition()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void ReleaseConstraint()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _jumpForce = 180f;
    }
}
