using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] CinemachineCamera _cinemachineCamera;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    public void Init(Transform target)
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();

        // 타겟 설정
        _target = target;
        CameraTarget cameraTarget = new CameraTarget();
        cameraTarget.TrackingTarget = _target;
        _cinemachineCamera.Follow = _target;
    }

    public void ShakeCamera(float intensity = 1.5f, float time = 0.5f)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        _startingIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }

    public void ShakeTimer()
    {
        _shakeTimer -= Time.deltaTime;
        if (_shakeTimer <= 0)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
        }
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            ShakeTimer();
        }
    }
}