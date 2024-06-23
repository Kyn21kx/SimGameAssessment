using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerInteractable))]
public class ShopkeeperInteractions : MonoBehaviour
{
    [SerializeField]
    private InventoryBag m_inventory;

    private IInteractable m_interactionHandler;

    private void Start()
    {
        Assert.IsNotNull(this.m_inventory, "Inventory is not set for the shopkeeper!");
        this.m_interactionHandler = GetComponent<IInteractable>();
    }

    public void ShowWares(IInteractable sender)
    {
        Assert.AreEqual(this.m_interactionHandler, sender, "Interaction handlers do not match for shopkeeper!");
        this.m_inventory.ShowInventory();
    }

    private void Update()
    {
        //If we were interacting with our sender, and the inventory is not showing, complete the interaction
        if (this.m_interactionHandler.IsBeingInteractedWith && !this.m_inventory.IsShown)
        {
            this.m_interactionHandler.CompleteInteraction();
        }
    }
}

