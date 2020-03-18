using System;

namespace ARGear.Sdk.Data
{
    public class ARGEnum
    {
        [Serializable]
        public enum ContentsType
        {
            ARGItem = 0,
            FilterItem = 1,
            Beauty = 2,
            Bulge = 3
        }
        
        [Serializable]
        public enum BeautyType
        {
            VLINE = 0,
            FACE_SLIM = 1,
            JAW = 2,
            CHIN= 3,
            EYE = 4,
            EYE_GAP = 5,
            NOSE_LINE = 6,
            NOSE_SIDE = 7,
            NOSE_LENGTH = 8,
            MOUTH_SIZE = 9 ,
            EYE_BACK = 10,
            EYE_CORNER = 11,
            LIP_SIZE = 12,
            SKIN_FACE = 13,
            SKIN_DARK_CIRCLE = 14,
            SKIN_MOUTH_WRINKLE = 15
        }
        
        [Serializable]
        public enum BulgeType
        {
            NONE = -1,
            FUN1 = 1,
            FUN2 = 2,
            FUN3 = 3,
            FUN4 = 4,
            FUN5 = 5,
            FUN6 = 6
        }
    }
}