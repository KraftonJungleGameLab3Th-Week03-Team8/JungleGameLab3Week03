using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleTapInteraction : IInputInteraction
{
    private float lastTapTime = -1f;  // 마지막 탭 시간
    private const float doubleTapThreshold = 0.3f; // 더블탭 간격 제한 (0.3초)

    public void Process(ref InputInteractionContext context)
    {
        if (context.control.IsPressed()) // 키를 누르는 순간 실행
        {
            float currentTime = Time.time;
            if (lastTapTime > 0 && (currentTime - lastTapTime) <= doubleTapThreshold)
            {
                // 더블탭으로 인식
                context.Performed();
                lastTapTime = -1f; // 리셋
            }
            else
            {
                // 첫 번째 탭 처리
                lastTapTime = currentTime;
                context.Started();
            }
        }
    }

    public void Reset()
    {
        lastTapTime = -1f;
    }
}