using UnityEngine;

public class TimeManager
{
    public bool isPause { get; private set; }

    private float _pauseTimeScale = 0f;
    private float _playTimeScale = 1f;
    public void SetPause()
    {
        isPause = true;
        Time.timeScale = _pauseTimeScale;
    }

    public void ReleasePause()
    {
        isPause = false;
        Time.timeScale = _playTimeScale;
    }
}
