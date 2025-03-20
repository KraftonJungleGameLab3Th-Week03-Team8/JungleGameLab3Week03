using System;
using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputManager _inputManager;
    [SerializeField] private float _downForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _downForce = 300f;
    }

    private void OnEnable()
    {
        _inputManager = InputManager.Instance;
        _inputManager.downAction += OnDownCanceled;
    }

    private void OnDisable()
    {
        _inputManager.jumpChargeAction -= OnDownCanceled;
        
    }
    void OnDownCanceled()
    {
        _rb.AddForce(Vector2.down * _downForce, ForceMode2D.Impulse);
    }
}
