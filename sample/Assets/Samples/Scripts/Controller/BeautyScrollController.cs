using System;
using System.Collections;
using System.Collections.Generic;
using ARGear;
using ARGear.Sdk.Data;
using Samples.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BeautyScrollController : BaseScrollController
{
    public Slider beautyLevelSlider;
    public ARGEnum.BeautyType currentType;
    private Button _currentButton;
    
    private List<string> _assetPathList;
    private bool _isClicked = false;

    void Awake()
    {
        _assetPathList = new List<string>
        {
            "beauty_vline_btn_default",
            "beauty_face_slim_btn_default",
            "beauty_jaw_btn_default",
            "beauty_chin_btn_default",
            "beauty_eye_btn_default",
            "beauty_eyegap_btn_default",
            "beauty_nose_line_btn_default",
            "beauty_nose_side_btn_default",
            "beauty_nose_length_btn_default",
            "beauty_mouth_size_btn_default",
            "beauty_eyeback_btn_default",
            "beauty_eyecorner_btn_default",
            "beauty_lip_size_btn_default",
            "beauty_skin_btn_default",
            "beauty_dark_circle_btn_default",
            "beauty_mouth_wrinkle_btn_default"
        };
    }

    void Start()
    {
        beautyLevelSlider.value = 0.0f;
        beautyLevelSlider.onValueChanged.AddListener(
            delegate { ValueUpdate(); }
        );
        
        InitItem();
    }

    void Update()
    {
        if (_currentButton != null)
        {
            _currentButton.Select();
        }
    }

    private void InitItem()
    {
        int size = _assetPathList.Count;
        
        GameObject listItem;
        for (int i = 0; i < size; i++)
        {
            listItem = (GameObject)Instantiate(prefab, transform);
            var button = listItem.GetComponentInChildren<Button>();
            button.gameObject.AddComponent<ButtonValue>().index = i;
            button.onClick.AddListener(delegate() { OnButtonClick(button); });
            button.enabled = false;
            SetButtonImage(button, _assetPathList[i]);
        }
    }

    private void SetButtonImage(Button button, string path)
    {
        Texture2D texture = Resources.Load(path, typeof(Texture2D)) as Texture2D;
        if (texture != null)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            button.GetComponentInChildren<Image>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            button.enabled = true;
        }
    }
    
    public override void OnButtonClick(Button button)
    {
        _isClicked = true;
        _currentButton = button;
        
        if (button.GetComponent<ButtonValue>() == null) beautyLevelSlider.value = 0.0f;
        
        int buttonIndex = button.GetComponent<ButtonValue>().index;
        currentType = (ARGEnum.BeautyType) buttonIndex;

        float buttonValue = SampleManager.Instance.BeautyDataCustom[buttonIndex];
        if (buttonValue > 0.0f)
        {
            float value = buttonValue;
            beautyLevelSlider.value = value / 100.0f;
        }
        else
        {
            beautyLevelSlider.value = 0.0f;
        }
    }

    public void SetSliderValue(float v)
    {
        beautyLevelSlider.value = v / 100.0f;
    }
    
    void ValueUpdate()
    {
        if (_isClicked)
        {
            _isClicked = false;
            ARGearManager.Instance.SetBeauty(SampleManager.Instance.BeautyDataCustom);
            return;
        }
        
        float sliderValue = beautyLevelSlider.value * 100.0f;
        
        SampleManager.Instance.BeautyDataCustom[(int) currentType] = sliderValue;
        ARGearManager.Instance.SetBeauty(SampleManager.Instance.BeautyDataCustom);
    }
}