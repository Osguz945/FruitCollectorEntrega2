using System;
using UnityEngine;

public class IntervalSpawner : MonoBehaviour
{
    [SerializeField, Min(0.05f)] private float intervalSeconds = 1.0f;
    [SerializeField] private MonoBehaviour selectorBehaviour;
    [SerializeField] private MonoBehaviour factoryBehaviour;

    [Space(10)]
    [SerializeField] private LayerMask blockingLayers;
    [SerializeField] private float spawnCheckRadius = 0.5f;
    [SerializeField] private int maxSpawnAttempts = 20;

    private IFruitSelector selector;
    private IFruitFactory factory;

    private float timer;
    private bool isSpawning;

    private void Awake()
    {
        selector = selectorBehaviour as IFruitSelector;
        if (selector == null)
            throw new InvalidOperationException("IntervalSpawner: selectorBehaviour must implement IFruitSelector.");

        factory = factoryBehaviour as IFruitFactory;
        if (selector == null)
            throw new InvalidOperationException("IntervalSpawner: selectorBehaviour must implement IFruitSelector.");
    }


    private void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer < intervalSeconds) return;

        timer = 0f;
        Spawn();
    }


    private void Spawn()
    {
        if (TryFindSpawnPosition(out Vector2 spawnPosition))
        {
            factory.Create(selector.PickRandomFruitData(), spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No valid spawn position found.");
        }
    }


    private bool TryFindSpawnPosition(out Vector2 position)
    {
        IRandomProvider randomProvider = new UnityRandomProvider();

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float x = randomProvider.Range(-8, 8);
            float y = randomProvider.Range(-4, 4);

            Vector2 candidate = new Vector2(x, y);

            bool blocked = Physics2D.OverlapCircle(
                candidate,
                spawnCheckRadius,
                blockingLayers
            );

            if (!blocked)
            {
                position = candidate;
                return true;
            }
        }

        position = Vector2.zero;
        return false;
    }


    public void StartSpawning()
    {
        timer = 0f;
        isSpawning = true;
    }


    public void StopSpawning()
    {
        isSpawning = false;
    }


    private void OnEnable()
    {
        StartSpawning();
    }


    private void OnDisable()
    {
        StopSpawning();
    }
}