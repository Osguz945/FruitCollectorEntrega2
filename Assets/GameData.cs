using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public string itemId;
    public int amount;
}

[System.Serializable]
public class WorldFruitData
{
    public string fruitId;
    public float x;
    public float y;
}

// NUEVO: Molde para guardar un cofre individual
[System.Serializable]
public class ChestSaveData
{
    public string chestId;
    public List<ItemData> items = new List<ItemData>();
}

[System.Serializable]
public class GameData
{
    public float[] playerPosition = new float[2];
    public List<ItemData> playerInventory = new List<ItemData>();

    // NUEVO: Ahora es una lista de cofres, no solo uno
    public List<ChestSaveData> chestsData = new List<ChestSaveData>();

    public float totalPlayTime;
    public string lastSaveDate;
    public List<WorldFruitData> worldFruits = new List<WorldFruitData>();
}