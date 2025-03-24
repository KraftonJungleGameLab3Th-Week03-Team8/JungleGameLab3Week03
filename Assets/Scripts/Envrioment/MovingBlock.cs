using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private bool _isHorizontal;
    [SerializeField] private bool _isVertical;
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    private Vector2 _startPoint;
    private Vector2 _endPoint;

    private void Start()
    {
        _startPoint = transform.position;

        if(_isHorizontal)
        {
            _endPoint = new Vector2(_startPoint.x + _distance, _startPoint.y);
        }
        else if (_isVertical)
        {
            _endPoint = new Vector2(_startPoint.x, _startPoint.y + _distance);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(_startPoint, _endPoint, Mathf.PingPong(Time.time * _speed, 1));
    }
}
