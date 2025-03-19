using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveSpeed;
    //참조를 매 프레임마다 받는건 아닌것 같아서.. 상관없나
    private Rigidbody2D _rb;

    //싱글톤 인스턴스를 미리 받아오라고요? 그런말한거같은데...
    private InputManager _inputManager;

    private void Start()
    {
        _moveSpeed = 5f;
        _inputManager = InputManager.Instance;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //if(InputManager.Instance.IsMove)
        if (!_inputManager.IsDash)
        {
            _rb.linearVelocityX = _inputManager.MoveDir.x * _moveSpeed;
        }
    }

}
