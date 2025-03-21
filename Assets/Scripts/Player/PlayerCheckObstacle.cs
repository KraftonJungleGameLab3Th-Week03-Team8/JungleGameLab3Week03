using UnityEngine;

public class PlayerCheckObstacle : MonoBehaviour
{
    BoxCollider2D _boxCollider;

    [Header("Ray Settings")]
    [SerializeField] private float _playerWidth;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _rayYOffset;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _wallLayer;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerWidth = _boxCollider.size.x * transform.localScale.x;
        _playerHeight = _boxCollider.size.y * transform.localScale.y;
        _rayYOffset = 0.02f;

        // 레이어 마스크 설정
        _groundLayer = 1 << (int)Define.Platform.Ground;
        _wallLayer = 1 << (int)Define.Platform.Wall;
    }

    // 땅 감지
    public void CheckGround(ref bool isGround)
    {
        // 좌측 하단에서 가로(right) 방향으로 레이 쏘기
        Ray2D ray = new Ray2D(transform.position - new Vector3(_playerWidth / 2, _playerHeight / 2 + _rayYOffset, 0), Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _playerWidth, _groundLayer);
        Debug.DrawRay(ray.origin, ray.direction * _playerWidth, Color.red);   // 디버깅

        //땅에 닿았을 때
        if (hit.collider != null)
        {
            Manager.Game.PlayerController.RB.gravityScale = 0f;
            //Debug.Log("Ground");
            isGround = true;
            Manager.Game.PlayerController.IsLanding = false;
            Manager.Game.PlayerController.IsDash = false;
            Manager.Game.PlayerController.LandOnGround();
        }
        else // 공중에 떠있을 때
        {
            Manager.Game.PlayerController.RB.gravityScale = 4f;
            isGround = false;
        }
    }

    // 벽 감지
    public void CheckWall(ref bool isWall, ref bool isLeftWall, ref bool isRightWall)
    {
        
        // 왼쪽 벽 체크
        Vector3 startPosition = transform.position;
        Ray2D leftRay = new Ray2D(startPosition, Vector2.left);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRay.origin, leftRay.direction, _playerWidth * 0.55f, _wallLayer);
        Debug.DrawRay(leftRay.origin, leftRay.direction * _playerWidth * 0.55f, Color.blue);   // 디버깅
        
        // 오른쪽 벽 체크
        Ray2D rightRay = new Ray2D(startPosition, Vector2.right);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRay.origin, rightRay.direction, _playerWidth * 0.55f, _wallLayer);
        Debug.DrawRay(rightRay.origin, rightRay.direction * _playerWidth * 0.55f, Color.green);   // 디버깅
        
        // 둘 중 하나라도 닿으면 IsWall = true
        if (leftHit.collider != null || rightHit.collider != null)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            isWall = true;
        }
        else
        {
            isWall = false;
        }
        
        // 왼쪽 벽 체크
        if (leftHit.collider != null)
        {
            Debug.Log("Left Wall");
            isLeftWall = true;
        }
        else
        {
            isLeftWall = false;
        }
        
        // 오른쪽 벽 체크
        if (rightHit.collider != null)
        {
            Debug.Log("Right Wall");
            isRightWall = true;
        }
        else
        {
            isRightWall = false;
        }
    }
}