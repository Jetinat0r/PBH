using UnityEngine;

[CreateAssetMenu(menuName = "Card Type")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Type { get; private set; }
}