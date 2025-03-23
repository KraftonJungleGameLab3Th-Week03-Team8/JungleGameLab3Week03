using UnityEngine;

public class PauseKeyTutorial : MonoBehaviour
{
    public Define.KeyskinName[] keyNames = { Define.KeyskinName.space};
    public bool isHold;
    public bool isDoubleTab;
    public bool usePuase;
    public bool useOnlyOnce;
    [SerializeField] private bool _isExecuted;
    private Vector3 _displayPosOffset = new Vector3(0, 1f, 0f);
    public bool isInProcess = false;
    private PlayerController _playerCtrl;

    private void Update()
    {
        if (!isInProcess)
            return;

        Manager.UI.SetKeyGuidePosition(_playerCtrl.transform.position + _displayPosOffset);

        if (!usePuase)
            return;
        //if(InputKet...)
        if (CheckInputKey() || testTrigger)
        {
            if (!useOnlyOnce)
                _isExecuted = false;
            isInProcess = false;
            Manager.Game.ReleasePause();
            Manager.UI.HideKeyGuide();
        }
    }

    bool testTrigger;
    [ContextMenu("Action Tutorial")]
    void Test()
    {
        testTrigger = true;
    }

    private bool CheckInputKey()
    {
        switch (keyNames[0])
        {
            case Define.KeyskinName.space:
                if(Manager.Game.PlayerController.IsChargeLanding)
                {
                    return true;
                }
                break;
            case Define.KeyskinName.leftArrow:
                if (Manager.Game.PlayerController.IsDash)
                {
                    if (Manager.Input.MoveDir.x < 0)
                        return true;
                }
                break;
            case Define.KeyskinName.rightArrow:
                Debug.Log("Manager.Input.IsPressDash: " + Manager.Input.IsPressDash);
                if(Manager.Game.PlayerController.IsDash)
                {
                    Debug.Log("Manager.Input.MoveDir.x: " + Manager.Input.MoveDir.x);
                    if (Manager.Input.MoveDir.x > 0)
                        return true;
                }
                break;
            case Define.KeyskinName.downArrow:
                break;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isExecuted)
            return;
        _playerCtrl = collision.GetComponent<PlayerController>();
        //if (collision.CompareTag("Player")
        if (_playerCtrl != null)
        {
            testTrigger = false;
            _isExecuted = true;
            isInProcess = true;
            if(usePuase)
                Manager.Game.SetPause();
            
            //Manager.UI.SetKeyGuidePosition(playerCtrl.transform.position + _displayPosOffset);
            foreach(Define.KeyskinName keyName in keyNames)
            {
                Manager.UI.SetKeyGuide(keyName.ToString(), isHold, isDoubleTab);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isInProcess)
            return;
        PlayerController playerCtrl = collision.GetComponent<PlayerController>();
        //if (collision.CompareTag("Player")
        if (playerCtrl != null)
        {
            isInProcess = false;
            if (!useOnlyOnce)
                _isExecuted = false;
            //Manager.Time.ReleasePause();
            Manager.UI.HideKeyGuide();
        }
    }


}
