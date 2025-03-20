using UnityEngine;

public class PlayerCheckObstacle : MonoBehaviour
{
    Rigidbody2D _rb;
    BoxCollider2D _boxCollider;

    [Header("Ray Settings")]
    [SerializeField] private float _playerWidth;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _rayYOffset;

    [SerializeField] LayerMask _groundLayer = 1 << 6;
    [SerializeField] LayerMask _wallLayer = 1 << 7;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerWidth = _boxCollider.size.x * transform.localScale.x;
        _playerHeight = _boxCollider.size.y * transform.localScale.y;
        _rayYOffset = 0.02f;
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
            Debug.Log("Ground");

            isGround = true;
            if (Manager.Input.IsDown)
            {
                Manager.Input.IsDown = false;
                _rb.constraints = RigidbodyConstraints2D.None;
            }
        }
        else // 공중에 떠있을 때
        {
            isGround = false;
        }
    }

    // 벽 감지
    public void CheckWall(ref bool isWall)
    {
        // 좌측에서 가로(right) 방향으로 레이 쏘기
        Vector3 startPosition = transform.position - new Vector3(_playerWidth * 0.55f, 0, 0);
        Ray2D ray = new Ray2D(startPosition, Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _playerWidth * 1.1f, _wallLayer);
        Debug.DrawRay(ray.origin, ray.direction * _playerWidth * 1.1f, Color.blue);   // 디버깅

        //땅에 닿았을 때
        if (hit.collider != null)
        {
            Debug.Log("Wall");

            isWall = true;
            if (Manager.Input.IsDown)
            {
                Manager.Input.IsDown = false;
                _rb.constraints = RigidbodyConstraints2D.None;
            }
        }
        else // 벽에 안닿았을 때
        {
            isWall = false;
        }
    }
}