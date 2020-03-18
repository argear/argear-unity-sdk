using System.Collections;
using Samples.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StickerScrollController : BaseScrollController
{
    void Start()
    {
    }

    void Update()
    {
        if (!SampleManager.Instance.IsHttpComplete || isInit) return;
        
        isInit = true;
        InitItem();
    }

    private void InitItem()
    {
        int size = SampleManager.Instance.StickerContents.Count;
        
        GameObject listItem;
        for (int i = 0; i < size; i++)
        {
            listItem = (GameObject)Instantiate(prefab, transform);
            var button = listItem.GetComponentInChildren<Button>();
            button.gameObject.AddComponent<ButtonValue>().index = i;
            button.onClick.AddListener(delegate() { OnButtonClick(button); });
            button.enabled = false;
        }
        
        SetImageButtons(SampleManager.Instance.StickerContents);
    }
    
    IEnumerator DownloadImage(Image image, string url)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var downloadTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Rect rect = new Rect(0, 0, downloadTexture.width, downloadTexture.height);
            image.sprite = Sprite.Create(downloadTexture, rect, new Vector2(0.5f, 0.5f));
        }
    }
    
    public override void OnButtonClick(Button button)
    {
        if (button.GetComponent<ButtonValue>() == null) return;
        SampleManager.Instance.SetSticker(button.GetComponent<ButtonValue>().index);
    }
}
