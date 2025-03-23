using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIKeySkin : MonoBehaviour
{
    private static string _nameHold = "Hold";
    private static string _nameDoubleTab = "DoubleTab";
    public void Show(bool isHold, bool isDoubleTab)
    {
        SetAlpha(1f, isHold, isDoubleTab);
    }

    public void Hide()
    {
        SetAlpha(0f, false, false);
    }

    public void SetAlpha(float alpha, bool isHold, bool isDoubleTab)
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = SetAlphaColor(image.color, alpha);
        }
        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == _nameHold)
            {
                text.color = SetAlphaColor(text.color, (isHold) ? alpha : 0f);
            }
            else if (text.name == _nameDoubleTab)
            {
                text.color = SetAlphaColor(text.color, (isDoubleTab) ? alpha : 0f);
            }
            else
            {
                text.color = SetAlphaColor(text.color, alpha);
            }
        }
    }


    static public Color SetAlphaColor(Color color, float setAlpha)
    {
        Color setAlpaColor = color;
        setAlpaColor.a = setAlpha;
        return setAlpaColor;
    }
}
