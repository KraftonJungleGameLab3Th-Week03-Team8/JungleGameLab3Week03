using System.Collections;
using UnityEngine;

public class PlayerFlashJump : MonoBehaviour
{
    
    // 콘테스트 : 김동영ver 플래시 점프입니다. 일단 이게 최고임
    [Tooltip("대쉬 힘")]
    [SerializeField] private float _force;
    [Tooltip("대쉬 각도")]
    [SerializeField] private float _acceleration;

    private void Start()
    {
        _force = 20f;
        _acceleration = 0.8f;

        Manager.Input.dashAction += Dash;
    }

    private void Dash(Rigidbody2D rb, Vector2 dir)
    {
        rb.AddTorque(1000f);
        rb.linearVelocity = Vector2.zero;
        Vector2 dashDirection = (dir + new Vector2(0, _acceleration)).normalized;
        Debug.Log("dashDirection : " + dashDirection);
        rb.linearVelocity = dashDirection * _force;
    }
}