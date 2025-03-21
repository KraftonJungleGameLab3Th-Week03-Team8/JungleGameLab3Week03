using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 0;
    [SerializeField] private float _fallMultiplier = 5f;
    [SerializeField] private float _gravityScale = 3f;
    //[SerializeField] private float _longJumpForce = 0;

    private void Start()
    {
        _jumpForce = 200f;
        _fallMultiplier = 5f;
        //_longJumpForce = 3000f;
        Manager.Input.jumpAction += Jump;
        _gravityScale = Manager.Game.PlayerController.RB.gravityScale;
    }

    private void Jump(Rigidbody2D rb)
    {
        if (Manager.Game.PlayerController.IsGround)
        {
            Debug.Log("점프");
            rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            //Manager.Game.PlayerController.IsJump = true;
            StartCoroutine(WaitOneSecondCouroutine());
        }
    }

    IEnumerator WaitOneSecondCouroutine()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        Manager.Game.PlayerController.IsJump = true;
    }

    public void controlJumpGravity(Rigidbody2D rb)
    {
        //Debug.Log(rb.linearVelocityY);
        // 떨어질때 중력 증가
        if (rb.linearVelocityY < 0)
        {
            Debug.Log("중력 3단계");
            rb.gravityScale = _gravityScale * _fallMultiplier;
        }
        else if (rb.linearVelocityY > 0 && Manager.Input.IsHoldJump)
        {
            Debug.Log("중력 1단계");
            rb.gravityScale = _gravityScale;
        }
        else if (rb.linearVelocityY > 0 && !Manager.Input.IsHoldJump)
        {
            Debug.Log("중력 2단계");
            rb.gravityScale = _gravityScale * (_fallMultiplier / 2);
        }
    }

    /*
    private void LongJump(Rigidbody2D rb)
    {
        if (Manager.Game.PlayerController.IsJump)
        {
            Debug.Log("롱 점프");
            rb.AddForce(Vector2.up * _longJumpForce, ForceMode2D.Force);
        }
    }
    */
}