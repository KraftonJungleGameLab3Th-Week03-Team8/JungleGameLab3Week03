using UnityEngine;

public class PlayerJumpCharge : MonoBehaviour
{
    //일단 rigidbody 다 가져올게요. 나중에 리팩토링 ㄱㄱ
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputManager.Instance.jumpChargeAction += ConstraintPosition;
    }

    private void OnDisable()
    {
        InputManager.Instance.jumpChargeAction -= ConstraintPosition;
    }

    private void ConstraintPosition()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
