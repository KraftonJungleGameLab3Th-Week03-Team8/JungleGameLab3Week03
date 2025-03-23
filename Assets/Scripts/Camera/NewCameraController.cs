using Unity.Cinemachine;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    [SerializeField] Transform _target;

    [SerializeField] CinemachineCamera _cinemachineCamera;

    public void Init(Transform target)
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();

        _target = target;

        CameraTarget cameraTarget = new CameraTarget();
        cameraTarget.TrackingTarget = _target;

        _cinemachineCamera.Follow = _target;
    }
}
