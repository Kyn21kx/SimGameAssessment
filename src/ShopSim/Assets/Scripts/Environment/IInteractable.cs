using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    bool IsBeingInteractedWith { get; } //Naming is hard :c

    void PromptInteraction();

    void DisablePrompt();

    void Interact(KeyCode triggerKey);

    void SetInteractionText(string text);

    void SetInteractionKeys(KeyCode[] keys);

    void CompleteInteraction();
}

