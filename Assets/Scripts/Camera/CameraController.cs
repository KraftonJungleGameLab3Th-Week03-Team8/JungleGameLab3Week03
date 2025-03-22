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
    
    [SerializeField] private Vector2 _targetPositionOffset;
    [Min(0f)]
    [SerializeField] private float _dampingOffset;
    [SerializeField] private Vector2 _minTrackingRange;
    [SerializeField] private Vector2 _maxTrackingRange;
    private bool _isTracking;
    private float _boundaryRangeX = 8.5f;

    [SerializeField] private Vector2 _lookAtOffset;


    [Header("Test ShakeCamera")]
    public float testSecPlayTime;
    public float testSecPlayFreq;
    public float testShakeRange;
    public bool testIsLerp;

    private Vector2 _shakePos;

    private void Start()
    {
        Init();
        //Invoke("Init", 0.5f);
    }

    public void Init()
    {
        //cam = Camera.main;
        _cam = FindAnyObjectByType<Camera>();
        _camTr = _cam.transform;

        SetTarget(FindAnyObjectByType<PlayerInput>().transform);
        //SetTarget(GameObject.Find("DummyTarget").transform);
    }

    public void SetTarget(Transform target)
    {
        _targetTr = target;
    }

    private void LateUpdate()
    {
        if (_targetTr == null)
            return;

        Vector3 targetPos = new Vector3(_targetTr.position.x + targetPositionOffset.x, _targetTr.position.y + targetPositionOffset.y, _originPos.z);
        Vector3 localTargetPos = _camTr.InverseTransformPoint(targetPos);

        // Camera Boundary Area
        if (targetPos.x > _boundaryRangeX)
            targetPos.x = _boundaryRangeX;
        if (targetPos.x < -_boundaryRangeX)
            targetPos.x = -_boundaryRangeX;

        // Tracking Range
        if(localTargetPos.x < minTrackingRange.x 
            || localTargetPos.x > maxTrackingRange.x 
            || localTargetPos.y < minTrackingRange.y 
            || localTargetPos.y > maxTrackingRange.y)   
        {
            _isTracking = true;
        }

        // Tracking
        if(_isTracking)
        {
            _camTr.position = (dampingOffset == 0f) ? targetPos : Vector3.Lerp(_camTr.position, targetPos, Time.deltaTime / dampingOffset);
            if (localTargetPos.sqrMagnitude < 0.01f)
                _isTracking = false;

            if (dampingOffset > 0f)
            {
                dampingOffset = dampingOffset / 10f * 9f;
                if (dampingOffset < 0.001f)
                    dampingOffset = 0f;
            }
                
        }

        _camTr.position += new Vector3(_shakePos.x ,_shakePos.y, 0f);
    }

    public void DashEffect()
    {
        //Debug.Log("DashEffect()");
        //StartCoroutine(DashEffectCorountine());
        dampingOffset = 4f;
    }

    IEnumerator DashEffectCorountine()
    {
        float orginDampingOffset = _dampingOffset;
        _dampingOffset = orginDampingOffset * 10f;
        yield return new WaitForSeconds(0.2f);
        _dampingOffset = orginDampingOffset;
    }


    public void LookAtDirection(Vector2 dir)
    {
        
    }


    #region Shake Camera zRotation

    //[ContextMenu("ShakeZRotCamera()")]
    //private void ShakeZRotCamera()
    //{
    //    StartCoroutine(ShakeZRotCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    //}

    //public void ShakeZRotCamera(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    //{
    //    StartCoroutine(ShakeZRotCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    //}

    //private IEnumerator ShakeZRotCameraCoroutine(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    //{
    //    for (int i = 0; i < secPlayTime * secPlayFreq; i++)
    //    {
    //        if (isLerp)
    //        {
    //            float shakeRangeLerp = Mathf.Lerp(shakeRange, 0.1f, i / (secPlayTime * secPlayFreq));
    //            //Debug.Log("zShakeRangeLerp: " + shakeRangeLerp);
    //            _camTr.eulerAngles = new Vector3(0f, 0f, Random.Range(-shakeRangeLerp, shakeRangeLerp));
    //        }
    //        else
    //            _camTr.eulerAngles = new Vector3(0f, 0f, Random.Range(-shakeRange, shakeRange));
    //        yield return new WaitForSeconds(1f / secPlayFreq);
    //    }
    //    _camTr.eulerAngles = Vector3.zero;
    //}
    #endregion

    #region Shake Camera xyPosition
    [ContextMenu("ShakePosCamera() Test")]
    private void ShakePosCamera()
    {
        StartCoroutine(ShakePosCameraCoroutine(testSecPlayTime, testSecPlayFreq, testShakeRange, testIsLerp));
    }


    /*
     * Landing 시작 시
     * Landing 완료(착시 시)
     *  01: (0.2f, 80f, 0.2f, true);
     *  02: (0.3f, 80f, 0.4f, true);
     *  04: (0.6f, 80f, 0.7f, true);
     *  07: (3f, 80f, 1.4f, true);
     *  10: (5f, 100f, 2f, true);
     */
    [ContextMenu("ShakePosCamera(01)")]
    private void ShakePosCamera01()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.2f, 80, 0.2f, true));
    }
    [ContextMenu("ShakePosCamera(02)")]
    private void ShakePosCamera02()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.3f, 80, 0.4f, true));
    }
    [ContextMenu("ShakePosCamera(04)")]
    private void ShakePosCamera04()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.6f, 80f, 0.7f, true));
    }
    [ContextMenu("ShakePosCamera(07)")]
    private void ShakePosCamera07()
    {
        StartCoroutine(ShakePosCameraCoroutine(3f, 80f, 1.4f, true));
    }
    [ContextMenu("ShakePosCamera(10)")]
    private void ShakePosCamera10()
    {
        StartCoroutine(ShakePosCameraCoroutine(5f, 100f, 2f, true));
    }

    public void ShakePosCamera(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {
        StartCoroutine(ShakePosCameraCoroutine(secPlayTime, secPlayFreq, shakeRange, isLerp));
    }
    private IEnumerator ShakePosCameraCoroutine(float secPlayTime, float secPlayFreq, float shakeRange, bool isLerp)
    {
        for (int i = 0; i < secPlayTime * secPlayFreq; i++)
        {
            if (isLerp)
            {
                float shakeRangeLerp = Mathf.Lerp(shakeRange, 0.01f, i / (secPlayTime * secPlayFreq));
                //Debug.Log("zShakeRangeLerp: " + shakeRangeLerp);\
                _shakePos = new Vector3(Random.Range(-shakeRangeLerp, shakeRangeLerp), Random.Range(-shakeRangeLerp, shakeRangeLerp), 0f);
                //_camTr.position = shakePos;
            }
            else
                _shakePos = new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), 0f);
                //_camTr.position = new Vector3(Random.Range(-shakeRange, shakeRange), Random.Range(-shakeRange, shakeRange), _originPos.z);

            yield return new WaitForSeconds(1f / secPlayFreq);
        }
        _shakePos = Vector2.zero;
        //_camTr.position = _originPos;
    }
    #endregion
}
