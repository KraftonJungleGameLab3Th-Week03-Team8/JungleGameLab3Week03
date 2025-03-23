using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    private float _landingForce = 10f;  // 랜딩 힘
    private float _chargedLandingForce; // 충전된 랜딩 힘
    public float ChargedLandForce => _chargedLandForce;

    private void Start()
    {
        Manager.Input.landingAction += Landing;
    }

    private void Landing(Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.angularVelocity = 0;
        _chargedLandForce = Manager.Game.PlayerController.PlayerAirStop.CurrentRotation / 200;
        rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.AddForce(Vector2.down * (_landingForce + _chargedLandingForce), ForceMode2D.Impulse);
    }
}