using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;

/// <summary>
/// UI component for the Inventory Item (with references to UI elements)
/// </summary>
[RequireComponent(typeof(Image))]
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
        }
    }

    private Image m_borderImage;

    [SerializeField]
    private Image m_itemImage;

    [SerializeField]
    private TextMeshProUGUI m_nameText;

    [SerializeField]
    private TextMeshProUGUI m_priceText;

    [SerializeField] //Serialize just for debug purposes
    private InventoryItem m_item;

    private void Start()
    {
        this.m_borderImage = GetComponent<Image>();
    }

}

