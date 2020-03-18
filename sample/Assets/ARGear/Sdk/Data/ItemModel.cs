using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModel
{
    [SerializeField] public string uuid;
    [SerializeField] public string title;
    [SerializeField] public string description;
    [SerializeField] private string thumbnail;
    public string thumbnailUrl => thumbnail;
    [SerializeField] private string zip_file;
    public string zipFileUrl => zip_file;

    [SerializeField] private int num_stickers;
    public int numStickers => num_stickers;

    [SerializeField] private int num_effects;
    public int numEffects => num_effects;

    [SerializeField] private int num_bgms;
    public int numBgms => num_bgms;

    [SerializeField] private int num_filters;
    public int numFilters => num_filters;

    [SerializeField] private int num_masks;
    public int numMasks => num_masks;

    [SerializeField] private bool has_trigger;
    public bool hasTrigger => has_trigger;

    [SerializeField] public string status;
    [SerializeField] private long updated_at;
    public long updatedAt => updated_at;
    [SerializeField] public string type;
}
