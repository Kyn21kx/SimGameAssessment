using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    /// <summary>
    /// Initialization of the game manager happens on awake
    /// Every other subsystem is initialized on start
    /// bc of possible dependencies on the game manager
    /// </summary>
    private void Awake()
    {
        EntityFetcher.s_Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        EntityFetcher.s_MainCamera = Camera.main;
        EntityFetcher.s_PlayerExpressions = EntityFetcher.s_Player.GetComponent<PlayerExpressions>();
        EntityFetcher.s_CameraActions = EntityFetcher.s_MainCamera.GetComponent<CameraActions>();
    }
}

