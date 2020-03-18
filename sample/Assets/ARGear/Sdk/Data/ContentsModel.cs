using System.Collections.Generic;

[System.Serializable]
public class ContentsResponse
{
    public string api_key;
    public string name;
    public string description;
    public string status;
    public long last_updated_at;
    public List<CategoryModel> categories;
}