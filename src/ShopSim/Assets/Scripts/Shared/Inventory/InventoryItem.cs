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

public enum ItemOwner
{
    Player,
    Shopkeeper
}

[Serializable]
public class InventoryItem
{
    public string m_name;

    public Sprite m_icon;

    public int m_price;

    public int m_count = 1;

    public ItemType m_type;

    public ItemOwner m_owner;

    public InventoryItem()
    {

    }

    //Hacky solution, but it will work, FIXME if time allows
    public InventoryItem(InventoryItem other)
    {
        this.m_name = other.m_name;
        this.m_icon = other.m_icon;
        this.m_count = other.m_count;
        this.m_type = other.m_type;
        this.m_owner = other.m_owner;
        this.m_price = other.m_price;
    }
}
