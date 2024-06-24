using UnityEngine;

public static class EntityFetcher
{
    public static GameObject s_Player { get; set; }

    public static Camera s_MainCamera { get; set; }

    public static PlayerExpressions s_PlayerExpressions { get; set; }

    public static CameraActions s_CameraActions { get; set; }

    public static InventoryBag s_PlayerInventoryBag { get; set; }

    public static InventoryBag s_ShopKeeperInventoryBag { get; set; }

    public static ShopkeeperInteractions s_ShopInteractionsRef { get; set; }

}

