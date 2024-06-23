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
        EntityFetcher.Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        EntityFetcher.MainCamera = Camera.main;
    }
}

