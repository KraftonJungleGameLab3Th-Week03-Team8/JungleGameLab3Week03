using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] CinemachineCamera _cinemachineCamera;
    CinemachineFollow _cinemachineFollow;
    CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    // 줌 인/아웃
    private float _originalOrthographicSize;
    private float _zommInLimit = 3f;            // 줌 인 제한
    [SerializeField] private float _zoomInWeight = 0.005f;
    [SerializeField] private float _zoomOutWeight = 0.1f;
    [SerializeField] private float _zoomInFollowOffsetY = -2f;

     
    public void Init(Transform target)
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _cinemachineFollow = GetComponent<CinemachineFollow>();
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();

        _originalOrthographicSize = _cinemachineCamera.Lens.OrthographicSize;

        // 타겟 설정
        _target = target;
        CameraTarget cameraTarget = new CameraTarget();
        cameraTarget.TrackingTarget = _target;
        _cinemachineCamera.Follow = _target;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            ShakeTimer();
        }


        if (_cinemachineCamera != null)
        {
            if (Manager.Game.PlayerController.IsAirStop && !Manager.Game.PlayerController.IsGround)
            {
                ZoomIn();
            }
            else if (Manager.Game.PlayerController.IsLanding || Manager.Game.PlayerController.IsGround)
            {
                ZoomOut();
            }
        }
    }

    #region 흔들림
    public void ShakeCamera(float intensity = 1.5f, float time = 0.5f)
    {
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;   // 진폭
        _startingIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }

    public void ShakeTimer()
    {
        _shakeTimer -= Time.deltaTime;
        if (_shakeTimer <= 0)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
        }
    }
    #endregion

    #region 줌 인/아웃
    public void ZoomIn()
    {
        _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _zommInLimit, _zoomInWeight);
        _cinemachineFollow.FollowOffset.y = Mathf.Lerp(_cinemachineFollow.FollowOffset.y, _zoomInFollowOffsetY, _zoomInWeight);
    }

    public void ZoomOut()
    {
        _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _originalOrthographicSize, _zoomOutWeight);
        _cinemachineFollow.FollowOffset.y = Mathf.Lerp(_cinemachineFollow.FollowOffset.y, 0f, _zoomInWeight);
    }
    #endregion
}