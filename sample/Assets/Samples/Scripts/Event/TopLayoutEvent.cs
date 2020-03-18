using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.UI;

public class TopLayoutEvent : MonoBehaviour
{
    public GameObject SettingPanel;
    
    void Start()
    {
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate()
            {
                OnTopFunctionButton(buttonItem);
            });
        }
    }

    void OnTopFunctionButton(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("SettingButton"))
        {
            SettingPanel.SetActive(true);
        }
        else if (button.name.Equals("CameraChangeButton"))
        {
            ARGearManager.Instance.ChangeCameraFacing();
        }
    }
}
