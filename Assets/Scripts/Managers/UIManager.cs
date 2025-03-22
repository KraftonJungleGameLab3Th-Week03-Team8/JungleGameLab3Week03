using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager
{
    private Canvas _inGameUI;
    private Canvas _titleUI;
    private Canvas _timerUI;
    private Canvas _keyGuideUI;

    private float _playTime;

    public void Init()
    {
        //Debug.Log(float.MaxValue);
        
        _inGameUI = GameObject.FindAnyObjectByType<InGameUI>().GetComponent<Canvas>();
        _titleUI = _inGameUI.transform.GetChild(0).GetComponent<Canvas>();
        _timerUI = _inGameUI.transform.GetChild(1).GetComponent<Canvas>();
        _keyGuideUI = GameObject.FindAnyObjectByType<UIKeyGuideDisplay>().GetComponent<Canvas>();

        Manager.Input.gameStartAction += GameStart; // 게임 시작 액션 등록
    }

    // 게임 시작
    public void GameStart()
    {
        _titleUI.enabled = false;
        _timerUI.enabled = true;

        Manager.Game.Init();
        _playTime = 0f;
    }

    // 시간 측정
    public void UpdateTime()
    {
        _playTime += Time.deltaTime;
        SetTimer(_playTime);
    }

    private void SetTimer(float timer)
    {
        _timerUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:00} : {1:00} : {2:00} . {3:00}"
            , (int)timer / 3600
            , (int)timer / 60 % 60
            , (int)timer % 60
            , (int)(timer * 100f) % 100); ;
    }

    // 랜딩 후 혼잣말
    private IEnumerator MumbleCoroutine(string message)
    {
        Manager.Game.PlayerController.MumbleText.enabled = true;
        //Manager.Game.PlayerController.MumbleText.color =
        //    UIKeyGuideDisplay.SetAlphaColor(Manager.Game.PlayerController.MumbleText.color, 1f);
        Manager.Game.PlayerController.MumbleText.text = message;
        yield return new WaitForSeconds(1.0f);
        Manager.Game.PlayerController.MumbleText.enabled = false;
        //Manager.Game.PlayerController.MumbleText.color =
        //    UIKeyGuideDisplay.SetAlphaColor(Manager.Game.PlayerController.MumbleText.color, 0f);

    }

    // 키 가이드 제어
    public void SetKeyGuidePosition(Vector3 pos)
    {
        _keyGuideUI.transform.position = pos;
    }
    public void SetKeyGuide(string[] keyNames)
    {
        _keyGuideUI.GetComponent<UIKeyGuideDisplay>().ShowKey(keyNames);
    }
    public void SetKeyGuide(string keyName, bool isHold, bool isDubleTab)
    {
        _keyGuideUI.GetComponent<UIKeyGuideDisplay>().ShowKey(keyName, isHold, isDubleTab);
    }
    public void HideKeyGuide()
    {
        _keyGuideUI.GetComponent<UIKeyGuideDisplay>().HideKey();
    }
}