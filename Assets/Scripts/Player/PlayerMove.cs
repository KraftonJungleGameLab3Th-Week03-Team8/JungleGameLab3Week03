using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float _moveSpeed = 5f;

    public void Move(Rigidbody2D rb)
    {
        if(!Manager.Game.PlayerController.IsDash)
        {
            Manager.Game.PlayerController.IsMove = true;
            rb.linearVelocityX = Manager.Input.MoveDir.x * _moveSpeed;
        }
        else
        {
            Manager.Game.PlayerController.IsMove = false;
            Debug.Log("무브 안됨(IsDash)");
        }
    }
}