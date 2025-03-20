using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    
    void OnEnable()
    {
        _inputManager = InputManager.Instance;
    }

    void Update()
    {
        //레이케스팅으로 down 방향으로 쏴서 땅에 닿았는지 확인
        Ray2D ray = new Ray2D(transform.position, Vector2.down);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, LayerMask.GetMask("Ground"));
        
        //debug 시각화
        Debug.DrawRay(ray.origin, ray.direction, Color.red); 
        
        //닿았으면 _inputManager의 _isGrounded를 true로 바꿔줌
        if (hit.collider != null)
        {
            _inputManager.IsGround = true;
        }
        else
        {
            _inputManager.IsGround = false;
        }
        
    }
}
