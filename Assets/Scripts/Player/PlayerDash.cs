using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Tooltip("대쉬 힘")] [SerializeField] private float _force;          // 플래시 점프 거리 조절
    [Tooltip("대쉬 각도")] [SerializeField] private float _acceleration; // 정규화 취해서 각도로 사용 (클수록 위를 향함)

    private void Start()
    {
        _force = 13f;
        _acceleration = 0.8f;

        Manager.Input.dashAction += Dash;
    }

    private void Dash(Rigidbody2D rb, Vector2 dir)
    {
        Manager.Game.PlayerController.TrailRenderer.enabled = true;
        Vector2 dashDirection = (dir + new Vector2(0, _acceleration)).normalized;
        Debug.Log("dashDirection : " + dashDirection);
        rb.linearVelocity = dashDirection * _force;
        Debug.Log("대시 rb.linearVelocity: " + rb.linearVelocity);
    }
}