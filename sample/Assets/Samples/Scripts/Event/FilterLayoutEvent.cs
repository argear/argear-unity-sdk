using System.Collections;
using System.Collections.Generic;
using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.UI;

public class FilterLayoutEvent : MonoBehaviour
{
    public GameObject FunctionPanel;
    public GameObject FilterPanel;
    
    void Start()
    {
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate()
            {
                OnFilterButton(buttonItem);
            });
        }
    }

    void OnFilterButton(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("CancelButton"))
        {
            FunctionPanel.SetActive(true);
            FilterPanel.SetActive(false);
        }
        else if (button.name.Equals("ClearButton"))
        {
            ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.FilterItem);
        }
    }
}