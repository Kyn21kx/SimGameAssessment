using Auxiliars;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PlayerInteractable : MonoBehaviour, IInteractable
{
    public bool IsBeingInteractedWith => this.m_isBeingInteractedWith;

    [SerializeField]
    private float m_interactionRadius;

    [SerializeField]
    private GameObject m_promptObject;

    [SerializeField]
    private KeyCode m_interactionKey;

    [SerializeField]
    private bool m_showGizmos;

    [SerializeField]
    UnityEvent<IInteractable> onInteraction;

    private bool m_isBeingInteractedWith;

    private void Start()
    {
        Assert.IsNotNull(this.m_promptObject, "Null prompt object, interaction will not be visible to the player!");
        this.m_promptObject.SetActive(false);
    }

    private void Update()
    {
        this.DetectPlayer();
    }

    public void Interact()
    {
        this.onInteraction.Invoke(this);
    }

    public void PromptInteraction()
    {
        this.m_promptObject.SetActive(true);
        EntityFetcher.s_PlayerExpressions.TryEnqueueExpression(FacialExpression.Shocked);
    }

    public void DisablePrompt()
    {
        this.m_promptObject.SetActive(false);
    }

    private void DetectPlayer()
    {
        if (this.m_isBeingInteractedWith) return;

        Vector2 playerPos = EntityFetcher.s_Player.transform.position;
        float sqrDis = SpartanMath.DistanceSqr(this.transform.position, playerPos);
        if (sqrDis < this.m_interactionRadius * this.m_interactionRadius)
        {
            this.PromptInteraction();
            return;
        }
        this.DisablePrompt();
    }

    private void OnDrawGizmos()
    {
        if (!this.m_showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.m_interactionRadius);
    }
}
