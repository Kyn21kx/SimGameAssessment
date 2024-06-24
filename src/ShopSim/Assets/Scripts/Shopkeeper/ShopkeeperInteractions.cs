using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerInteractable))]
public class ShopkeeperInteractions : MonoBehaviour
{
    public ShopState ShopState => this.m_shopState;

    [SerializeField]
    private InventoryBag m_inventory;

    [SerializeField]
    private GeneralMessenger m_uiMessenger;

    private IInteractable m_interactionHandler;

    private ShopState m_shopState;

    private void Start()
    {
        EntityFetcher.s_ShopInteractionsRef = this;
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
            case KeyCode.E:
                this.m_shopState = ShopState.PlayerSelling;
                EntityFetcher.s_PlayerInventoryBag.ShowInventory();
                break;
            default:
                throw new System.Exception($"Key code {keyCode} not handled");
        }
    }

    public void OnClickCallback(InventoryBag source, InventoryItem item)
    {
        //If the player is buying, check if they have the money for it, if not, send a small camera shake
        if (this.m_shopState != ShopState.PlayerBuying) return;

        if (ScoringManager.s_Money < item.m_price)
        {
            //Audio queue, camera shake and if we have time, a message
            EntityFetcher.s_CameraActions.SendCameraShake(0.1f, 0.2f);
            this.m_uiMessenger.SetText("Not enough money to buy this item!", Color.red);
            return;
        }
        //The transaction was succesful
        this.m_uiMessenger.SetText($"Bought {item.m_name} for ${item.m_price}!", Color.green);

        //Transfer ownership to the player's inventory
        InventoryItem playerOwnedItem = new InventoryItem(item);
        playerOwnedItem.m_owner = ItemOwner.Player;
        playerOwnedItem.m_count = 1;
        //Never keep the price the same as we bought it for
        playerOwnedItem.m_price = Mathf.FloorToInt(playerOwnedItem.m_price * 0.8f);
        EntityFetcher.s_PlayerInventoryBag.AddToInventory(playerOwnedItem);

        //Update the money
        ScoringManager.s_Money -= item.m_price;

        //Reduce its count on the shopkeeper side
        item.m_count--;
    }

    private void Update()
    {
        //If we were interacting with our sender, and the inventory is not showing, complete the interaction
        if (this.m_interactionHandler.IsBeingInteractedWith && !this.m_inventory.IsShown && !EntityFetcher.s_PlayerInventoryBag.IsShown)
        {
            this.m_interactionHandler.CompleteInteraction();
            this.m_shopState = ShopState.None;
        }
    }
}

