using System;
using UnityEngine;

/// <summary>
/// Example implementation of item types, the only one that matters here is Outfit
/// </summary>
public enum ItemType
{
    Outfit,
    Utility,
    Other
}

[Serializable]
public class InventoryItem
{
    public string m_name;

    public Sprite m_icon;

    public int m_price;

    public int m_count = 1;

    public ItemType m_type;
}
