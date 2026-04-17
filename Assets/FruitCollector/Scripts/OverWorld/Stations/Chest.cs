using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D), typeof(Animator))]
public sealed class Chest : MonoBehaviour, IInteractable
{
    public static readonly int ANIMATOR_OPENED_HASH = Animator.StringToHash("Opened");

    [SerializeField] private string chestId = "chest_01";

    [Header("Límites del Cofre")]
    [SerializeField] private int maxSlots = 5;
    [SerializeField] private int maxStack = 10;

    private EInteractionState InteractionState;
    private Collider2D triggerCollider;
    private Animator animator;

    private Dictionary<string, int> chestItems = new Dictionary<string, int>();
    private PlayerInventory currentPlayerInventory;

    public string ChestId => chestId;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;
        animator = GetComponent<Animator>();
    }

    public void Interact(IInteractor interactor)
    {
        if (interactor == null) return;
        currentPlayerInventory = interactor.Transform.GetComponent<PlayerInventory>();

        if (!animator.GetBool(ANIMATOR_OPENED_HASH)) Open();
        else Close();
    }

    private void Open()
    {
        InteractionState = EInteractionState.INTERACTING;
        animator.SetBool(ANIMATOR_OPENED_HASH, true);

        if (currentPlayerInventory != null)
        {
            var playerItems = currentPlayerInventory.GetItems();

            // Si el jugador no tiene nada en la mochila, le avisamos y terminamos
            if (playerItems.Count == 0)
            {
                Debug.Log("Tu inventario está vacío. No hay nada que guardar en el cofre.");
                return;
            }

            List<string> keys = new List<string>(playerItems.Keys);

            // NUEVO: Bandera para comprobar si realmente hemos transferido algo
            bool somethingTransferred = false;

            foreach (var key in keys)
            {
                int playerAmount = playerItems[key];

                if (chestItems.ContainsKey(key))
                {
                    int spaceLeft = maxStack - chestItems[key];

                    if (spaceLeft > 0)
                    {
                        int amountToMove = Mathf.Min(spaceLeft, playerAmount);
                        chestItems[key] += amountToMove;
                        playerItems[key] -= amountToMove;

                        somethingTransferred = true; // ¡Hemos conseguido guardar algo!

                        if (playerItems[key] <= 0)
                            playerItems.Remove(key);
                        else
                            Debug.LogWarning($"El stack de {key} se ha llenado. Te han sobrado {playerItems[key]} en tu inventario.");
                    }
                    else
                    {
                        Debug.LogWarning($"No se ha guardado {key}. El cofre ya tiene el stack al máximo ({maxStack}).");
                    }
                }
                else if (chestItems.Count < maxSlots)
                {
                    int amountToMove = Mathf.Min(playerAmount, maxStack);
                    chestItems.Add(key, amountToMove);
                    playerItems[key] -= amountToMove;

                    somethingTransferred = true; // ¡Hemos conseguido guardar algo!

                    if (playerItems[key] <= 0)
                        playerItems.Remove(key);
                    else
                        Debug.LogWarning($"El stack de {key} se ha llenado en el nuevo hueco. Te han sobrado {playerItems[key]} en tu inventario.");
                }
                else
                {
                    Debug.LogWarning($"El cofre está lleno (Max {maxSlots} huecos). No se pudo guardar: {key}");
                }
            }

            // NUEVO: El mensaje final ahora depende de lo que haya pasado
            if (somethingTransferred)
            {
                Debug.Log($"Intercambio con cofre {chestId} finalizado con éxito.");
            }
            else
            {
                Debug.LogWarning($"Intercambio cancelado. No se ha podido guardar nada porque el cofre {chestId} está lleno.");
            }
        }
    }

    private void Close()
    {
        InteractionState = EInteractionState.FINISHED;
        animator.SetBool(ANIMATOR_OPENED_HASH, false);
    }

    public EInteractionState GetInteractionState() => InteractionState;
    public Dictionary<string, int> GetItems() => chestItems;

    public void LoadItems(List<ItemData> loadedItems)
    {
        chestItems.Clear();
        foreach (var item in loadedItems)
        {
            chestItems.Add(item.itemId, item.amount);
        }
    }
}