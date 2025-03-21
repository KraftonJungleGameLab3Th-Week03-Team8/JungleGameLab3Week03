using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _defaultJumpForce = 0;
    [SerializeField] private float _jumpForce = 0;
    [SerializeField] private float _jumpForceChargeValue;

    private void Start()
    {
        _jumpForce = 0;
        _jumpForceChargeValue = 5f;
        Manager.Input.jumpAction += Jump;
    }

    private void FixedUpdate()
    {
        if(Manager.Input.IsPressJump)  // 차지 점프 시, 점프 파워 증가
        {
            _jumpForce += _jumpForceChargeValue;
        }
    }

    private void Jump(Rigidbody2D rb)
    {
        if (Manager.Game.PlayerController.IsGround)
        {
            Debug.Log("점프");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpForce = _defaultJumpForce;
        }
    }
}