using UnityEngine;

public class PlayerJumpLegacy : MonoBehaviour
{
    //[Legacy - charge jump]

    //[SerializeField] private float _defaultJumpForce = 0;
    //[SerializeField] private float _jumpForce = 0;
    //[SerializeField] private float _jumpForceChargeValue;
    //[SerializeField] private float _jumpForceLimit;

    //private void Start()
    //{
    //    _jumpForce = 0;
    //    _jumpForceChargeValue = 15f;
    //    _jumpForceLimit = 800f;
    //    Manager.Input.jumpAction += Jump;
    //}

    //private void FixedUpdate()
    //{
    //    if (Manager.Input.IsPressJump && _jumpForce < _jumpForceLimit)  // 차지 점프 시, 점프 파워 증가
    //    {
    //        _jumpForce += _jumpForceChargeValue;
    //    }
    //}

    //private void Jump(Rigidbody2D rb)
    //{
    //    if (Manager.Game.PlayerController.IsGround)
    //    {
    //        Debug.Log("점프");
    //        rb.constraints = RigidbodyConstraints2D.None;
    //        rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    //        _jumpForce = _defaultJumpForce;
    //    }
    //}
}
