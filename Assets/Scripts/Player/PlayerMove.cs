using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveSpeed;
    private Rigidbody2D _rb;

    private void Start()
    {
        _moveSpeed = 5f;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(!Manager.Input.IsDash)
        {
            _rb.linearVelocityX = Manager.Input.MoveDir.x * _moveSpeed;
        }
        else
        {
            Debug.Log("무브 안됨(IsDash)");
        }
    }
}
