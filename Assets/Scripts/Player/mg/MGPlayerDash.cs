using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MGPlayerDash : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputManager _inputManager;
    [SerializeField] private float _dashDistance = 4f;
    [SerializeField] private float _dashTime = 0.5f;
    private float _prevGravityScale;
    private bool _isDashed;
    private bool _isDashing;
    private float _dashDeltaTime;
    private Vector2 _startPos;
    private Vector2 _destPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _isDashed = true;
        _isDashing = false;
}

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        _inputManager.dashAction += Dash;
        //_inputManager.airStopAction += StopDash;
    }

    private void OnDisable()
    {
        _inputManager.dashAction -= Dash;
        //_inputManager.airStopAction -= StopDash;
    }

    private void FixedUpdate()
    {
        if (!_isDashing)
            return;
        
        if (_dashDeltaTime < _dashTime) // Dashing
        {
            _rb.MovePosition(Vector2.Lerp(_startPos, _destPos, _dashDeltaTime / _dashTime));
            _dashDeltaTime += Time.fixedDeltaTime;
        }

        if (_dashDeltaTime >= _dashTime)
        {
            _rb.MovePosition(_destPos);

            StopDash();
        }
    }

    private void Dash(Vector2 dir)
    {
        if (_isDashed && _isDashing)
            return;

        _startPos = _rb.position;
        _destPos = new Vector2(_rb.position.x + _dashDistance * dir.x, _rb.position.y);

        _prevGravityScale = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.linearVelocity = Vector2.zero;
        _dashDeltaTime = 0f;
        _isDashing = true;
    }

    //  대시중 중단 시, 호출하는게 좋다 생각함
    private void StopDash()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.gravityScale = _prevGravityScale;
        _isDashed = true;
        _isDashing = false;
        _inputManager.IsDash = false;
    }

    [ContextMenu("Dash(right)")]
    private void TestDashRight()
    {
        Dash(Vector2.right);
    }

    [ContextMenu("Dash(left)")]
    private void TestDashLeft()
    {
        Dash(Vector2.left);
    }
}
