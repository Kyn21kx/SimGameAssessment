using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class InventoryBag : MonoBehaviour
{
    private const float SLOT_SPACING_X = 250f; 
    private const float SLOT_SPACING_Y = 100f;

    private const string PATH_TO_ITEM_SPRITES = "Assets/ThirdParty/Cainos/Customizable Pixel Character/Texture/Cloth";
    private const string CLOTHING_PREFIX = "TX Pixel Character - Cloth -";

    private Dictionary<string, InventoryItem> m_items;
    public bool IsShown => this.m_uiPanel.gameObject.activeInHierarchy;

    [SerializeField]
    private Image m_uiPanel;

    [SerializeField]
    private GameObject m_slotPrefab;

    [SerializeField]
    private GameObject m_slotParent;

    [SerializeField]
    private bool m_fillRandomly;

    private void Start()
    {
        Assert.IsNotNull(this.m_uiPanel, "UI panel for inventory is null, please set it in the editor!");
        this.m_uiPanel.gameObject.SetActive(false);
        if (!this.m_fillRandomly) return;
        this.FillInventoryAtRandom();
    }

    public void ShowInventory()
    {
        this.m_uiPanel.gameObject.SetActive(true);
    }

    public void DismissInventory()
    {
        this.m_uiPanel.gameObject.SetActive(false);
    }

    public void AddToInventory(InventoryItem item)
    {
        if (this.m_items.ContainsKey(item.m_name))
        {
            this.m_items[item.m_name].m_count++;
            return;
        }
        this.m_items[item.m_name] = item;
        //Get the next available panel position and place the item there
    }

    public InventoryItem GetFromInventoryOrDefault(string itemName)
    {
        bool contains = this.m_items.TryGetValue(itemName, out InventoryItem result);
        return contains ? result : null;
    }

    public ICollection<InventoryItem> GetAllInventoryItems()
    {
        return this.m_items.Values;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            this.DismissInventory();
        }
    }

    /// <summary>
    /// This is just a test method for the purposes of this take home assignment
    /// </summary>
    public void FillInventoryAtRandom()
    {
        //Get a list of possible items and then just fill them up at random
        ICollection<InventoryItem> items = this.GetItemsFromPath(PATH_TO_ITEM_SPRITES);

    }

    private ICollection<InventoryItem> GetItemsFromPath(string path)
    {
        return null;
    }

}

