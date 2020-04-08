using System.Collections;
using System.Collections.Generic;
using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.UI;

public class StickerLayoutEvent : MonoBehaviour
{
    public GameObject FunctionPanel;
    public GameObject StickerPanel;

    void OnEnable()
    {
        ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.Bulge);
    }

    void Start()
    {
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate()
            {
                OnStickerButton(buttonItem);
            });
        }
    }

    void OnStickerButton(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("CancelButton"))
        {
            FunctionPanel.SetActive(true);
            StickerPanel.SetActive(false);
        }
        else if (button.name.Equals("ClearButton"))
        {
            ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.ARGItem);
        }
    }
}