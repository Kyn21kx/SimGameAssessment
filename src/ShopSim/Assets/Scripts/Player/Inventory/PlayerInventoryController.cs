using UnityEngine;

[RequireComponent(typeof(InventoryBag))]
[RequireComponent(typeof(MovementController))]
public class PlayerInventoryController : MonoBehaviour
{
    private InventoryBag m_inventory;

    private void Start()
    {
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

    private void OnSlotClickCallback(InventoryBag source, InventoryItem item)
    {
        //Get the state from the shopkeeper's controller if it's available
        ShopkeeperInteractions shopRef = EntityFetcher.s_ShopInteractionsRef;
        if (shopRef == null || shopRef.ShopState == ShopState.None)
        {
            //Default to player behaviour
            return;
        }

        switch (shopRef.ShopState)
        {
            case ShopState.PlayerBuying:
                break;
            case ShopState.PlayerSelling:
                break;
        }
    }

    public void EquipOutfit()
    {

    }

    public bool TryBuyItem()
    {
        return false;
    }

    public bool TrySellItem()
    {
        return false;
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
