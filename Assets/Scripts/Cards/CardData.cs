using UnityEngine;

public enum CardType // Defines bullet pattern
{
    Horizontal,
    Vertical,
    Cross,
    X
}

[CreateAssetMenu(fileName = "NewCardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;

    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public string type;
}