using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerInteractionController : MonoBehaviour, IInteractionLockable
{
    [SerializeField] private Behaviour[] componentsToDisable;

    private bool isLocked;

    public void LockInteraction()
    {
        if (isLocked) return;

        foreach (var component in componentsToDisable)
        {
            if (component != null)
                component.enabled = false;
        }

        isLocked = true;
    }

    public void UnlockInteraction()
    {
        if (!isLocked) return;

        foreach (var component in componentsToDisable)
        {
            if (component != null)
                component.enabled = true;
        }

        isLocked = false;
    }
}