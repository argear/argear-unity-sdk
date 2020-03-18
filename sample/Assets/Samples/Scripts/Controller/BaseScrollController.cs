using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BaseScrollController : MonoBehaviour
{
    public GameObject prefab;
    protected bool isInit = false;
    
    protected virtual async void SetImageButtons(List<ItemModel> data)
    {
        var buttons = gameObject.GetComponentsInChildren<Button>();
        for (var i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                Texture2D texture = await GetRemoteTexture((data[i].thumbnailUrl));
                var scaleTexture = ScaleTexture(texture, 120, 120);
                
                Rect rect = new Rect(0, 0, scaleTexture.width, scaleTexture.height);
                buttons[i].GetComponentInChildren<Image>().sprite = Sprite.Create(scaleTexture, rect, new Vector2(0.5f, 0.5f));
                buttons[i].enabled = true;
            }
        }
    }
    
    protected async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncRequest = request.SendWebRequest();
            while (asyncRequest.isDone == false)
            {
                await Task.Yield();
            }

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log( $"{ request.error }, URL:{ request.url }" );
                return null;
            }
            else
            {
                return DownloadHandlerTexture.GetContent(request);
            }
        }
    }
    
    protected Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth,targetHeight,source.format,true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight); 
        for (var px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth))); 
        }
        result.SetPixels(rpixels,0); 
        result.Apply(); 
        return result; 
    }

    public virtual void OnButtonClick(Button button)
    {
    }
}
