using Auxiliars;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour, IInteractable
{
    public bool IsBeingInteractedWith => this.m_isBeingInteractedWith;

    [SerializeField]
    private float m_interactionRadius;

    [SerializeField]
    private bool m_showGizmos;

    private bool m_isBeingInteractedWith;

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
        if (this.m_isBeingInteractedWith) return;

        Vector2 playerPos = EntityFetcher.Player.transform.position;
        float sqrDis = SpartanMath.DistanceSqr(this.transform.position, playerPos);
        if (sqrDis < this.m_interactionRadius * this.m_interactionRadius)
        {
            this.PromptInteraction();
        }
    }

    private void OnDrawGizmos()
    {
        if (!this.m_showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.m_interactionRadius);
    }
}
