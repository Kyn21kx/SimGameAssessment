using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Linq;

public class InventoryBag : MonoBehaviour
{
    private const float SLOT_SPACING_X = 250f; 
    private const float SLOT_SPACING_Y = 400f;

    private const string PATH_TO_ITEM_SPRITES = "Cloth/";
    private const string CLOTHING_PREFIX = "TX Pixel Character - Cloth - ";

    private Dictionary<string, InventoryItem> m_items;
    public bool IsShown => this.m_uiPanel.gameObject.activeInHierarchy;

    [SerializeField]
    private Image m_uiPanel;

    [SerializeField]
    private InventorySlotUI m_slotPrefab;

    [SerializeField]
    private GameObject m_slotParent;

    [SerializeField]
    private bool m_fillRandomly;

    private int m_lineIndex = 0;

    private void Start()
    {
        this.m_lineIndex = 0;
        this.m_items = new Dictionary<string, InventoryItem>();
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
        //Instantiate the prefab for the next item
        InventorySlotUI slot = Instantiate(this.m_slotPrefab, this.m_slotParent.transform);
        slot.Item = item;
        //Get the next available panel position and place the item there
        int currPositionIndex = (this.m_items.Count - 1) % 9;

        if (currPositionIndex == 0)
        {
            //Go down one line
            this.m_lineIndex++;
        }
        Vector3 targetPosition = new Vector2(currPositionIndex * SLOT_SPACING_X, (this.m_lineIndex - 1) * SLOT_SPACING_Y);
        slot.RectTransform.position += targetPosition;
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        //Place them into the inventory (this is only for the shopkeeper)
        foreach (InventoryItem item in items)
        {
            this.AddToInventory(item);
        }
    }

    private ICollection<InventoryItem> GetItemsFromPath(string path)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(path);
        Assert.IsTrue(allSprites.Any(), "Could not find clothing resources!");

        return allSprites
            .Where(_ => Random.Range(0, 2) == 0) //Random insert
            .Select(sprite => new InventoryItem
            {
                m_icon = sprite,
                m_count = Random.Range(1, 65),
                m_name = sprite.name.Replace(CLOTHING_PREFIX, string.Empty),
                m_price = Random.Range(100, 1001),
                m_type = ItemType.Outfit,
                m_owner = ItemOwner.Shopkeeper
            }).ToArray();
    }

}

