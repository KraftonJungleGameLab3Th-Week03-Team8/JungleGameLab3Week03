using UnityEngine;

public class PlayerAirStop : MonoBehaviour
{
    public float CurrentRotation => _currentRotation;
    public float StartHeight => _startHeight;
    private Rigidbody2D _rb;
    [SerializeField] private float _currentRotation;
    [SerializeField] private float _startHeight;
    [SerializeField] private float _rotateSpeedCharge;
    [SerializeField] private float _rotationLimit;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rotateSpeedCharge = 0.5f;
        _rotationLimit = 3000f;

        Manager.Input.airStopAction += OnDownStarted;
    }

    private void FixedUpdate()
    {
        if (Manager.Input.IsPressLand && Manager.Game.PlayerController.IsCanAirStop)
        {
            _currentRotation = _rb.rotation;
            if (_currentRotation <= _rotationLimit)
            {
                Rotate();
            }
        }
        else
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.angularVelocity = 0;
            _rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void Rotate()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _rb.AddTorque(_rotateSpeedCharge);
    }

    private void OnDownStarted()
    {
        _startHeight = transform.position.y;
    }
}