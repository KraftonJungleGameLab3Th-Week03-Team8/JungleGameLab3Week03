using System;
using UnityEngine;

public class PlayerAirStop : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputManager _inputManager;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateSpeedCharge;
    [SerializeField] private float _rotateSpeedMax;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rotateSpeed = 10f;
        _rotateSpeedCharge = 0.5f;
        _rotateSpeedMax = 25f;
    }

    private void FixedUpdate()
    {
        if (_inputManager.IsChargeDown)
        {
            if (_rotateSpeed <= _rotateSpeedMax)
            {
                _rotateSpeed += _rotateSpeedCharge;
                _rb.AddTorque(_rotateSpeed);
            }
        }
    }

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        _inputManager.airStopAction += OnDownStarted;
    }

    private void OnDisable()
    {
        _inputManager.airStopAction -= OnDownStarted;
    }

    private void OnDownStarted()
    {
        _rotateSpeed = 10f;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
