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
    }

}

