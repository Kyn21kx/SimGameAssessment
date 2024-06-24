using UnityEngine;

[RequireComponent(typeof(InventoryBag))]
[RequireComponent(typeof(MovementController))]
public class PlayerInventoryController : MonoBehaviour
{
    private const string MATERIAL_ASSET_PATH = "Material/Cloth/";
    private const string MATERIAL_CLOTHING_PREFIX = "MT Pixel Character - Cloth - ";

    private InventoryBag m_inventory;

    [SerializeField]
    private GeneralMessenger m_uiMessenger;

    [SerializeField]
    private SkinnedMeshRenderer m_clothRenderer;

    private string m_equippedItemName;

    private void Start()
    {
        this.m_equippedItemName = string.Empty;
        this.m_inventory = GetComponent<InventoryBag>();
        EntityFetcher.s_PlayerInventoryBag = this.m_inventory;
    }

    private void Update()
    {
        if (!GameManager.s_IsInMiniGame && Input.GetKeyDown(KeyCode.I))
        {
            this.InventoryToggleVisible();
        }
    }

    public void OnSlotClickCallback(InventoryBag source, InventoryItem item)
    {
        //Get the state from the shopkeeper's controller if it's available
        ShopkeeperInteractions shopRef = EntityFetcher.s_ShopInteractionsRef;
        if (shopRef == null || shopRef.ShopState == ShopState.None)
        {
            //Default to player behaviour
            this.EquipOutfit(item);
            return;
        }
        if (shopRef.ShopState != ShopState.PlayerSelling) return; //Not our responsibility
        this.SellItem(item);
    }

    public void EquipOutfit(InventoryItem item)
    {
        if (this.m_equippedItemName == item.m_name)
        {
            this.m_uiMessenger.SetText("This item is already equipped!", Color.red);
            EntityFetcher.s_CameraActions.SendCameraShake(0.1f, 0.2f);
            return;
        }
        //Find the material on the cloth asset
        string pathToFind = MATERIAL_ASSET_PATH + MATERIAL_CLOTHING_PREFIX + item.m_name;
        var mat = Resources.Load<Material>(pathToFind);

        //Change the renderer's material
        this.m_clothRenderer.material = mat;
        item.m_count--;
        this.m_uiMessenger.SetText("Item equipped!", Color.yellow);
        this.m_equippedItemName = item.m_name;
    }

    public void SellItem(InventoryItem item)
    {
        //The transaction was succesful
        this.m_uiMessenger.SetText($"Sold {item.m_name} for ${item.m_price}!", Color.green);

        //Transfer ownership to the player's inventory
        InventoryItem shopKeeperOwnedItem = new InventoryItem(item);
        shopKeeperOwnedItem.m_owner = ItemOwner.Player;
        shopKeeperOwnedItem.m_count = 1;
        EntityFetcher.s_ShopKeeperInventoryBag.AddToInventory(shopKeeperOwnedItem);

        //Update the money
        ScoringManager.s_Money += item.m_price;

        //Reduce its count on the player side
        item.m_count--;
    }

    private void InventoryToggleVisible()
    {
        if (!this.m_inventory.IsShown)
        {
            this.m_inventory.ShowInventory();
            return;
        }
        this.m_inventory.DismissInventory();
    }

}
