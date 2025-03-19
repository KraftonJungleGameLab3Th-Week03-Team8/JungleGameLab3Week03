using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //일단 rigidbody 다 가져올게요. 나중에 리팩토링 ㄱㄱ
    private Rigidbody2D _rb;
    private float _jumpForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _jumpForce = 15;
    }

    private void OnEnable()
    {
        InputManager.Instance.jumpChargeAction += ConstraintPosition;
        InputManager.Instance.jumpAction += ReleaseConstraint;
        InputManager.Instance.jumpAction += Jump;
    }

    private void OnDisable()
    {
        InputManager.Instance.jumpChargeAction -= ConstraintPosition;
        InputManager.Instance.jumpAction -= ReleaseConstraint;
        InputManager.Instance.jumpAction -= Jump;
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
    }
}
