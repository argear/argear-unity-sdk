using System;
using System.Collections.Generic;
using ARGear;
using ARGear.Sdk.Data;
using ARGearSDK;
using UnityEngine;

namespace Samples.Scripts
{
    public class SampleManager : SingletonMonoBehaviour<SampleManager>
    {
        private HttpComponent _http;
        
        public FaceComponent[] faceComponents = new FaceComponent[ARGearDefine.MAX_TRACK_FACE];
        public BlendShapeWeight[] blendShapeWeights = new BlendShapeWeight[ARGearDefine.MAX_TRACK_FACE];
        
        // Sdk Data
        public ContentsResponse CmsResponseData { get; set; }
        public List<ItemModel> FilterContents { get; set; }
        public List<ItemModel> StickerContents { get; set; }
        
        public float[] BeautyDataDefault = { 0f, 0f, 0f, 50f, 0f, 50f, 0f, 0f, 50f, 50f, 0f, 50f, 50f, 0f, 0f, 0f };
        public float[] BeautyDataCustom;

        public bool IsHttpComplete { get; set; }

        void Awake()
        {
            BeautyDataCustom = (float[])BeautyDataDefault.Clone();
            
            _http = gameObject.AddComponent<HttpComponent>();
            _http.SetHttpComplete(DrawUi);
            _http.SetDownloadComplete(DownloadComplete);
        }

        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        void LateUpdate()
        {
#if UNITY_ANDROID
            FaceComponent.UpdatePost();
#endif
        }
        
        private void DrawUi(string data)
        {
            CmsResponseData = JsonUtility.FromJson<ContentsResponse>(data);
            if (CmsResponseData?.categories == null) return;

            FilterContents = new List<ItemModel>();
            StickerContents = new List<ItemModel>();
        
            foreach (var category in CmsResponseData.categories)
            {
                if (category.title.Equals("filters"))
                {
                    foreach (var item in category.items)
                    {
                        FilterContents.Add(item);
                    }
                }
                else
                {
                    foreach (var item in category.items)
                    {
                        StickerContents.Add(item);
                    }
                }
            }

            IsHttpComplete = true;
        }
        
        public void SetSticker(int index)
        {
            if (CmsResponseData != null)
            {
                if (StickerContents != null)
                {
                    string signedUrl = ARGearManager.Instance.RequestSignedUrl(
                        StickerContents[index].zipFileUrl, StickerContents[index].title, StickerContents[index].uuid);

                    _http.StartDownload(ARGEnum.ContentsType.ARGItem, signedUrl, StickerContents[index].uuid);
                }
            }
        }
        
        public void SetFilter(int index)
        {
            if (CmsResponseData != null)
            {
                if (FilterContents != null)
                {
                    string signedUrl = ARGearManager.Instance.RequestSignedUrl(
                        FilterContents[index].zipFileUrl, FilterContents[index].title, FilterContents[index].uuid);
                    
                    _http.StartDownload(ARGEnum.ContentsType.FilterItem, signedUrl, FilterContents[index].uuid);
                }
            }
        }
        
        private void DownloadComplete(ARGEnum.ContentsType type, string uuid)
        {
            ARGearManager.Instance.SetItem(type, Application.persistentDataPath + "/" + uuid, uuid);
        }
    }
}