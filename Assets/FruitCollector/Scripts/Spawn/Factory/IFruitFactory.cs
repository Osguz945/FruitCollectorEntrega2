using UnityEngine;
public interface IFruitFactory
{
    Fruit Create(FruitData data, Vector3 position, Quaternion rotation, Transform parent = null);
}