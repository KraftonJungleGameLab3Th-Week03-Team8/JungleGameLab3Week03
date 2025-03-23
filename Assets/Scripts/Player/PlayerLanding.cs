using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private float _landForce = 200f;

    private void Start()
    {
        Manager.Input.landAction += Land;
    }

    private void Land(Rigidbody2D rb)
    {
        rb.angularVelocity = 0;
        rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.AddForce(Vector2.down * _landForce, ForceMode2D.Impulse);
    }
}