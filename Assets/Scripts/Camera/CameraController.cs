using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    private Camera _cam;
    private Transform _camTr;
    private Vector3 _originPos = new Vector3(0f, 0f, -10f);
    
    [Header("Tracking Target")]    
    private Transform _targetTr;    
    public Vector2 targetPositionOffset { get { return _targetPositionOffset; } set { _targetPositionOffset = value; } }
    public float dampingOffset { get { return _dampingOffset; } set { _dampingOffset = value; } }
    public Vector2 minTrackingRange { get { return _minTrackingRange; } set { _minTrackingRange = value; } }
    public Vector2 maxTrackingRange { get { return _maxTrackingRange; } set { _maxTrackingRange = value; } }
    private bool _isTracking;

    public Vector2 _targetPositionOffset;
    [Min(0f)]
    public float _dampingOffset;
    public Vector2 _minTrackingRange;
    public Vector2 _maxTrackingRange;

    [Header("Test ShakeCamera")]
    public float secPlayTime;
    public float secPlayFreq;
    public float shakeRange;
    public bool isLerp;
    

    private void Start()
    {
        //cam = Camera.main;
        _cam = FindAnyObjectByType<Camera>();
        _camTr = _cam.transform;

        SetTarget(FindAnyObjectByType<PlayerInput>().transform);
    }

    public void SetTarget(Transform target)
    {
        _targetTr = target;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(_targetTr.position.x + targetPositionOffset.x, _targetTr.position.y + targetPositionOffset.y, _originPos.z);
        Vector3 localTargetPos = _camTr.InverseTransformPoint(targetPos);

        //Tracking Range
        //Rect trackingRange = new Rect()
        if(localTargetPos.x < minTrackingRange.x 
            || localTargetPos.x > maxTrackingRange.x 
            || localTargetPos.y < minTrackingRange.y 
            || localTargetPos.y > maxTrackingRange.y)   
        {
            _isTracking = true;
        }

        if(_isTracking)
        {
            //_camTr.position = targetPos;

            _camTr.position = (dampingOffset == 0f) ? targetPos : Vector3.Lerp(_camTr.position, targetPos, Time.deltaTime / dampingOffset);
            if (localTargetPos.sqrMagnitude < 0.01f)
                _isTracking = false;
        }

        // Camera Boundary
        Vector2 minScreenPoint = Vector2.zero;
        Vector3 maxScreenPoint = new Vector3(_cam.pixelWidth, _cam.pixelHeight, 0f);

        Vector2 minBoundaryCam = new Vector2(-10f, -10f);
        Vector2 maxBoundaryCam = new Vector2(10f, 10f);

        // 제한 범위로 고정 시키는 방법

        //_cam.ScreenToWorldPoint(minScreenPoint);
        //Debug.Log("ScreenToWorldPoint: " + _cam.ScreenToWorldPoint(minScreenPoint));
    }

    //void OnGUI()
    //{
    //    Vector3 point = new Vector3();
    //    Event currentEvent = Event.current;
    //    Vector2 mousePos = new Vector2();

    //    // Get the mouse position from Event.
    //    // Note that the y position from Event is inverted.
    //    mousePos.x = currentEvent.mousePosition.x;
    //    mousePos.y = _cam.pixelHeight - currentEvent.mousePosition.y;

    //    point = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));

    //    GUILayout.BeginArea(new Rect(20, 20, 250, 120));
    //    GUILayout.Label("Screen pixels: " + _cam.pixelWidth + ":" + _cam.pixelHeight);
    //    GUILayout.Label("Mouse position: " + mousePos);
    //    GUILayout.Label("World position: " + point.ToString("F3"));
    //    GUILayout.EndArea();
    //}

    #region Shake Camera zRotation

    [ContextMenu("ShakeZRotCamera()")]
    private void ShakeZRotCamera()
    {
        StartCoroutine(ShakeZRotCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    }
    
    public void ShakeZRotCamera(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {
        StartCoroutine(ShakeZRotCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    }

    /*
     * Max (4f, 100f, 3f, true);
     */
    private IEnumerator ShakeZRotCameraCoroutine(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {
        for(int i = 0; i < secPlayTime * secPlayFreq; i++)
        {
            if(isLerp)
            {
                float shakeRangeLerp = Mathf.Lerp(shakeRange, 0.1f, i / (secPlayTime * secPlayFreq));
                //Debug.Log("zShakeRangeLerp: " + shakeRangeLerp);
                _camTr.eulerAngles = new Vector3(0f, 0f, Random.Range(-shakeRangeLerp, shakeRangeLerp));
            }
            else
                _camTr.eulerAngles = new Vector3(0f, 0f, Random.Range(-shakeRange, shakeRange));
            yield return new WaitForSeconds(1f / secPlayFreq);
        }
        _camTr.eulerAngles = Vector3.zero;
    }
    #endregion

    #region Shake Camera xyPosition
    [ContextMenu("ShakePosCamera()")]
    private void ShakePosCamera()
    {
        StartCoroutine(ShakePosCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    }

    public void ShakePosCamera(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {

    }
    private IEnumerator ShakePosCameraCoroutine(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {
        for (int i = 0; i < secPlayTime * secPlayFreq; i++)
        {
            if (isLerp)
            {
                float shakeRangeLerp = Mathf.Lerp(shakeRange, 0.01f, i / (secPlayTime * secPlayFreq));
                //Debug.Log("zShakeRangeLerp: " + shakeRangeLerp);
                _camTr.position = new Vector3(Random.Range(-shakeRangeLerp, shakeRangeLerp), Random.Range(-shakeRangeLerp, shakeRangeLerp), _originPos.z);
            }
            else
                _camTr.position = new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), _originPos.z);

            yield return new WaitForSeconds(1f / secPlayFreq);
        }
        _camTr.position = _originPos;
    }
    #endregion
}
