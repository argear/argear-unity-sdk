using System.Collections.Generic;
using System.Runtime.InteropServices;
using ARGear.Sdk.Data;
using UnityEngine;

namespace ARGear.Sdk
{
    public class ARGearNative
    {
#if UNITY_ANDROID
        private AndroidJavaObject pluginClass;
#elif UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void ARGearInit(string apiUrl, string apiKey, string apiSecretKey, string authKey, int[] config);
        [DllImport ("__Internal")]
        private static extern void ARGearResume();
        [DllImport ("__Internal")]
        private static extern void ARGearPause();
        [DllImport ("__Internal")]
        private static extern void ARGearDestroy();
        [DllImport ("__Internal")]
        private static extern int ARGearGetRenderWidth();
        [DllImport ("__Internal")]
        private static extern int ARGearGetRenderHeight();
        [DllImport ("__Internal")]
        private static extern float ARGearGetHorizontalViewAngle();
        [DllImport ("__Internal")]
        private static extern System.IntPtr ARGearGetRenderedMTLTexID();
        [DllImport ("__Internal")]
        private static extern int ARGearGetRenderedGLTexID();
        [DllImport("__Internal")]
        private static extern int ARGearGetSegmentationTextureId();
        [DllImport ("__Internal")]
        private static extern bool ARGearTrackedFaceValidation(int index);
        [DllImport ("__Internal")]
        private static extern double[] ARGearGetRotationMatrix(int index);
        [DllImport ("__Internal")]
        private static extern double[] ARGearGetTranslationVector(int index);
        [DllImport ("__Internal")]
        private static extern float[] ARGearGetBlendShapeWeight(int index);
        [DllImport ("__Internal")]
        private static extern float[] ARGearGetLandmark(int index);
        [DllImport ("__Internal")]
        private static extern float[] ARGearGetMesh(int index);
        [DllImport ("__Internal")]
        private static extern void ARGearSetDrawLandmark(bool isVisible);
        [DllImport("__Internal")]
        private static extern void ARGearChangeCameraFacing();
        [DllImport("__Internal")]
        private static extern string ARGearRequestSignedUrl(string url, string title, string uuid);
        [DllImport ("__Internal")]
        private static extern int ARGearSetItem(int type, string path, string uuid);
        [DllImport("__Internal")]
        private static extern void ARGearSetFilterLevel(float level);
        [DllImport("__Internal")]
        private static extern void ARGearSetBeauty(float[] values);
        [DllImport ("__Internal")]
        private static extern int ARGearSetBulge(int type);
        [DllImport ("__Internal")]
        private static extern int ARGearClearContents(int type);
#endif

        private string Url;
        private string Key;
        private string SecretKey;
        private string AuthKey;
        private int[] InferenceConfig;

        public ARGearNative(string url, string key, string secretKey, string authKey, int[] inferenceConfig)
        {
            Url = url;
            Key = key;
            SecretKey = secretKey;
            AuthKey = authKey;
            InferenceConfig = inferenceConfig;
#if UNITY_ANDROID
            ARGearAndroidInit();
#elif UNITY_IOS
            ARGearInit(url, key, secretKey, authKey, inferenceConfig);
#endif
        }

        private void ARGearAndroidInit()
        {
#if UNITY_ANDROID
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    if (pluginClass == null)
                    {
                        pluginClass = new AndroidJavaObject("com.argear.unityplugin.ARGearPlugin",
                            currentActivity,
                            Url,
                            Key,
                            SecretKey,
                            AuthKey,
                            InferenceConfig);
                    }
                }
            }
#endif
        }
        
        public void Resume()
        {
#if UNITY_ANDROID
            bool validSession = pluginClass.Call<bool>("onResume");
            if (!validSession)
            {
                ARGearAndroidInit();
            }
#elif UNITY_IOS
            ARGearResume();
#endif
        }

        public void Pause()
        {
#if UNITY_ANDROID
            pluginClass.Call<bool>("onPause");
#elif UNITY_IOS
            ARGearPause();
#endif
        }
        
        public void Destroy()
        {
#if UNITY_ANDROID
            pluginClass.Call<int>("onDestroy");
#elif UNITY_IOS
            ARGearDestroy();
#endif
        }

        public int GetCameraWidth()
        {
#if UNITY_ANDROID
            return pluginClass.Call<int>("getCameraWidth");
#elif UNITY_IOS
            return ARGearGetRenderWidth();
#endif
        }

        public int GetCameraHeight()
        {
#if UNITY_ANDROID
            return pluginClass.Call<int>("getCameraHeight");
#elif UNITY_IOS
            return ARGearGetRenderHeight();
#endif
        }
        
        public float GetHorizontalViewAngle()
        {
#if UNITY_ANDROID
            return pluginClass.Call<float>("getHorizontalViewAngle");
#elif UNITY_IOS
            return ARGearGetHorizontalViewAngle();
#endif
        }

        public void OnDrawFrame(int width, int height)
        {
#if UNITY_ANDROID
            pluginClass.Call("onDrawFrame", width, height);
#endif
        }

        public System.IntPtr GetRenderedTexID()
        {
#if UNITY_ANDROID
            return (System.IntPtr) pluginClass.Call<int>("getRenderedTexID");
#elif UNITY_IOS
            return ARGearGetRenderedMTLTexID();
            // return (System.IntPtr) ARGearGetRenderedGLTexID();
#endif
        }

        public bool TrackedFaceValidation(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<bool>("trackedFaceValidation", index);
#elif UNITY_IOS
            return ARGearTrackedFaceValidation(index);
#endif
        }

        public double[] GetTranslationVector(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<double[]>("getTranslationVector", index);
#elif UNITY_IOS
            return ARGearGetTranslationVector(index);
#endif
        }

        public double[] GetRotationMatrix(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<double[]>("getRotationMatrix", index);
#elif UNITY_IOS
            return ARGearGetRotationMatrix(index);
#endif
        }

        public float[] GetBlendShapeWeight(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<float[]>("getBlendShapeWeight", index);
#elif UNITY_IOS
            return ARGearGetBlendShapeWeight(index);
#endif
        }
        
        public float[] GetLandmark(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<float[]>("getLandmark", index);
#elif UNITY_IOS
            return ARGearGetLandmark(index);
#endif  
        }
        
        public float[] GetMesh(int index)
        {
#if UNITY_ANDROID
            return pluginClass.Call<float[]>("getMesh", index);
#elif UNITY_IOS
            return ARGearGetMesh(index);
#endif  
        }
        
        public int GetSegmentationTextureId()
        {
#if UNITY_ANDROID
            return pluginClass.Call<int>("getSegmentationTextureId");
#elif UNITY_IOS
            return ARGearGetSegmentationTextureId();
#endif
        }

        public void SetDrawLandmark(bool isVisible)
        {
#if UNITY_ANDROID
            pluginClass.Call("setDrawLandmark", isVisible, false);
#elif UNITY_IOS
            ARGearSetDrawLandmark(isVisible);
#endif
        }
        
        public void ChangeCameraFacing()
        {
#if UNITY_ANDROID
            pluginClass.Call("changeCameraFacing");
#elif UNITY_IOS
            ARGearChangeCameraFacing();
#endif
        }

        public string RequestSignedUrl(string url, string title, string uuid)
        {
#if UNITY_ANDROID
            return pluginClass.Call<string>("requestSignedUrl", url, title, uuid);
#elif UNITY_IOS
            return ARGearRequestSignedUrl(url, title, uuid);
#endif
        }
        
        public void SetItem(ARGEnum.ContentsType type, string path, string uuid)
        {
#if UNITY_ANDROID
            pluginClass.Call("setItem", (int)type, path, uuid);
#elif UNITY_IOS
            ARGearSetItem((int)type, path, uuid);
#endif
        }

        public void SetFilterLevel(float level)
        {
#if UNITY_ANDROID
            pluginClass.Call("setFilterLevel", (int)(level * 100));
#elif UNITY_IOS
            ARGearSetFilterLevel(level);
#endif
        }

        public void SetBeauty(float[] values)
        {
#if UNITY_ANDROID
            pluginClass.Call("setBeauty", values);
#elif UNITY_IOS
            ARGearSetBeauty(values);
#endif
        }
        
        public void SetBulge(ARGEnum.BulgeType type)
        {
#if UNITY_ANDROID
            pluginClass.Call("setBulge", (int)type);
#elif UNITY_IOS
            ARGearSetBulge((int)type);
#endif
        }

        public void ClearContents(ARGEnum.ContentsType type)
        {
#if UNITY_ANDROID
            pluginClass.Call("clearContent", (int)type);
#elif UNITY_IOS
            ARGearClearContents((int)type);
#endif
        }
    }
}