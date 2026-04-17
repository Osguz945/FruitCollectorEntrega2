using System;
using UnityEngine;

public class FruitSelector : MonoBehaviour, IFruitSelector
{
    [SerializeField] private FruitData[] availableFruits;

    private readonly IRandomProvider randomProvider = new UnityRandomProvider();
    

    public FruitData PickRandomFruitData()
    {
        if (availableFruits == null || availableFruits.Length == 0)
            throw new InvalidOperationException("FruitFactory has no FruitData assets assigned.");

        int index = randomProvider.Range(0, availableFruits.Length);
        var data = availableFruits[index];

        if (data == null)
            throw new InvalidOperationException("FruitFactory has a null FruitData reference in the list.");

        return data;

    }
    // NUEVO: Método para buscar una fruta específica por su ID
    public FruitData GetFruitDataById(string id)
    {
        foreach (var data in availableFruits)
        {
            if (data.Id == id) return data;
        }
        return null;
    }
}