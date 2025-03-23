using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    private Camera _cam;
    private Transform _camTr;
    private readonly Vector3 _originPos = new Vector3(0f, 0f, -10f);

    [field: SerializeField] public float test { get; set; }
    
    [Header("Tracking Target")]    
    private Transform _targetTr;
    [field: SerializeField] public Vector2 targetPositionOffset { get; set; }
    [field: SerializeField] [Min(0f)]public float dampingOffset { get; set; }
    [field: SerializeField] public Vector2 minNonTrackingRange { get; set; }
    [field: SerializeField] public Vector2 maxNonTrackingRange { get; set; }
    
    [SerializeField] private float _lookAtOffsetX = 9f;
    [SerializeField] private float _lookAtOffsetY = -5f;
    [field: SerializeField] public Vector2 lookAtDir { get; set; }

    [SerializeField] private bool _isLookAt;
    private bool _isTracking;
    private float _boundaryRangeX = 8.5f;

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

        Vector2 posTargetOffset = targetPositionOffset;
        if(lookAtDir != Vector2.zero)
        {
            if (lookAtDir.x > 0f)
                posTargetOffset.x = _lookAtOffsetX;
            else if(lookAtDir.x < 0f)
                posTargetOffset.x = -_lookAtOffsetX;

            if (lookAtDir.y > 0f)
                posTargetOffset.y = _lookAtOffsetY;
            else if (lookAtDir.y < 0f)
                posTargetOffset.y = -_lookAtOffsetY;
        }

        Vector3 targetPos = new Vector3(_targetTr.position.x + posTargetOffset.x, _targetTr.position.y + posTargetOffset.y, _originPos.z);
        Vector3 localTargetPos = _camTr.InverseTransformPoint(targetPos);

        // Camera Boundary Area
        if (targetPos.x > _boundaryRangeX)
            targetPos.x = _boundaryRangeX;
        if (targetPos.x < -_boundaryRangeX)
            targetPos.x = -_boundaryRangeX;

        // Non-Tracking Range
        if(localTargetPos.x < minNonTrackingRange.x 
            || localTargetPos.x > maxNonTrackingRange.x 
            || localTargetPos.y < minNonTrackingRange.y 
            || localTargetPos.y > maxNonTrackingRange.y)   
        {
            _isTracking = true;
        }

        // Tracking
        if(_isTracking)
        {
            _camTr.position = (dampingOffset == 0f) ? targetPos : Vector3.Lerp(_camTr.position, targetPos, Time.deltaTime / dampingOffset);
            if (localTargetPos.sqrMagnitude < 0.01f)
                _isTracking = false;

            //if (dampingOffset > 0f)
            //{
            //    dampingOffset = dampingOffset / 10f * 9f;
            //    if (dampingOffset < 0.001f)
            //        dampingOffset = 0f;
            //}
                
        }

        _camTr.position += new Vector3(_shakePos.x ,_shakePos.y, 0f);
    }

    public void DashEffect()
    {
        //Debug.Log("DashEffect()");
        //StartCoroutine(DashEffectCorountine());
        //dampingOffset = 4f;
        ShakePosCamera00();
    }

    IEnumerator DashEffectCorountine()
    {
        //float orginDampingOffset = _dampingOffset;
        //_dampingOffset = orginDampingOffset * 10f;
        yield return new WaitForSeconds(0.2f);
        //_dampingOffset = orginDampingOffset;
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
     *  00: (0.15f, 40f, 0.1f, false);
     *  01: (0.2f, 80f, 0.2f, true);
     *  02: (0.3f, 80f, 0.4f, true);
     *  04: (0.6f, 80f, 0.7f, true);
     *  07: (3f, 80f, 1.4f, true);
     *  10: (5f, 100f, 2f, true);
     */
    [ContextMenu("ShakePosCamera(00)")]
    public void ShakePosCamera00()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.15f, 70, 0.1f, false));
    }
    [ContextMenu("ShakePosCamera(01)")]
    public void ShakePosCamera01()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.2f, 80, 0.2f, true));
    }
    [ContextMenu("ShakePosCamera(02)")]
    public void ShakePosCamera02()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.3f, 80, 0.4f, true));
    }
    [ContextMenu("ShakePosCamera(04)")]
    public void ShakePosCamera04()
    {
        StartCoroutine(ShakePosCameraCoroutine(0.6f, 80f, 0.7f, true));
    }
    [ContextMenu("ShakePosCamera(07)")]
    public void ShakePosCamera07()
    {
        StartCoroutine(ShakePosCameraCoroutine(3f, 80f, 1.4f, true));
    }
    [ContextMenu("ShakePosCamera(10)")]
    public void ShakePosCamera10()
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
