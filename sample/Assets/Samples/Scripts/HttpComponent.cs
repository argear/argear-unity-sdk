using System.Collections;
using System.Collections.Generic;
using System.IO;
using ARGear;
using ARGear.Sdk.Data;
using UnityEngine;
using UnityEngine.Networking;

public class HttpComponent : MonoBehaviour
{
     public delegate void OnHttpComplete(string data);
     private OnHttpComplete _onHttpComplete = null;
     
     public delegate void OnDownloadComplete(ARGEnum.ContentsType type, string uuid);
     private OnDownloadComplete _onDownloadComplete = null;
     
     void Start()
     {
          StartCoroutine(Get(ARGearManager.Instance.ApiUrl, "/api/v3/" + ARGearManager.Instance.ApiKey));
     }

     public void StartDownload(ARGEnum.ContentsType type, string url, string uuid)
     {
          StartCoroutine(DownloadFileRoutine(type, url, uuid));
     }

     private IEnumerator Get(string url, string subRoute)
     {
          string uri = url + subRoute;
          UnityWebRequest request = UnityWebRequest.Get(uri);
          {
               yield return request.SendWebRequest();

               if (request.isNetworkError)
               {
                    Debug.Log(request.error + " / " + request.responseCode);
               }
               else
               {
                    _onHttpComplete(request.downloadHandler.text);
               }
          }
     }
     
     private IEnumerator Post(string url, Dictionary<string, string> formFields)
     {
          UnityWebRequest request = UnityWebRequest.Post(url, formFields);
          {
               request.SetRequestHeader("Content-Type", "application/json");

               yield return request.SendWebRequest();

               if (request.isNetworkError)
               {
                    Debug.Log(request.error + " / " + request.responseCode);
               }
               else
               {
                    _onHttpComplete(request.downloadHandler.text);
               }
          }
     }
     
     private IEnumerator Post(string url, byte[] data)
     {
          UnityWebRequest request = UnityWebRequest.Put(url, data);
          {
               request.method = "POST";
               request.SetRequestHeader("Content-Type", "application/json");

               yield return request.SendWebRequest();

               if (request.isNetworkError)
               {
                    Debug.Log(request.error + " / " + request.responseCode);
               }
               else
               {
                    _onHttpComplete(request.downloadHandler.text);
               }
          }
     }
     
     public IEnumerator DownloadFileRoutine(ARGEnum.ContentsType type, string url, string uuid)
     {
          UnityWebRequest request = UnityWebRequest.Get(url);
          yield return request.SendWebRequest();

          if (request.isNetworkError)
          {
               Debug.Log("DownloadError");
          }
          else
          {
               SaveFile(type, uuid, request.downloadHandler.data);
          }
     }
     
     public void SaveFile(ARGEnum.ContentsType type, string uuid, byte[] bytes)
     {
          string destination = Application.persistentDataPath + "/" + uuid;
          if (File.Exists(destination))
          {
               File.Delete(destination);
          }
          File.WriteAllBytes(destination, bytes);

          _onDownloadComplete(type, uuid);
     }
     
     public void SetHttpComplete(OnHttpComplete listener)
     {
          _onHttpComplete = listener;
     }
     
     public void SetDownloadComplete(OnDownloadComplete listener)
     {
          _onDownloadComplete = listener;
     }
}