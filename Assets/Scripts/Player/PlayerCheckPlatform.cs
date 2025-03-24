using UnityEngine;

public class PlayerCheckPlatform : MonoBehaviour
{
    BoxCollider2D _boxCollider;

    [Header("Ray Settings")]
    [SerializeField] private float _playerWidth;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _rayYOffset;
    [SerializeField] private float _isCanAirStopHeight;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _wallLayer;

    [Header("현재 벽")]
    public Transform CurrentWall => _currentWall;
    [SerializeField] Transform _currentWall;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerWidth = _boxCollider.size.x * transform.localScale.x;
        _playerHeight = _boxCollider.size.y * transform.localScale.y;
        _rayYOffset = 0.02f;

        _isCanAirStopHeight = 0.9f;

        // 레이어 마스크 설정
        _groundLayer = (1 << (int)Define.Platform.Ground) | (1 << (int)Define.Platform.MoveGround);
        _wallLayer = 1 << (int)Define.Platform.Wall;
    }

    // 땅 감지
    public void CheckGround(ref bool isGround , ref float coyoteTime , ref float coyoteTimeTimer)
    {
        // 좌측 하단에서 가로(right) 방향으로 레이 쏘기
        Ray2D ray = new Ray2D(transform.position - new Vector3(_playerWidth * 0.5f, _playerHeight * 0.5f + _rayYOffset, 0), Vector2.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _playerWidth * 0.5f * 2, _groundLayer);
        Debug.DrawRay(ray.origin, ray.direction * _playerWidth * 0.5f * 2, Color.red);   // 디버깅

        //땅에 닿았을 때
        if (hit.collider != null)
        {
            if (Manager.Game.PlayerController.IsLanding)
                Manager.Game.CameraController.ShakeCamera();

            if (isGround == false)
            {
                Manager.Game.PlayerController.DetachWallState();

                isGround = true;
                coyoteTimeTimer = coyoteTime;   // 코요테 타임 초기화
                if (Manager.Game.PlayerController.IsLanding)
                {  
                    // extraForce가 높이랑 회전얼마나했는지 다 계산한 값. 충격 세기 => 카메라 흔드는 정도에도 아래와 같이 계산해서 적용하면 될것.
                    float extraForce = (Manager.Game.PlayerController.PlayerAirStop.StartHeight 
                        - transform.position.y) * 5
                        + Manager.Game.PlayerController.PlayerAirStop.CurrentRotation / 200;

                    LandingEffect.MakeLandingEffect(extraForce);
                    
                }
                Manager.Game.PlayerController.LandOnGroundState();
            }
        }
        else // 공중에 떠있을 때
        {
            Manager.Game.PlayerController.RB.gravityScale = 4f;
            isGround = false;
            coyoteTimeTimer -= Time.deltaTime;  // 코요테 타임 감소
        }
    }

    // 플레이어 앞 벽 체크
    public void CheckWall(ref bool isWall)
    {
        Transform visual = Manager.Game.PlayerController.Visual;
        Vector3 startPosition = visual.position;
        RaycastHit2D hit = Physics2D.Raycast(startPosition, visual.right, _playerWidth * 0.55f, _wallLayer);
        Debug.DrawRay(startPosition, visual.right * _playerWidth * 0.55f, Color.green);   // 디버깅

        // 벽 감지
        if (hit.collider != null)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            isWall = true;
            // 닿았을때 대시, 벽점프 초기화 하면 점프하고 다시 벽에 붙는 문제 발생
            // 사유) 벽닿은 상태로 그대로 손을 놓으면 공중 제어가 되어야하고 점프를하면 공중제어는 못해도 점프는 초기화 되어야함.

            //Manager.Game.PlayerController.IsDash = false; <- 이것 때문에 연속 벽 잡기가 안되어서 주석처리함

            _currentWall = hit.collider.transform;
        }
        else
        {
            isWall = false;
        }
    }

    public void CheckLandingHeight(ref bool isCanAirStop)
    {
        // 현재 위치에서 Ray를 쏴서 땅을 감지하고 땅과의 높이 계산
        Transform visual = Manager.Game.PlayerController.Visual;

        float offset = (Manager.Game.PlayerController.IsSeeRight) ? -_playerWidth / 2 : _playerWidth / 2;
        Vector3 startPosition = visual.position + Vector3.right * offset;

        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, 100f, _groundLayer);
        Debug.DrawRay(startPosition, Vector2.down * _isCanAirStopHeight, Color.red);   // 최소 히어로 랜딩 높이 디버깅

        // 랜딩 가능한 높이 감지
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(visual.position, hit.point);
            //Debug.Log("땅과의 거리 : " + distance);
            isCanAirStop = distance >= _isCanAirStopHeight;
        }
        else
        {
            isCanAirStop = false;
        }
    }
}