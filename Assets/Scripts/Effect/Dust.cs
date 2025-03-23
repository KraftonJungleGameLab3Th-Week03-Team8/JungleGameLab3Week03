using System.Collections;
using UnityEngine;

public class Dust : MonoBehaviour
{
    private Rigidbody2D _rb;
    private ConstantForce2D _cf;
    [SerializeField] private float _upForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private float _torqueForce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cf = GetComponent<ConstantForce2D>();

        StartCoroutine(LandEffectCouroutine());
    }

    private IEnumerator LandEffectCouroutine()
    {
        _rb.linearVelocity = new Vector2(0, _upForce);
        _cf.force = new Vector2(Random.Range(-_horizontalForce, _horizontalForce), 0);

        _cf.torque = Random.Range(-_torqueForce, _torqueForce);

        yield return new WaitForSeconds(0.05f);
        _rb.gravityScale = Random.Range(0.5f, 1.5f);

        Destroy(gameObject, Random.Range(0.5f,1.5f));
    }
}
