public interface IInteractable
{
    void Interact(IInteractor interactor);
    EInteractionState GetInteractionState();
}