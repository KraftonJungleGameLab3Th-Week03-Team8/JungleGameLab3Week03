using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody2D _rb;

    [Header("Ray Settings")]
    private float _playerWidth;
    private float _playerHeight;
    private Ray2D _ray;
    [SerializeField]private float _rayYOffset;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        var boxCollider = GetComponent<BoxCollider2D>();
        _playerWidth = boxCollider.size.x * transform.localScale.x;
        _playerHeight = boxCollider.size.y * transform.localScale.y;
        _rayYOffset = 0.02f;
    }

    private void Update()
    {
        // 좌측 하단에서 가로(right) 방향으로 레이 쏘기
        _ray = new Ray2D(transform.position - new Vector3(_playerWidth / 2, _playerHeight / 2 + _rayYOffset, 0), Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, _playerWidth, LayerMask.GetMask("Ground"));

        DrawDebugRay();

        _inputManager.IsGround = hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _inputManager.IsDash = false;
        }
    }

    private void DrawDebugRay()
    {
        // 실제 레이캐스트 길이만큼 디버그용 레이 그리기
        Debug.DrawRay(_ray.origin, _ray.direction * _playerWidth, Color.red);
    }
}