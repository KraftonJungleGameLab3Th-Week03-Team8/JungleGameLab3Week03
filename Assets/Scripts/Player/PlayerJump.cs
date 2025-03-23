using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // 원래는 점프데이터 클래스를 만들어서 관리해야함. 일단 기능구현부터.
    public float JumpCutGravityMultiplier => _jumpCutGravityMultiplier;
    public float MaxFallSpeed => _maxFallSpeed;
    public float JumpHangTime => _jumpHangTime;
    public float JumpHangGravityMultiplier => _jumpHangGravityMultiplier;
    public float FallGravityMultiplier => _fallGravityMultiplier;

    [Header("JumpForce 결정변수")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpTimeToApex;

    private float _gravityStrength;
    private float _jumpForce;

    [Header("점프 상태 중력 관리 변수")]
    [SerializeField] private float _jumpCutGravityMultiplier;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private float _jumpHangTime;
    [SerializeField] private float _jumpHangGravityMultiplier;
    [SerializeField] private float _fallGravityMultiplier;

    private void Start()
    {
        Manager.Input.jumpAction += Jump;

        #region 중력 세팅
        // 점프 높이와 시간 기반으로 중력 가속도 계산
        _gravityStrength = -(2 * _jumpHeight) / Mathf.Pow(_jumpTimeToApex, 2);

        // 중력 계수 설정 (유니티 기본 중력값과 비교해 상대적 스케일)
        Manager.Game.PlayerController.GravityScale = _gravityStrength / Physics2D.gravity.y;

        // 점프 시 필요한 초기 속도 계산
        _jumpForce = Mathf.Abs(_gravityStrength) * _jumpTimeToApex;
        #endregion
    }

    private void Jump(Rigidbody2D rb)
    {
        /* 점프 버퍼링을 위한 코드
        if (rb.linearVelocityY < 0)
        {
            _jumpForce -= rb.linearVelocityY;
        }
        */
        Debug.Log("점프");
        rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        StartCoroutine(WaitOneSecondCouroutine());
    }

    private IEnumerator WaitOneSecondCouroutine()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        Manager.Game.PlayerController.IsJump = true;
    }
}