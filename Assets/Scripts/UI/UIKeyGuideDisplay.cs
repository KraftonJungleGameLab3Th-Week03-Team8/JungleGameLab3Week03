using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIKeyGuideDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform displayArea;

    //private static string _strDisplayArea = "Display Area";
    public static string pathSpace = "space";
    public static string pathLeftArrow = "leftArrow";
    public static string pathRightArrow = "rightArrow";
    public static string pathDownArrow = "downArrow";

    [Header("Test")]
    public string[] showKeys;
    public bool isTrackingPlayerHead;
    public bool isHold;
    public bool isDoubleTab;


    private void Awake()
    {
        displayArea = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Start()
    {
        ResetKey();
    }

    private void Update()
    {
        if (isTrackingPlayerHead)
        {
            transform.position = Manager.Game.PlayerController.transform.position;
            transform.position += Vector3.up;
        }
    }

    [ContextMenu("ShowKey(Test)")]
    private void TestShowKey()
    {
        ShowKey(showKeys);
    }

    [ContextMenu("ShowKey(Test0)")]
    private void TestShowKey0()
    {
        ShowKey(showKeys[0], isHold, isDoubleTab);
    }

    public void ShowKey(string[] keyNames)
    {
        foreach(string keyName in keyNames)
        {
            ShowKey(keyName, false, false);
        }
    }

    public void ShowKey(string keyName, bool isHold, bool isDubleTab)
    {
        UIKeySkin keySkin = transform.Find(keyName).GetComponent<UIKeySkin>();
        if (keySkin != null)
        {
            keySkin?.transform.SetParent(displayArea);
            keySkin?.Show(isHold, isDubleTab);
        }
    }

    [ContextMenu("HideKey()")]
    public void HideKey()
    {
        foreach(UIKeySkin keySkin in displayArea.GetComponentsInChildren<UIKeySkin>())
        {
            keySkin.transform.SetParent(transform);
            keySkin.Hide();
        }
    }

    public void ResetKey()
    {
        HideKey();
        foreach (UIKeySkin keySkin in GetComponentsInChildren<UIKeySkin>())
        {
            keySkin.Hide();
        }
    }
}
