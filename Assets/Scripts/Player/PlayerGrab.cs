using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] private float _wallJumpForceX;
    [SerializeField] private float _wallJumpForceY;

    private void Start()
    {
        _wallJumpForceX = 5f; // KDY
        _wallJumpForceY = 13f; // KDY

        Manager.Input.wallJumpAction += GrabJump;
    }

    public void Grab(Rigidbody2D rb)    // 벽 잡기
    {
        float moveX = Manager.Input.MoveDir.x;
        bool isWall = Manager.Game.PlayerController.IsWall;

        if (isWall)
        {
            rb.gravityScale = 0f;   // 중력 X
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    public void GrabJump(Rigidbody2D rb)    // 벽 점프
    {
        Vector2 jumpDirection = Vector2.right;
        float targetDirection = (Manager.Game.PlayerController.IsSeeRight) ? -1 : 1;
        jumpDirection.x *= targetDirection;
        //Manager.Game.PlayerController.Flip(-Manager.Game.PlayerController.Visual.transform.right.x);
        Manager.Game.PlayerController.Flip(jumpDirection.x);


        Vector2 force = new Vector2(_wallJumpForceX, _wallJumpForceY);
        //force.x *= -Mathf.Sign(Manager.Input.MoveDir.x);
        force.x *= -Mathf.Sign(-targetDirection);
        Debug.LogWarning("벽 점프 시, MoveDir: " + Manager.Input.MoveDir);

        if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= rb.linearVelocity.x;
        if (rb.linearVelocity.y < 0)
            force.y -= rb.linearVelocity.y;

        Debug.LogWarning("force" + force);
        rb.AddForce(force, ForceMode2D.Impulse);

        // 상태 설정
        Manager.Game.PlayerController.DetachWall();
    }
}