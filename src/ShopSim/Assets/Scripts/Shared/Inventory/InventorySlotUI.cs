using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Assertions;

/// <summary>
/// UI component for the Inventory Item (with references to UI elements)
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class InventorySlotUI : MonoBehaviour
{
    public InventoryItem Item {
        get => this.m_item;
        set
        {
            this.m_item = value;
            //Update the UI values
            this.m_itemImage.sprite = value.m_icon;
            this.m_nameText.text = value.m_name;
            this.m_priceText.text = $"${value.m_price}.00";
            this.m_countText.text = $"Count: {value.m_count}";
        }
    }

    public RectTransform RectTransform => this.m_rectTransform;

    public UnityEvent<InventoryBag, InventoryItem> OnClickCallback { get => m_onClickCallback; set => m_onClickCallback = value; }

    [SerializeField]
    private RectTransform m_rectTransform;

    [SerializeField]
    private Image m_itemImage;

    [SerializeField]
    private TextMeshProUGUI m_nameText;

    [SerializeField]
    private TextMeshProUGUI m_priceText;

    [SerializeField]
    private TextMeshProUGUI m_countText;

    [SerializeField]
    private UnityEvent<InventoryBag, InventoryItem> m_onClickCallback;

    [SerializeField] //Serialize just for debug purposes
    private InventoryItem m_item;

    public void OnSlotClick()
    {
        InventoryBag sourceBag = this.m_item.m_owner switch {
           ItemOwner.Player => EntityFetcher.s_PlayerInventoryBag,
           ItemOwner.Shopkeeper => EntityFetcher.s_ShopKeeperInventoryBag,
           _ => throw new System.Exception($"Case not handled for owner {this.m_item.m_owner}")
        };
        Assert.IsNotNull(sourceBag, $"Inventory bag not found for the item {this.m_item.m_name} owner!");
        this.m_onClickCallback.Invoke(sourceBag, this.m_item);
        this.Item = this.m_item; //Run updates
        this.CheckForAvailability();
    }

    private void OnEnable()
    {
        if (this.m_item != null)
        {
            //Update the UI values
            this.m_itemImage.sprite = this.m_item.m_icon;
            this.m_nameText.text = this.m_item.m_name;
            this.m_priceText.text = $"${this.m_item.m_price}.00";
            this.m_countText.text = $"Count: {this.m_item.m_count}";
        }
        this.CheckForAvailability();
    }

    private void CheckForAvailability()
    {
        if (this.m_item.m_count <= 0)
        {
            //This will leave the inventory a little weirdly spaced out, but it's ok
            InventoryBag sourceBag = this.m_item.m_owner switch
            {
                ItemOwner.Player => EntityFetcher.s_PlayerInventoryBag,
                ItemOwner.Shopkeeper => EntityFetcher.s_ShopKeeperInventoryBag,
                _ => throw new System.Exception($"Case not handled for owner {this.m_item.m_owner}")
            };
            sourceBag.RemoveFromInternalDictionary(this.m_item.m_name);
            Destroy(this.gameObject);
        }
    }

}

