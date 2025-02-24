using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public CardType cardType;

    public enum CardType // Defines bullet pattern
    {
        Horizontal,
        Vertical,
        Cross,
        X
    }


        
}