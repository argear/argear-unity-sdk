using System;
using System.Collections.Generic;
using System.Diagnostics;
using ARGear.Sdk;
using ARGear.Sdk.BasicApi;
using ARGear.Sdk.Data;
using ARGearSDK;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;

namespace ARGear
{
    public class ARGearManager : SingletonMonoBehaviour<ARGearManager>
    {
        public string ApiUrl;
        public string ApiKey;
        public string SecretKey;
        public string AuthKey;

        public bool isBlendShape = false;
        
        [SerializeField]
        public List<InferenceConfig.Feature> InferenceConfigs = new List<InferenceConfig.Feature>
        {
            InferenceConfig.Feature.FACE_TRACKING
            // InferenceConfig.Feature.FACE_BLEND_SHAPES
        };
        
        public Material materialPlane;
        public Camera MainCamera { get; private set;  }
        public MeshRenderer MeshBackground { get; private set; }
        public Texture RenderTexture { get { return MeshBackground.sharedMaterial.mainTexture; } }
        
        bool isUpdateFov { get; set; }
        public float CameraZoomOffset { get; set; }

        private bool faceTransformsVisible = false;
        List<Transform> faceTransform = new List<Transform>();

        public ARGCamera ARGcamera { get; private set; }
        public ARGearNative ARGearNative { get { return ARGcamera.ArGearNative; } }
        public ARGFace[] ARGFaces { get { return ARGcamera.CameraFaces; } }
        
        void Awake()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
            
            isUpdateFov = true;

            MainCamera = Camera.main;
            MeshBackground = GameObject.CreatePrimitive(PrimitiveType.Quad).GetComponent<MeshRenderer>();
            MeshBackground.transform.parent = this.transform;
            MeshBackground.sharedMaterial = materialPlane;
            MeshBackground.GetComponent<MeshCollider>().enabled = false;
            
            ARGcamera = gameObject.AddComponent<ARGCamera>();
            ARGcamera.SetScreen(MeshBackground);
            
            for (var i = 0; i < ARGearDefine.MAX_TRACK_FACE; i++)
            {
                var tf = new GameObject().transform;
                tf.gameObject.hideFlags = HideFlags.HideInHierarchy;
                faceTransform.Add(tf);
            }
        }
        
        public Transform GetFaceTransform(int index)
        {
            if (faceTransform == null) return null;
            return index > faceTransform.Count ? null : faceTransform[index].transform;
        }

        void OnPostRender()
        {
            GL.Clear(true, true, Color.black);
            
            ARGcamera.DrawFrame();
#if UNITY_ANDROID
            if (isBlendShape)
            {
                UpdateFaceObjects();
                faceTransformsVisible = true;
            }
            else
            {
                if (faceTransformsVisible)
                {
                    if (faceTransform == null) return;
                    for (var i = 0; i < ARGFaces.Length; i++)
                    {
                        faceTransform[i].transform.localScale = new Vector3(0, 0,0);
                        faceTransform[i].transform.localPosition = new Vector3(0, 0,0);
                        faceTransform[i].transform.localRotation = new Quaternion(0, 0,0, 0);
                    }

                    faceTransformsVisible = false;
                }
            }
#endif
            
            GL.InvalidateState();
        }
        
        public void UpdateFaceObjects()
        {
            ARGcamera.GetRigidPose();
            if (faceTransform != null)
            {
                for (int i = 0; i < ARGFaces.Length; i++)
                {   
                    if (faceTransform[i] != null)
                    {
                        if (ARGFaces[i].isValid)
                        {   
                            faceTransform[i].transform.localPosition = ARGFaces[i].localPosition;
                            faceTransform[i].transform.localRotation = ARGFaces[i].localRotation;
                        }
                        else
                        {   
                            faceTransform[i].transform.localPosition = new Vector3();
                            faceTransform[i].transform.localRotation = new Quaternion();
                        }
                    }
                }
            }
        }
        
        void LateUpdate()
        {
            if (isUpdateFov == true)
            {
                ARGcamera.RefreshArrayInfo(MainCamera);
                MainCamera.fieldOfView += CameraZoomOffset;
            }
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                ARGearNative.Pause();
            else
                ARGearNative.Resume();
        }
        
        private void OnDestroy()
        {
            ARGearNative.Destroy();
        }
        
        public void ChangeCameraFacing()
        {
            ARGcamera.FlipCameraVertical();
            ARGearNative.ChangeCameraFacing();
        }

        public string RequestSignedUrl(string url, string title, string uuid)
        {
            return ARGearNative.RequestSignedUrl(url, title, uuid);
        }
        
        public void SetItem(ARGEnum.ContentsType type, string filePath, string uuid)
        {
            ARGearNative.SetItem(type, filePath, uuid);
        }
        
        public void SetBeauty(float[] values)
        {
#if UNITY_ANDROID
            ARGearNative.SetBeauty(ConvertBeautyData(values));
#else            
            ARGearNative.SetBeauty(values);
#endif
        }
        
        public void SetBulge(ARGEnum.BulgeType type)
        {
            ARGearNative.SetBulge(type);
        }

        public void SetFilterLevel(float level)
        {
            ARGearNative.SetFilterLevel(level);
        }
        
        public void ClearContents(ARGEnum.ContentsType type)
        {
            ARGearNative.ClearContents(type);
        }
        
        public void SetDrawLandmark(bool isVisible)
        {
            ARGearNative.SetDrawLandmark(isVisible);
        }
        
        private float[] ConvertBeautyData(float[] values)
        {
            if (values == null) return null;

            float[] returnValue = new float[ARGearDefine.MAX_BEAUTY_TYPE];
            for (int i = 0; i < values.Length; i++)
            {
                if ( (ARGEnum.BeautyType) i == ARGEnum.BeautyType.CHIN ||
                     (ARGEnum.BeautyType) i == ARGEnum.BeautyType.EYE_GAP ||
                     (ARGEnum.BeautyType) i == ARGEnum.BeautyType.NOSE_LENGTH ||
                     (ARGEnum.BeautyType) i == ARGEnum.BeautyType.MOUTH_SIZE ||
                     (ARGEnum.BeautyType) i == ARGEnum.BeautyType.EYE_CORNER ||
                     (ARGEnum.BeautyType) i == ARGEnum.BeautyType.LIP_SIZE )
                {
                    returnValue[i] = (values[i] - 50.0f) * 2.0f;
                }
                else
                {
                    returnValue[i] = values[i];
                }
            }
            
            return returnValue;
        }
    }
}