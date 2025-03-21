using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private float _downForce = 300f;

    private void Start()
    {
        Manager.Input.downAction += OnDownCanceled;
    }

    private void OnDownCanceled(Rigidbody2D rb)
    {
        rb.angularVelocity = 0;
        rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.AddForce(Vector2.down * _downForce, ForceMode2D.Impulse);
    }
}