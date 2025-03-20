using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputManager _inputManager;
    [SerializeField] private float _dashForce;
    //[SerializeField] private float _dashDistance;
    // 콘테스트 : 한수찬ver 거리말고 시간으로 대시범위를 잡았습니다.
    [SerializeField] private float _dashTime;
    [SerializeField] private float _reduceDashForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dashForce = 300f;
        _dashTime = 0.25f;
        _reduceDashForce = -50f;
    }

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        _inputManager.dashAction += RemoveGravity;
        _inputManager.dashAction += Dash;
    }

    private void OnDisable()
    {
        _inputManager.dashAction -= Dash;
        _inputManager.dashAction -= RemoveGravity;
    }

    private void Dash(Vector2 dir)
    {
        //_rb.AddForce(dir * _dashForce, ForceMode2D.Impulse);
        StartCoroutine(DashCoroutine(dir));
    }

    private IEnumerator DashCoroutine(Vector2 dir)
    {
        float elapsedTime = 0f;
        _rb.AddForce(dir * _dashForce, ForceMode2D.Impulse);

        while (elapsedTime < _dashTime)
        {
            elapsedTime += Time.deltaTime;
            _rb.AddForce(dir * _reduceDashForce, ForceMode2D.Force);
            yield return null;
        }
        //yield return new WaitForSeconds(_dashTime);
    }

    private void RemoveGravity(Vector2 dir)
    {
        StartCoroutine(RemoveGravityCoroutine());
    }

    private IEnumerator RemoveGravityCoroutine()
    {
        float elapsedTime = 0f;
        while(elapsedTime < _dashTime)
        {
            elapsedTime += Time.deltaTime;
            _rb.linearVelocityY = 0f;
            yield return null;
        }
        _inputManager.IsDash = false;
    }
}
