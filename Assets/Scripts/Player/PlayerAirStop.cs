using System;
using UnityEngine;

public class PlayerAirStop : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _defaultRotateSpeed = 10f;
    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed { get { return _rotateSpeed; } }
    [SerializeField] private float _rotateSpeedCharge;
    [SerializeField] private float _rotateSpeedMax;
    [SerializeField] private float _startRotation;
    public float StartRotation { get { return _startRotation; } }
    [SerializeField] private float _startHeight;
    public float StartHeight { get { return _startHeight; } }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Manager.Input.airStopAction += OnDownStarted;

        _rotateSpeed = 10f;
        _rotateSpeedCharge = 0.5f;
        _rotateSpeedMax = 30f;
    }

    private void FixedUpdate()
    {
        if (Manager.Input.IsPressLand)
        {
            if (_rotateSpeed <= _rotateSpeedMax)
            {
                
                Rotate();
            }
        }
    }

    private void Rotate()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _rotateSpeed += _rotateSpeedCharge;
        if (_rotateSpeed <= 20f)
        {
            _rb.AddTorque(_rotateSpeed);
        }
    }

    private void OnDownStarted()
    {
        Manager.Game.PlayerController.SetGravityScale(Manager.Game.PlayerController.GravityScale);
        _startHeight = transform.position.y;
        _startRotation = _rotateSpeed;
        _rotateSpeed = _defaultRotateSpeed;
    }
}