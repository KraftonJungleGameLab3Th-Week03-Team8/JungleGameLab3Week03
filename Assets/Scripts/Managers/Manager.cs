using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    #region 매니저
    public static GameManager Game { get { return Instance._game; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI {  get { return Instance._ui; } }

    private GameManager _game = new GameManager();
    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            //Invoke("Init", 0.2f);
        }
    }

    private void Update()
    {
        UI.UpdateTime();
    }

    // Manager 초기화
    private void Init()
    {
        /*
         TODO
         초기화가 필요한 매니저 초기화 시키기
         */
        Resource.Init();
        Sound.Init();
        Input.Init();
        UI.Init();
    }

    public void Clear()
    {
        Input.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}