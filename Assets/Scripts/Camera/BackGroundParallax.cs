using UnityEngine;

public class BackGroundParallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxFactorY = 0.3f;

    private float startY;

    private void Start()
    {
        cameraTransform = Manager.Game.NewCameraController.transform;
        startY = transform.position.y;
    }

    private void LateUpdate()
    {
        float deltaY = cameraTransform.position.y * parallaxFactorY;
        transform.position = new Vector3(transform.position.x, startY + deltaY, transform.position.z);
    }
}
