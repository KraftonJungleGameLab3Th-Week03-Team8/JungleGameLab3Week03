using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyGuideDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform displayArea;
    // 튜토리얼 키 출력 제어
    // 특정 키, 플레이어 머리 위에 출력(월드 스페이스 활용)
    // 컨트롤러별(Keyboard/Mouse, GamePad, ...)
        // Move
        // Jump
            // Charging
            // AirStop
            // Landing
        // Dash
        // 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowKey(string[] keyNames)
    {
        foreach(string keyName in keyNames)
        {
            RectTransform keyTr = GameObject.Find(keyName).GetComponent<RectTransform>();
            keyTr.parent = displayArea;
            keyTr.GetComponent<Image>().color = SetAlphaColor(keyTr.GetComponent<Image>().color, 1f);
            keyTr.GetComponentInChildren<TextMeshProUGUI>().color = SetAlphaColor(keyTr.GetComponentInChildren<TextMeshProUGUI>().color, 1f);            
        }
        
    }

    public void ResetKey()
    {

    }

    public static Color SetAlphaColor(Color color, float setAlpha)
    {
        Color setAlpaColor = color;
        setAlpaColor.a = setAlpha;
        return setAlpaColor;
    }
}
