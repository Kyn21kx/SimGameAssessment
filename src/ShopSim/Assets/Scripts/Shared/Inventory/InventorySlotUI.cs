using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] //Serialize just for debug purposes
    private InventoryItem m_item;

}

