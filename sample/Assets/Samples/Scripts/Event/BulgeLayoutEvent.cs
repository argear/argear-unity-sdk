using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.UI;

public class BulgeLayoutEvent : MonoBehaviour
{
    public GameObject FunctionPanel;
    public GameObject BulgePanel;

    void Start()
    {
        var buttonArray = this.GetComponentsInChildren<Button>();
        foreach (var buttonItem in buttonArray)
        {
            buttonItem.onClick.AddListener(delegate()
            {
                OnBulgeButton(buttonItem);
            });
        }
    }

    void OnBulgeButton(Button button)
    {
        if (button == null) return;
        if (button.name.Equals("CancelButton"))
        {
            FunctionPanel.SetActive(true);
            BulgePanel.SetActive(false);
        }
        else if (button.name.Equals("ClearButton"))
        {
            ARGearManager.Instance.ClearContents(ARGEnum.ContentsType.Bulge);
        }
        else if (button.name.Equals("Fun1Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN1);
        }
        else if (button.name.Equals("Fun2Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN2);
        }
        else if (button.name.Equals("Fun3Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN3);
        }
        else if (button.name.Equals("Fun4Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN4);
        }
        else if (button.name.Equals("Fun5Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN5);
        }
        else if (button.name.Equals("Fun6Button"))
        {
            ARGearManager.Instance.SetBulge(ARGEnum.BulgeType.FUN6);
        }
    }
}