using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InventoryBag))]
[RequireComponent(typeof(MovementController))]
public class PlayerInventoryController : MonoBehaviour
{
    private const string MATERIAL_ASSET_PATH = "Assets/ThirdParty/Cainos/Customizable Pixel Character/Material/Cloth/";
    private const string MATERIAL_CLOTHING_PREFIX = "MT Pixel Character - Cloth - ";
    private const string MAT_EXTENSION = ".mat";

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

        switch (shopRef.ShopState)
        {
            case ShopState.PlayerBuying:
                break;
            case ShopState.PlayerSelling:
                break;
        }
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
        string pathToFind = MATERIAL_ASSET_PATH + MATERIAL_CLOTHING_PREFIX + item.m_name + MAT_EXTENSION;
        var mat = AssetDatabase.LoadAssetAtPath<Material>(pathToFind);

        //Change the renderer's material
        this.m_clothRenderer.material = mat;
        item.m_count--;
        this.m_uiMessenger.SetText("Item equipped!", Color.yellow);
        this.m_equippedItemName = item.m_name;
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
