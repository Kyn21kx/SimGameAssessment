using UnityEngine;

public class PlayerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float m_interactionRadius;

    private void Update()
    {
        this.DetectPlayer();
    }

    public void Interact()
    {

    }

    public void PromptInteraction()
    {

    }

    private void DetectPlayer()
    {
        Vector2 playerPos = EntityFetcher.Player.transform.position;
    }
}
