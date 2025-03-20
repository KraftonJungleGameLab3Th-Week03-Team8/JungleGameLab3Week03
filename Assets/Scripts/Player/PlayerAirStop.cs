using System;
using UnityEngine;

public class PlayerAirStop : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _rotateSpeedCharge;
    [SerializeField] private float _rotateSpeedMax;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Manager.Input.airStopAction += OnDownStarted;

        _rotateSpeed = 10f;
        _rotateSpeedCharge = 0.5f;
        _rotateSpeedMax = 25f;
    }

    private void FixedUpdate()
    {
        if(Manager.Input.IsChargeDown)
        {
            if (_rotateSpeed <= _rotateSpeedMax)
            {
                Rotate();
            }
        }
    }

    private void Rotate()
    {
        _rotateSpeed += _rotateSpeedCharge;
        _rb.AddTorque(_rotateSpeed);
    }

    private void OnDownStarted()
    {
        _rotateSpeed = 10f;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}