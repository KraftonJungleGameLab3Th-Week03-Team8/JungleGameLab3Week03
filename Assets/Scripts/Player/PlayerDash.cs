using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float _dashForce;
    //[SerializeField] private float _dashDistance;
    // 콘테스트 : 한수찬ver 거리말고 시간으로 대시범위를 잡았습니다.
    [SerializeField] private float _dashTime;
    [SerializeField] private float _reduceDashForce;

    private void Start()
    {
        _dashForce = 1000f;
        _dashTime = 0.25f;
        _reduceDashForce = -2000f;

        Manager.Input.dashAction += Dash;
    }

    private void Dash(Rigidbody2D rb, Vector2 dir)
    {
        StartCoroutine(DashCoroutine(rb, dir));
    }

    private IEnumerator DashCoroutine(Rigidbody2D rb, Vector2 dir)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        float elapsedTime = 0f;
        rb.AddForce(dir * _dashForce, ForceMode2D.Impulse);

        while (elapsedTime < _dashTime && Manager.Input.IsPressDash)
        {
            elapsedTime += Time.fixedDeltaTime;
            // 감속
            rb.AddForce(dir * _reduceDashForce, ForceMode2D.Force);
            _reduceDashForce -= Time.fixedDeltaTime * 3000f;
            yield return new WaitForFixedUpdate();
        }
        Manager.Input.IsPressDash = false;
        _reduceDashForce = -1800f;

        if (!Manager.Input.IsPressLand && !Manager.Input.IsPressLand)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}