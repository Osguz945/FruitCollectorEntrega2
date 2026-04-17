using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerInventory : MonoBehaviour, IStorable
{
    [Header("Límites del Inventario")]
    [SerializeField] private int maxSlots = 5; // Cuántos tipos de fruta distintos puede llevar
    [SerializeField] private int maxStack = 10; // Cuántas frutas iguales caben en un mismo hueco

    private Dictionary<string, int> items = new Dictionary<string, int>();

    public bool Store(IPickable item)
    {
        string itemId = item.Id;

        // CASO A: Ya tenemos este tipo de fruta en la mochila
        if (items.ContainsKey(itemId))
        {
            if (items[itemId] < maxStack)
            {
                items[itemId]++;
                Debug.Log($"Recogido: {item.DisplayName}. Total: {items[itemId]}/{maxStack}");
                return true; // Recogida con éxito
            }
            else
            {
                Debug.LogWarning($"¡No puedes llevar más {item.DisplayName}! Stack lleno.");
                return false; // Rechazada (se queda en el suelo)
            }
        }
        // CASO B: Es una fruta nueva
        else
        {
            if (items.Count < maxSlots)
            {
                items.Add(itemId, 1);
                Debug.Log($"Nuevo objeto: {item.DisplayName}. Huecos ocupados: {items.Count}/{maxSlots}");
                return true; // Recogida con éxito
            }
            else
            {
                Debug.LogWarning("¡Inventario lleno! No tienes más huecos libres.");
                return false; // Rechazada (se queda en el suelo)
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