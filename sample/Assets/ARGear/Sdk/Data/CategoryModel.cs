using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryModel
{
    public string uuid;
    public string title;
    public string description;
    public bool is_bundle;
    public long updated_at;
    public string status;
    public List<ItemModel> items;
}