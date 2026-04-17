using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(IInteractionLockable))]
public sealed class PlayerInteractor : MonoBehaviour, IInteractor
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private bool visibleRadius = false;
    [SerializeField] private LayerMask interactionLayer;

    private readonly Collider2D[] results = new Collider2D[8];
    private ContactFilter2D contactFilter;

    private IInteractionLockable interactionLock;

    public Transform Transform => transform;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(interactionLayer);
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;

        interactionLock = GetComponent<IInteractionLockable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        int count = Physics2D.OverlapCircle(
            transform.position,
            interactionRadius,
            contactFilter,
            results
        );

        for (int i = 0; i < count; i++)
        {
            var collider = results[i];
            if (collider == null) continue;

            if (collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact(this);

                if(interactable.GetInteractionState() == EInteractionState.INTERACTING)
                    interactionLock.LockInteraction();

                else if (interactable.GetInteractionState() == EInteractionState.FINISHED)
                    interactionLock.UnlockInteraction();

                return;
            }
        }
    }


    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!visibleRadius) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
    #endif
}