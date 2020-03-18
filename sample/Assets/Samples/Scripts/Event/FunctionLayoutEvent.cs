using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class FunctionLayoutEvent : MonoBehaviour
{
    public GameObject FunctionPanel;
    public GameObject SettingPannel;
    
    public GameObject FilterPanel;
    public GameObject StickerPanel;
    public GameObject BeautyPanel;
    public GameObject BulgePanel;
    
    void Start()
    {
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate() { OnButtonClick(buttonItem); });
        }
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {	
            if (Input.GetKey(KeyCode.Escape))
            {
                if (FunctionPanel.active)
                {
                    Application.Quit();
                    return;
                }
            }
        }  
    }

    void OnButtonClick(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("FilterButton"))
        {
            FunctionPanel.SetActive(false);
            FilterPanel.SetActive(true);
        }
        else if (button.name.Equals("ContentButton"))
        {
            FunctionPanel.SetActive(false);
            StickerPanel.SetActive(true);
        }
        else if (button.name.Equals("BeautyButton"))
        {
            FunctionPanel.SetActive(false);
            BeautyPanel.SetActive(true);
        }
        else if (button.name.Equals("BulgeButton"))
        {
            FunctionPanel.SetActive(false);
            BulgePanel.SetActive(true);
        }
    }
}
