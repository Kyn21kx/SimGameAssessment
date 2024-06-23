public interface IInteractable
{
    bool IsBeingInteractedWith { get; } //Naming is hard :c

    void PromptInteraction();

    void DisablePrompt();

    void Interact();
}

