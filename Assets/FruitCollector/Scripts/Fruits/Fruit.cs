using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public sealed class Fruit : MonoBehaviour, IPickable
{
    [SerializeField] private FruitData data;
    private SpriteRenderer spriteRenderer;

    public string Id => data != null ? data.Id : string.Empty;
    public string DisplayName => data != null ? data.DisplayName : "(Fruit)";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If this fruit was placed manually, ensure it still renders
        if (data != null) ApplyData(data);

        // Ensure trigger collider
        var collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    public void Configure(FruitData fruitData)
    {
        data = fruitData;
        ApplyData(fruitData);
    }

    private void ApplyData(FruitData fruitData)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = fruitData.Sprite;
        gameObject.name = $"Fruit_{fruitData.DisplayName}";
    }

    public void Pick(IStorable receiver)
    {
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));

        bool wasStored = receiver.Store(this);

        if (wasStored)
        {
            Destroy(gameObject);
        }
    }
}