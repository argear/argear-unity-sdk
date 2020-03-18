using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.UI;

public class SettingLayoutEvent : MonoBehaviour
{
    public GameObject SettingPanel;

    public Button background;

    public Toggle landmark;
    public Toggle blendShape;

    void Start()
    {
        background.onClick.AddListener(delegate {
            SettingPanel.SetActive(false);
        });
        
        landmark.onValueChanged.AddListener(delegate {
            ARGearManager.Instance.SetDrawLandmark(landmark.isOn);
        });
        
        blendShape.onValueChanged.AddListener(delegate
        {
            // ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.ARGItem);
            // ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.Bulge);
            // ARGearManager.Instance.isBlendShape = blendShape.isOn;
        });
    }
}