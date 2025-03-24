using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem _moveParticle;
    [SerializeField] ParticleSystem _fallParticle;
    [Range(0, 10)] [SerializeField] int occurAfterVelocity;
    [Range(0, 10)] [SerializeField] float dustFormationPeriod;
    [SerializeField] Rigidbody2D _playerRB;
    [SerializeField] float counter;

    private void Start()
    {
        _playerRB = transform.parent.GetComponent<Rigidbody2D>();
        _moveParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        _fallParticle = transform.GetChild(1).GetComponent<ParticleSystem>();

        occurAfterVelocity = 3;
        dustFormationPeriod = 0.15f;
    }

    private void Update()
    {
        counter += Time.deltaTime;
        // Debug.LogWarning("(파티클)플레이어 속도: " + _playerRB.linearVelocityX);
        if (Manager.Game.PlayerController.IsGround && Mathf.Abs(_playerRB.linearVelocityX) > occurAfterVelocity )
        {
            // Debug.LogWarning("플레이어 파티클");
            if (counter > dustFormationPeriod)
            {
                // Debug.LogWarning("플레이어 파티클2");
                _moveParticle.Play();
                counter = 0;
            }
        }
    }

    public void PlayFallParticle()
    {
        _fallParticle.Play();
    }
}
