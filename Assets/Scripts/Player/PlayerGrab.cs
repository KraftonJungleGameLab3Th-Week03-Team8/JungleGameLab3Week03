using System;
using System.Collections;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] private float _wallJumpForceX;
    [SerializeField] private float _wallJumpForceY;
    
    [SerializeField] private float _corutineTime;
    // 대쉬, 벽점프 제거용 코루틴
    Coroutine _dashCoroutine;

    private void Start()
    {
        _wallJumpForceX = 50f;
        _wallJumpForceY = 200f;
        
        //테스트용 변수
        _corutineTime = 100f;
    }

    public void Grab(Rigidbody2D rb)
    {        
        float moveX = Manager.Input.MoveDir.x;

        bool touchingWall = Manager.Game.PlayerController.IsWall;
        bool pushingIntoWall =
            (Manager.Game.PlayerController.IsLeftWall && moveX < -0.1f) ||
            (Manager.Game.PlayerController.IsRightWall && moveX > 0.1f);

        bool canGrab = !Manager.Game.PlayerController.IsGround && touchingWall && pushingIntoWall;

        if (canGrab)
        {
            // ✅ Grab 진입 시 상태 초기화 (중요!)
            Manager.Game.PlayerController.IsGrabJump = false;
            Manager.Game.PlayerController.IsWallJumping = false;

            Manager.Game.PlayerController.IsGrab = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        else
        {
            Manager.Game.PlayerController.IsGrab = false;
        }
    }

    public void GrabJump(Rigidbody2D rb)
    {
        Vector2 force = new Vector2(_wallJumpForceX, _wallJumpForceY);
        force.x *= -Mathf.Sign(Manager.Input.MoveDir.x);

        if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= rb.linearVelocity.x;
        if (rb.linearVelocity.y < 0)
            force.y -= rb.linearVelocity.y;

        rb.AddForce(force, ForceMode2D.Impulse);

        // 상태 설정
        Manager.Game.PlayerController.IsGrabJump = true;
        Manager.Game.PlayerController.IsGrab = false;

        Manager.Game.PlayerController.IsWallJumping = true;
    }
}
