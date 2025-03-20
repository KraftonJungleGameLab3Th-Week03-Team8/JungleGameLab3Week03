using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //일단 rigidbody 다 가져올게요. 나중에 리팩토링 ㄱㄱ
    private Rigidbody2D _rb;
    [SerializeField] private float _jumpForce;
    private float _jumpForceChargeValue;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _jumpForce = 0;
        _jumpForceChargeValue = 5f;

        Init();
    }

    private void FixedUpdate()
    {
        if(Manager.Input.IsChargeJump)  // 차지 점프 시, 점프 파워 증가
        {
            _jumpForce += _jumpForceChargeValue;
        }
    }

    public void Init()
    {
        Manager.Input.jumpChargeAction += ConstraintPosition;
        Manager.Input.jumpAction += Jump;
    }

    private void ConstraintPosition()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void Jump()
    {
        Debug.Log("점프");

        _rb.constraints = RigidbodyConstraints2D.None;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _jumpForce = 0;
    }
}