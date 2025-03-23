using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private float _landForce = 15f;
    private float _chargedLandForce;
    public float ChargedLandForce => _chargedLandForce;

    private void Start()
    {
        Manager.Input.landAction += Land;
    }

    private void Land(Rigidbody2D rb)
    {
        rb.angularVelocity = 0;
        _chargedLandForce = Manager.Game.PlayerController.PlayerAirStop.CurrentRotation / 200;
        rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.AddForce(Vector2.down * (_landForce + _chargedLandForce), ForceMode2D.Impulse);
    }
}