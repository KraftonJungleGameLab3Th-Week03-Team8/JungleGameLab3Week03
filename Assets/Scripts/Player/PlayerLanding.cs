using System;
using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _downForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _downForce = 300f;
        Manager.Input.downAction += OnDownCanceled;
    }

    void OnDownCanceled()
    {
        _rb.angularVelocity = 0;
        _rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        _rb.AddForce(Vector2.down * _downForce, ForceMode2D.Impulse);
    }
}
