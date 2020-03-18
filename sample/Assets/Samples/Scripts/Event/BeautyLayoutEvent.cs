using System.Collections;
using System.Collections.Generic;
using ARGear;
using ARGear.Sdk.Data;
using Samples.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BeautyLayoutEvent : MonoBehaviour
{
    public GameObject FunctionPanel;
    public GameObject BeautyPanel;
    public BeautyScrollController controller;
    
    void Start()
    {
        ARGearManager.Instance.SetBeauty(SampleManager.Instance.BeautyDataCustom);
        
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate()
            {
                OnBeautyButton(buttonItem);
            });
        }

        controller = this.GetComponentInChildren<BeautyScrollController>();
    }

    void OnBeautyButton(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("CancelButton"))
        {
            FunctionPanel.SetActive(true);
            BeautyPanel.SetActive(false);
        }
        else if (button.name.Equals("ClearButton"))
        {
            ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.Beauty);
            
            SampleManager.Instance.BeautyDataCustom = (float[]) SampleManager.Instance.BeautyDataDefault.Clone();
            ARGearManager.Instance.SetBeauty(SampleManager.Instance.BeautyDataCustom);

            if (controller != null)
            {
                controller.SetSliderValue(SampleManager.Instance.BeautyDataCustom[(int) controller.currentType]);
            }
        }
    }
}