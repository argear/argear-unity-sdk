using System;

namespace ARGear.Sdk.Data
{
    [Serializable]
    public class InferenceConfig
    {
        // [Serializable]
        // public enum Debug
        // {
        //     NONE = 0,
        //     FACE_RECT_HW = 1,
        //     FACE_RECT_SW = 8,
        //     FACE_LANDMARK = 32,
        //     FACE_AXIES = 64
        // }
        
        [Serializable]
        public enum Feature
        {
            FACE_TRACKING = 0,
            //FACE_BLEND_SHAPES = 1,
            SEGMENTATION_HALF = 2
        }
    }
}