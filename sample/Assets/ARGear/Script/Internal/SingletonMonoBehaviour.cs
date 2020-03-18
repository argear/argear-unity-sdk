using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    static T instance;
    public static T Instance
    {
        get
        {
            if( instance == null )
            { 
                instance = GameObject.FindObjectOfType<T>();
                if( instance == null )
                {   var newObject = new GameObject();
                    instance = newObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
