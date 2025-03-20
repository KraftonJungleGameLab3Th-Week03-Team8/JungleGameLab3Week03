using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    #region 매니저
    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI {  get { return Instance._ui; } }
    #endregion

    private void Awake()
    {
        Init();
    }

    void Update()
    {

    }

    private void Init()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        /*
         TODO
         초기화가 필요한 매니저 초기화 시키기
         */
        Input.Init();
        Sound.Init();
    }
}