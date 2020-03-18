using UnityEngine;
using System.Collections;
using ARGear;
using Samples.Scripts;

public class BlendShapeWeight : MonoBehaviour
{
    public int index = -1;
    
    int blendShapeCount;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    string[] properties;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    void Start()
    {
        blendShapeCount = skinnedMesh.blendShapeCount;
        properties = new string[52] {
            "blendShape2.eyeBlink_L", "blendShape2.eyeLookDown_L", "blendShape2.eyeLookIn_L", "blendShape2.eyeLookOut_L", 
            "blendShape2.eyeLookUp_L", "blendShape2.eyeSquint_L", "blendShape2.eyeWide_L",
            "blendShape2.eyeBlink_R", "blendShape2.eyeLookDown_R", "blendShape2.eyeLookIn_R", "blendShape2.eyeLookOut_R",
            "blendShape2.eyeLookUp_R", "blendShape2.eyeSquint_R", "blendShape2.eyeWide_R",
            "blendShape2.jawForward", "blendShape2.jawLeft", "blendShape2.jawRight", "blendShape2.jawOpen",
            "blendShape2.mouthClose", "blendShape2.mouthFunnel", "blendShape2.mouthPucker", "blendShape2.mouthRight", "blendShape2.mouthLeft",
            "blendShape2.mouthSmile_L", "blendShape2.mouthSmile_R", "blendShape2.mouthFrown_R", "blendShape2.mouthFrown_L",
            "blendShape2.mouthDimple_L", "blendShape2.mouthDimple_R", "blendShape2.mouthStretch_L", "blendShape2.mouthStretch_R", 
            "blendShape2.mouthRollLower", "blendShape2.mouthRollUpper", "blendShape2.mouthShrugLower", "blendShape2.mouthShrugUpper",
            "blendShape2.mouthPress_L", "blendShape2.mouthPress_R", "blendShape2.mouthLowerDown_L", "blendShape2.mouthLowerDown_R",
            "blendShape2.mouthUpperUp_L", "blendShape2.mouthUpperUp_R", "blendShape2.browDown_L", "blendShape2.browDown_R",
            "blendShape2.browInnerUp", "blendShape2.browOuterUp_L", "blendShape2.browOuterUp_R", "blendShape2.cheekPuff", 
            "blendShape2.cheekSquint_L", "blendShape2.cheekSquint_R", "blendShape2.noseSneer_L", "blendShape2.noseSneer_R", "blendShape2.tongueOut"};

        if (index < 0)
        {
            for (int i = 0; i < SampleManager.Instance.blendShapeWeights.Length; i++)
            {
                if (SampleManager.Instance.blendShapeWeights[i] != null) continue;
                if (index >= 0) continue;
                
                SampleManager.Instance.blendShapeWeights[i] = this;
                index = i;
            }
        }
    }

    void Update()
    {
        var face = ARGearManager.Instance.ARGFaces[index];
        if (face?.blendShapeWeight != null)
        {
            for (var i = 0; i < blendShapeCount; i++)
            {
                var property = properties[i];

                for (var idx = 0; idx < blendShapeCount; idx++)
                {
                    var targetBlendShapeName = skinnedMesh.GetBlendShapeName(idx);
                    if (property == targetBlendShapeName){
                        skinnedMeshRenderer.SetBlendShapeWeight(idx, face.blendShapeWeight[i] * 100);
                    }
                }
            }
        }
    }
}

