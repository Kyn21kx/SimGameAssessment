using System.Linq;
using Auxiliars;
using TMPro;
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
    private KeyCode[] m_interactionKeys;

    [SerializeField]
    private bool m_showGizmos;

    [SerializeField]
    UnityEvent<KeyCode> onInteraction;

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

    public void Interact(KeyCode triggerKeyCode)
    {
        this.m_isBeingInteractedWith = true;
        //TODO: Maybe set this on the actual callback, idk
        var movementRef = EntityFetcher.s_Player.GetComponent<MovementController>();
        movementRef.CanMove = false;
        this.onInteraction.Invoke(triggerKeyCode);
    }

    public void PromptInteraction()
    {
        this.m_promptObject.SetActive(true);
        EntityFetcher.s_PlayerExpressions.TryEnqueueExpression(FacialExpression.Shocked);
        KeyCode resultingKey = KeyCode.None;
        //Get the first out of the possible keys
        foreach (var keyCode in this.m_interactionKeys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                resultingKey = keyCode;
                break;
            }
        }

        if (resultingKey != KeyCode.None)
        {
            this.DisablePrompt();
            this.Interact(resultingKey);
        }
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

    public void CompleteInteraction()
    {
        this.m_isBeingInteractedWith = false;
        var movementRef = EntityFetcher.s_Player.GetComponent<MovementController>();
        movementRef.CanMove = true;
    }

    public void SetInteractionText(string text)
    {
        var textRef = this.m_promptObject.GetComponentInChildren<TextMeshPro>();
        textRef.text = text;
    }

    public void SetInteractionKeys(KeyCode[] keys)
    {
        this.m_interactionKeys = keys;
    }
}
