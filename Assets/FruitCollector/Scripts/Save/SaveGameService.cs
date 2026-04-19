using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization; 

public sealed class SaveGameService : MonoBehaviour
{
    public enum SaveFormat { JSON, XML }

    [Header("Configuración de Guardado")]
    [SerializeField] private SaveFormat currentFormat = SaveFormat.JSON; 

    [Header("Referencias Generales")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Referencias de Frutas")]
    [SerializeField] private FruitFactory fruitFactory;
    [SerializeField] private FruitSelector fruitSelector;

    private string Extension => currentFormat == SaveFormat.JSON ? ".json" : ".xml";
    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame" + Extension);
    private string BackupPath => Path.Combine(Application.persistentDataPath, "savegame_backup" + Extension);

    private float currentSessionTime = 0f;
    private float previousTotalTime = 0f;

    private void Update()
    {
        currentSessionTime += Time.deltaTime;
    }

    public void SaveGame()
    {
        Debug.Log($"Guardando partida en formato {currentFormat}...");

        if (File.Exists(SavePath))
        {
            File.Copy(SavePath, BackupPath, true);
        }

        GameData data = new GameData();
        data.playerPosition[0] = playerMovement.transform.position.x;
        data.playerPosition[1] = playerMovement.transform.position.y;

        foreach (var item in playerInventory.GetItems())
            data.playerInventory.Add(new ItemData { itemId = item.Key, amount = item.Value });

        data.totalPlayTime = previousTotalTime + currentSessionTime;
        data.lastSaveDate = DateTime.Now.ToString("O");

        Fruit[] activeFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        foreach (Fruit f in activeFruits)
        {
            data.worldFruits.Add(new WorldFruitData { fruitId = f.Id, x = f.transform.position.x, y = f.transform.position.y });
        }

        Chest[] allChests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
        foreach (Chest c in allChests)
        {
            ChestSaveData chestData = new ChestSaveData();
            chestData.chestId = c.ChestId;
            foreach (var item in c.GetItems())
            {
                chestData.items.Add(new ItemData { itemId = item.Key, amount = item.Value });
            }
            data.chestsData.Add(chestData);
        }

        string serializedText = "";

        if (currentFormat == SaveFormat.JSON)
        {
            serializedText = JsonUtility.ToJson(data, true);
        }
        else if (currentFormat == SaveFormat.XML)
        {
      
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, data);
                serializedText = writer.ToString();
            }
        }
        File.WriteAllText(SavePath, serializedText);
        Debug.Log($"¡Partida guardada con éxito en: {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning($"No se ha encontrado ningún archivo {Extension} de guardado.");
            return;
        }

        Debug.Log($"Cargando partida desde {currentFormat}...");
        string fileText = File.ReadAllText(SavePath);
        GameData data = null;

        if (currentFormat == SaveFormat.JSON)
        {
            data = JsonUtility.FromJson<GameData>(fileText);
        }
        else if (currentFormat == SaveFormat.XML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using (StringReader reader = new StringReader(fileText))
            {
                data = (GameData)serializer.Deserialize(reader);
            }
        }

        if (data != null)
        {
            playerMovement.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], 0f);
            playerInventory.LoadItems(data.playerInventory);

            previousTotalTime = data.totalPlayTime;
            currentSessionTime = 0f;

            TimeSpan timePlayed = TimeSpan.FromSeconds(data.totalPlayTime);
            Debug.Log($"Tiempo total de juego: {timePlayed.Hours} horas y {timePlayed.Minutes} minutos.");

            if (DateTime.TryParse(data.lastSaveDate, out DateTime lastDate))
            {
                TimeSpan timeSinceLast = DateTime.Now - lastDate;
                Debug.Log($"Hace {timeSinceLast.Days} días, {timeSinceLast.Hours} horas y {timeSinceLast.Minutes} minutos desde tu última sesión de juego.");
            }

            Fruit[] activeFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
            foreach (Fruit f in activeFruits)
            {
                Destroy(f.gameObject);
            }

            foreach (WorldFruitData wfData in data.worldFruits)
            {
                FruitData fData = fruitSelector.GetFruitDataById(wfData.fruitId);
                if (fData != null)
                {
                    Vector3 spawnPos = new Vector3(wfData.x, wfData.y, 0f);
                    fruitFactory.Create(fData, spawnPos, Quaternion.identity);
                }
            }

            Chest[] allChests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest c in allChests)
            {
                ChestSaveData savedChestInfo = data.chestsData.Find(x => x.chestId == c.ChestId);
                if (savedChestInfo != null)
                {
                    c.LoadItems(savedChestInfo.items);
                }
            }

            Debug.Log("¡Partida cargada con éxito!");
        }
    }
}