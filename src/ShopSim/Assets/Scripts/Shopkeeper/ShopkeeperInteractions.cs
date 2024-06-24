using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerInteractable))]
public class ShopkeeperInteractions : MonoBehaviour
{
    [SerializeField]
    private InventoryBag m_inventory;

    private IInteractable m_interactionHandler;

    private ShopState m_shopState;

    private void Start()
    {
        EntityFetcher.s_ShopKeeperInventoryBag = this.m_inventory;
        Assert.IsNotNull(this.m_inventory, "Inventory is not set for the shopkeeper!");
        this.m_interactionHandler = GetComponent<IInteractable>();
        this.m_shopState = ShopState.None;
    }

    public void ShowWares(KeyCode keyCode)
    {
        //Before showing the inventory, we need to know if we buy / sell
        switch (keyCode)
        {
            case KeyCode.Q:
                this.m_inventory.ShowInventory();
                this.m_shopState = ShopState.PlayerBuying;
                break;
            default:
                throw new System.Exception($"Key code {keyCode} not handled");
        }
    }

    private void Update()
    {
        //If we were interacting with our sender, and the inventory is not showing, complete the interaction
        if (this.m_interactionHandler.IsBeingInteractedWith && !this.m_inventory.IsShown)
        {
            this.m_interactionHandler.CompleteInteraction();
            this.m_shopState = ShopState.None;
        }
    }
}

