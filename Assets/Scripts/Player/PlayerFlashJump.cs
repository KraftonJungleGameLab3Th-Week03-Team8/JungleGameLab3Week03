using System.Collections;
using UnityEngine;

public class PlayerFlashJump : MonoBehaviour
{
    // 콘테스트 : 김동영ver 플래시 점프입니다. 일단 이게 최고임
    [Tooltip("대쉬 힘")]
    [SerializeField] private float _force;  // 플래시 점프 거리 조절
    [Tooltip("대쉬 각도")]
    [SerializeField] private float _acceleration;   // 정규화 취해서 각도로 사용 (클수록 위를 향함)

    private void Start()
    {
        _force = 20f;
        _acceleration = 0.8f;

        Manager.Input.dashAction += Dash;
    }

    private void Dash(Rigidbody2D rb, Vector2 dir)
    {
        Debug.Log("대시" + dir);

        //rb.linearVelocity = Vector2.zero;
        Vector2 dashDirection = (dir + new Vector2(0, _acceleration)).normalized;
        Debug.Log("dashDirection : " + dashDirection);
        rb.linearVelocity = dashDirection * _force;
        //rb.AddForce(dashDirection * _force, ForceMode2D.Impulse);

        //StartCoroutine(WaitOneFrameCoroutine());
    }

    //IEnumerator WaitOneFrameCoroutine()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    Manager.Game.PlayerController.IsDash = true;
    //}
}