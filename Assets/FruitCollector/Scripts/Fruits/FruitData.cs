using UnityEngine;

[CreateAssetMenu(menuName = "Fruit Picker/Fruits/Fruit Data", fileName = "FruitData")]
public sealed class FruitData : ScriptableObject
{
    private const string KEY_NAME = "fruit:";

    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite sprite;

    public string Id => id;
    public string DisplayName => displayName;
    public Sprite Sprite => sprite;


    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = KEY_NAME + name.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = name.Trim();
    }
}