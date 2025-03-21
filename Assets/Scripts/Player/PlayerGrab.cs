using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public void Grab(Rigidbody2D rb)
    {        
        float moveX = Manager.Input.MoveDir.x;

        // 벽에 붙는 조건
        if (!Manager.Game.PlayerController.IsGround && Manager.Game.PlayerController.IsWall &&
            ((Manager.Game.PlayerController.IsLeftWall && moveX < -0.1f) || (Manager.Game.PlayerController.IsRightWall && moveX > 0.1f)))
        {
            rb.gravityScale = 0f;
            Manager.Game.PlayerController.IsGrab = true;
            rb.linearVelocity = new Vector2(0f, 0f); // 벽 잡았을때 떨어지게 하려면 y값 추가
        }
        else
        {
            Manager.Game.PlayerController.IsGrab = false;
        }
    }
}
