using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerInventory : MonoBehaviour, IStorable
{
    [Header("Límites del Inventario")]
    [SerializeField] private int maxSlots = 5; 
    [SerializeField] private int maxStack = 10;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    public bool Store(IPickable item)
    {
        string itemId = item.Id;

        if (items.ContainsKey(itemId))
        {
            if (items[itemId] < maxStack)
            {
                items[itemId]++;
                Debug.Log($"Recogido: {item.DisplayName}. Total: {items[itemId]}/{maxStack}");
                return true; 
            }
            else
            {
                Debug.LogWarning($"¡No puedes llevar más {item.DisplayName}! Stack lleno.");
                return false; 
            }
        }
        
        else
        {
            if (items.Count < maxSlots)
            {
                items.Add(itemId, 1);
                Debug.Log($"Nuevo objeto: {item.DisplayName}. Huecos ocupados: {items.Count}/{maxSlots}");
                return true; 
            }
            else
            {
                Debug.LogWarning("¡Inventario lleno! No tienes más huecos libres.");
                return false;
            }
        }
    }

    public Dictionary<string, int> GetItems()
    {
        return items;
    }

    public void LoadItems(List<ItemData> loadedItems)
    {
        items.Clear();
        foreach (var item in loadedItems)
        {
            items.Add(item.itemId, item.amount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (!other.TryGetComponent<IPickable>(out var pickable))
            return;

        pickable.Pick(this);
    }
}