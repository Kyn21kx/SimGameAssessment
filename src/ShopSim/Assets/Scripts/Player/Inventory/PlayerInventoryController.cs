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
