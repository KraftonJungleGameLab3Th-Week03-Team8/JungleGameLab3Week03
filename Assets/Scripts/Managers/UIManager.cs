using System.Collections;
using UnityEngine;

public class UIManager
{
    Canvas _inGameUI;
    Canvas _titleUI;
    Canvas _timerUI;

    public void Init()
    {
        _inGameUI = GameObject.FindAnyObjectByType<InGameUI>().GetComponent<Canvas>();
        _titleUI = _inGameUI.transform.GetChild(0).GetComponent<Canvas>();
        _timerUI = _inGameUI.transform.GetChild(1).GetComponent<Canvas>();

        Manager.Input.gameStartAction += GameStart; // 게임 시작 액션 등록
    }

    // 게임 시작
    public void GameStart()
    {
        _titleUI.enabled = false;
        _timerUI.enabled = true;

        Manager.Game.Init();
    }

    // 시간 측정
    public void UpdateTime()
    {

    }

    // 랜딩 후 혼잣말
    IEnumerator MumbleCoroutine(string message)
    {
        Manager.Game.PlayerController.MumbleText.enabled = true;
        Manager.Game.PlayerController.MumbleText.text = message;
        yield return new WaitForSeconds(1.0f);
        Manager.Game.PlayerController.MumbleText.enabled = false;
    }
}