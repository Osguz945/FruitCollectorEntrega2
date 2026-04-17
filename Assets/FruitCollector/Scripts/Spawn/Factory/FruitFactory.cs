using System;
using UnityEngine;

public sealed class FruitFactory : MonoBehaviour, IFruitFactory
{
    [SerializeField] private Fruit fruitPrefab;    
    

    public Fruit Create(FruitData data, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (fruitPrefab == null)
            throw new InvalidOperationException("FruitFactory needs a Fruit prefab.");

        // Instantiate the prefab
        Fruit instance = Instantiate(fruitPrefab, position, rotation, parent);

        // Configure fruit data
        instance.Configure(data);

        return instance;
    }
}