using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _moveSpeed = 6f;
    }

    public void Move(Rigidbody2D rb)
    {
        bool isCanMove = !_playerController.IsDash && !_playerController.IsAirStop && !_playerController.IsGrabJump && !_playerController.IsFrontGround && !_playerController.IsLanding;
        if (isCanMove)
        {
            rb.linearVelocityX = Manager.Input.MoveDir.x * _moveSpeed;
            Manager.Game.PlayerController.Flip(Manager.Input.MoveDir.x);

            Debug.Log("이동 " + rb.linearVelocityX);
        }
        else if(Manager.Input.MoveDir.x == 0) 
        {
            Manager.Game.PlayerController.IsMove = false;
            Debug.Log("무브 안됨");
        }
    }
}