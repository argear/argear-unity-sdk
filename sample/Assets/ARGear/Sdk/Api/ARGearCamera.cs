using System;
using System.Collections.Generic;
using System.Linq;
using ARGear.Sdk.Data;
using ARGearSDK;
using UnityEngine;

namespace ARGear.Sdk.BasicApi
{
    public class ARGCamera : MonoBehaviour
    {
        protected Renderer _screen = null;
        protected int _rotation;
        protected bool _textureFlipHorizontally = false;
        protected bool _textureFilpVertical = false;
        protected int _width;
        protected int _height;
        protected int _orientation;
        protected float _horizontalFieldOfView;
        
        protected ARGFace[] mFaces = new ARGFace[ARGearDefine.MAX_TRACK_FACE];
        protected ARGSegmentation mSegmentation = new ARGSegmentation();

        public int Rotation { get { return _rotation; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public float HorizontalFieldOfView { get { return _horizontalFieldOfView; } }
        
        public ARGFace[] CameraFaces { get { return mFaces; } }
        public ARGSegmentation Segmentation { get { return mSegmentation; } }

        private ARGearNative _arGearNative;
        public ARGearNative ArGearNative { get { return _arGearNative; } }
        
        private System.IntPtr texPtr;
        private Texture2D _renderTex = null;
        
        void Awake()
        {
            for (int i = 0; i < ARGearDefine.MAX_TRACK_FACE; i++)
            {
                mFaces[i] = new ARGFace();
            }
        }

        void Start()
        {
            var featureArray = ARGearManager.Instance.InferenceConfigs.ToArray();
            var inferConfig = featureArray.Cast<int>().ToArray();
            
            _arGearNative = new ARGearNative(ARGearManager.Instance.ApiUrl,
                                        ARGearManager.Instance.ApiKey,
                                             ARGearManager.Instance.SecretKey,
                                             ARGearManager.Instance.AuthKey,
                                             inferConfig);
            
#if UNITY_IOS
            // ios raw data is rotate 90 degree
            _screen.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
#endif
        }
        
        void Update()
        {
            // check device orientation
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) 
            {
                _orientation = 0;
            }
            else
            {
                _orientation = 270;
            }

            // check native sdk
            if (_arGearNative == null) return;
            _width = _arGearNative.GetCameraWidth();
            _height = _arGearNative.GetCameraHeight();
            _horizontalFieldOfView = _arGearNative.GetHorizontalViewAngle();
            _rotation = 90;
        }

        public void DrawFrame()
        {
            DrawCameraFrame((int)Screen.width, (int)Screen.height);
        }

        private void DrawCameraFrame(int width, int height)
        {
            System.IntPtr tex = System.IntPtr.Zero;
            int frameWidth = 0, frameHeight = 0;

            if (_arGearNative != null)
            {
                _arGearNative.OnDrawFrame(width, height);   
                tex = _arGearNative.GetRenderedTexID();
                frameWidth = _arGearNative.GetCameraWidth();
                frameHeight = _arGearNative.GetCameraHeight();
            }
            
            if (tex != System.IntPtr.Zero)
            {
                SetCameraTexture(frameWidth, frameHeight, tex);
            }

            if (_arGearNative != null)
            {
                var num = 0;
                for (var i = 0; i < mFaces.Length; i++)
                {
                    mFaces[i].isValid = _arGearNative.TrackedFaceValidation(i);
                    if (mFaces[i].isValid)
                    {
                        num++;
                        mFaces[i].translationVector = _arGearNative.GetTranslationVector(i);
                        mFaces[i].rotationMatrix = _arGearNative.GetRotationMatrix(i);
                        mFaces[i].landmark = _arGearNative.GetLandmark(i);
                        mFaces[i].blendShapeWeight = _arGearNative.GetBlendShapeWeight(i);
                    }
                }
                mSegmentation.textureId = _arGearNative.GetSegmentationTextureId();
            }
        }
        
        private void SetCameraTexture(int width, int height, System.IntPtr ptr)
        {
            if (_renderTex == null)
            {
                if (ptr != System.IntPtr.Zero && width * height != 0)
                {
                    texPtr = ptr;
                    _width = width;
                    _height = height;
                    _renderTex = Texture2D.CreateExternalTexture(width, height, TextureFormat.ARGB32, false, false, texPtr);
                    _screen.material.mainTexture = _renderTex;
                }
            }
            else
            {
                if ((ptr != texPtr || _width != width || _height != height) && width * height != 0)
                {
                    texPtr = ptr;
                    _width = width;
                    _height = height;
                    _renderTex = Texture2D.CreateExternalTexture(width, height, TextureFormat.ARGB32, false, false, texPtr);
                    _screen.material.mainTexture = _renderTex;
                }
            }
        }
        
        public void FlipCameraVertical()
        {
#if UNITY_IOS
            _textureFilpVertical = !_textureFilpVertical;
#endif
        }
        
        public void SetScreen(Renderer screen)
        {
            _screen = screen;
        }
        
        public void RefreshArrayInfo(Camera camera)
        {
            if (_width <= 0 || _height <= 0)
            {
                return;
            }
            
            float rotatedWidth;
            float rotatedHeight;
            float verticalFovRadian;

            if ((_rotation % 180) == 90)
            {
                //  Portrait
                rotatedWidth = _height; 
                rotatedHeight = _width; 
                verticalFovRadian = _horizontalFieldOfView * Mathf.Deg2Rad;
            }
            else
            {
                //  Landscape
                rotatedWidth = _width;
                rotatedHeight = _height;
                verticalFovRadian = Mathf.Atan(Mathf.Tan(_horizontalFieldOfView * Mathf.Deg2Rad * 0.5f) * _height / _width) * 2.0f;
            }

            //  Portrait mode
            float scaleFactor = Screen.width / rotatedWidth;
            if (rotatedHeight * scaleFactor < Screen.height)
            {
                scaleFactor = Screen.height / rotatedHeight;
            }

            float screenVerticalFov = Mathf.Atan(Mathf.Tan(verticalFovRadian * 0.5f) * Screen.height / (rotatedHeight * scaleFactor)) * 2.0f;
            camera.fieldOfView = screenVerticalFov * Mathf.Rad2Deg;

            //  Setup the screen
            float farDist = camera.farClipPlane - 0.01f; // 1cm close to the far clip plane

            float effectiveWidth;
            float effectiveHeight;
            if ((_rotation % 180) == 90)
            {
                //  Portrait
                effectiveHeight = farDist * Mathf.Tan(_horizontalFieldOfView * Mathf.Deg2Rad * 0.5f) * 2.0f;
                effectiveWidth = effectiveHeight * rotatedWidth / rotatedHeight;
            }
            else
            {
                //  Landscape
                effectiveWidth = farDist * Mathf.Tan(_horizontalFieldOfView * Mathf.Deg2Rad * 0.5f) * 2.0f;
                effectiveHeight = effectiveWidth * rotatedHeight / rotatedWidth;
            }

            _screen.transform.localPosition = new Vector3(0, 0, farDist);

#if UNITY_ANDROID
            if ((_rotation % 180) == 90)
            {
                _screen.transform.localScale =
                    new Vector3(
                        effectiveWidth,
                        effectiveHeight * ((_textureFlipHorizontally) ? -1.0f : 1.0f),
                        1.0f);
            }
            else
            {
                _screen.transform.localScale =
                    new Vector3(
                        effectiveWidth * (( _textureFlipHorizontally) ? 1.0f : -1.0f),
                        effectiveHeight,
                        1.0f);
            }
#elif UNITY_IOS
            if ((_rotation % 180) == 90)
            {
                _screen.transform.localScale =
                    new Vector3(
                        effectiveHeight,
                        effectiveWidth * ((_textureFilpVertical) ? -1.0f : 1.0f),
                        1.0f);
            }
            else
            {
                _screen.transform.localScale =
                    new Vector3(
                        effectiveHeight,
                        effectiveWidth * (( _textureFilpVertical) ? 1.0f : -1.0f),
                        1.0f);
            }
#endif
        }
        
        public void GetRigidPose()
        {
            if (mFaces == null) return;
            foreach (var faceObject in mFaces)
            {
                Vector3 pos;
                Quaternion rot;

                if (faceObject == null)
                {
                    return;
                }

                if (!faceObject.isValid)
                {
                    rot = new Quaternion();
                    pos = new Vector3();

                    faceObject.localRotation = rot;
                    faceObject.localPosition = pos;
                    return;
                }

                bool mirrorHorizontally = true;
                double[] rotM = faceObject.rotationMatrix;
                double[] tv = faceObject.translationVector;

                Matrix4x4 transformationM = new Matrix4x4();
                transformationM.SetRow(0, new Vector4((float)rotM[0], (float)rotM[1], (float)rotM[2], (float)tv[0]));
                transformationM.SetRow(1, new Vector4((float)rotM[3], (float)rotM[4], (float)rotM[5], (float)tv[1]));
                transformationM.SetRow(2, new Vector4((float)rotM[6], (float)rotM[7], (float)rotM[8], (float)tv[2]));
                transformationM.SetRow(3, new Vector4(0, 0, 0, 1));

                Matrix4x4 invertYM = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, -1, 1));
                transformationM = invertYM * transformationM;

                Matrix4x4 rigidMotion = transformationM;

                var scale = 0.001f;
                if (_textureFlipHorizontally ^ mirrorHorizontally)
                {
                    rigidMotion = Matrix4x4.TRS(
                                      Vector3.zero,
                                      Quaternion.identity,
                                      new Vector3(-1.0f, 1.0f, 1.0f)) * Matrix4x4.TRS(
                                      Vector3.zero,
                                      Quaternion.AngleAxis(_orientation, Vector3.back),
                                      new Vector3(-scale, scale, scale)) * rigidMotion;
                }
                else
                {
                    rigidMotion = Matrix4x4.TRS(
                                      Vector3.zero,
                                      Quaternion.AngleAxis(Math.Abs(_orientation - _rotation), Vector3.back),
                                      new Vector3(-scale, scale, scale)) * rigidMotion;
                }
                
                pos = rigidMotion.GetColumn(3);
                rot = Quaternion.LookRotation(rigidMotion.GetColumn(2), rigidMotion.GetColumn(1));

                faceObject.localRotation = rot;
                faceObject.localPosition = pos;
            }
        }
    }
}