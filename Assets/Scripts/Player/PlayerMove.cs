using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    PlayerController _playerController;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _moveSpeed = 6f;
    }

    public void Move(Rigidbody2D rb)
    {
        bool isCanMove = !_playerController.IsDash && !_playerController.IsChargeLanding && !_playerController.IsGrabJump;
        if (isCanMove)
        {
            rb.linearVelocityX = Manager.Input.MoveDir.x * _moveSpeed;
            Manager.Game.PlayerController.Flip(rb.linearVelocityX);
        }
        else
        {
            Manager.Game.PlayerController.IsMove = false;
            Debug.Log("무브 안됨");
        }
    }
}