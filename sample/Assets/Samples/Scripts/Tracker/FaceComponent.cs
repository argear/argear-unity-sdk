using System.Collections;
using System.Collections.Generic;
using ARGear;
using Samples.Scripts;
using UnityEngine;

public class FaceComponent : MonoBehaviour
{
    static Dictionary<int, FaceComponent> dicInstances = new Dictionary<int, FaceComponent>();

    public int indexFace = -1;
    [SerializeField] GameObject blendShapeModel;

    public GameObject BodyFace { get; set; }
    public Transform HeadTransform {  get { return BodyFace.transform; } }
    public bool IsEnable { get; set; }
    public bool IsHeadEnable { get; set; }
    bool preValidState { get; set; }
    
    public static FaceComponent GetFaceByIndex( int index )
    {
        if (dicInstances.ContainsKey(index) == false) return null;
        return dicInstances[index];
    }
    
    public static void UpdatePost()
    {
        if (dicInstances == null) return;
        foreach (var face in dicInstances)
        {
            if (face.Value != null && face.Value.gameObject.activeInHierarchy == true)
            {
                face.Value.UpdatePostRender();
            }
        }
    }

    public void StateReset()
    {
        preValidState = false;
    }

    void Awake()
    {
        IsHeadEnable = true;
        IsEnable = true;
    }

    void Start()
    {
        BodyFace = blendShapeModel;

        // if not pre indexing
        if (indexFace < 0)
        {
            for (int i = 0; i < SampleManager.Instance.faceComponents.Length; i++)
            {
                if (SampleManager.Instance.faceComponents[i] != null) continue;
                if (indexFace >= 0) continue;
                
                SampleManager.Instance.faceComponents[i] = this;
                indexFace = i;
            }
        }

        if (dicInstances.ContainsKey(indexFace) == true)
        {
            dicInstances[indexFace] = this;
        }
        else
        {
            dicInstances.Add(indexFace, this);
        }
    }

    void Update()
    {
        CheckState();
        
        bool currentEnable = ARGearManager.Instance.ARGFaces[indexFace].isValid;
        IsEnable = currentEnable;
        
        if (currentEnable == false) return;
    }

    public void UpdatePostRender()
    {
        bool currentEnable = ARGearManager.Instance.ARGFaces[indexFace].isValid;
        IsEnable = currentEnable;
        
        if (currentEnable == false) return;

        UpdateFace();
        UpdatePoints();
    }

    void CheckState()
    {
        var faceFirstIsValid = ARGearManager.Instance.ARGFaces[indexFace].isValid;
        
        if (BodyFace.transform.position.z < 0.1f)
            faceFirstIsValid = false;
        
        if (faceFirstIsValid != preValidState)
        {
            BodyFace.SetActive(faceFirstIsValid);
            preValidState = faceFirstIsValid;
        }
        if (IsHeadEnable == false && BodyFace.activeSelf == true)
            BodyFace.SetActive(false);
    }

    void UpdateFace()
    {
        var transformFace = ARGearManager.Instance.GetFaceTransform(indexFace);
        BodyFace.transform.localRotation = transformFace.localRotation;
        BodyFace.transform.localPosition = transformFace.localPosition;
    }

    void UpdatePoints()
    {
        var transformBg = ARGearManager.Instance.MeshBackground.transform;
        Vector3 positionStart = transformBg.TransformPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 positionLast = transformBg.TransformPoint(new Vector3(-0.5f,-0.5f, 0.0f));

        var faceFirst = ARGearManager.Instance.ARGFaces[indexFace];
        var transformFace = ARGearManager.Instance.GetFaceTransform(indexFace);
    }
}
