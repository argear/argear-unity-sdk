using System;
using UnityEngine;

namespace ARGear.Sdk.Data
{
    [Serializable]
    public class ARGFace
    {
        public bool isValid = false;
        public double[] rotationMatrix;
        public double[] translationVector;
        public float[] landmark;
        public float[] blendShapeWeight;
        
        public Vector3 localPosition;
        public Quaternion localRotation;
    }
}