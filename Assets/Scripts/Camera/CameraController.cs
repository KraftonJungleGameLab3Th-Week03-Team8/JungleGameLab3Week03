using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] CinemachineCamera _cinemachineCamera;

    public void Init(Transform target)
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();

        // 타겟 설정
        _target = target;
        CameraTarget cameraTarget = new CameraTarget();
        cameraTarget.TrackingTarget = _target;
        _cinemachineCamera.Follow = _target;
    }
}